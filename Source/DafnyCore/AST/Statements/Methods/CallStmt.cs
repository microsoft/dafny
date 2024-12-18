using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Microsoft.Dafny;

class OverrideCenter : OriginWrapper {
  public OverrideCenter(IOrigin wrappedToken, Token newCenter) : base(wrappedToken) {
    this.Center = newCenter;
  }

  public override Token Center { get; }

  public override IOrigin WithVal(string newVal) {
    throw new System.NotImplementedException();
  }

  public override int col {
    get => Center.col;
    set => throw new System.NotImplementedException();
  }

  public override int line {
    get => Center.line;
    set => throw new System.NotImplementedException();
  }

  public override int pos {
    get => Center.pos;
    set => throw new System.NotImplementedException();
  }

  public override string val {
    get => Center.val;
    set => throw new System.NotImplementedException();
  }

  public override string LeadingTrivia {
    get => Center.LeadingTrivia;
    set => throw new System.NotImplementedException();
  }

  public override string TrailingTrivia {
    get => Center.TrailingTrivia;
    set => throw new System.NotImplementedException();
  }

  public override Token Next {
    get => Center.Next;
    set => throw new System.NotImplementedException();
  }

  public override Token Prev {
    get => Center.Prev;
    set => throw new System.NotImplementedException();
  }
}

/// <summary>
/// A CallStmt is always resolved.  It is typically produced as a resolved counterpart of the syntactic AST note ApplySuffix.
/// </summary>
public class CallStmt : Statement, ICloneable<CallStmt> {

  [ContractInvariantMethod]
  void ObjectInvariant() {
    Contract.Invariant(MethodSelect.Member is Method);
    Contract.Invariant(cce.NonNullElements(Lhs));
    Contract.Invariant(cce.NonNullElements(Args));
  }

  public override IEnumerable<INode> Children => Lhs.Concat(new Node[] { MethodSelect, Bindings });
  public override IEnumerable<IdentifierExpr> GetAssignedLocals() {
    return Lhs.Select(lhs => lhs.Resolved).OfType<IdentifierExpr>();
  }

  public readonly List<Expression> Lhs;
  public readonly MemberSelectExpr MethodSelect;
  private readonly IOrigin overrideToken;
  public readonly ActualBindings Bindings;
  public List<Expression> Args => Bindings.Arguments;
  public Expression OriginalInitialLhs = null;

  public Expression Receiver => MethodSelect.Obj;
  public Method Method => (Method)MethodSelect.Member;

  public CallStmt(IOrigin rangeOrigin, List<Expression> lhs, MemberSelectExpr memSel, List<ActualBinding> args, Token overrideToken = null)
    : base(
      /* TODO remove */ new OverrideCenter(rangeOrigin, overrideToken ?? memSel.EndToken.Next)) {
    Contract.Requires(rangeOrigin != null);
    Contract.Requires(cce.NonNullElements(lhs));
    Contract.Requires(memSel != null);
    Contract.Requires(memSel.Member is Method);
    Contract.Requires(cce.NonNullElements(args));

    this.Lhs = lhs;
    this.MethodSelect = memSel;
    this.overrideToken = overrideToken;
    
    this.Bindings = new ActualBindings(args);
  }

  public CallStmt Clone(Cloner cloner) {
    return new CallStmt(cloner, this);
  }

  public CallStmt(Cloner cloner, CallStmt original) : base(cloner, original) {
    MethodSelect = (MemberSelectExpr)cloner.CloneExpr(original.MethodSelect);
    Lhs = original.Lhs.Select(cloner.CloneExpr).ToList();
    Bindings = new ActualBindings(cloner, original.Bindings);
    overrideToken = original.overrideToken;
  }

  /// <summary>
  /// This constructor is intended to be used when constructing a resolved CallStmt. The "args" are expected
  /// to be already resolved, and are all given positionally.
  /// </summary>
  public CallStmt(IOrigin rangeOrigin, List<Expression> lhs, MemberSelectExpr memSel, List<Expression> args)
    : this(rangeOrigin, lhs, memSel, args.ConvertAll(e => new ActualBinding(null, e))) {
    Bindings.AcceptArgumentExpressionsAsExactParameterList();
  }

  public override IEnumerable<Expression> NonSpecificationSubExpressions {
    get {
      foreach (var e in base.NonSpecificationSubExpressions) { yield return e; }
      foreach (var ee in Lhs) {
        yield return ee;
      }
      yield return MethodSelect;
      foreach (var ee in Args) {
        yield return ee;
      }
    }
  }

  public override void ResolveGhostness(ModuleResolver resolver, ErrorReporter reporter, bool mustBeErasable,
    ICodeContext codeContext, string proofContext,
    bool allowAssumptionVariables, bool inConstructorInitializationPhase) {
    var callee = this.Method;
    Contract.Assert(callee != null);  // follows from the invariant of CallStmt
    this.IsGhost = callee.IsGhost;
    if (proofContext != null && !callee.IsLemmaLike) {
      reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_no_calls_in_proof, this,
        $"in {proofContext}, calls are allowed only to lemmas");
    } else if (mustBeErasable) {
      if (!this.IsGhost) {
        reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_only_ghost_calls, this,
          "only ghost methods can be called from this context");
      }
    } else {
      int j;
      if (!callee.IsGhost) {
        // check in-parameters
        ExpressionTester.CheckIsCompilable(resolver, reporter, this.Receiver, codeContext);
        j = 0;
        foreach (var e in this.Args) {
          Contract.Assume(j < callee.Ins.Count);  // this should have already been checked by the resolver
          if (!callee.Ins[j].IsGhost) {
            ExpressionTester.CheckIsCompilable(resolver, reporter, e, codeContext);
          }
          j++;
        }
      }
      j = 0;
      foreach (var e in this.Lhs) {
        var resolvedLhs = e.Resolved;
        if (callee.IsGhost || callee.Outs[j].IsGhost) {
          // LHS must denote a ghost
          if (resolvedLhs is IdentifierExpr) {
            var ll = (IdentifierExpr)resolvedLhs;
            if (!ll.Var.IsGhost) {
              if (ll is AutoGhostIdentifierExpr && ll.Var is LocalVariable) {
                // the variable was actually declared in this statement, so auto-declare it as ghost
                ((LocalVariable)ll.Var).MakeGhost();
              } else {
                reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_out_parameter_must_be_ghost, this,
                  "actual out-parameter{0} is required to be a ghost variable", this.Lhs.Count == 1 ? "" : " " + j);
              }
            }
          } else if (resolvedLhs is MemberSelectExpr) {
            var ll = (MemberSelectExpr)resolvedLhs;
            if (!ll.Member.IsGhost) {
              reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_out_parameter_must_be_ghost_field, this,
                "actual out-parameter{0} is required to be a ghost field", this.Lhs.Count == 1 ? "" : " " + j);
            }
          } else {
            // this is an array update, and arrays are always non-ghost
            reporter.Error(MessageSource.Resolver, ResolutionErrors.ErrorId.r_out_parameter_must_be_ghost, this,
              "actual out-parameter{0} is required to be a ghost variable", this.Lhs.Count == 1 ? "" : " " + j);
          }
        }
        j++;
      }
    }
  }
}
