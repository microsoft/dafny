using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Microsoft.Dafny;

/// <summary>
/// Fills the field ForallStmt.EffectiveEnsuresClauses
/// </summary>
public class ForallStmtRewriter : IRewriter {
  public ForallStmtRewriter(ErrorReporter reporter) : base(reporter) {
    Contract.Requires(reporter != null);
  }

  internal override void PostResolveIntermediate(ModuleDefinition m) {
    var forallVisitor = new ForAllStmtVisitor(Reporter);
    foreach (var decl in ModuleDefinition.AllCallablesIncludingPrefixDeclarations(m.TopLevelDecls)) {
      forallVisitor.Visit(decl, true);
    }
  }

  internal class ForAllStmtVisitor : TopDownVisitor<bool> {
    readonly ErrorReporter reporter;
    public ForAllStmtVisitor(ErrorReporter reporter) {
      Contract.Requires(reporter != null);
      this.reporter = reporter;
    }
    protected override bool VisitOneStmt(Statement stmt, ref bool st) {
      if (stmt is not ForallStmt { CanConvert: true } forallStmt) {
        return true; //visit the sub-parts with the same "st"
      }

      switch (forallStmt.Kind) {
        case ForallStmt.BodyKind.Proof:
          VisitProof(forallStmt);
          break;
        case ForallStmt.BodyKind.Assign:
          VisitAssign(forallStmt);
          break;
        case ForallStmt.BodyKind.Call:
          VisitCall(forallStmt);
          break;
        default:
          Contract.Assert(false);  // unexpected kind
          break;
      }
      return true;  //visit the sub-parts with the same "st"
    }

    private static void VisitCall(ForallStmt s) {
      var s0 = (CallStmt)s.S0;
      var argsSubstMap = new Dictionary<IVariable, Expression>(); // maps formal arguments to actuals
      Contract.Assert(s0.Method.Ins.Count == s0.Args.Count);
      for (int i = 0; i < s0.Method.Ins.Count; i++) {
        argsSubstMap.Add(s0.Method.Ins[i], s0.Args[i]);
      }

      var substituter = new AlphaConvertingSubstituter(s0.Receiver, argsSubstMap,
        s0.MethodSelect.TypeArgumentSubstitutionsWithParents());
      // Strengthen the range of the "forall" statement with the precondition of the call, suitably substituted with the actual parameters.
      if (Attributes.Contains(s.Attributes, "_autorequires")) {
        var range = s.Range;
        foreach (var req in s0.Method.Req) {
          var p = substituter.Substitute(req.E); // substitute the call's actuals for the method's formals
          range = Expression.CreateAnd(range, p);
        }

        s.Range = range;
      }

      // substitute the call's actuals for the method's formals
      var term = s0.Method.Ens.Count != 0
        ? substituter.Substitute(s0.Method.Ens[0].E)
        : Expression.CreateBoolLiteral(new AutoGeneratedOrigin(s.Tok), true);
      for (int i = 1; i < s0.Method.Ens.Count; i++) {
        term = new BinaryExpr(s0.Method.Ens[i].Tok, BinaryExpr.ResolvedOpcode.And, term,
          substituter.Substitute(s0.Method.Ens[i].E));
      }

      var expr = new ForallExpr(new AutoGeneratedOrigin(s.Tok), s.Origin, s.BoundVars, s.Range, term, s.Attributes) {
        Type = Type.Bool, // resolve here
        Bounds = s.Bounds
      };
      s.EffectiveEnsuresClauses = new List<Expression> { expr };
    }

