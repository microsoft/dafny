using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using IntervalTree;
using Microsoft.Boogie;
using Microsoft.Dafny.LanguageServer.Language;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace Microsoft.Dafny.LanguageServer.Workspace;

public class SymbolTable {

  public static SymbolTable Empty() {
    return new SymbolTable();
  }

  private SymbolTable() {
    Usages = ImmutableDictionary<IDeclarationOrUsage, ISet<IDeclarationOrUsage>>.Empty;
    Declarations = ImmutableDictionary<IDeclarationOrUsage, IDeclarationOrUsage>.Empty;
  }

      return result;
    });
  }

  public SymbolTable(IReadOnlyList<(IDeclarationOrUsage usage, IDeclarationOrUsage declaration)> usages) {
    var safeUsages = usages.Where(k => k.declaration.NameToken.Uri != null).ToList();
    Declarations = safeUsages.DistinctBy(k => k.usage).
      ToImmutableDictionary(k => k.usage, k => k.declaration);
    Usages = safeUsages.GroupBy(u => u.declaration).ToImmutableDictionary(
      g => g.Key,
      g => (ISet<IDeclarationOrUsage>)g.Select(k => k.usage).ToHashSet());
    
    var symbols = safeUsages.Select(u => u.declaration).
      Concat(usages.Select(u => u.usage)).
      Where(s => !AutoGeneratedToken.Is(s.NameToken));
    var symbolsByFile = symbols.GroupBy(s => s.NameToken.Uri);
    foreach (var symbolsForFile in symbolsByFile) {
      var nodePositions = new IntervalTree<Position, IDeclarationOrUsage>();
      this.nodePositions.Add(symbolsForFile.Key, nodePositions);
      foreach (var symbolForFile in symbolsForFile) {
        var range = symbolForFile.NameToken.GetLspRange();
        nodePositions.Add(range.Start, range.End, symbolForFile);
      }
    }
  }

  private readonly Dictionary<Uri, IIntervalTree<Position, IDeclarationOrUsage>> nodePositions = new();
  private ImmutableDictionary<IDeclarationOrUsage, IDeclarationOrUsage> Declarations { get; }
  private ImmutableDictionary<IDeclarationOrUsage, ISet<IDeclarationOrUsage>> Usages { get; }

  public ISet<Location> GetUsages(Uri uri, Position position) {
    if (nodePositions.TryGetValue(uri, out var forFile)) {
      return forFile.Query(position).
        SelectMany(node => Usages.GetOrDefault(node, () => (ISet<IDeclarationOrUsage>)new HashSet<IDeclarationOrUsage>())).
        Select(u => new Location { Uri = u.NameToken.Filepath, Range = u.NameToken.GetLspRange() }).ToHashSet();
    }
    return Sets.Empty<Location>();
  }

  public Location? GetDeclaration(Uri uri, Position position) {
    if (!nodePositions.TryGetValue(uri, out var forFile)) {
      return null;
    }

    var referenceNodes = forFile.Query(position);
    return referenceNodes.Select(node => Declarations.GetOrDefault(node, () => (IDeclarationOrUsage?)null))
      .Where(x => x != null).Select(
        n => new Location {
          Uri = DocumentUri.From(n!.NameToken.Uri),
          Range = n.NameToken.GetLspRange()
        }).FirstOrDefault();
  }
}