using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Dafny;

/// <summary>
/// Contains a string value and a range
/// Why not just store an IToken and use the value from there instead of storing a separate string?
/// Because generated names don't have an associated token
/// </summary>
public class Name : RangeNode {

  public Name Prepend(string prefix) {
    return new Name(RangeToken.MakeAutoGenerated(), prefix + Value);
  }

  public Name Append(string suffix) {
    return new Name(RangeToken, Value + suffix);
  }

  public Name Update(Func<string, string> update) {
    return new Name(RangeToken, update(Value));
  }
  public string Value { get; set; }

  public Name(Cloner cloner, Name original) : base(cloner, original) {
    Value = original.Value;
  }

  public Name(IOrigin range, string value) : base(range) {
    this.Value = value;
  }

  public Name(Token token) : this(new RangeToken(token, token), token.val) {
  }

  public Name(string value) : base(Microsoft.Dafny.RangeToken.NoToken) {
    this.Value = value;
  }

  public override IEnumerable<INode> Children => Enumerable.Empty<Node>();
  public override IEnumerable<INode> PreResolveChildren => Children;

  public Name Clone(Cloner cloner) {
    return new Name(cloner.Tok(RangeToken), Value);
  }

  public override string ToString() => Value;
}