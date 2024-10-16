using System;
using System.Collections.Generic;
using System.Linq;
using DafnyCore.Verifier;
using Microsoft.Boogie;
using Microsoft.Dafny.Triggers;
using VC;

namespace Microsoft.Dafny;

public record VerificationTaskResult(IVerificationTask Task, VerificationRunResult Result);

public class ProofDependencyWarnings {
  private static DafnyOptions options;
  private static ErrorReporter reporter;
  private static ProofDependencyManager manager;


  public static void ReportSuspiciousDependencies(DafnyOptions dafnyOptions, IEnumerable<VerificationTaskResult> parts,
    ErrorReporter reporter, ProofDependencyManager depManager) {
    manager = depManager;
    ProofDependencyWarnings.reporter = reporter;
    options = dafnyOptions;
    foreach (var resultsForScope in parts.GroupBy(p => p.Task.ScopeId)) {
      WarnAboutSuspiciousDependenciesForImplementation(resultsForScope.Key,
        resultsForScope.Select(p => p.Result).ToList());
    }
  }

  public static void WarnAboutSuspiciousDependenciesUsingStoredPartialResults(DafnyOptions dafnyOptions, ErrorReporter reporter, ProofDependencyManager depManager) {
    manager = depManager;
    ProofDependencyWarnings.reporter = reporter;
    options = dafnyOptions;
    var verificationResults = (dafnyOptions.Printer as DafnyConsolePrinter).VerificationResults.ToList();
    var orderedResults =
      verificationResults.OrderBy(vr =>
        (vr.Implementation.Tok.filename, vr.Implementation.Tok.line, vr.Implementation.Tok.col));

    foreach (var (implementation, result) in orderedResults) {
      if (result.Outcome != VcOutcome.Correct) {
        continue;
      }
      var unusedFunctions = UnusedFunctions(implementation.Name, result.VCResults.SelectMany(r => r.CoveredElements), result.VCResults.SelectMany(r => r.AvailableAxioms));
      WarnAboutSuspiciousDependencies(implementation.Name, result.VCResults, unusedFunctions);
    }
  }

  public static void WarnAboutSuspiciousDependenciesForImplementation(string name,
    IReadOnlyList<VerificationRunResult> results) {
    if (results.Any(r => r.Outcome != SolverOutcome.Valid)) {
      return;
    }
    var unusedFunctions = UnusedFunctions(name, results.SelectMany(r => r.CoveredElements), results.Select(DafnyConsolePrinter.DistillVCResult).SelectMany(r => r.AvailableAxioms));
    WarnAboutSuspiciousDependencies(name, results.Select(DafnyConsolePrinter.DistillVCResult).ToList(), unusedFunctions);
  }

  private static List<Function> UnusedFunctions(string implementationName, IEnumerable<TrackedNodeComponent> coveredElements,
    IEnumerable<Axiom> axioms) {
    if (!((options.Get(CommonOptionBag.SuggestProofRefactoring) || options.Get(CommonOptionBag.AnalyseProofs)) && manager.idsByMemberName[implementationName].Decl is Method)) {
      return new List<Function>();
    }

    var unusedFunctions = new List<Function>();
    if (manager.idsByMemberName[implementationName].Decl is not Method method) {
      return unusedFunctions;
    }

    var usedFunctions = coveredElements.Select(manager.GetFullIdDependency).OfType<FunctionDefinitionDependency>()
      .Select(dep => dep.function).Deduplicate((a, b) => a.Equals(b));

    unusedFunctions = VisibleFunctions().Except(usedFunctions).ToList();

    return unusedFunctions;

    HashSet<Function> VisibleFunctions() {
      var functions = new HashSet<Function>();

      foreach (var visibleFunction in axioms.Select(GetFunctionFromAttributed).Where(f => f != null)) {
        functions.Add(visibleFunction);
      }

      return functions;

      Function GetFunctionFromAttributed(ICarriesAttributes construct) {
        var values = construct.FindAllAttributes("id");
        if (!values.Any()) {
          return null;
        }
        var id = (string)values.Last().Params.First();
        if (manager.ProofDependenciesById.TryGetValue(id, out var dep) && dep is FunctionDefinitionDependency fdd) {
          return fdd.function;
        }
        return null;
      }
    }
  }

