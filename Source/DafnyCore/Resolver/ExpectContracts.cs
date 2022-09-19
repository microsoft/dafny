using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Boogie;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.Dafny;

public class ExpectContracts : IRewriter {
  private readonly ClonerButDropMethodBodies cloner = new();
  private readonly Dictionary<MemberDecl, MemberDecl> wrappedDeclarations = new();
  private readonly Dictionary<MemberDecl, string> fullNames = new();
  private readonly Dictionary<string, MemberDecl> newDeclarationsByName = new();
  private CallRedirector callRedirector;

  public ExpectContracts(ErrorReporter reporter) : base(reporter) {
    callRedirector = new(reporter);
  }

  /// <summary>
  /// Create an expect statement that checks the given contract clause
  /// expression and fails with a message that points to the original
  /// location of the contract clause if it is not true at runtime.
  ///
  /// If the given clause is not compilable, emit a warning and construct
  /// an `expect true` statement with a message explaining the situation.
  /// </summary>
  /// <param name="expr">The contract clause expression to evaluate.</param>
  /// <param name="exprType">Either "requires" or "ensures", to use in the
  /// failure message.</param>
  /// <returns>The newly-created expect statement.</returns>
  private Statement CreateContractExpectStatement(AttributedExpression expr, string exprType) {
    var tok = expr.E.tok;
    var msg = $"Runtime failure of {exprType} clause from {tok.filename}:{tok.line}:{tok.col}";
    var exprToCheck = expr.E;
    if (ExpressionTester.UsesSpecFeatures(exprToCheck)) {
      Reporter.Warning(MessageSource.Rewriter, tok,
        $"The {exprType} clause at this location cannot be compiled to be tested at runtime, because it references ghost state.");
      exprToCheck = new LiteralExpr(tok, true);
      msg += " (not checked because it references ghost state)";
    }
    var msgExpr = Expression.CreateStringLiteral(tok, msg);
    return new ExpectStmt(tok, expr.E.EndToken, exprToCheck, msgExpr, null);
  }

  /// <summary>
  /// Creates a block statement that includes an expect statement for every
  /// requires clause, followed by the provided call statement, followed by
  /// an expect statement for every ensures clause.
  /// </summary>
  /// <param name="requires">The list of requires clause expressions.</param>
  /// <param name="ensures">The list of ensures clause expressions.</param>
  /// <param name="callStmt">The call statement to include.</param>
  /// <returns>The newly-created block statement.</returns>
  private BlockStmt MakeContractCheckingBody(List<AttributedExpression> requires,
    List<AttributedExpression> ensures, Statement callStmt) {
    var expectRequiresStmts = requires.Select(req =>
      CreateContractExpectStatement(req, "requires"));
    var expectEnsuresStmts = ensures.Select(ens =>
      CreateContractExpectStatement(ens, "ensures"));
    var callStmtList = new List<Statement>() { callStmt };
    var bodyStatements = expectRequiresStmts.Concat(callStmtList).Concat(expectEnsuresStmts);
    return new BlockStmt(callStmt.Tok, callStmt.EndTok, bodyStatements.ToList());
  }

  private static Expression MakeApplySuffix(IToken tok, string name, List<Expression> args) {
    var nameExpr = new NameSegment(tok, name, null);
    var argBindings = args.ConvertAll(arg => new ActualBinding(null, arg));
    return new ApplySuffix(tok, null, nameExpr, argBindings, tok);
  }

  private bool ShouldGenerateWrapper(MemberDecl decl) {
    return !decl.IsGhost &&
           decl is not Constructor &&
           callRedirector.HasExternAttribute(decl);
  }

