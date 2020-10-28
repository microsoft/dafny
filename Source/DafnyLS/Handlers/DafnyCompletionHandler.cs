﻿using DafnyLS.Language;
using DafnyLS.Language.Symbols;
using DafnyLS.Util;
using DafnyLS.Workspace;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DafnyLS.Handlers {
  public class DafnyCompletionHandler : CompletionHandler {
    private readonly ILogger _logger;
    private readonly IDocumentDatabase _documents;
    private readonly ISymbolGuesser _symbolGuesser;

    public DafnyCompletionHandler(ILogger<DafnyDefinitionHandler> logger, IDocumentDatabase documents, ISymbolGuesser symbolGuesser) : base(CreateRegistrationOptions()) {
      _logger = logger;
      _documents = documents;
      _symbolGuesser = symbolGuesser;
    }

    private static CompletionRegistrationOptions CreateRegistrationOptions() {
      return new CompletionRegistrationOptions {
        DocumentSelector = DocumentSelector.ForLanguage("dafny"),
        ResolveProvider = false,
        TriggerCharacters = new Container<string>(".")
      };
    }

    // TODO when is this method called and for what?
    public override bool CanResolve(CompletionItem completionItem) {
      return false;
    }

    // TODO when is this method called and for what?
    public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken) {
      return Task.FromResult(request);
    }

    public override Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken) {
      DafnyDocument? document;
      if(!_documents.TryGetDocument(request.TextDocument, out document)) {
        _logger.LogWarning("location requested for unloaded document {}", request.TextDocument.Uri);
        return Task.FromResult(new CompletionList());
      }
      return Task.FromResult(new CompletionProcessor(_symbolGuesser, document, request, cancellationToken).Process());
    }

    private class CompletionProcessor {
      private readonly ISymbolGuesser _symbolGuesser;
      private readonly DafnyDocument _document;
      private readonly CompletionParams _request;
      private readonly CancellationToken _cancellationToken;

      public CompletionProcessor(ISymbolGuesser symbolGuesser, DafnyDocument document, CompletionParams request, CancellationToken cancellationToken) {
        _symbolGuesser = symbolGuesser;
        _document = document;
        _request = request;
        _cancellationToken = cancellationToken;
      }

      public CompletionList Process() {
        if(GetTriggerCharacter() == ".") {
          return CreateDotCompletionList();
        }
        return new CompletionList();
      }

      private string GetTriggerCharacter() {
        // Cannot use _request.Context.TriggerCharacter at this time, since _request.Context appears to be always null.
        var documentText = _document.Text.Text;
        int absolutePosition = _request.Position.ToAbsolutePosition(documentText, _cancellationToken) - 1;
        return documentText[absolutePosition].ToString();
      }

      private CompletionList CreateDotCompletionList() {
        IEnumerable<ISymbol> members;
        if(_symbolGuesser.TryGetTypeBefore(_document, GetDotPosition(), _cancellationToken, out var typeSymbol)) {
          // TODO Introduce a specialized symbol interface for types. At this time, the most types are treated as a UserDefinedType => class.
          if(typeSymbol is ClassSymbol classSymbol) {
            members = classSymbol.Members;
          } else {
            // TODO This should never happen at this time.
            throw new InvalidOperationException($"received a type symbol of type {typeSymbol.GetType()}, but expected a ClassSymbol");
          }
        } else {
          members = Enumerable.Empty<ISymbol>();
        }
        return CreateCompletionListFromSymbols(members);
      }

      private Position GetDotPosition() {
        return new Position(_request.Position.Line, _request.Position.Character - 1);
      }

      private CompletionList CreateCompletionListFromSymbols(IEnumerable<ISymbol> symbols) {
        var completionItems = symbols.WithCancellation(_cancellationToken)
          .Where(symbol => !IsConstructor(symbol))
          .Select(CreateCompletionItem)
          .ToArray();
        return new CompletionList(completionItems);
      }

      private bool IsConstructor(ISymbol symbol) {
        return symbol is MethodSymbol method
          && method.Identifier == "_ctor";
      }

      private CompletionItem CreateCompletionItem(ISymbol symbol) {
        return new CompletionItem {
          Label = symbol.Identifier,
          Kind = GetCompletionKind(symbol),
          InsertText = GetCompletionText(symbol),
          Detail = (symbol as ILocalizableSymbol)?.GetDetailText(_cancellationToken)
        };
      }

      private CompletionItemKind GetCompletionKind(ISymbol symbol) {
        return symbol switch {
          ClassSymbol _ => CompletionItemKind.Class,
          MethodSymbol _ => CompletionItemKind.Method,
          FunctionSymbol _ => CompletionItemKind.Function,
          VariableSymbol _ => CompletionItemKind.Variable,
          FieldSymbol _ => CompletionItemKind.Field,
          _ => CompletionItemKind.Reference
        };
      }

      private string GetCompletionText(ISymbol symbol) {
        return symbol switch {
          MethodSymbol _ => $"{symbol.Identifier}(",
          FunctionSymbol _ => $"{symbol.Identifier}(",
          _ => symbol.Identifier
        };
      }
    }
  }
}
