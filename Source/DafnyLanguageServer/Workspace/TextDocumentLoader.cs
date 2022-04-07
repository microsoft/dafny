﻿using IntervalTree;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Language.Symbols;
using Microsoft.Dafny.LanguageServer.Workspace.Notifications;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Boogie;
using Microsoft.Extensions.Logging;
using VC;

namespace Microsoft.Dafny.LanguageServer.Workspace {
  /// <summary>
  /// Text document loader implementation that offloads the whole load procedure on one dedicated
  /// thread with a stack size of 256MB. Since only one thread is used, document loading is implicitely synchronized.
  /// The verification runs on the calling thread.
  /// </summary>
  /// <remarks>
  /// The increased stack size is necessary to solve the issue https://github.com/dafny-lang/dafny/issues/1447.
  /// </remarks>
  public class TextDocumentLoader : ITextDocumentLoader {
    // 256MB
    private const int MaxStackSize = 0x10000000;

    private DafnyOptions Options => DafnyOptions.O;
    private readonly IDafnyParser parser;
    private readonly ISymbolResolver symbolResolver;
    private readonly ISymbolTableFactory symbolTableFactory;
    private readonly IProgramVerifier verifier;
    private readonly IGhostStateDiagnosticCollector ghostStateDiagnosticCollector;
    private readonly ICompilationStatusNotificationPublisher notificationPublisher;
    private readonly ILoggerFactory loggerFactory;
    private readonly ILogger<TextDocumentLoader> logger;

    private TextDocumentLoader(
      ILoggerFactory loggerFactory,
      IDafnyParser parser,
      ISymbolResolver symbolResolver,
      IProgramVerifier verifier,
      ISymbolTableFactory symbolTableFactory,
      IGhostStateDiagnosticCollector ghostStateDiagnosticCollector,
      ICompilationStatusNotificationPublisher notificationPublisher) {
      this.parser = parser;
      this.symbolResolver = symbolResolver;
      this.verifier = verifier;
      this.symbolTableFactory = symbolTableFactory;
      this.ghostStateDiagnosticCollector = ghostStateDiagnosticCollector;
      this.notificationPublisher = notificationPublisher;
      this.loggerFactory = loggerFactory;
      this.logger = loggerFactory.CreateLogger<TextDocumentLoader>();
    }

    static readonly ThreadTaskScheduler LargeStackScheduler = new(MaxStackSize);

    public static TextDocumentLoader Create(
      IDafnyParser parser,
      ISymbolResolver symbolResolver,
      IProgramVerifier verifier,
      ISymbolTableFactory symbolTableFactory,
      IGhostStateDiagnosticCollector ghostStateDiagnosticCollector,
      ICompilationStatusNotificationPublisher notificationPublisher,
      ILoggerFactory loggerFactory
      ) {
      return new TextDocumentLoader(loggerFactory, parser, symbolResolver, verifier, symbolTableFactory, ghostStateDiagnosticCollector, notificationPublisher);
    }

    public DafnyDocument CreateUnloaded(TextDocumentItem textDocument, CancellationToken cancellationToken) {
      var errorReporter = new DiagnosticErrorReporter(textDocument.Uri);
      return CreateDocumentWithEmptySymbolTable(
        loggerFactory.CreateLogger<SymbolTable>(),
        textDocument,
        errorReporter,
        parser.CreateUnparsed(textDocument, errorReporter, cancellationToken),
        loadCanceled: true
      );
    }

    public async Task<DafnyDocument> LoadAsync(TextDocumentItem textDocument, CancellationToken cancellationToken) {
#pragma warning disable CS1998
      return await await Task.Factory.StartNew(async () => LoadInternal(textDocument, cancellationToken), cancellationToken,
#pragma warning restore CS1998
        TaskCreationOptions.None, LargeStackScheduler);
    }

    private DafnyDocument LoadInternal(TextDocumentItem textDocument, CancellationToken cancellationToken) {
      var errorReporter = new DiagnosticErrorReporter(textDocument.Uri);
      var program = parser.Parse(textDocument, errorReporter, cancellationToken);
      IncludePluginLoadErrors(errorReporter, program);
      if (errorReporter.HasErrors) {
        notificationPublisher.SendStatusNotification(textDocument, CompilationStatus.ParsingFailed);
        return CreateDocumentWithEmptySymbolTable(loggerFactory.CreateLogger<SymbolTable>(), textDocument, errorReporter, program, loadCanceled: false);
      }

      var compilationUnit = symbolResolver.ResolveSymbols(textDocument, program, cancellationToken);
      var symbolTable = symbolTableFactory.CreateFrom(program, compilationUnit, cancellationToken);
      if (errorReporter.HasErrors) {
        notificationPublisher.SendStatusNotification(textDocument, CompilationStatus.ResolutionFailed);
      } else {
        notificationPublisher.SendStatusNotification(textDocument, CompilationStatus.CompilationSucceeded);
      }
      var ghostDiagnostics = ghostStateDiagnosticCollector.GetGhostStateDiagnostics(symbolTable, cancellationToken).ToArray();

      return new DafnyDocument(Options, textDocument, errorReporter.GetDiagnostics(textDocument.Uri),
        Array.Empty<Diagnostic>(), Array.Empty<Counterexample>(),
        ghostDiagnostics, program, symbolTable);
    }

