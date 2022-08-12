using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Dafny.Plugins;

namespace Microsoft.Dafny;


class CompileTargetOption : TargetOption {
  public new static readonly CompileTargetOption Instance = new();
  public override string LongName => "compileTarget";
}
class TargetOption : CommandLineOption<Compiler> {
  public static readonly TargetOption Instance = new();
  public override object GetDefaultValue(DafnyOptions options) => "cs";
  public override string LongName => "target";
  public override string ShortName => null;
  public override string Description => "missing";
  public override bool CanBeUsedMultipleTimes => false;

  public override ParseOptionResult Parse(DafnyOptions dafnyOptions, IEnumerable<string> arguments) {
    var target = arguments.First();
    var compilers = dafnyOptions.Plugins.SelectMany(p => p.GetCompilers()).ToList();
    var compiler = compilers.LastOrDefault(c => c.TargetId == target);
    if (compiler == null) {
      var known = String.Join(", ", compilers.Select(c => $"'{c.TargetId}' ({c.TargetLanguage})"));
      return new FailedOption($"No compiler found for compileTarget \"{target}\"; expecting one of {known}");
    }

    return new ParsedOption(1, target);
  }

  public override void PostProcess(DafnyOptions options) {
    options.Compiler = Get(options);
    base.PostProcess(options);
  }
}