  /// <summary>
  /// Create a wrapper for the given function or method declaration that
  /// dynamically checks all of its preconditions, calls it, and checks
  /// all of its postconditions before returning. The new wrapper will
  /// later be added as a sibling of the original declaration.
  /// </summary>
  /// <param name="parent">The declaration containing the on to be wrapped.</param>
  /// <param name="decl">The declaration to be wrapped.</param>
  private void GenerateWrapper(TopLevelDeclWithMembers parent, MemberDecl decl) {
    var tok = decl.tok; // TODO: do better

    var newName = decl.Name + "_checked";
    MemberDecl newDecl = null;

    if (decl is Method origMethod) {
      var newMethod = cloner.CloneMethod(origMethod);
      newMethod.Name = newName;

      var args = newMethod.Ins.Select(Expression.CreateIdentExpr).ToList();
      var outs = newMethod.Outs.Select(Expression.CreateIdentExpr).ToList();
      var applyExpr = MakeApplySuffix(tok, origMethod.Name, args);
      var applyRhs = new ExprRhs(applyExpr);
      var callStmt = new UpdateStmt(tok, tok, outs, new List<AssignmentRhs>() { applyRhs });

      var body = MakeContractCheckingBody(origMethod.Req, origMethod.Ens, callStmt);
      newMethod.Body = body;
      newDecl = newMethod;
    } else if (decl is Function origFunc) {
      var newFunc = cloner.CloneFunction(origFunc);
      newFunc.Name = newName;

      var args = origFunc.Formals.Select(Expression.CreateIdentExpr).ToList();
      var callExpr = MakeApplySuffix(tok, origFunc.Name, args);
      newFunc.Body = callExpr;

      var localName = origFunc.Result?.Name ?? "__result";
      var local = new LocalVariable(tok, tok, localName, origFunc.ResultType, false);
      var localExpr = new IdentifierExpr(tok, localName);
      var callRhs = new ExprRhs(callExpr);

      var lhss = new List<Expression> { localExpr };
      var locs = new List<LocalVariable> { local };
      var rhss = new List<AssignmentRhs> { callRhs };

      var callStmt = origFunc.Result?.Name is null
        ? (Statement)new VarDeclStmt(tok, tok, locs, new UpdateStmt(tok, tok, lhss, rhss))
        : (Statement)new UpdateStmt(tok, tok, lhss, rhss);

      var body = MakeContractCheckingBody(origFunc.Req, origFunc.Ens, callStmt);

      if (origFunc.Result?.Name is null) {
        body.AppendStmt(new ReturnStmt(tok, tok, new List<AssignmentRhs> { new ExprRhs(localExpr) }));
      }
      newFunc.ByMethodBody = body;
      newDecl = newFunc;
    }

    if (newDecl is not null) {
      newDecl.Attributes = null;
      wrappedDeclarations.Add(decl, newDecl);
      callRedirector.AddFullName(newDecl, decl.FullName + "_checked");
    }
  }

  /// <summary>
  /// Add wrappers for certain top-level declarations in the given module.
  /// This runs after the first pass of resolution so that it has access to
  /// attributes and call targets, but it generates pre-resolution syntax.
  /// This is okay because the program gets resolved again during compilation.
  /// </summary>
  /// <param name="moduleDefinition">The module to generate wrappers for and in.</param>
  internal override void PostResolveIntermediate(ModuleDefinition moduleDefinition) {

    // Keep a list of members to wrap so that we don't modify the collection we're iterating over.
    List<(TopLevelDeclWithMembers, MemberDecl)> membersToWrap = new();

    // Find module members to wrap.
    foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
      foreach (var decl in topLevelDecl.Members) {
        if (ShouldGenerateWrapper(decl)) {
          membersToWrap.Add((topLevelDecl, decl));
        }
      }
    }

    // Generate a wrapper for each of the members identified above.
    foreach (var (topLevelDecl, decl) in membersToWrap) {
      GenerateWrapper(topLevelDecl, decl);
    }

