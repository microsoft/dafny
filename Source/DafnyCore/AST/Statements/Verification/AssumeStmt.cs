using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Dafny.Auditor;

namespace Microsoft.Dafny;

public class AssumeStmt : PredicateStmt, ICloneable<AssumeStmt>, ICanFormat {
  public AssumeStmt Clone(Cloner cloner) {
    return new AssumeStmt(cloner, this);
  }

  public AssumeStmt(Cloner cloner, AssumeStmt original) : base(cloner, original) {
  }

  public AssumeStmt(RangeToken rangeToken, Expression expr, Attributes attrs)
    : base(rangeToken, expr, attrs) {
    Contract.Requires(rangeToken != null);
    Contract.Requires(expr != null);
  }
  public override IEnumerable<Expression> SpecificationSubExpressions {
    get {
      foreach (var e in base.SpecificationSubExpressions) { yield return e; }
      yield return Expr;
    }
  }

  public override IEnumerable<Assumption> Assumptions(Declaration decl) {
    yield return new Assumption(decl, tok, AssumptionDescription.AssumeStatement(
      Attributes.Contains(Attributes, Attributes.AxiomAttributeName)));
  }

  public bool SetIndent(int indentBefore, TokenNewIndentCollector formatter) {
    return formatter.SetIndentAssertLikeStatement(this, indentBefore);
  }

  public override void Resolve(INewOrOldResolver resolver, ResolutionContext context) {
    base.Resolve(resolver, context);

    if (!resolver.Options.Get(CommonOptionBag.AllowAxioms) && !this.IsExplicitAxiom()) {
      resolver.Reporter.Warning(MessageSource.Resolver, ResolutionErrors.ErrorId.r_assume_statement_without_axiom, Tok, "assume statement has no {:axiom} annotation");
    }
  }
}
