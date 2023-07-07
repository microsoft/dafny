using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using IntervalTree;
using Microsoft.Boogie;
using Microsoft.Dafny.LanguageServer.Language;
using Microsoft.Dafny.LanguageServer.Workspace.ChangeProcessors;
using Microsoft.Dafny.LanguageServer.Workspace.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.Dafny.LanguageServer.Workspace;

/*
 * Ideal view is if there is a single Global IdeState,
 * Compilations may then update only part of this global state, the part for which they have affecting Uris.
 * 
 */

public record FilePosition(Uri Uri, Position Position);

// TODO test that it does not send diagnostics for files that it does not own.
// How do you determine ownership when two projects include the same unopened file?
public class ProjectManager : IDisposable {
  private readonly IRelocator relocator;

  private readonly IServiceProvider services;
  private readonly VerificationResultCache verificationCache;
  public DafnyProject Project { get; }
  private readonly IdeStateObserver observer;
  public CompilationManager CompilationManager { get; private set; }
  private IDisposable observerSubscription;
  private readonly ILogger<ProjectManager> logger;
  private readonly ExecutionEngine boogieEngine;

  /// <summary>
  /// The version of this project.
  /// Is incremented when any file in the project is updated.
  /// Is used as part of project-wide notifications.
  /// Can be used by the client to ignore outdated notifications
  /// </summary>
  private int version;
  public int OpenFileCount { get; private set; }

  private bool VerifyOnOpenChange => options.Get(ServerCommand.Verification) == VerifyOnMode.Change;
  private bool VerifyOnSave => options.Get(ServerCommand.Verification) == VerifyOnMode.Save;
  public List<FilePosition> ChangedVerifiables { get; set; } = new();
  public List<Location> RecentChanges { get; set; } = new();

  private readonly SemaphoreSlim workCompletedForCurrentVersion = new(1);
  private readonly DafnyOptions options;
  private readonly DafnyOptions serverOptions;

#pragma warning disable CS8618
  public ProjectManager(IServiceProvider services, VerificationResultCache verificationCache, DafnyProject project) {
#pragma warning restore CS8618
    this.services = services;
    this.verificationCache = verificationCache;
    Project = project;
    serverOptions = services.GetRequiredService<DafnyOptions>();
    logger = services.GetRequiredService<ILogger<ProjectManager>>();
    relocator = services.GetRequiredService<IRelocator>();
    services.GetRequiredService<IFileSystem>();

    Project = project;
    options = DetermineProjectOptions(project, serverOptions);
    observer = new IdeStateObserver(this.services.GetRequiredService<ILogger<IdeStateObserver>>(),
      this.services.GetRequiredService<ITelemetryPublisher>(),
      this.services.GetRequiredService<INotificationPublisher>(),
      this.services.GetRequiredService<ITextDocumentLoader>(),
      project);
    boogieEngine = new ExecutionEngine(options, this.verificationCache);
    options.Printer = new OutputLogger(logger);

    CompilationManager = new CompilationManager(
      this.services,
      options,
      boogieEngine,
      new Compilation(version, Project),
      null
    );

    observerSubscription = Disposable.Empty;
  }

  private const int MaxRememberedChanges = 100;
  private const int MaxRememberedChangedVerifiables = 5;

  public void UpdateDocument(DidChangeTextDocumentParams documentChange) {
    var lastPublishedState = observer.LastPublishedState;
    var migratedVerificationTree = lastPublishedState.VerificationTree == null ? null
      : relocator.RelocateVerificationTree(lastPublishedState.VerificationTree, documentChange, CancellationToken.None);
    lastPublishedState = lastPublishedState with {
      ImplementationIdToView = MigrateImplementationViews(documentChange, lastPublishedState.ImplementationIdToView),
      SignatureAndCompletionTable = relocator.RelocateSymbols(lastPublishedState.SignatureAndCompletionTable, documentChange, CancellationToken.None),
      VerificationTree = migratedVerificationTree
    };

    lock (RecentChanges) {
      var newChanges = documentChange.ContentChanges.Where(c => c.Range != null).
        Select(contentChange => new Location {
          Range = contentChange.Range!,
          Uri = documentChange.TextDocument.Uri
        });
      var migratedChanges = RecentChanges.Select(location => {
        var newRange = relocator.RelocateRange(location.Range, documentChange, CancellationToken.None);
        if (newRange == null) {
          return null;
        }
        return new Location {
          Range = newRange,
          Uri = location.Uri
        };
      }).Where(r => r != null);
      RecentChanges = newChanges.Concat(migratedChanges).Take(MaxRememberedChanges).ToList()!;
    }

    StartNewCompilation(migratedVerificationTree, lastPublishedState);
    TriggerVerificationForFile(documentChange.TextDocument.Uri.ToUri());
  }

