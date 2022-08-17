namespace Microsoft.Dafny;

public class OutOption : OutputOption {
  public static readonly OutOption Instance = new();
  public override string LongName => "out";
}

public class OutputOption : StringOption {
  public static readonly OutputOption Instance = new();
  public override object DefaultValue => null;
  public override string LongName => "output";
  public override string ShortName => "o";
  public override string ArgumentName => "file";
  public override string Category => "Compilation options";
  public override string Description => "filename and location for the generated target language files";
  public override string PostProcess(DafnyOptions options) {
    options.DafnyPrintCompiledFile = Get(options);
    return null;
  }
}
