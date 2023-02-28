using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Xunit.Abstractions;

namespace XUnitExtensions.Lit {
  public class LitCommandWithRedirection : ILitCommand {

    public static LitCommandWithRedirection Parse(Token[] tokens, LitTestConfiguration config) {
      var commandSymbol = tokens[0].Value;
      var argumentList = tokens[1..].ToList();
      var argumentValueList = argumentList.Select(t => t.Value).ToList();
      string? inputFile = null;
      string? outputFile = null;
      var appendOutput = false;
      string? errorFile = null;
      var redirectInIndex = argumentValueList.IndexOf("<");
      if (redirectInIndex >= 0) {
        inputFile = config.ApplySubstitutions(argumentValueList[redirectInIndex + 1]).Single();
        argumentList.RemoveRange(redirectInIndex, 2);
      }
      var redirectOutIndex = argumentValueList.IndexOf(">");
      if (redirectOutIndex >= 0) {
        outputFile = config.ApplySubstitutions(argumentValueList[redirectOutIndex + 1]).Single();
        argumentList.RemoveRange(redirectOutIndex, 2);
      }
      var redirectAppendIndex = argumentValueList.IndexOf(">>");
      if (redirectAppendIndex >= 0) {
        outputFile = config.ApplySubstitutions(argumentValueList[redirectAppendIndex + 1]).Single();
        appendOutput = true;
        argumentList.RemoveRange(redirectAppendIndex, 2);
      }
      var redirectErrorIndex = argumentValueList.IndexOf("2>");
      if (redirectErrorIndex >= 0) {
        errorFile = config.ApplySubstitutions(argumentValueList[redirectErrorIndex + 1]).Single();
        argumentList.RemoveRange(redirectErrorIndex, 2);
      }
      var redirectErrorAppendIndex = argumentValueList.IndexOf("2>>");
      if (redirectErrorAppendIndex >= 0) {
        errorFile = config.ApplySubstitutions(argumentValueList[redirectErrorAppendIndex + 1]).Single();
        appendOutput = true;
        argumentList.RemoveRange(redirectErrorAppendIndex, 2);
      }

      var arguments = argumentList.
        SelectMany(a => config.ApplySubstitutions(a.Value).Select(v => a with { Value = v })).
        SelectMany(a => a.Kind == Kind.MustGlob ? ExpandGlobs(a.Value) : new[] { a.Value }).ToList();

      if (config.Commands.TryGetValue(commandSymbol, out var command)) {
        return new LitCommandWithRedirection(command(arguments, config), inputFile, outputFile, appendOutput, errorFile);
      }

      commandSymbol = config.ApplySubstitutions(commandSymbol).Single();

      return new LitCommandWithRedirection(
        new ShellLitCommand(commandSymbol, arguments, config.PassthroughEnvironmentVariables),
        inputFile, outputFile, appendOutput, errorFile);
    }

    private readonly ILitCommand command;
    private readonly string? inputFile;
    private readonly string? outputFile;
    private readonly bool append;
    private readonly string? errorFile;

    public LitCommandWithRedirection(ILitCommand command, string? inputFile, string? outputFile, bool append, string? errorFile) {
      this.command = command;
      this.inputFile = inputFile;
      this.outputFile = outputFile;
      this.append = append;
      this.errorFile = errorFile;
    }

    public (int, string, string) Execute(ITestOutputHelper? outputHelper, TextReader? inReader, TextWriter? outWriter, TextWriter? errWriter) {
      var inputReader = inputFile != null ? new StreamReader(inputFile) : inReader;
      var outputWriter = outputFile != null ? new StreamWriter(outputFile, append) : outWriter;
      var errorWriter = errorFile != null ? new StreamWriter(errorFile, append) : errWriter;
      var result = command.Execute(outputHelper, inputReader, outputWriter, errorWriter);
      inputReader?.Close();
      outputWriter?.Close();
      errorWriter?.Close();
      return result;
    }

    protected static IEnumerable<string> ExpandGlobs(string chunk) {
      var matcher = new Matcher();
      matcher.AddInclude(chunk);
      var result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo("/")));
      return result.Files.Select(f => "/" + f.Path);
    }

    public override string ToString() {
      var builder = new StringBuilder();
      builder.Append(command);
      if (inputFile != null) {
        builder.Append(" < ");
        builder.Append(inputFile);
      }
      if (outputFile != null) {
        builder.Append(append ? " >> " : " > ");
        builder.Append(outputFile);
      }
      if (errorFile != null) {
        builder.Append(" 2> ");
        builder.Append(errorFile);
      }
      return builder.ToString();
    }
  }
}
