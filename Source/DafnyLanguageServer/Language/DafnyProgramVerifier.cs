﻿using Microsoft.Boogie;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VC;
using System;
using Microsoft.Dafny.LanguageServer.Workspace;
using VCGeneration;

namespace Microsoft.Dafny.LanguageServer.Language {
  /// <summary>
  /// dafny-lang based implementation of the program verifier. Since it makes use of static members,
  /// any access is synchronized. Moreover, it ensures that exactly one instance exists over the whole
  /// application lifetime.
  /// </summary>
  /// <remarks>
  /// dafny-lang makes use of static members and assembly loading. Since thread-safety of this is not guaranteed,
  /// this verifier serializes all invocations.
  /// </remarks>
  public class DafnyProgramVerifier : IProgramVerifier {
    private static readonly object InitializationSyncObject = new();
    private static bool initialized;

    private readonly ILogger logger;
    private readonly VerifierOptions options;
    private readonly SemaphoreSlim mutex = new(1);
    private readonly VerificationResultCache cache = new();

    DafnyOptions Options => DafnyOptions.O;

    private DafnyProgramVerifier(
      ILogger<DafnyProgramVerifier> logger,
      VerifierOptions options
      ) {
      this.logger = logger;
      this.options = options;
    }

    /// <summary>
    /// Factory method to safely create a new instance of the verifier. It ensures that global/static
    /// settings are set exactly ones.
    /// </summary>
    /// <param name="logger">A logger instance that may be used by this verifier instance.</param>
    /// <param name="options">Settings for the verifier.</param>
    /// <returns>A safely created dafny verifier instance.</returns>
    public static DafnyProgramVerifier Create(
      ILogger<DafnyProgramVerifier> logger, IOptions<VerifierOptions> options) {
      lock (InitializationSyncObject) {
        if (!initialized) {
          // TODO This may be subject to change. See Microsoft.Boogie.Counterexample
          //      A dash means write to the textwriter instead of a file.
          // https://github.com/boogie-org/boogie/blob/b03dd2e4d5170757006eef94cbb07739ba50dddb/Source/VCGeneration/Couterexample.cs#L217
          DafnyOptions.O.ModelViewFile = "-";
          DafnyOptions.O.VerifySnapshots = (int)options.Value.VerifySnapshots;
          initialized = true;
          logger.LogTrace("Initialized the boogie verifier with " +
                          "VerifySnapshots={VerifySnapshots}.",
                          DafnyOptions.O.VerifySnapshots);
        }
        return new DafnyProgramVerifier(logger, options.Value);
      }
    }

    private static int GetConfiguredCoreCount(VerifierOptions options) {
      return options.VcsCores == 0
        ? Math.Max(1, Environment.ProcessorCount / 2)
        : Convert.ToInt32(options.VcsCores);
    }

    public async Task<VerificationResult> VerifyAsync(DafnyDocument document,
                                     IVerificationProgressReporter progressReporter,
                                     CancellationToken cancellationToken) {
      await mutex.WaitAsync(cancellationToken);
      var program = document.Program;
      try {
        // The printer is responsible for two things: It logs boogie errors and captures the counter example model.
        var errorReporter = (DiagnosticErrorReporter)program.reporter;
        if (options.GutterStatus) {
          progressReporter.RecomputeVerificationTree();
          progressReporter.ReportRealtimeDiagnostics(document);
        }

        var printer = new ModelCapturingOutputPrinter(logger, errorReporter, progressReporter, options.GutterStatus);
        // Do not set these settings within the object's construction. It will break some tests within
        // VerificationNotificationTest and DiagnosticsTest that rely on updating these settings.
        DafnyOptions.O.TimeLimit = options.TimeLimit;
        DafnyOptions.O.VcsCores = GetConfiguredCoreCount(options);
        DafnyOptions.O.Printer = printer;

        var executionEngine = new ExecutionEngine(DafnyOptions.O, cache);
        var translated = Translator.Translate(program, errorReporter, new Translator.TranslatorFlags {
          InsertChecksums = true,
          ReportRanges = true
        });
        var moduleTasks = translated.Select(t => {
          var (moduleName, boogieProgram) = t;
          var programId = program.FullName;
          var boogieProgramId = (programId ?? "main_program_id") + "_" + moduleName;
          return VerifyWithBoogieAsync(TextWriter.Null, executionEngine, boogieProgram, cancellationToken, boogieProgramId);
        }).ToList();
        await Task.WhenAll(moduleTasks);
        var verified = moduleTasks.All(t => t.Result);
        return new VerificationResult(verified, printer.SerializedCounterExamples);
      }
      finally {
        mutex.Release();
      }
    }

