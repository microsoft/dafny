using System;
using System.Text;

namespace Microsoft.Dafny;

public class RangeToken : OriginWrapper {
  public override IOrigin StartToken => WrappedToken;

  public override IOrigin EndToken => endToken ?? StartToken;

  public bool InclusiveEnd => endToken != null;

  public override bool Equals(object obj) {
    if (obj is RangeToken other) {
      return StartToken.Equals(other.StartToken) && EndToken.Equals(other.EndToken);
    }
    return false;
  }

  public override int GetHashCode() {
    return HashCode.Combine(StartToken.GetHashCode(), EndToken.GetHashCode());
  }

  public DafnyRange ToDafnyRange(bool includeTrailingWhitespace = false) {
    var startLine = StartToken.line - 1;
    var startColumn = StartToken.col - 1;
    var endLine = EndToken.line - 1;
    int whitespaceOffset = 0;
    if (includeTrailingWhitespace) {
      string trivia = EndToken.TrailingTrivia;
      // Don't want to remove newlines or comments -- just spaces and tabs
      while (whitespaceOffset < trivia.Length && (trivia[whitespaceOffset] == ' ' || trivia[whitespaceOffset] == '\t')) {
        whitespaceOffset++;
      }
    }

    var endColumn = EndToken.col + (InclusiveEnd ? EndToken.val.Length : 0) + whitespaceOffset - 1;
    return new DafnyRange(
      new DafnyPosition(startLine, startColumn),
      new DafnyPosition(endLine, endColumn));
  }

  public RangeToken(IOrigin startToken, IOrigin endToken) : base(startToken) {
    this.endToken = endToken;
  }

  public string PrintOriginal() {
    var token = StartToken;
    var originalString = new StringBuilder();
    originalString.Append(token.val);
    while (token.Next != null && token.pos < EndToken.pos) {
      originalString.Append(token.TrailingTrivia);
      token = token.Next;
      originalString.Append(token.LeadingTrivia);
      originalString.Append(token.val);
    }

    return originalString.ToString();
  }

  public int Length() {
    return EndToken.pos - StartToken.pos;
  }

  // TODO rename to Generated, and Token.NoToken to Token.Generated, and remove AutoGeneratedToken.
  public static RangeToken NoToken = new(Token.NoToken, Token.NoToken);
  private readonly IOrigin endToken;

  public override IOrigin WithVal(string newVal) {
    throw new NotImplementedException();
  }

  public override bool IsSourceToken => this != NoToken;
}