using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Microsoft.Dafny;

/// <summary>
/// Represents "module name = path;", where name is an identifier and path is a possibly qualified name.
/// </summary>
public class AliasModuleDecl : OrderedModuleDecl, ICanFormat {
  public readonly ModuleQualifiedId TargetQId; // generated by the parser, this is looked up
  public readonly List<IToken> Exports; // list of exports sets
  [FilledInDuringResolution] public bool ShadowsLiteralModule;  // initialized early during Resolution (and used not long after that); true for "import opened A = A" where "A" is a literal module in the same scope

  public AliasModuleDecl(Cloner cloner, AliasModuleDecl original, ModuleDefinition parent)
    : base(cloner, original, parent) {
    if (original.TargetQId != null) { // TODO is this null check necessary?
      TargetQId = new ModuleQualifiedId(cloner, original.TargetQId);

      /*
       * Refinement cloning happens in PreResolver, which is after the ModuleQualifiedId.Root fields are set,
       * so this field must be copied as part of refinement cloning.
       * However, letting refinement cloning set CloneResolvedFields==true causes exceptions for an uninvestigated reason,
       * so we will clone this field even when !CloneResolvedFields.
       */
      TargetQId.Root = original.TargetQId.Root;
    }
    Exports = original.Exports;
  }

  public AliasModuleDecl(DafnyOptions options, RangeToken rangeToken, ModuleQualifiedId path, Name name,
    ModuleDefinition parent, bool opened, List<IToken> exports, Guid cloneId)
    : base(options, rangeToken, name, parent, opened, cloneId) {
    Contract.Requires(path != null && path.Path.Count > 0);
    Contract.Requires(exports != null);
    Contract.Requires(exports.Count == 0 || path.Path.Count == 1);
    TargetQId = path;
    Exports = exports;
  }

  public override ModuleDefinition Dereference() { return Signature.ModuleDef; }

  public bool SetIndent(int indentBefore, TokenNewIndentCollector formatter) {
    if (OwnedTokens.FirstOrDefault() is { } theToken) {
      formatter.SetOpeningIndentedRegion(theToken, indentBefore);
    }

    return true;
  }

  public override IEnumerable<INode> Children => base.Children.Concat(new INode[] { TargetQId });
}