    private async Task<bool> VerifyWithBoogieAsync(TextWriter output,
      ExecutionEngine engine, Boogie.Program program,
      CancellationToken cancellationToken, string programId) {
      program.Resolve(engine.Options);
      program.Typecheck(engine.Options);

      engine.EliminateDeadVariables(program);
      engine.CollectModSets(program);
      engine.CoalesceBlocks(program);
      engine.Inline(program);
      var uniqueRequestId = Guid.NewGuid().ToString();
      using (cancellationToken.Register(() => CancelVerification(uniqueRequestId))) {
        try {
          var statistics = new PipelineStatistics();
          var outcome = await engine.InferAndVerify(output, program, statistics, programId, null, uniqueRequestId);
          return Main.IsBoogieVerified(outcome, statistics);
        } catch (Exception e) when (e is not OperationCanceledException) {
          if (!cancellationToken.IsCancellationRequested) {
            throw;
          }
          // It appears that Boogie disposes resources that are still in use upon cancellation.
          // Therefore, we log this error and proceed with the cancellation.
          logger.LogDebug(e, "boogie error occured when cancelling the verification");
          throw new OperationCanceledException(cancellationToken);
        }
      }
    }

    private void CancelVerification(string requestId) {
      logger.LogDebug("requesting verification cancellation of {RequestId}", requestId);
      ExecutionEngine.CancelRequest(requestId);
    }

    private class ModelCapturingOutputPrinter : OutputPrinter {
      private readonly ILogger logger;
      private readonly DiagnosticErrorReporter errorReporter;
      private readonly IVerificationProgressReporter progressReporter;
      private StringBuilder? serializedCounterExamples;
      private readonly bool reportVerificationDiagnostics;

      public string? SerializedCounterExamples => serializedCounterExamples?.ToString();

      public ModelCapturingOutputPrinter(
          ILogger logger,
          DiagnosticErrorReporter errorReporter,
          IVerificationProgressReporter progressReporter,
          bool reportVerificationDiagnostics) {
        this.logger = logger;
        this.errorReporter = errorReporter;
        this.progressReporter = progressReporter;
        this.reportVerificationDiagnostics = reportVerificationDiagnostics;
      }

      public void AdvisoryWriteLine(TextWriter writer, string format, params object[] args) {
      }

      public ExecutionEngineOptions? Options { get; set; }

      public void ErrorWriteLine(TextWriter tw, string s) {
        logger.LogError(s);
      }

      public void ErrorWriteLine(TextWriter tw, string format, params object[] args) {
        logger.LogError(format, args);
      }

      public void Inform(string s, TextWriter tw) {
        logger.LogInformation(s);
      }

      public void ReportBplError(IToken tok, string message, bool error, TextWriter tw, [AllowNull] string category) {
        logger.LogError(message);
      }

      public void ReportImplementationsBeforeVerification(Implementation[] implementations) {
        if (reportVerificationDiagnostics) {
          progressReporter.ReportImplementationsBeforeVerification(implementations);
        }
      }

      public void ReportStartVerifyImplementation(Implementation implementation) {
        if (reportVerificationDiagnostics) {
          progressReporter.ReportStartVerifyImplementation(implementation);
        }
      }

      public void ReportEndVerifyImplementation(Implementation implementation, Boogie.VerificationResult result) {
        if (reportVerificationDiagnostics) {
          progressReporter.ReportEndVerifyImplementation(implementation, result);
        }
      }

      public void ReportSplitResult(Split split, VCResult vcResult) {
        if (reportVerificationDiagnostics) {
          progressReporter.ReportAssertionBatchResult(split, vcResult);
        }
      }

      public void WriteErrorInformation(ErrorInformation errorInfo, TextWriter tw, bool skipExecutionTrace) {
        CaptureCounterExamples(errorInfo);
        errorReporter.ReportBoogieError(errorInfo);
      }

      private void CaptureCounterExamples(ErrorInformation errorInfo) {
        if (errorInfo.ModelWriter is StringWriter modelString) {
          // We do not know a-priori how many errors we'll receive. Therefore we capture all models
          // in a custom stringbuilder and reset the original one to not duplicate the outputs.
          serializedCounterExamples ??= new StringBuilder();
          serializedCounterExamples.Append(modelString.ToString());
          modelString.GetStringBuilder().Clear();
        }
      }

      public void WriteTrailer(TextWriter writer, PipelineStatistics stats) {
      }
    }
  }
}