  private static void WarnAboutSuspiciousDependencies(string scopeName,
    IReadOnlyList<DafnyConsolePrinter.VerificationRunResultPartialCopy> assertCoverage, List<Function> unusedFunctions) {
    var potentialDependencies = manager.GetPotentialDependenciesForDefinition(scopeName);
    var coveredElements = assertCoverage.SelectMany(tp => tp.CoveredElements);
    var usedDependencies =
      coveredElements
        .Select(manager.GetFullIdDependency)
        .OrderBy(dep => dep.Range)
        .ThenBy(dep => dep.Description);
    var unusedDependencies =
      potentialDependencies
        .Except(usedDependencies)
        .OrderBy(dep => dep.Range)
        .ThenBy(dep => dep.Description).ToList();

    foreach (var unusedDependency in unusedDependencies) {
      if (options.Get(CommonOptionBag.WarnContradictoryAssumptions) || options.Get(CommonOptionBag.AnalyseProofs)) {
        if (unusedDependency is ProofObligationDependency obligation) {
          if (ShouldWarnVacuous(scopeName, obligation)) {
            var message = $"proved using contradictory assumptions: {obligation.Description}";
            if (obligation.ProofObligation is AssertStatementDescription) {
              message += ". (Use the `{:contradiction}` attribute on the `assert` statement to silence.)";
            }
            reporter.Warning(MessageSource.Verifier, "", obligation.Range, message);
          }
        }

        if (unusedDependency is EnsuresDependency ensures) {
          if (ShouldWarnVacuous(scopeName, ensures)) {
            reporter.Warning(MessageSource.Verifier, "", ensures.Range,
              $"ensures clause proved using contradictory assumptions");
          }
        }
      }

      if (options.Get(CommonOptionBag.WarnRedundantAssumptions) || options.Get(CommonOptionBag.AnalyseProofs)) {
        if (unusedDependency is RequiresDependency requires) {
          reporter.Warning(MessageSource.Verifier, "", requires.Range, $"unnecessary requires clause");
        }

        if (unusedDependency is AssumptionDependency assumption) {
          if (ShouldWarnUnused(assumption)) {
            reporter.Warning(MessageSource.Verifier, "", assumption.Range,
              $"unnecessary (or partly unnecessary) {assumption.Description}");
          }
        }
      }
    }

    if ((options.Get(CommonOptionBag.SuggestProofRefactoring) || options.Get(CommonOptionBag.AnalyseProofs)) && manager.idsByMemberName[scopeName].Decl is Method m) {
      SuggestFunctionHiding(unusedFunctions, m);
      SuggestByProofRefactoring(scopeName, assertCoverage.ToList());
    }
  }

  private static void SuggestFunctionHiding(List<Function> unusedFunctions, Method m) {
    if (unusedFunctions.Any()) {
      reporter.Info(MessageSource.Verifier, m.Body.StartToken,
        $"Consider hiding {(unusedFunctions.Count > 1 ? "these functions, which are" : "this function, which is")} unused by the proof: {unusedFunctions.Comma()}");
    }
  }

  private static void SuggestByProofRefactoring(string scopeName,
    IReadOnlyList<DafnyConsolePrinter.VerificationRunResultPartialCopy> verificationRunResults) {
    foreach (var (dep, lAsserts) in ComputeAssertionsUsedByFact(scopeName, verificationRunResults)) {
      var factIsOnlyUsedByOneAssertion = lAsserts.Count == 1;
      if (!factIsOnlyUsedByOneAssertion) {
        continue;
      }

      DafnyConsolePrinter.AssertCmdPartialCopy cmd = null;
      foreach (var assert in lAsserts) {
        cmd = assert;
      }

      manager.ProofDependenciesById.TryGetValue(cmd.Id, out var consumer);

      if (consumer != null && (dep == consumer || consumer.Range.Intersects(dep.Range))) {
        continue;
      }

      RangeToken range = null;
      var factProvider = "";
      var factConsumer = "";
      var recommendation = "";
      var completeInformation = true;

      switch (dep) {
        case AssumedProofObligationDependency:
        case AssumptionDependency: {
            range = dep.Range;
            factProvider = "fact";
            recommendation = "moving it into";
            break;
          }
        case RequiresDependency: {
            range = dep.Range;
            factProvider = "requires clause";
            recommendation = "labelling it and revealing it in";
            break;
          }
        default: completeInformation = false; break;
      }

      switch (consumer) {
        case CallDependency call: {
            factConsumer = $"precondtion{(call.call.Method.Req.Count > 1 ? "s" : "")} of the method call {call.RangeString()}";
            break;
          }
        case ProofObligationDependency { ProofObligation: AssertStatementDescription }: {
            factConsumer = $"assertion {consumer.RangeString()}";
            break;
          }
        default: completeInformation = false; break;
      }

      if (completeInformation) {
        reporter.Info(MessageSource.Verifier, range,
          $"This {factProvider} was only used to prove the {factConsumer}. Consider {recommendation} a by-proof.");
      }
    }
  }

