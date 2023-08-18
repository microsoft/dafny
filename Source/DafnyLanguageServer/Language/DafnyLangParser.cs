﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Dafny.LanguageServer.Workspace;

namespace Microsoft.Dafny.LanguageServer.Language {
  /// <summary>
  /// Parser implementation that makes use of the parse of dafny-lang. It may only be initialized exactly once since
  /// it requires initial setup of static members.
  /// </summary>
  /// <remarks>
  /// dafny-lang makes use of static members and assembly loading. Since thread-safety of this is not guaranteed,
  /// this parser serializes all invocations.
  /// </remarks>
  public sealed class DafnyLangParser : IDafnyParser {
    private readonly DafnyOptions options;
    private readonly IFileSystem fileSystem;
    private readonly ITelemetryPublisher telemetryPublisher;
    private readonly ILogger<DafnyLangParser> logger;
    private readonly SemaphoreSlim mutex = new(1);
    private readonly ProgramParser programParser;

    public DafnyLangParser(DafnyOptions options, IFileSystem fileSystem, ITelemetryPublisher telemetryPublisher,
      ILogger<DafnyLangParser> logger, ILogger<CachingParser> innerParserLogger) {
      this.options = options;
      this.fileSystem = fileSystem;
      this.telemetryPublisher = telemetryPublisher;
      this.logger = logger;
      programParser = options.Get(ServerCommand.UseCaching)
        ? new CachingParser(innerParserLogger, fileSystem, telemetryPublisher)
        : new ProgramParser(innerParserLogger, fileSystem);
    }

    private int concurrentParses;

    public Program Parse(Compilation compilation, ErrorReporter reporter, CancellationToken cancellationToken) {
      var current = Interlocked.Increment(ref concurrentParses);
      logger.LogDebug($"Concurrent parsers is {current}");
      try {
        try {
          mutex.Wait(cancellationToken);
        } catch (OperationCanceledException) {
          logger.LogInformation("Cancelled parsing before it began");
          throw;
        }
        var beforeParsing = DateTime.Now;
        try {
          var rootSourceUris = compilation.RootUris;
          List<DafnyFile> dafnyFiles = new();
          foreach (var rootSourceUri in rootSourceUris) {
            try {
              dafnyFiles.Add(new DafnyFile(reporter.Options, rootSourceUri, () => fileSystem.ReadFile(rootSourceUri)));
              if (logger.IsEnabled(LogLevel.Trace)) {
                logger.LogTrace(
                  $"Parsing file with uri {rootSourceUri} and content\n{fileSystem.ReadFile(rootSourceUri).ReadToEnd()}");
              }
            } catch (IOException) {
              logger.LogError($"Tried to parse file {rootSourceUri} that could not be found");
            }
          }

          return programParser.ParseFiles(compilation.Project.ProjectName, dafnyFiles, reporter, cancellationToken);
        }
        finally {
          telemetryPublisher.PublishTime("Parse", compilation.Project.Uri.ToString(), DateTime.Now - beforeParsing);
          mutex.Release();
        }

      }
      finally {
        Interlocked.Decrement(ref concurrentParses);
      }
    }
  }
}