    private void VisitAssign(ForallStmt stmt) {
      if (stmt.BoundVars.Count == 0) {
        return;
      }

      var s0 = (SingleAssignStmt)stmt.S0;
      if (s0.Rhs is not ExprRhs exprRhs) {
        return;
      }

      List<Expression> exprList = new List<Expression>();
      Expression Fi = null;
      Func<Expression, Expression> lhsBuilder = null;
      var lhs = s0.Lhs.Resolved;
      var i = stmt.BoundVars[0];
      if (stmt.BoundVars.Count == 1) {
        // Detect the following cases:
        //   0: forall i | R(i) { F(i).f := E(i); }
        //   1: forall i | R(i) { A[F(i)] := E(i); }
        //   2: forall i | R(i) { F(i)[N] := E(i); }
        switch (lhs) {
          case MemberSelectExpr memberSelect:
            Fi = memberSelect.Obj;
            lhsBuilder = e => new MemberSelectExpr(memberSelect.tok, e, memberSelect.MemberName) {
              Member = memberSelect.Member,
              TypeApplicationAtEnclosingClass = memberSelect.TypeApplicationAtEnclosingClass,
              TypeApplicationJustMember = memberSelect.TypeApplicationJustMember,
              Type = memberSelect.Type
            };
            break;
          case SeqSelectExpr ll: {
              Contract.Assert(ll.SelectOne);
              if (!FreeVariablesUtil.ContainsFreeVariable(ll.Seq, false, i)) {
                Fi = ll.E0;
                lhsBuilder = e => new SeqSelectExpr(ll.tok, true, ll.Seq, e, null, ll.CloseParen) {
                  Type = ll.Type
                };
              } else if (!FreeVariablesUtil.ContainsFreeVariable(ll.E0, false, i)) {
                Fi = ll.Seq;
                lhsBuilder = e => new SeqSelectExpr(ll.tok, true, e, ll.E0, null, ll.CloseParen) {
                  Type = ll.Type
                };
              }

              break;
            }
        }
      }

      var rhs = exprRhs.Expr;
      bool usedInversion = false;
      if (Fi != null) {
        var j = new BoundVar(i.tok, i.Name + "#inv", Fi.Type);
        var jj = Expression.CreateIdentExpr(j);
        var jList = new List<BoundVar>() { j };
        var range = Expression.CreateAnd(ModuleResolver.GetImpliedTypeConstraint(i, i.Type), stmt.Range);
        var vals = InvertExpression(i, j, range, Fi);
#if DEBUG_PRINT
          resolve.Options.Writer.WriteLine("DEBUG: Trying to invert:");
          resolve.Options.Writer.WriteLine("DEBUG:   " + Printer.ExprToString(s.Range) + " && " + j.Name + " == " + Printer.ExprToString(Fi));
          if (vals == null) {
            resolve.Options.Writer.WriteLine("DEBUG: Can't");
          } else {
            resolve.Options.Writer.WriteLine("DEBUG: The inverse is the disjunction of the following:");
            foreach (var val in vals) {
              resolve.Options.Writer.WriteLine("DEBUG:   " + Printer.ExprToString(val.Range) + " && " + Printer.ExprToString(val.FInverse) + " == " + i.Name);
            }
          }
#endif
        if (vals != null) {
          foreach (var val in vals) {
            lhs = lhsBuilder(jj);
            Attributes attributes = new Attributes("trigger", new List<Expression>() { lhs }, stmt.Attributes);
            var newRhs = Substitute(rhs, i, val.FInverse);
            var newBounds = SubstituteBoundedPoolList(stmt.Bounds, i, val.FInverse);

            var msg = string.Format("rewrite: forall {0}: {1} {2}| {3} {{ {4} := {5}; }}",
              j.Name,
              j.Type.ToString(),
              Printer.AttributesToString(reporter.Options, attributes),
              Printer.ExprToString(reporter.Options, val.Range),
              Printer.ExprToString(reporter.Options, lhs),
              Printer.ExprToString(reporter.Options, newRhs));
            reporter.Info(MessageSource.Resolver, stmt.Tok, msg);

            var expr = new ForallExpr(stmt.Tok, stmt.Origin, jList, val.Range,
              new BinaryExpr(s0.Tok, BinaryExpr.ResolvedOpcode.EqCommon, lhs, newRhs),
              attributes) {
              Type = Type.Bool,
              Bounds = newBounds,
            };
            exprList.Add(expr);
          }

          usedInversion = true;
        }
      }

      if (!usedInversion) {
        var expr = new ForallExpr(stmt.Tok, stmt.Origin, stmt.BoundVars, stmt.Range,
          new BinaryExpr(s0.Tok, BinaryExpr.ResolvedOpcode.EqCommon, lhs, rhs),
          stmt.Attributes) {
          Type = Type.Bool,
          Bounds = stmt.Bounds
        };
        exprList.Add(expr);
      }

      stmt.EffectiveEnsuresClauses = exprList;
    }