    // TODO should we include this for each document? I think it's better not to show these errors as diagnostics, but through something like a showMessage request.
    private static void IncludePluginLoadErrors(DiagnosticErrorReporter errorReporter, Dafny.Program program) {
      foreach (var error in DafnyLanguageServer.PluginLoadErrors) {
        errorReporter.Error(MessageSource.Compiler, program.GetFirstTopLevelToken(), error);
      }
    }

    private DafnyDocument CreateDocumentWithEmptySymbolTable(
      ILogger<SymbolTable> logger,
      TextDocumentItem textDocument,
      DiagnosticErrorReporter errorReporter,
      Dafny.Program program,
      bool loadCanceled
    ) {
      return new DafnyDocument(
        Options,
        textDocument,
        errorReporter.GetDiagnostics(textDocument.Uri),
        new List<Diagnostic>(),
        Array.Empty<Counterexample>(),
        Array.Empty<Diagnostic>(),
        program,
        CreateEmptySymbolTable(program, logger),
        loadCanceled
      );
    }

    private static SymbolTable CreateEmptySymbolTable(Dafny.Program program, ILogger<SymbolTable> logger) {
      return new SymbolTable(
        logger,
        new CompilationUnit(program),
        new Dictionary<object, ILocalizableSymbol>(),
        new Dictionary<ISymbol, SymbolLocation>(),
        new IntervalTree<Position, ILocalizableSymbol>(),
        symbolsResolved: false
      );
    }

    public IObservable<DafnyDocument> VerifyAsync(DafnyDocument document, CancellationToken cancellationToken) {
      notificationPublisher.SendStatusNotification(document.Text, CompilationStatus.VerificationStarted);
      var progressReporter = new VerificationProgressReporter(document.Text, notificationPublisher);
      var programErrorReporter = new DiagnosticErrorReporter(document.Uri);
      document.Program.Reporter = programErrorReporter;
      var implementationTasks = verifier.VerifyAsync(document.Program, progressReporter, cancellationToken);
      foreach (var implementationTask in implementationTasks) {
        implementationTask.Run();
      }

      Task.WhenAll(implementationTasks.Select(t => t.ActualTask)).ContinueWith(t => {
        logger.LogDebug($"Finished verification with {t.Result.Sum(r => r.Errors.Count)} errors.");
        var verified = t.Result.All(r => r.Outcome == ConditionGeneration.Outcome.Correct);
        var compilationStatusAfterVerification = verified
          ? CompilationStatus.VerificationSucceeded
          : CompilationStatus.VerificationFailed;
        notificationPublisher.SendStatusNotification(document.Text, compilationStatusAfterVerification);
      }, cancellationToken);

      var concurrentDictionary = new ConcurrentBag<Diagnostic>();
      var counterExamples = new ConcurrentStack<Counterexample>();
      var documentTasks = implementationTasks.Select(it => {
        return it.ActualTask.ContinueWith(t => {

          var errorReporter = new DiagnosticErrorReporter(document.Uri);
          foreach (var counterExample in t.Result.Errors) {
            counterExamples.Push(counterExample);
            errorReporter.ReportBoogieError(counterExample.CreateErrorInformation(t.Result.Outcome, Options.ForceBplErrors));
          }
          var outcomeError = t.Result.GetOutcomeError(Options);
          if (outcomeError != null) {
            errorReporter.ReportBoogieError(outcomeError);
          }
          foreach (var diagnostic in errorReporter.GetDiagnostics(document.Uri)) {
            concurrentDictionary.Add(diagnostic);
          }

          return document with {
            VerificationDiagnostics = concurrentDictionary.ToArray(),
            CounterExamples = counterExamples.ToArray(),
          };
        }, cancellationToken);
      });
      return documentTasks.Select(documentTask => documentTask.ToObservable()).Merge();
    }

    private record Request(CancellationToken CancellationToken) {
      public TaskCompletionSource<DafnyDocument> Document { get; } = new();
    }

    private class VerificationProgressReporter : IVerificationProgressReporter {
      private ICompilationStatusNotificationPublisher publisher { get; init; }
      private TextDocumentItem document { get; init; }

      public VerificationProgressReporter(TextDocumentItem document,
                                          ICompilationStatusNotificationPublisher publisher) {
        this.document = document;
        this.publisher = publisher;
      }

      public void ReportProgress(string message) {
        publisher.SendStatusNotification(document, CompilationStatus.VerificationStarted, message);
      }
    }
  }
}
