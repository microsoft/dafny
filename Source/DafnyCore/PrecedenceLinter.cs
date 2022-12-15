//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Microsoft.Boogie;

namespace Microsoft.Dafny {

  public class PrecedenceLinter : IRewriter {
    internal override void PostResolve(Program program) {
      base.PostResolve(program);
      foreach (var moduleDefinition in program.Modules()) {
        foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
          foreach (var callable in topLevelDecl.Members.OfType<ICallable>()) {
            var visitor = new PrecedenceLinterVisitor(this.Reporter);
            visitor.Visit(callable, null);
          }
        }
      }
    }

    public PrecedenceLinter(ErrorReporter reporter) : base(reporter) {
    }
  }

  class LeftMargin {
    public int Column;

    public LeftMargin(int column) {
      Column = column;
    }
  }

  /// <summary>
  /// The PrecedenceLinterVisitor builds on Dafny's general TopDownVisitor. During the traversal, the overridable method VisitOneExpr
  /// performs some action on the given expression expr and parameter "st". The design of TopDownVisitor makes it possible for
  /// VisitOneExpr(expr, ...) to affect the traversal in the following ways:
  ///
  /// * It can request that no child of expr be visited. This is indicated by returning false.
  /// * It can request that the children of expr be visited using the same parameter value st. This is indicated by returning
  ///   true (and not assigning to st).
  /// * It can request that the children of expr be visited and provide a new parameter value st that will be passed to those
  ///   children. This is done by returning true and setting st to a different value.
  ///
  /// So, essentially, the traversal passes in one st value and gets a bool and an st back. This is encoded by making the bool
  /// be the method return value and making st a ref parameter.
  ///
  /// PrecedenceLinterVisitor.VisitOneExpr sometimes returns false, indicating that the main traversal should not go into the
  /// children of expr. Those children will therefore not be included in the stats gathered in st. Instead, PrecedenceLinterVisitor.VisitOneExpr
  /// will kick off a new visitation of its children, using a separate st object. This is how PrecedenceLinterVisitor can let the
  /// minimum-left-margin information be computed for just a part of the tree.
  ///
  /// The PrecedenceLinterVisitor also needs to be able to gather stats for all the nodes that are being visited. The design of
  /// TopDownVisitor does not use the ref parameter to accumulate values across the children; instead, it passes the same st to all
  /// children of expr. For this reason, PrecedenceLinterVisitor chooses the type of st to be a pointer to an integer.
  ///
  /// But PrecedenceLinterVisitor never uses the third case above. PrecedenceLinterVisitor would therefore be simpler if st were
  /// not a ref parameter in what is inherited from TopDownVisitor. Indeed, if we had another TopDownVisitor that streamlined this
  /// functionality, we could use it. Such a TopDownVisitor would then declare st (or, then more appropriately named "context") as
  /// an ordinary in-parameter to VisitOneExpr, since the method would only need to return a bool.
  /// </summary>
  class PrecedenceLinterVisitor : TopDownVisitor<LeftMargin> {

    private readonly ErrorReporter reporter;

    public PrecedenceLinterVisitor(ErrorReporter reporter) {
      this.reporter = reporter;
    }

    /// <summary>
    /// Regarding the "st" parameter, see the comment above the class.
    /// </summary>
    protected override bool VisitOneExpr(Expression expr, ref LeftMargin st) {
      if (AutoGeneratedToken.Is(expr.tok) || expr is DefaultValueExpression || expr.GetRangeToken() == null) {
        return false; // don't traverse further
      }

      if (st != null) {
        int column;
        if (expr is DatatypeUpdateExpr or SeqSelectExpr or SeqUpdateExpr or MultiSelectExpr or TernaryExpr) {
          // These expressions are handled below as being all-independent components, which means the
          // formatting of their internal structure is not relevant to the enclosing context. Yet, the .tok of
          // these expressions sits someone inside that internal structure (for example, the .tok of
          // a SeqSelectExpr is the open-bracket). To avoid looking at that internal structure, we instead
          // use the .StartToken for these expressions.
          column = expr.StartToken.col;
        } else {
          column = expr.tok.col;
        }
        if (column < st.Column) {
          st.Column = column;
        }
      }

      // Our aim is to try to detect if some expressions are longer than the user intended.
      // This can happen if the user accidentally left off parentheses around a lower-precedence
      // operator (e.g., ==>). As a guide, we look at how the user formatted the code,
      // that is, we inspect line and column information.

      if (expr is BinaryExpr bin && (bin.Op == BinaryExpr.Opcode.Imp || bin.Op == BinaryExpr.Opcode.Exp || bin.Op == BinaryExpr.Opcode.Iff)) {
        VisitLhsComponent(expr.tok, bin.E0,
          // For
          //   a)  LHS ==> RHS
          //   b)  LHS ==>
          //         RHS-somewhere-on-this-line
          // use LHS.StartToken as the left margin.
          bin.E0.StartToken.line == expr.tok.line ? bin.E0.StartToken.col :
          // For
          //   c)  LHS0 &&
          //       LHS1 ==> RHS
          // use expr.tok (that is, the location of ==>) as the left margin. This is bound to generate a warning.
          bin.E1.StartToken.line == expr.tok.line ? expr.tok.col :
          // For
          //   d)  LHS0 &&
          //       LHS1 ==>
          //         RHS-somewhere-on-this-line
          //   e)  LHS0 &&
          //       LHS1
          //       ==>
          //         RHS-somewhere-on-this-line
          // use LHS.StartToken as the left margin.
          bin.E0.StartToken.col,
          "left-hand operand of " + BinaryExpr.OpcodeString(bin.Op));
        VisitRhsComponent(expr.tok, bin.E1, "right-hand operand of " + BinaryExpr.OpcodeString(bin.Op));
        return false; // indicate that we've already processed expr's subexpressions

      } else if (expr is ITEExpr ifThenElse) {
        VisitIndependentComponent(ifThenElse.Test);
        VisitIndependentComponent(ifThenElse.Thn);
        VisitRhsComponent(expr.tok, ifThenElse.Els, "else branch of if-then-else");
        return false; // indicate that we've already processed expr's subexpressions

      } else if (expr is QuantifierExpr quantifierExpr) {
        Attributes.SubExpressions(quantifierExpr.Attributes).Iter(VisitIndependentComponent);
        if (quantifierExpr.Range != null) {
          VisitIndependentComponent(quantifierExpr.Range);
        }
        VisitRhsComponent(expr.tok, quantifierExpr.Term,
          expr.tok.line == quantifierExpr.Term.StartToken.line ? expr.tok.col + 1 : quantifierExpr.Term.StartToken.col,
          "body of " + (expr is ForallExpr ? "forall" : "exists"));
        return false; // indicate that we've already processed expr's subexpressions

      } else if (expr is LetExpr letExpr) {
        Attributes.SubExpressions(letExpr.Attributes).Iter(VisitIndependentComponent);
        letExpr.RHSs.Iter(VisitIndependentComponent);
        VisitRhsComponent(expr.tok, letExpr.Body, "body of let-expression");
        return false; // indicate that we've already processed expr's subexpressions

      } else if (expr is OldExpr or FreshExpr or UnchangedExpr or DatatypeValue or DisplayExpression or MapDisplayExpr) {
        // In these expressions, all subexpressions are contained in parentheses, so there's no risk of precedence confusion
        expr.SubExpressions.Iter(VisitIndependentComponent);
        return false; // indicate that we've already processed expr's subexpressions

      } else if (expr is FunctionCallExpr functionCallExpr) {
        return VisitComponentsAsIndependentExceptOne(expr, functionCallExpr.Receiver, st);
      } else if (expr is ApplyExpr applyExpr) {
        return VisitComponentsAsIndependentExceptOne(expr, applyExpr.Function, st);
      } else if (expr is DatatypeUpdateExpr datatypeUpdateExpr) {
        return VisitComponentsAsIndependentExceptOne(expr, datatypeUpdateExpr.Root, st);
      } else if (expr is SeqSelectExpr selectExpr) {
        return VisitComponentsAsIndependentExceptOne(expr, selectExpr.Seq, st);
      } else if (expr is SeqUpdateExpr seqUpdateExpr) {
        return VisitComponentsAsIndependentExceptOne(expr, seqUpdateExpr.Seq, st);
      } else if (expr is MultiSelectExpr multiSelectExpr) {
        return VisitComponentsAsIndependentExceptOne(expr, multiSelectExpr.Array, st);

      } else if (expr is TernaryExpr ternaryExpr) {
        VisitIndependentComponent(ternaryExpr.E0);
        Visit(ternaryExpr.E1, st);
        Visit(ternaryExpr.E2, st);
        return false; // indicate that we've already processed expr's subexpressions

#if REVISIT_AFTER_PR_2734
      } else if (expr is NestedMatchExpr nestedMatchExpr) {
        // Handle each case like the "else" of an if-then-else
        Attributes.SubExpressions(nestedMatchExpr.Attributes).Iter(VisitIndependentComponent);
        VisitIndependentComponent(nestedMatchExpr.Source);
        var n = nestedMatchExpr.Cases.Count;
        for (var i = 0; i < n; i++) {
          var body = nestedMatchExpr.Cases[i].Body;
          if (i == n - 1 && !nestedMatchExpr.UsesOptionalBraces) {
            VisitRhsComponent(body.StartToken, body, "case expression");
          } else {
            VisitIndependentComponent(body);
          }
        }
        return false; // indicate that we've already processed expr's subexpressions
#else
      } else if (expr is NestedMatchExpr nestedMatchExpr) {
        // Handle each case like the "else" of an if-then-else
        Attributes.SubExpressions(nestedMatchExpr.Attributes).Iter(VisitIndependentComponent);
        VisitIndependentComponent(nestedMatchExpr.Source);
        // Handle the cases via MatchExpr
        Visit(nestedMatchExpr.ResolvedExpression, null);
        return false; // indicate that we've already processed expr's subexpressions
      } else if (expr is MatchExpr matchExpr) {
        // The Source was already handled in NestedMatchExpr
        foreach (var mc in matchExpr.Cases) {
          VisitIndependentComponent(mc.Body);
        }
        return false; // indicate that we've already processed expr's subexpressions
#endif
      }

      return base.VisitOneExpr(expr, ref st);
    }

    /// <summary>
    /// For each subexpression of "expr", call "VisitIndependentComponent" unless the subexpression
    /// is "exception", in which case call "Visit(exception, st)".
    /// For convenience to the caller, this method always returns "false".
    /// </summary>
    bool VisitComponentsAsIndependentExceptOne(Expression expr, Expression exception, LeftMargin st) {
      foreach (var e in expr.SubExpressions) {
        if (e == exception) {
          Visit(e, st);
        } else {
          VisitIndependentComponent(e);
        }
      }
      return false;
    }

    void VisitIndependentComponent(Expression expr) {
      Visit(expr, null);
    }

    void VisitLhsComponent(IToken errorToken, Expression expr, int leftMargin, string what) {
      if (expr is ParensExpression || expr.StartToken.line == errorToken.line) {
        VisitIndependentComponent(expr);
      } else {
        var st = new LeftMargin(leftMargin);
        Visit(expr, st);
        if (st.Column < leftMargin) {
          this.reporter.Warning(MessageSource.Rewriter, errorToken,
            $"unusual indentation in {what} (which starts at {LineCol(expr.StartToken)}); do you perhaps need parentheses?");
        }
      }
    }

    void VisitRhsComponent(IToken errorToken, Expression expr, string what) {
      if (expr.StartToken == null) {
        // Might be a resolved expression.
        VisitIndependentComponent(expr);
      } else {
        VisitRhsComponent(errorToken, expr, expr.StartToken.col, what);
      }
    }

    void VisitRhsComponent(IToken errorToken, Expression expr, int rightMargin, string what) {
      if (expr is ParensExpression || errorToken is IncludeToken) {
        VisitIndependentComponent(expr);
      } else {
        var st = new LeftMargin(rightMargin);
        Visit(expr, st);
        if (st.Column < rightMargin) {
          this.reporter.Warning(MessageSource.Rewriter, errorToken,
            $"unusual indentation in {what} (which ends at {LineCol(expr.EndToken)}); do you perhaps need parentheses?");
        }
      }
    }

    static string LineCol(IToken tok) {
      return $"line {tok.line}, column {tok.col}";
    }
  }
}