    private static void VisitProof(ForallStmt s) {
      var term = s.Ens.Count != 0 ? s.Ens[0].E : Expression.CreateBoolLiteral(new AutoGeneratedOrigin(s.Tok), true);
      for (int i = 1; i < s.Ens.Count; i++) {
        term = new BinaryExpr(s.Ens[i].Tok, BinaryExpr.ResolvedOpcode.And, term, s.Ens[i].E);
      }

      s.EffectiveEnsuresClauses = new List<Expression> {
        new ForallExpr(s.Tok, s.Origin, s.BoundVars, s.Range, term, s.Attributes) {
          Type = Type.Bool, // resolve here
          Bounds = s.Bounds
        }
      };
    }

    internal class ForallStmtTranslationValues {
      public readonly Expression Range;
      public readonly Expression FInverse;
      public ForallStmtTranslationValues(Expression range, Expression fInverse) {
        Contract.Requires(range != null);
        Contract.Requires(fInverse != null);
        Range = range;
        FInverse = fInverse;
      }
      public ForallStmtTranslationValues Subst(IVariable j, Expression e) {
        Contract.Requires(j != null);
        Contract.Requires(e != null);
        Dictionary<TypeParameter, Type> typeMap = new Dictionary<TypeParameter, Type>();
        var substMap = new Dictionary<IVariable, Expression>();
        substMap.Add(j, e);
        Substituter sub = new Substituter(null, substMap, typeMap);
        var v = new ForallStmtTranslationValues(sub.Substitute(Range), sub.Substitute(FInverse));
        return v;
      }
    }

