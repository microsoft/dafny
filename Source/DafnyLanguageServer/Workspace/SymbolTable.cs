using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using IntervalTree;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace Microsoft.Dafny.LanguageServer.Workspace;

public class SymbolTable {

  public static SymbolTable CreateFrom(ILogger logger, Program program, CancellationToken cancellationToken) {
    var visited = program.Visit(a => true, b => { });

    var usages = visited.OfType<IHasReferences>().Where(v => !AutoGeneratedToken.Is(v.NameToken))
      .SelectMany(r => {
        var usages = r.GetReferences();
        if (usages == null) {
          logger.LogError($"Value of type {r.GetType()} returned a null for ${nameof(IHasReferences.GetReferences)}");
          usages = Array.Empty<IDeclarationOrUsage>();
        }
        return usages.Where(d => d != null).Select(declaration =>
          ((IDeclarationOrUsage)r, declaration));
      }).ToList();

    var relevantDafnySymbolKinds = new HashSet<SymbolKind> {
      SymbolKind.Function,
      SymbolKind.Class,
      SymbolKind.Enum,
      SymbolKind.Method,
      SymbolKind.EnumMember,
      SymbolKind.Struct,
      SymbolKind.Interface,
      SymbolKind.Namespace,
    };
    // Since these definitions are checked for whether they
    // contain substrings when answering workspace/resolve queries,
    // performance can be improved by storing their names in a
    // data structure that makes this operation cheaper, such as
    // a suffix tree.
    var definitions = visited
      .OfType<ISymbol>()
      .Where(symbol => symbol.Kind.HasValue && relevantDafnySymbolKinds.Contains(symbol.Kind.Value))
      .ToImmutableList();

    return new SymbolTable(usages, definitions);
  }

  public static SymbolTable Empty() {
    return new SymbolTable();
  }

  private SymbolTable() {
    DeclarationToUsages = ImmutableDictionary<IDeclarationOrUsage, ISet<IDeclarationOrUsage>>.Empty;
    UsageToDeclaration = ImmutableDictionary<IDeclarationOrUsage, IDeclarationOrUsage>.Empty;
    Definitions = ImmutableList<ISymbol>.Empty;
  }

  public SymbolTable(IReadOnlyList<(IDeclarationOrUsage usage, IDeclarationOrUsage reference)> usages, ImmutableList<ISymbol> definitions) {
    var safeUsages1 = usages.Where(k => k.usage.NameToken.Uri != null).ToImmutableList();
    var safeUsages = usages.Where(k => k.usage.NameToken.Uri != null && k.reference.NameToken.Uri != null).ToImmutableList();

    var usageDeclarations = safeUsages1.Select(k => KeyValuePair.Create(k.usage, k.reference));
    var selfDeclarations = safeUsages1.Select(k => KeyValuePair.Create(k.reference, k.reference));
    UsageToDeclaration = usageDeclarations.Concat(selfDeclarations).DistinctBy(pair => pair.Key).ToImmutableDictionary();

    DeclarationToUsages = safeUsages1.GroupBy(u => u.reference).ToImmutableDictionary(
      g => g.Key,
      g => (ISet<IDeclarationOrUsage>)g.Select(k => k.usage).ToHashSet());

    Definitions = definitions;

    var symbols = safeUsages.Select(u => u.reference).
      Concat(safeUsages.Select(u => u.usage)).
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

  /// <summary>
  /// Maps each symbol declaration to itself, and each symbol usage to the symbol's declaration.
  /// </summary>
  public ImmutableDictionary<IDeclarationOrUsage, IDeclarationOrUsage> UsageToDeclaration { get; }

  /// <summary>
  /// Maps each symbol declaration to usages of the symbol, not including the declaration itself.
  /// </summary>
  public ImmutableDictionary<IDeclarationOrUsage, ISet<IDeclarationOrUsage>> DeclarationToUsages { get; }

  /// <summary>
  ///  A list of all definitions, such as methods, classes, functions, etc., used for workspace-wide symbol
  /// lookup.
  /// </summary>
  public ImmutableList<ISymbol> Definitions { get; }

  public IEnumerable<Location> GetUsages(Uri uri, Position position) {
    if (nodePositions.TryGetValue(uri, out var forFile)) {
      return forFile.Query(position).
        SelectMany(node => DeclarationToUsages.GetOrDefault(node, () => (ISet<IDeclarationOrUsage>)new HashSet<IDeclarationOrUsage>())).
        Select(u => new Location { Uri = u.NameToken.Filepath, Range = u.NameToken.GetLspRange() });
    }
    return Enumerable.Empty<Location>();
  }

  public Location? GetDeclaration(Uri uri, Position position) {
    var node = GetNode(uri, position);
    return node == null ? null : NodeToLocation(node);
  }

  internal static Location NodeToLocation(IDeclarationOrUsage node) {
    return new Location {
      Uri = DocumentUri.From(node.NameToken.Uri),
      Range = node.NameToken.GetLspRange()
    };
  }

  public IDeclarationOrUsage? GetNode(Uri uri, Position position) {
    if (!nodePositions.TryGetValue(uri, out var forFile)) {
      return null;
    }
    return forFile.Query(position)
      .Select(node => UsageToDeclaration.GetOrDefault(node, () => (IDeclarationOrUsage?)null))
      .FirstOrDefault(x => x != null);
  }
}
