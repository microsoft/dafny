// Copyright (C) Microsoft Corporation.  All Rights Reserved.
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------
using Microsoft.Dafny;
using PODesc = Microsoft.Dafny.ProofObligationDescription;

namespace DafnyCore.Verifier;

// A proof dependency represents a particular part of a Dafny program
// that may be used in the process of proving its correctness. When
// Boogie proves a particular verification condition, it returns a
// list of strings, returned by the SMT solver, indicating which
// program elements were used in completing the proof. After this, the
// ProofDependencyManager can map each string to a more structured
// ProofDependency.
public abstract class ProofDependency {
  public abstract string Description { get; }

  public abstract IOrigin Range { get; }

  public string LocationString() {
    if (Range?.StartToken is null) {
      return "<no location>";
    }
    var fn = Range.StartToken.filename;
    var sl = Range.StartToken.line;
    var sc = Range.StartToken.col;
    return $"{fn}({sl},{sc})";
  }

  public string RangeString() {
    if (Range?.StartToken is null) {
      return "<no range>";
    }
    var fn = Range.StartToken.filename;
    var sl = Range.StartToken.line;
    var sc = Range.StartToken.col;
    var el = Range.EndToken.line;
    var ec = Range.EndToken.col;
    return $"{fn}({sl},{sc})-({el},{ec})";
  }

  public string OriginalString() {
    return Range?.PrintOriginal();
  }
}

// Represents the portion of a Dafny program corresponding to a proof
// obligation. This is particularly important to track because if a particular
// assertion batch can be proved without proving one of the assertions that is
// a proof obligation within it, that assertion must have been proved vacuously.
public class ProofObligationDependency : ProofDependency {
  public override IOrigin Range { get; }

  public ProofObligationDescription ProofObligation { get; }

  public override string Description =>
      $"{ProofObligation.SuccessDescription}";

  public ProofObligationDependency(Microsoft.Boogie.IToken tok, ProofObligationDescription proofObligation) {
    Range = tok as RangeToken ?? (proofObligation as AssertStatementDescription)?.AssertStatement.RangeToken ?? BoogieGenerator.ToDafnyToken(true, tok);
    ProofObligation = proofObligation;
  }
}

public class AssumedProofObligationDependency : ProofDependency {
  public override IOrigin Range { get; }

  public ProofObligationDescription ProofObligation { get; }

  public override string Description =>
      $"assumption that {ProofObligation.SuccessDescription}";

  public AssumedProofObligationDependency(IOrigin tok, ProofObligationDescription proofObligation) {
    var unwrapped = tok.Unwrap();
    Range = unwrapped as RangeToken 
      ?? ((proofObligation as AssertStatementDescription)?.AssertStatement.RangeToken 
         ?? new RangeToken((Token)unwrapped, (Token)unwrapped));
    ProofObligation = proofObligation;
  }
}

// Represents the assumption of a requires clause in the process of
// proving a Dafny definition.
public class RequiresDependency : ProofDependency {
  private Expression requires;

  private IOrigin tok;

  public override IOrigin Range =>
    tok as RangeToken ?? requires.RangeToken;

  public override string Description =>
    $"requires clause";

  public RequiresDependency(IOrigin token, Expression requires) {
    this.requires = requires;
    this.tok = token;
  }
}

// Represents the goal of proving an ensures clause of a Dafny definition.
public class EnsuresDependency : ProofDependency {
  private readonly Expression ensures;

  private readonly IOrigin tok;

  public override IOrigin Range =>
    tok as RangeToken ?? ensures.RangeToken;

  public override string Description =>
    "ensures clause";

  public EnsuresDependency(IOrigin token, Expression ensures) {
    this.ensures = ensures;
    this.tok = token;
  }
}

// Represents the goal of proving a specific requires clause of a specific
// call.
public class CallRequiresDependency : ProofDependency {
  public readonly CallDependency call;
  private readonly RequiresDependency requires;

  public override IOrigin Range => call.Range;

  public override string Description =>
    $"requires clause at {requires.RangeString()} from call";

  public CallRequiresDependency(CallDependency call, RequiresDependency requires) {
    this.call = call;
    this.requires = requires;
  }
}

// Represents the assumption of a specific ensures clause of a specific
// call.
public class CallEnsuresDependency : ProofDependency {
  public readonly CallDependency call;
  private readonly EnsuresDependency ensures;

  public override IOrigin Range => call.Range;

  public override string Description =>
    $"ensures clause at {ensures.RangeString()} from call";

  public CallEnsuresDependency(CallDependency call, EnsuresDependency ensures) {
    this.call = call;
    this.ensures = ensures;
  }
}

// Represents the fact that a particular call occurred.
public class CallDependency : ProofDependency {
  public readonly CallStmt call;

  public override IOrigin Range => call.RangeToken;

  public override string Description =>
    $"call";

  public CallDependency(CallStmt call) {
    this.call = call;
  }
}

// Represents the assumption of a predicate in an `assume` statement.
public class AssumptionDependency : ProofDependency {
  public override IOrigin Range => Expr.RangeToken;

  public override string Description =>
    comment ?? OriginalString();

  public bool WarnWhenUnused { get; }

  private readonly string comment;

  public Expression Expr { get; }

  public AssumptionDependency(bool warnWhenUnused, string comment, Expression expr) {
    this.WarnWhenUnused = warnWhenUnused;
    this.comment = comment;
    this.Expr = expr;
  }
}

// Represents the invariant of a loop.
public class InvariantDependency : ProofDependency {
  private readonly Expression invariant;

  public override IOrigin Range => invariant.RangeToken;

  public override string Description =>
    $"loop invariant";

  public InvariantDependency(Expression invariant) {
    this.invariant = invariant;
  }
}

// Represents an assignment statement. This includes the assignment to an
// out parameter implicit in a return statement.
public class AssignmentDependency : ProofDependency {
  public override IOrigin Range { get; }

  public override string Description =>
     "assignment (or return)";

  public AssignmentDependency(IOrigin rangeToken) {
    this.Range = rangeToken;
  }
}

// Represents dependency of a proof on the definition of a specific function.
public class FunctionDefinitionDependency : ProofDependency {
  public override IOrigin Range => function.RangeToken;

  public override string Description =>
    $"function definition for {function.Name}";

  public Function function;

  public FunctionDefinitionDependency(Function f) {
    function = f;
  }
}