  private void StartNewCompilation(VerificationTree? migratedVerificationTree,
    IdeState lastPublishedState) {
    version++;
    logger.LogDebug("Clearing result for workCompletedForCurrentVersion");

    CompilationManager.CancelPendingUpdates();
    CompilationManager = new CompilationManager(
      services,
      options,
      boogieEngine,
      new Compilation(version, Project),
      // TODO do not pass this to CompilationManager but instead use it in FillMissingStateUsingLastPublishedDocument
      migratedVerificationTree
    );

    observerSubscription.Dispose();
    var migratedUpdates = CompilationManager.CompilationUpdates.Select(document =>
      document.ToIdeState(lastPublishedState));
    observerSubscription = migratedUpdates.Subscribe(observer);

    CompilationManager.Start();
  }


  public void TriggerVerificationForFile(Uri triggeringFile) {
    if (VerifyOnOpenChange) {
      var _ = VerifyEverythingAsync(null);
    } else {
      logger.LogDebug("Setting result for workCompletedForCurrentVersion");
    }
  }

  private static DafnyOptions DetermineProjectOptions(DafnyProject projectOptions, DafnyOptions serverOptions) {
    var result = new DafnyOptions(serverOptions);

    foreach (var option in ServerCommand.Instance.Options) {
      var hasProjectFileValue = projectOptions.TryGetValue(option, TextWriter.Null, out var projectFileValue);
      if (hasProjectFileValue) {
        result.Options.OptionArguments[option] = projectFileValue;
        result.ApplyBinding(option);
      }
    }

    return result;
  }

  private Dictionary<ImplementationId, IdeImplementationView> MigrateImplementationViews(DidChangeTextDocumentParams documentChange,
    IReadOnlyDictionary<ImplementationId, IdeImplementationView> oldVerificationDiagnostics) {
    var result = new Dictionary<ImplementationId, IdeImplementationView>();
    foreach (var entry in oldVerificationDiagnostics) {
      var newRange = relocator.RelocateRange(entry.Value.Range, documentChange, CancellationToken.None);
      if (newRange != null) {
        result.Add(entry.Key with {
          Position = relocator.RelocatePosition(entry.Key.Position, documentChange, CancellationToken.None)
        }, entry.Value with {
          Range = newRange,
          Diagnostics = relocator.RelocateDiagnostics(entry.Value.Diagnostics, documentChange, CancellationToken.None)
        });
      }
    }
    return result;
  }

  public void Save(TextDocumentIdentifier documentId) {
    if (VerifyOnSave) {
      logger.LogDebug("Clearing result for workCompletedForCurrentVersion");
      var _2 = VerifyEverythingAsync(documentId.Uri.ToUri());
    }
  }

  public async Task<bool> CloseDocument() {
    OpenFileCount--;
    if (OpenFileCount == 0) {
      await CloseAsync();
      return true;
    }

    return false;
  }

  public async Task CloseAsync() {
    CompilationManager.CancelPendingUpdates();
    try {
      await CompilationManager.LastDocument;
      observer.OnCompleted();
    } catch (OperationCanceledException) {
    }
  }

  public async Task<CompilationAfterParsing> GetLastDocumentAsync() {
    await workCompletedForCurrentVersion.WaitAsync();
    workCompletedForCurrentVersion.Release();
    return await CompilationManager.LastDocument;
  }