    // Add the generated wrappers to the module.
    foreach (var (topLevelDecl, decl) in membersToWrap) {
      if (wrappedDeclarations.ContainsKey(decl)) {
        topLevelDecl.Members.Add(wrappedDeclarations[decl]);
      }
    }
  }

  class CallRedirector : TopDownVisitor<MemberDecl> {
    internal readonly Dictionary<MemberDecl, MemberDecl> newRedirections = new();
    internal readonly Dictionary<MemberDecl, string> newFullNames = new();
    private readonly ErrorReporter reporter;

    public CallRedirector(ErrorReporter reporter) {
      this.reporter = reporter;
    }

    internal void AddFullName(MemberDecl decl, string fullName) {
      newFullNames.Add(decl, fullName);
    }

    internal bool HasTestAttribute(MemberDecl decl) {
      return decl.Attributes is not null && Attributes.Contains(decl.Attributes, "test");
    }

    internal bool HasExternAttribute(MemberDecl decl) {
      return decl.Attributes is not null && Attributes.Contains(decl.Attributes, "extern");
    }

    private bool ShouldCallWrapper(MemberDecl caller, MemberDecl callee) {
      if (!HasExternAttribute(callee)) {
        return false;
      }
      // If there's no wrapper for the callee, don't try to call it, but warn.
      if (!newRedirections.ContainsKey(callee)) {
        reporter.Warning(MessageSource.Rewriter, caller.tok, $"Internal: no wrapper for {callee.FullDafnyName}");
        return false;
      }

      var opt = DafnyOptions.O.TestContracts;
      return ((HasTestAttribute(caller) && opt == DafnyOptions.ContractTestingMode.ExternsInTests) ||
              (opt == DafnyOptions.ContractTestingMode.AllExterns)) &&
             // Skip if the caller is a wrapper, otherwise it'd just call itself recursively.
             !newRedirections.ContainsValue(caller);
    }

    protected override bool VisitOneExpr(Expression expr, ref MemberDecl decl) {
      if (expr is FunctionCallExpr fce) {
        var f = fce.Function;
        var targetName = f.Name;
        //Console.WriteLine($"Function call to {targetName}");
        if (ShouldCallWrapper(decl, f)) {
          var newTarget = newRedirections[f];
          //Console.WriteLine($"Call (expression) to {f.FullName} redirecting to {newTarget.FullName}");
          var resolved = (FunctionCallExpr)fce.Resolved;
          resolved.Function = (Function)newTarget;
          resolved.Name = newTarget.Name;
        }
      }

      return true;
    }

    protected override bool VisitOneStmt(Statement stmt, ref MemberDecl decl) {
      if (stmt is CallStmt cs) {
        var m = cs.Method;
        var targetName = m.Name;
        //Console.WriteLine($"Method call to {m.FullName}");
        if (ShouldCallWrapper(decl, m)) {
          var newTarget = newRedirections[m];
          //Console.WriteLine($"Call (statement) to {m.FullName} redirecting to {newTarget.FullName}");
          var resolved = (MemberSelectExpr)cs.MethodSelect.Resolved;
          resolved.Member = newTarget;
          resolved.MemberName = newTarget.Name;
        }
      }

      return true;
    }
  }

  internal override void PostCompileCloneAndResolve(ModuleDefinition moduleDefinition) {
    foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
      // Keep track of current declarations by name to avoid redirecting
      // calls to functions or methods from obsolete modules (those that
      // existed prior to processing by CompilationCloner).
      foreach (var decl in topLevelDecl.Members) {
        var noCompileName = decl.FullName.Replace("_Compile", "");
        //Console.WriteLine(($"Adding {noCompileName}"));
        newDeclarationsByName.Add(noCompileName, decl);
      }
    }

    foreach (var (caller, callee) in wrappedDeclarations) {
      var callerFullName = caller.FullName;
      var calleeFullName = callRedirector.newFullNames[callee];
      //Console.WriteLine($"Looking up {caller.FullName} -> {calleeFullName}");
      if (newDeclarationsByName.ContainsKey(callerFullName) &&
          newDeclarationsByName.ContainsKey(calleeFullName)) {
        var callerDecl = newDeclarationsByName[caller.FullName];
        var calleeDecl = newDeclarationsByName[calleeFullName];
        if (!callRedirector.newRedirections.ContainsKey(callerDecl)) {
          callRedirector.newRedirections.Add(callerDecl, calleeDecl);
        }
      }
    }

    foreach (var topLevelDecl in moduleDefinition.TopLevelDecls.OfType<TopLevelDeclWithMembers>()) {
      foreach (var decl in topLevelDecl.Members) {
        if (decl is ICallable callable) {
          //Console.WriteLine($"Visiting {decl.FullName}");
          callRedirector.Visit(callable, decl);
        }
      }
    }
  }
}