    /// <summary>
    /// Find piecewise inverse of F under R.  More precisely, find lists of expressions P and F-1
    /// such that
    ///     R(i) && j == F(i)
    /// holds iff the disjunction of the following predicates holds:
    ///     P_0(j) && F-1_0(j) == i
    ///     ...
    ///     P_{n-1}(j) && F-1_{n-1}(j) == i
    /// If no such disjunction is found, return null.
    /// If such a disjunction is found, return for each disjunct:
    ///     * The predicate P_k(j), which is an expression that may have free occurrences of j (but no free occurrences of i)
    ///     * The expression F-1_k(j), which also may have free occurrences of j but not of i
    /// </summary>
    private List<ForallStmtTranslationValues> InvertExpression(BoundVar i, BoundVar j, Expression R, Expression F) {
      Contract.Requires(i != null);
      Contract.Requires(j != null);
      Contract.Requires(R != null);
      Contract.Requires(F != null);
      var vals = new List<ForallStmtTranslationValues>(InvertExpressionIter(i, j, R, F));
      if (vals.Count == 0) {
        return null;
      } else {
        return vals;
      }
    }
    /// <summary>
    /// See InvertExpression.
    /// </summary>
    private IEnumerable<ForallStmtTranslationValues> InvertExpressionIter(BoundVar i, BoundVar j, Expression R, Expression F) {
      Contract.Requires(i != null);
      Contract.Requires(j != null);
      Contract.Requires(R != null);
      Contract.Requires(F != null);
      F = F.Resolved;
      if (!FreeVariablesUtil.ContainsFreeVariable(F, false, i)) {
        // We're looking at R(i) && j == K.
        // We cannot invert j == K, but if we're lucky, R(i) contains a conjunct i==G.
        Expression r = Expression.CreateBoolLiteral(new AutoGeneratedOrigin(R.tok), true);
        Expression G = null;
        foreach (var c in Expression.Conjuncts(R)) {
          if (G == null && c is BinaryExpr) {
            var bin = (BinaryExpr)c;
            if (BinaryExpr.IsEqualityOp(bin.ResolvedOp)) {
              var id = bin.E0.Resolved as IdentifierExpr;
              if (id != null && id.Var == i) {
                G = bin.E1;
                continue;
              }
              id = bin.E1.Resolved as IdentifierExpr;
              if (id != null && id.Var == i) {
                G = bin.E0;
                continue;
              }
            }
          }
          r = Expression.CreateAnd(r, c);
        }
        if (G != null) {
          var jIsK = Expression.CreateEq(Expression.CreateIdentExpr(j), F, j.Type);
          var rr = Substitute(r, i, G);
          yield return new ForallStmtTranslationValues(Expression.CreateAnd(rr, jIsK), G);
        }
      } else if (F is IdentifierExpr) {
        var e = (IdentifierExpr)F;
        if (e.Var == i) {
          // We're looking at R(i) && j == i, which is particularly easy to invert:  R(j) && j == i
          var jj = Expression.CreateIdentExpr(j);
          yield return new ForallStmtTranslationValues(Substitute(R, i, jj), jj);
        }
      } else if (F is BinaryExpr) {
        var bin = (BinaryExpr)F;
        if (bin.ResolvedOp == BinaryExpr.ResolvedOpcode.Add && (bin.E0.Type.IsIntegerType || bin.E0.Type.IsRealType)) {
          if (!FreeVariablesUtil.ContainsFreeVariable(bin.E1, false, i)) {
            // We're looking at:  R(i) && j == f(i) + K.
            // By a recursive call, we'll ask to invert:  R(i) && j' == f(i).
            // For each P_0(j') && f-1_0(j') == i we get back, we yield:
            // P_0(j - K) && f-1_0(j - K) == i
            var jMinusK = Expression.CreateSubtract(Expression.CreateIdentExpr(j), bin.E1);
            foreach (var val in InvertExpression(i, j, R, bin.E0)) {
              yield return val.Subst(j, jMinusK);
            }
          } else if (!FreeVariablesUtil.ContainsFreeVariable(bin.E0, false, i)) {
            // We're looking at:  R(i) && j == K + f(i)
            // Do as in previous case, but with operands reversed.
            var jMinusK = Expression.CreateSubtract(Expression.CreateIdentExpr(j), bin.E0);
            foreach (var val in InvertExpression(i, j, R, bin.E1)) {
              yield return val.Subst(j, jMinusK);
            }
          }
        } else if (bin.ResolvedOp == BinaryExpr.ResolvedOpcode.Sub && (bin.E0.Type.IsIntegerType || bin.E0.Type.IsRealType)) {
          if (!FreeVariablesUtil.ContainsFreeVariable(bin.E1, false, i)) {
            // We're looking at:  R(i) && j == f(i) - K
            // Recurse on f(i) and then replace j := j + K
            var jPlusK = Expression.CreateAdd(Expression.CreateIdentExpr(j), bin.E1);
            foreach (var val in InvertExpression(i, j, R, bin.E0)) {
              yield return val.Subst(j, jPlusK);
            }
          } else if (!FreeVariablesUtil.ContainsFreeVariable(bin.E0, false, i)) {
            // We're looking at:  R(i) && j == K - f(i)
            // Recurse on f(i) and then replace j := K - j
            var kMinusJ = Expression.CreateSubtract(bin.E0, Expression.CreateIdentExpr(j));
            foreach (var val in InvertExpression(i, j, R, bin.E1)) {
              yield return val.Subst(j, kMinusJ);
            }
          }
        }
      } else if (F is ITEExpr) {
        var ife = (ITEExpr)F;
        // We're looking at R(i) && j == if A(i) then B(i) else C(i), which is equivalent to the disjunction of:
        //   R(i) && A(i) && j == B(i)
        //   R(i) && !A(i) && j == C(i)
        // We recurse on each one, yielding the results
        var r = Expression.CreateAnd(R, ife.Test);
        var valsThen = InvertExpression(i, j, r, ife.Thn);
        if (valsThen != null) {
          r = Expression.CreateAnd(R, Expression.CreateNot(ife.tok, ife.Test));
          var valsElse = InvertExpression(i, j, r, ife.Els);
          if (valsElse != null) {
            foreach (var val in valsThen) { yield return val; }
            foreach (var val in valsElse) { yield return val; }
          }
        }
      }
    }

    Expression Substitute(Expression expr, IVariable v, Expression e) {
      Dictionary<IVariable, Expression/*!*/> substMap = new Dictionary<IVariable, Expression>();
      Dictionary<TypeParameter, Type> typeMap = new Dictionary<TypeParameter, Type>();
      substMap.Add(v, e);
      Substituter sub = new Substituter(null, substMap, typeMap);
      return sub.Substitute(expr);
    }

    List<BoundedPool>/*?*/ SubstituteBoundedPoolList(List<BoundedPool>/*?*/ list, IVariable v, Expression e) {
      Dictionary<IVariable, Expression/*!*/> substMap = new Dictionary<IVariable, Expression>();
      Dictionary<TypeParameter, Type> typeMap = new Dictionary<TypeParameter, Type>();
      substMap.Add(v, e);
      Substituter sub = new Substituter(null, substMap, typeMap);
      return sub.SubstituteBoundedPoolList(list);
    }
  }
}