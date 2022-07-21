module {:extern @"Microsoft.Dafny.Helpers"} {:options "-functionSyntax:4"} Helpers {
  import opened System
  class {:extern "HelperString"} {:compile false} HelperString {
    static function Reindent(input: CsString, indentationBefore: CsString, lastIndentation: CsString): CsString
  }
}
module {:extern "System"} {:compile false} {:options "-functionSyntax:4"} System {
  trait {:extern " Collections.Generic.IEnumerator"} {:compile false} IEnumerator<T> {
    method MoveNext() returns (r: bool)
    function Current(): T reads this
  }
  type {:extern "Int32"} Int32(==)
  ghost function {:extern} GEq(left: Int32, right: Int32): (b: bool)
    ensures left == right ==> b
  type {:extern "String"} CsString(!new,==) {
    function Length(): Int32
  }
  class {:extern "String"} {:compile false} String {
    static function Concat(s1: CsString, s2: CsString): (r: CsString)
      ensures GEq(r.Length(), s1.Length())
      ensures GEq(r.Length(), s2.Length())
  }
}
module {:extern "Microsoft.Dafny"} {:compile false} {:options "-functionSyntax:4"} MicrosoftDafny {
  import opened System
  trait {:extern "IToken"} {:compile false} IToken {
    var val: CsString
    var LeadingTrivia: CsString
    var TrailingTrivia: CsString
    ghost var remainingTokens: nat
    var Next: IToken?

    ghost predicate Valid() reads * decreases remainingTokens {
      if Next == null then
        remainingTokens == 0
      else
        && remainingTokens > 0
        && Next.remainingTokens == remainingTokens - 1
        && Next.Valid()
    }
  }
}
module {:extern "Microsoft"} {:options "-functionSyntax:4"}  Microsoft {
  module {:extern "Dafny"} Dafny {
    module {:extern "TokenFormatter"} TokenFormatter {
      import opened MicrosoftDafny
      import opened System
      import opened Helpers

      newtype {:native} CInt = x: int | 0 <= x < 65535


      function {:extern} {:macro "new string(character, length)"} newString(character: char, length: CInt): CsString
      const {:extern "System", "String.Empty"} CsStringEmpty: CsString;

      trait ITokenIndentations {
        // Returns -1 if no indentation is set
        method GetIndentation(token: IToken, currentIndentation: CsString)
          returns (
            indentationBefore: CsString,
            lastIndentation: CsString,
            indentationAfter: CsString,
            wasSet: bool)
      }
      
      

      /** Prints the entire program without change */
      method printSource(firstToken: IToken) returns (s: CsString)
        requires firstToken.Valid()
      {
        var token: IToken? := firstToken;
        s := CsStringEmpty;
        while(token != null)
          decreases if token == null then 0 else token.remainingTokens + 1
          invariant token == null || token.Valid()
        {
          s := String.Concat(String.Concat(String.Concat(s, token.LeadingTrivia), token.val), token.TrailingTrivia);
          token := token.Next;
        }
      }

      /** Prints the entire program while fixing identation, based on a map */
      method printSourceReindent(firstToken: IToken, reindent: ITokenIndentations) returns (s: CsString)
        requires firstToken.Valid()
      {
        var token: IToken? := firstToken;
        s := CsStringEmpty;
        var currentIndent := CsStringEmpty;
        var currentIndentLast := CsStringEmpty;
        var isSet := false;
        var previousTrailingTrivia := CsStringEmpty;
        ghost var sLengthPrev := s.Length();
        while(token != null)
        decreases if token == null then 0 else token.remainingTokens + 1
        invariant token == null || token.Valid()
        invariant GEq(s.Length(), sLengthPrev);
        {
          sLengthPrev := s.Length();
          var indentationBefore, lastIndentation, indentationAfter, wasSet := reindent.GetIndentation(token, currentIndent);
          if(wasSet) {
            currentIndent := indentationBefore;
            currentIndentLast := lastIndentation;
            isSet := true;
          }
          var triviaSoFar := String.Concat(previousTrailingTrivia, token.LeadingTrivia);
          var newTrivia := if isSet then
            HelperString.Reindent(triviaSoFar, indentationBefore, lastIndentation) else triviaSoFar;
          // Had an error here: caught by an invariant
          //s := String.Concat(newTrivia, token.val);
          s := String.Concat(s, String.Concat(newTrivia, token.val));
          previousTrailingTrivia := token.TrailingTrivia;
          if(wasSet) {
            currentIndent := indentationAfter;
          }

          token := token.Next;
        }
        s := String.Concat(s, previousTrailingTrivia);
      }

      datatype State = Indent(i: Int32)

      /** Design of a Domain-specific language to specify the pre-indentation and post-indentation of tokens */
      /*trait TokenTriviaStateMachine {
        var initState: State
        var currentState: State

        function transitionMap(state: State, str: CsString): State

        method Transition(token: IToken) modifies this`currentState {
          var newState := transitionMap(currentState, token.val);
          currentState := newState;
        }

        method SetBeforeAfter(token: IToken, before: Int32, after: Int32)

        method Walkthrough(tokens: IEnumerator<IToken>)
          decreases *
          modifies this`currentState
        {
          currentState := initState;
          while true
            decreases * {
            var hasNext := tokens.MoveNext();
            if(!hasNext) {
              break;
            }
            var currentToken := tokens.Current();
            Transition(currentToken);
          }
        }
      }*/
    }
  }
}