  public async Task<IdeState> GetSnapshotAfterResolutionAsync() {
    try {
      var resolvedCompilation = await CompilationManager.ResolvedCompilation;
      logger.LogDebug($"GetSnapshotAfterResolutionAsync, resolvedDocument.Version = {resolvedCompilation.Version}, " +
                      $"observer.LastPublishedState.Version = {observer.LastPublishedState.Compilation.Version}, threadId: {Thread.CurrentThread.ManagedThreadId}");
    } catch (OperationCanceledException) {
      logger.LogDebug("Caught OperationCanceledException in GetSnapshotAfterResolutionAsync");
    }

    return observer.LastPublishedState;
  }

  public async Task<IdeState> GetIdeStateAfterVerificationAsync() {
    try {
      await GetLastDocumentAsync();
    } catch (OperationCanceledException) {
    }

    return observer.LastPublishedState;
  }

  // Test that when a project has multiple files, when saving/opening, only the affected Uri is verified when using OnSave.
  // Test that when a project has multiple files, everything is verified on opening one of them.
  private async Task VerifyEverythingAsync(Uri? uri) {
    var _1 = workCompletedForCurrentVersion.WaitAsync();
    try {
      var translatedDocument = await CompilationManager.TranslatedCompilation;

      var implementationTasks = translatedDocument.VerificationTasks;
      if (uri != null) {
        implementationTasks = implementationTasks.Where(d => ((IToken)d.Implementation.tok).Uri == uri).ToList(); ;
      }

      if (!implementationTasks.Any()) {
        // This doesn't work like normal??? 
        // What should change about CompilationManager.verificationCompleted
        CompilationManager.FinishedNotifications(translatedDocument);
      }

      lock (RecentChanges) {
        var freshlyChangedVerifiables = GetChangedVerifiablesFromRanges(translatedDocument, RecentChanges);
        ChangedVerifiables = freshlyChangedVerifiables.Concat(ChangedVerifiables).Distinct()
          .Take(MaxRememberedChangedVerifiables).ToList();
        RecentChanges = new List<Location>();
      }

      var implementationOrder = ChangedVerifiables.Select((v, i) => (v, i)).ToDictionary(k => k.v, k => k.i);
      var orderedTasks = implementationTasks.OrderBy(t => t.Implementation.Priority).CreateOrderedEnumerable(
        t => implementationOrder.GetOrDefault(new FilePosition(((IToken)t.Implementation.tok).Uri, t.Implementation.tok.GetLspPosition()), () => int.MaxValue),
        null, false).ToList();

      foreach (var implementationTask in orderedTasks) {
        CompilationManager.VerifyTask(translatedDocument, implementationTask);
      }
    }
    finally {
      logger.LogDebug("Setting result for workCompletedForCurrentVersion");
      workCompletedForCurrentVersion.Release();
    }
  }

  private IEnumerable<FilePosition> GetChangedVerifiablesFromRanges(CompilationAfterTranslation translated, IEnumerable<Location> changedRanges) {

    IntervalTree<Position, Position> GetTree(Uri uri) {
      // Refactor: use the translated Boogie program
      // instead of redoing part of that translation with the `DocumentVerificationTree` 
      // https://github.com/dafny-lang/dafny/issues/4264
      var tree = new DocumentVerificationTree(translated.Program, uri);
      VerificationProgressReporter.UpdateTree(options, translated, tree);
      var intervalTree = new IntervalTree<Position, Position>();
      foreach (var childTree in tree.Children) {
        intervalTree.Add(childTree.Range.Start, childTree.Range.End, childTree.Position);
      }

      return intervalTree;
    }

    Dictionary<Uri, IntervalTree<Position, Position>> trees = new();

    return changedRanges.SelectMany(changeRange => {
      var tree = trees.GetOrCreate(changeRange.Uri.ToUri(), () => GetTree(changeRange.Uri.ToUri()));
      return tree.Query(changeRange.Range.Start, changeRange.Range.End).Select(position => new FilePosition(changeRange.Uri.ToUri(), position));
    });
  }

  public void OpenDocument(Uri uri) {
    OpenFileCount++;
    var lastPublishedState = observer.LastPublishedState;
    var migratedVerificationTree = lastPublishedState.VerificationTree;

    StartNewCompilation(migratedVerificationTree, lastPublishedState);
    TriggerVerificationForFile(uri);
  }

  public void Dispose() {
    boogieEngine.Dispose();
  }
}
