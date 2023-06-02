using System;
using System.IO;

namespace XUnitExtensions.Lit; 

public class NonUniformTestCommand : ILitCommand {
  private NonUniformTestCommand(string reason) {
    Reason = reason;
  }

  public string Reason { get; }

  public static ILitCommand Parse(string line, LitTestConfiguration config) {
    return new NonUniformTestCommand(line);
  }

  public (int, string, string) Execute(TextReader inputReader,
    TextWriter outputWriter, TextWriter errorWriter) {
    return (0, "", "");
  }
}