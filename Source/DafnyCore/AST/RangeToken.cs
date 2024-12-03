using System;
using System.Text;
using Microsoft.Boogie;

namespace Microsoft.Dafny;

public class RangeToken : IOrigin {
  public bool IsMissingRange => false;

  public bool IsInherited(ModuleDefinition d) {
    return false;
  }

  public override bool Equals(object obj) {
    if (obj is RangeToken other) {
      return StartToken.Equals(other.StartToken) && EndToken.Equals(other.EndToken);
    }

    return false;
  }

  public override int GetHashCode() {
    return HashCode.Combine(StartToken.GetHashCode(), EndToken.GetHashCode());
  }

  public Token Center => StartToken; // TODO change to [optional] field
  public Token StartToken { get; private set; }
  public Token Centre { get; private set; }


  public Token EndToken => endToken ?? StartToken;
  public bool ContainsRange => true;

  public bool InclusiveEnd => endToken != null;

  public RangeToken(Token startToken, Token endToken) {
    StartToken = startToken;
    Centre = startToken; // TODO update
    this.endToken = endToken;
  }
  public int Length() {
    return EndToken.pos - StartToken.pos;
  }

  // TODO rename to Generated, and Token.NoToken to Token.Generated, and remove AutoGeneratedToken.
  public static RangeToken NoToken = new(Token.NoToken, Token.NoToken);
  private readonly Token endToken;

  public Uri Uri {
    get => Centre.Uri;
    set => Centre.Uri = value;
  }

  public string TrailingTrivia {
    get => Centre.TrailingTrivia;
    set => Centre.TrailingTrivia = value;
  }

  public string LeadingTrivia {
    get => Centre.LeadingTrivia;
    set => Centre.LeadingTrivia = value;
  }

  public Token Next {
    get => Centre.Next;
    set => Centre.Next = value;
  }

  public Token Prev {
    get => Centre.Prev;
    set => Centre.Prev = value;
  }

  public IOrigin WithVal(string val) {
    throw new NotImplementedException(); // TODO why is this needed?
  }

  public bool IsSourceToken => true;

  public int kind {
    get => Centre.kind;
    set => Centre.kind = value;
  }

  public int pos {
    get => Centre.pos;
    set => Centre.pos = value;
  }

  public int col {
    get => Centre.col;
    set => Centre.col = value;
  }

  public int line {
    get => Centre.line;
    set => Centre.line = value;
  }

  public string val {
    get => Centre.val;
    set => Centre.val = value;
  }

  public bool IsValid => Centre.IsValid;

  public BoogieRangeOrigin ToToken() {
    return new BoogieRangeOrigin(StartToken, EndToken, null);
  }

  public int CompareTo(IToken other) {
    return Centre.CompareTo(other);
  }

  public int CompareTo(IOrigin other) {
    return Centre.CompareTo((IToken)other);
  }
}