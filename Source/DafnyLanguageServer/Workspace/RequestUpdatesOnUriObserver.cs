using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Boogie;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.Dafny.LanguageServer.Workspace;

class RequestUpdatesOnUriObserver : IObserver<IObservable<DafnyDocument>>, IDisposable {
  private readonly MergeOrdered<DafnyDocument> mergeOrdered;
  private readonly IDisposable subscription;
  private readonly DiagnosticsObserver observer;

  public DafnyDocument PreviouslyPublishedDocument => observer.PreviouslyPublishedDocument;

  public RequestUpdatesOnUriObserver(
    ILogger logger,
    ITelemetryPublisher telemetryPublisher,
    INotificationPublisher notificationPublisher,
    DocumentTextBuffer document) {

    mergeOrdered = new MergeOrdered<DafnyDocument>();
    observer = new DiagnosticsObserver(logger, telemetryPublisher, notificationPublisher, document);
    subscription = mergeOrdered.Subscribe(observer);
  }

  public IObservable<bool> IdleChangesIncludingLast => mergeOrdered.IdleChangesIncludingLast;

  public void Dispose() {
    subscription.Dispose();
  }

  public void OnCompleted() {
    mergeOrdered.OnCompleted();
  }

  public void OnError(Exception error) {
    mergeOrdered.OnError(error);
  }

  public void OnNext(IObservable<DafnyDocument> value) {
    mergeOrdered.OnNext(value);
  }
}

class DiagnosticsObserver : IObserver<DafnyDocument> {
  private readonly ILogger logger;
  private readonly ITelemetryPublisher telemetryPublisher;
  private readonly INotificationPublisher notificationPublisher;
  public DafnyDocument PreviouslyPublishedDocument { get; private set; }

  public DiagnosticsObserver(ILogger logger, ITelemetryPublisher telemetryPublisher,
    INotificationPublisher notificationPublisher, DocumentTextBuffer document) {
    PreviouslyPublishedDocument = new DafnyDocument(document,
      new [] { new Diagnostic {
        // This diagnostic never gets sent to the client,
        // instead it forces the first computed diagnostics for a document to always be sent.
        // The message here describes the implicit client state before the first diagnostics have been sent.
        Message = "Resolution diagnostics have not been computed yet."
      }},
      false,
      new Dictionary<ImplementationId, ImplementationView>(),
      Array.Empty<Counterexample>(),
      Array.Empty<Diagnostic>(),
#pragma warning disable CS8625
      null,
      null,
#pragma warning restore CS8625
      null,
      true);
    this.logger = logger;
    this.telemetryPublisher = telemetryPublisher;
    this.notificationPublisher = notificationPublisher;
  }

  public void OnCompleted() {
    telemetryPublisher.PublishUpdateComplete();
  }

  public void OnError(Exception e) {
    if (e is TaskCanceledException) {
      OnCompleted();
    } else {
      logger.LogError(e, "error while handling document event");
      telemetryPublisher.PublishUnhandledException(e);
    }
  }

  public void OnNext(DafnyDocument document) {
    notificationPublisher.PublishNotifications(PreviouslyPublishedDocument, document);
    PreviouslyPublishedDocument = document;
  }
}