  private static Dictionary<ProofDependency, HashSet<DafnyConsolePrinter.AssertCmdPartialCopy>>
    ComputeAssertionsUsedByFact(string scopeName, IReadOnlyList<DafnyConsolePrinter.VerificationRunResultPartialCopy> vcResults) {
    var assertionsUsedByFact = manager.GetPotentialDependenciesForDefinition(scopeName)
      .Where(dep => dep is not EnsuresDependency)
      .ToDictionary(dep => dep, _ => new HashSet<DafnyConsolePrinter.AssertCmdPartialCopy> { });

    foreach (var (usedFacts, asserts) in vcResults.Select(r => (r.CoveredElements, r.Asserts))) {
      foreach (var factReference in usedFacts) {
        var factDependency = manager.GetFullIdDependency(factReference);
        var excludedDependencies = factDependency is EnsuresDependency;
        if (excludedDependencies) {
          continue;
        }

        assertionsUsedByFact.TryAdd(factDependency, new HashSet<DafnyConsolePrinter.AssertCmdPartialCopy>());

        bool NotSelfReferential(DafnyConsolePrinter.AssertCmdPartialCopy assert) =>
           !manager.ProofDependenciesById.TryGetValue(assert.Id, out var assertDependency)
                 || !(factDependency == assertDependency || factDependency is CallRequiresDependency req && req.call == assertDependency);

        assertionsUsedByFact[factDependency].UnionWith(asserts.Where(NotSelfReferential));
      }
    }

    return assertionsUsedByFact;
  }

  /// <summary>
  /// Some proof obligations that don't show up in the dependency list
  /// are innocuous. Either they come about because of internal Dafny
  /// design choices that the programmer has no control over, or they
  /// just aren't meaningful in context. This method identifies cases
  /// where it doesn't make sense to issue a warning. Many of these
  /// cases should perhaps be eliminated by changing the translator
  /// to not generate vacuous proof goals, but that may be a difficult
  /// change to make.
  /// </summary>
  /// <param name="verboseName"></param>
  /// <param name="dep">the dependency to examine</param>
  /// <returns>false to skip warning about the absence of this
  /// dependency, true otherwise</returns>
  private static bool ShouldWarnVacuous(string verboseName, ProofDependency dep) {
    if (dep is ProofObligationDependency poDep) {
      // Dafny generates some assertions about definite assignment whose
      // proofs are always vacuous. Since these aren't written by Dafny
      // programmers, it's safe to just skip them all.
      if (poDep.ProofObligation is DefiniteAssignment) {
        return false;
      }

      // Some proof obligations occur in a context that the Dafny programmer
      // doesn't have control of, so warning about vacuity isn't helpful.
      if (poDep.ProofObligation.ProvedOutsideUserCode) {
        return false;
      }

      // Don't warn about `assert false` being proved vacuously. If it's proved,
      // it must be vacuous, but it's also probably an attempt to prove that a
      // given branch is unreachable (often, but not always, in ghost code).
      var assertedExpr = poDep.ProofObligation.GetAssertedExpr(options);
      if (assertedExpr is not null &&
          Expression.IsBoolLiteral(assertedExpr, out var lit) &&
          lit == false) {
        return false;
      }

      if (poDep.ProofObligation is AssertStatementDescription { IsIntentionalContradiction: true }) {
        return false;
      }
    }

    // Ensures clauses are often proven vacuously during well-formedness checks.
    // There's unfortunately no way to identify these checks once Dafny has
    // been translated to Boogie other than looking at the name. This is a significant
    // limitation, because it means that function ensures clauses that are satisfied
    // only vacuously won't be reported. It would great if we could change the Boogie
    // encoding so that these unreachable-by-construction checks don't exist.
    if (verboseName.Contains("well-formedness") && dep is EnsuresDependency) {
      return false;
    }

    return true;
  }

  /// <summary>
  /// Some assumptions that don't show up in the dependency list
  /// are innocuous. In particular, `assume true` is often used
  /// as a place to attach attributes such as `{:split_here}`.
  /// Don't warn about such assumptions. Also don't warn about
  /// assumptions that aren't explicit (coming from `assume` or
  /// `assert` statements), for now, because they are difficult
  /// for the user to control.
  /// </summary>
  /// <param name="dep">the dependency to examine</param>
  /// <returns>false to skip warning about the absence of this
  /// dependency, true otherwise</returns>
  private static bool ShouldWarnUnused(ProofDependency dep) {
    if (dep is AssumptionDependency assumeDep) {
      if (assumeDep.Expr is not null &&
          Expression.IsBoolLiteral(assumeDep.Expr, out var lit) &&
          lit) {
        return false;
      }

      return assumeDep.WarnWhenUnused;
    }

    return true;
  }
}
