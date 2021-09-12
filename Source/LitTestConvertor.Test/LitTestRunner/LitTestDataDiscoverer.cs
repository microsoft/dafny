using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;
using XUnitExtensions;

namespace DafnyDriver.Test {
  public class LitTestDataDiscoverer : FileDataDiscoverer {

    private readonly LitTestConverter.LitTestConvertor convertor = new();

    public override bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod) {
      return true;
    }

    protected override IEnumerable<object[]> FileData(IAttributeInfo attributeInfo, IMethodInfo testMethod, string fileName) {
      var invokeDirectly = attributeInfo.GetNamedArgument<bool>(nameof(LitTestDataAttribute.InvokeCliDirectly));
      var basePath = GetBasePath(attributeInfo, testMethod);
      try {
        var (testCases, _) = convertor.ConvertLitCommands(basePath, fileName, invokeDirectly, File.ReadLines(fileName));
        return testCases.Select(testCase => new[] { testCase });
      } catch (Exception e) {
        var dummyTestCase = new DafnyTestCase(basePath, fileName, new(), new(), null, false);
        var skippedCase = new FileTheoryDataRow(dummyTestCase) {
          SourceInformation = new SourceInformation() { FileName = fileName, LineNumber = 0},
          TestDisplayName = basePath,
          Skip = $"Exception: {e}"
        };
        return new[] { new[] { skippedCase } };
      }
    }
  }
}

[DataDiscoverer("DafnyDriver.Test.LitTestDataDiscoverer", "LitTestConvertor.Test")]
public class LitTestDataAttribute : FileDataAttribute {
  public bool InvokeCliDirectly { get; set; }
}