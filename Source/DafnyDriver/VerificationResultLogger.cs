#nullable enable
//-----------------------------------------------------------------------------
//
// Copyright (C) Microsoft Corporation.  All Rights Reserved.
// Copyright by the contributors to the Dafny Project
// SPDX-License-Identifier: MIT
//
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using DafnyDriver.Commands;
using Microsoft.Boogie;
using Microsoft.VisualStudio.TestPlatform.Extensions.TrxLogger;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.Dafny {
  public record VerificationScope(string Name, Boogie.IToken Token);

  public record VerificationScopeResult(VerificationScope Scope, IReadOnlyList<VerificationTaskResult> Results);

  interface IVerificationResultFormatLogger {
    void LogScopeResults(VerificationScopeResult result);
    void Flush();
  }

  /// <summary>
  /// Utility to translate verification results into logs in several formats:
  ///  * TRX files, which can be understood and visualized by various .NET tools.
  ///  * CSV files, which are easier to parse and summarize. 
  ///  * human-readable text output.
  /// </summary>
  public class VerificationResultLogger {

    public static TestProperty ResourceCountProperty = TestProperty.Register("TestResult.ResourceCount", "TestResult.ResourceCount", typeof(int), typeof(TestResult));

    private readonly IList<IVerificationResultFormatLogger> formatLoggers = new List<IVerificationResultFormatLogger>();
    private readonly LocalTestLoggerEvents events;

    public VerificationResultLogger(DafnyOptions options, ProofDependencyManager depManager) {
      var loggerConfigs = options.Get(CommonOptionBag.VerificationLogFormat);
      // Provide just enough configuration for the loggers to work
      var parameters = new Dictionary<string, string> {
        ["TestRunDirectory"] = Constants.DefaultResultsDirectory
      };

      events = new LocalTestLoggerEvents();
      events.EnableEvents();
      foreach (var loggerConfig in loggerConfigs) {
        string loggerName;
        int semiColonIndex = loggerConfig.IndexOf(";", StringComparison.Ordinal);
        if (semiColonIndex >= 0) {
          loggerName = loggerConfig[..semiColonIndex];
          var parametersList = loggerConfig[(semiColonIndex + 1)..];
          foreach (string s in parametersList.Split(",")) {
            var equalsIndex = s.IndexOf("=", StringComparison.Ordinal);
            if (equalsIndex >= 0) {
              parameters.Add(s[..equalsIndex], s[(equalsIndex + 1)..]);
            } else {
              throw new ArgumentException($"unknown parameter to `/verificationLogger:csv`: {s}");
            }
          }
        } else {
          loggerName = loggerConfig;
        }

        if (loggerName == "trx") {
          var logger = new TrxLogger();
          logger.Initialize(events, parameters!);
        } else if (loggerName == "csv") {
          var csvLogger = new CSVTestLogger(options.OutputWriter);
          csvLogger.Initialize(events, parameters);
        } else if (loggerName == "json") {
          // This logger doesn't implement the ITestLogger interface because
          // it uses information that's tricky to encode in a TestResult.
          var jsonLogger = new JsonVerificationLogger(depManager, options.OutputWriter);
          jsonLogger.Initialize(parameters);
          formatLoggers.Add(jsonLogger);
        } else if (loggerName == "text") {
          // This logger doesn't implement the ITestLogger interface because
          // it uses information that's tricky to encode in a TestResult.
          var textLogger = new TextVerificationLogger(depManager, options.OutputWriter);
          textLogger.Initialize(parameters);
          formatLoggers.Add(textLogger);
        } else {
          throw new ArgumentException($"unsupported verification logger config: {loggerConfig}");
        }
      }
    }

    public void Report(CanVerifyResult canVerifyResult) {
      var scopeResults =
        canVerifyResult.Results
          .GroupBy(v => new VerificationScope(v.Task.ScopeId, v.Task.ScopeToken))
          .Select(g => new VerificationScopeResult(g.Key, g.ToList())).ToList();
      foreach (var scopeResult in scopeResults) {
        foreach (var formatLogger in formatLoggers) {
          formatLogger.LogScopeResults(scopeResult);
        }
        foreach (var result in VerificationToTestResults(scopeResult)) {
          events.RaiseTestResult(new TestResultEventArgs(result));
        }
      }
    }

    public void Finish() {
      foreach (var formatLogger in formatLoggers) {
        formatLogger.Flush();
      }
      events.RaiseTestRunComplete(new TestRunCompleteEventArgs(
        new TestRunStatistics(),
        false, false, null, null, new TimeSpan()
      ));
    }

    private static IEnumerable<TestResult> VerificationToTestResults(VerificationScopeResult result) {
      var testResults = new List<TestResult>();

      var verificationScope = result.Scope;
      foreach (var vcResult in result.Results.OrderBy(r => r.Result.VcNum).
                 Select(r => r.Result)) {
        var name = result.Results.Count() > 1
          ? verificationScope.Name + $" (assertion batch {vcResult.VcNum})"
          : verificationScope.Name;
        var testCase = new TestCase {
          FullyQualifiedName = name,
          ExecutorUri = new Uri("executor://dafnyverifier/v1"),
          Source = ((IToken)verificationScope.Token).Uri.LocalPath
        };
        var testResult = new TestResult(testCase) {
          StartTime = vcResult.StartTime,
          Duration = vcResult.RunTime
        };
        testResult.SetPropertyValue(ResourceCountProperty, vcResult.ResourceCount);
        if (vcResult.Outcome == SolverOutcome.Valid) {
          testResult.Outcome = TestOutcome.Passed;
        } else {
          testResult.Outcome = TestOutcome.Failed;
          testResult.ErrorMessage = vcResult.Outcome.ToString();
        }
        testResults.Add(testResult);
      }

      return testResults;
    }
  }
}
