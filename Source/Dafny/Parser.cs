using System.Collections.Generic;
using System.Numerics;
using Microsoft.Boogie;
using System.IO;
using System.Text;


using System;
using System.Diagnostics.Contracts;

namespace Microsoft.Dafny {



public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _digits = 2;
	public const int _hexdigits = 3;
	public const int _decimaldigits = 4;
	public const int _arrayToken = 5;
	public const int _arrayToken_q = 6;
	public const int _bvToken = 7;
	public const int _bool = 8;
	public const int _char = 9;
	public const int _int = 10;
	public const int _nat = 11;
	public const int _real = 12;
	public const int _ORDINAL = 13;
	public const int _object = 14;
	public const int _object_q = 15;
	public const int _string = 16;
	public const int _set = 17;
	public const int _iset = 18;
	public const int _multiset = 19;
	public const int _seq = 20;
	public const int _map = 21;
	public const int _imap = 22;
	public const int _charToken = 23;
	public const int _stringToken = 24;
	public const int _colon = 25;
	public const int _comma = 26;
	public const int _verticalbar = 27;
	public const int _doublecolon = 28;
	public const int _gets = 29;
	public const int _boredSmiley = 30;
	public const int _bullet = 31;
	public const int _dot = 32;
	public const int _backtick = 33;
	public const int _semi = 34;
	public const int _darrow = 35;
	public const int _assume = 36;
	public const int _calc = 37;
	public const int _case = 38;
	public const int _then = 39;
	public const int _else = 40;
	public const int _as = 41;
	public const int _by = 42;
	public const int _in = 43;
	public const int _decreases = 44;
	public const int _invariant = 45;
	public const int _function = 46;
	public const int _predicate = 47;
	public const int _inductive = 48;
	public const int _twostate = 49;
	public const int _copredicate = 50;
	public const int _lemma = 51;
	public const int _static = 52;
	public const int _protected = 53;
	public const int _import = 54;
	public const int _export = 55;
	public const int _class = 56;
	public const int _trait = 57;
	public const int _datatype = 58;
	public const int _codatatype = 59;
	public const int _var = 60;
	public const int _const = 61;
	public const int _newtype = 62;
	public const int _type = 63;
	public const int _iterator = 64;
	public const int _method = 65;
	public const int _colemma = 66;
	public const int _constructor = 67;
	public const int _modifies = 68;
	public const int _reads = 69;
	public const int _requires = 70;
	public const int _ensures = 71;
	public const int _ghost = 72;
	public const int _witness = 73;
	public const int _lbracecolon = 74;
	public const int _lbrace = 75;
	public const int _rbrace = 76;
	public const int _lbracket = 77;
	public const int _rbracket = 78;
	public const int _openparen = 79;
	public const int _closeparen = 80;
	public const int _openAngleBracket = 81;
	public const int _closeAngleBracket = 82;
	public const int _eq = 83;
	public const int _neq = 84;
	public const int _neqAlt = 85;
	public const int _star = 86;
	public const int _notIn = 87;
	public const int _ellipsis = 88;
	public const int _reveal = 89;
	public const int maxT = 157;

	const bool _T = true;
	const bool _x = false;
	const int minErrDist = 2;

	public Scanner/*!*/ scanner;
	public Errors/*!*/  errors;

	public Token/*!*/ t;    // last recognized token
	public Token/*!*/ la;   // lookahead token
	int errDist = minErrDist;

readonly Expression/*!*/ dummyExpr;
readonly AssignmentRhs/*!*/ dummyRhs;
readonly FrameExpression/*!*/ dummyFrameExpr;
readonly Statement/*!*/ dummyStmt;
readonly Statement/*!*/ dummyIfStmt;
readonly Include theInclude;
readonly ModuleDecl theModule;
readonly BuiltIns theBuiltIns;
readonly bool theVerifyThisFile;
int anonymousIds = 0;

/// <summary>
/// Holds the modifiers given for a declaration
///
/// Not all modifiers are applicable to all kinds of declarations.
/// Errors are given when a modify does not apply.
/// We also record the tokens for the specified modifiers so that
/// they can be used in error messages.
/// </summary>
struct DeclModifierData {
  public bool IsAbstract;
  public IToken AbstractToken;
  public bool IsGhost;
  public IToken GhostToken;
  public bool IsStatic;
  public IToken StaticToken;
  public bool IsProtected;
  public IToken ProtectedToken;
}

// Check that token has not been set, then set it.
public void CheckAndSetToken(ref IToken token)
{
    if (token != null) {
      SemErr(t, "Duplicate declaration modifier: " + t.val);
    }
    token = t;
}

/// <summary>
// A flags type used to tell what declaration modifiers are allowed for a declaration.
/// </summary>
[Flags]
enum AllowedDeclModifiers {
  None = 0,
  Abstract = 1,
  Ghost = 2,

  // Means ghost not allowed because already implicitly ghost.
  AlreadyGhost = 4,
  Static = 8,
  Protected = 16,
};

/// <summary>
/// Check the declaration modifiers against those that are allowed.
///
/// The 'allowed' parameter specifies which declaratio modifiers are allowed.
/// The 'declCaption' parameter should be a string describing the kind of declaration.
/// It is used in error messages.
/// Any declaration modifiers that are present but not allowed are cleared.
///</summary>
void CheckDeclModifiers(DeclModifierData dmod, string declCaption, AllowedDeclModifiers allowed)
{
  if (dmod.IsAbstract && ((allowed & AllowedDeclModifiers.Abstract) == 0)) {
    SemErr(dmod.AbstractToken, declCaption + " cannot be declared 'abstract'.");
    dmod.IsAbstract = false;
  }
  if (dmod.IsGhost) {
    if ((allowed & AllowedDeclModifiers.AlreadyGhost) != 0) {
      SemErr(dmod.GhostToken, declCaption + " cannot be declared ghost (they are 'ghost' by default).");
      dmod.IsGhost = false;
    } else if ((allowed & AllowedDeclModifiers.Ghost) == 0) {
      SemErr(dmod.GhostToken, declCaption + " cannot be declared 'ghost'.");
      dmod.IsGhost = false;
    }
  }
  if (dmod.IsStatic && ((allowed & AllowedDeclModifiers.Static) == 0)) {
    SemErr(dmod.StaticToken, declCaption + " cannot be declared 'static'.");
    dmod.IsStatic = false;
  }
  if (dmod.IsProtected && ((allowed & AllowedDeclModifiers.Protected) == 0)) {
    SemErr(dmod.ProtectedToken, declCaption + " cannot be declared 'protected'.");
    dmod.IsProtected = false;
  }
}

///<summary>
/// Parses top-level things (modules, classes, datatypes, class members) from "filename"
/// and appends them in appropriate form to "module".
/// Returns the number of parsing errors encountered.
/// Note: first initialize the Scanner.
///</summary>
public static int Parse (string/*!*/ filename, Include include, ModuleDecl module, BuiltIns builtIns, Errors/*!*/ errors, bool verifyThisFile=true) /* throws System.IO.IOException */ {
  Contract.Requires(filename != null);
  Contract.Requires(module != null);
  string s;
  if (filename == "stdin.dfy") {
    s = Microsoft.Boogie.ParserHelper.Fill(System.Console.In, new List<string>());
    return Parse(s, filename, filename, include, module, builtIns, errors, verifyThisFile);
  } else {
    using (System.IO.StreamReader reader = new System.IO.StreamReader(filename)) {
      s = Microsoft.Boogie.ParserHelper.Fill(reader, new List<string>());
      return Parse(s, filename, DafnyOptions.Clo.UseBaseNameForFileName ? Path.GetFileName(filename) : filename, include, module, builtIns, errors, verifyThisFile);
    }
  }
}
///<summary>
/// Parses top-level things (modules, classes, datatypes, class members)
/// and appends them in appropriate form to "module".
/// Returns the number of parsing errors encountered.
/// Note: first initialize the Scanner.
///</summary>
public static int Parse (string/*!*/ s, string/*!*/ fullFilename, string/*!*/ filename, ModuleDecl module, BuiltIns builtIns, ErrorReporter reporter, bool verifyThisFile=true) {
  Contract.Requires(s != null);
  Contract.Requires(filename != null);
  Contract.Requires(module != null);
  Errors errors = new Errors(reporter);
  return Parse(s, fullFilename, filename, null, module, builtIns, errors, verifyThisFile);
}

public static Parser SetupParser(string/*!*/ s, string/*!*/ fullFilename, string/*!*/ filename, Include include, ModuleDecl module,
                                 BuiltIns builtIns, Errors/*!*/ errors, bool verifyThisFile=true) {
  Contract.Requires(s != null);
  Contract.Requires(filename != null);
  Contract.Requires(module != null);
  Contract.Requires(errors != null);
  byte[]/*!*/ buffer = cce.NonNull( UTF8Encoding.Default.GetBytes(s));
  MemoryStream ms = new MemoryStream(buffer,false);
  Scanner scanner = new Scanner(ms, errors, fullFilename, filename);
  return new Parser(scanner, errors, include, module, builtIns, verifyThisFile);
}

public static Expression ParseExpression(string/*!*/ s, string/*!*/ fullFilename, string/*!*/ filename, Include include, ModuleDecl module,
                                         BuiltIns builtIns, Errors/*!*/ errors, bool verifyThisFile=true) {
  Parser parser = SetupParser(s, fullFilename, filename, include, module, builtIns, errors, verifyThisFile);
  parser.la = new Token();
  parser.la.val = "";
  parser.Get();
  Expression e;
  parser.Expression(out e, true, true, true);
  return e;
}

///<summary>
/// Parses top-level things (modules, classes, datatypes, class members)
/// and appends them in appropriate form to "module".
/// Returns the number of parsing errors encountered.
/// Note: first initialize the Scanner with the given Errors sink.
///</summary>
public static int Parse (string/*!*/ s, string/*!*/ fullFilename, string/*!*/ filename, Include include, ModuleDecl module,
                         BuiltIns builtIns, Errors/*!*/ errors, bool verifyThisFile=true) {
  Parser parser = SetupParser(s, fullFilename, filename, include, module, builtIns, errors, verifyThisFile);
  parser.Parse();
  return parser.errors.ErrorCount;
}

public Parser(Scanner/*!*/ scanner, Errors/*!*/ errors, Include include, ModuleDecl module, BuiltIns builtIns, bool verifyThisFile=true)
  : this(scanner, errors)  // the real work
{
  // initialize readonly fields
  dummyExpr = new LiteralExpr(Token.NoToken);
  dummyRhs = new ExprRhs(dummyExpr, null);
  dummyFrameExpr = new FrameExpression(dummyExpr.tok, dummyExpr, null);
  dummyStmt = new ReturnStmt(Token.NoToken, Token.NoToken, null);
  var dummyBlockStmt = new BlockStmt(Token.NoToken, Token.NoToken, new List<Statement>());
  dummyIfStmt = new IfStmt(Token.NoToken, Token.NoToken, false, null, dummyBlockStmt, null);
  theInclude = include; // the "include" that includes this file
  theModule = module;
  theBuiltIns = builtIns;
  theVerifyThisFile = verifyThisFile;
}

bool IsLabel(bool allowLabel) {
  if (!allowLabel) {
    return false;
  }
  scanner.ResetPeek();
  IToken x = scanner.Peek();
  return (la.kind == _ident || la.kind == _digits) && x.kind == _colon;
}

bool IsAlternative() {
  IToken x = scanner.Peek();
  return (la.kind == _lbrace && x.kind == _case)
      || la.kind == _case;
}

bool FollowedByColon() {
  IToken x = la;
  while (x.kind == _ident || x.kind == _openparen)
     x = scanner.Peek();
  return x.kind == _colon;
}

bool IsGets() {
  return la.kind == _gets;
}

// an existential guard starts with an identifier and is then followed by
// * a colon (if the first identifier is given an explicit type),
// * a comma (if there's a list of bound variables and the first one is not given an explicit type),
// * a start-attribute (if there's one bound variable and it is not given an explicit type and there are attributes), or
// * a bored smiley (if there's one bound variable and it is not given an explicit type).
bool IsExistentialGuard() {
  scanner.ResetPeek();
  if (la.kind == _ident) {
    Token x = scanner.Peek();
    if (x.kind == _colon || x.kind == _comma || x.kind == _boredSmiley || x.kind == _lbracecolon) {
      return true;
    }
  }
  return false;
}

bool IsLoopSpec() {
  return la.kind == _invariant || la.kind == _decreases || la.kind == _modifies;
}

bool IsWitness() {
  scanner.ResetPeek();
  if (la.kind == _witness) {
    return true;
  } else if (la.kind == _ghost) {
    Token x = scanner.Peek();
    return x.kind == _witness;
  }
  return false;
}

bool IsFunctionDecl() {
  switch (la.kind) {
    case _function:
    case _predicate:
    case _copredicate:
      return true;
    case _inductive:
      return scanner.Peek().kind != _lemma;
    case _twostate:
      var x = scanner.Peek();
      return x.kind == _function || x.kind == _predicate;
    default:
      return false;
  }
}

bool IsParenStar() {
  scanner.ResetPeek();
  Token x = scanner.Peek();
  return la.kind == _openparen && x.kind == _star;
}

bool IsEquivOp() {
  return la.val == "<==>" || la.val == "\u21d4";
}
bool IsImpliesOp() {
  return la.val == "==>" || la.val == "\u21d2";
}
bool IsExpliesOp() {
  return la.val == "<==" || la.val == "\u21d0";
}
bool IsAndOp() {
  return la.val == "&&" || la.val == "\u2227";
}
bool IsOrOp() {
  return la.val == "||" || la.val == "\u2228";
}
bool IsBitwiseAndOp() {
  return la.val == "&";
}
bool IsBitwiseOrOp() {
  return la.val == "|";
}
bool IsBitwiseXorOp() {
  return la.val == "^";
}
bool IsBitwiseOp() {
  return IsBitwiseAndOp() || IsBitwiseOrOp() || IsBitwiseXorOp();
}
bool IsAs() {
  return la.kind == _as;
}
bool IsRelOp() {
  return la.val == "=="
      || la.val == "<"
      || la.val == ">"
      || la.val == "<="
      || la.val == ">="
      || la.val == "!="
      || la.val == "in"
      || la.kind == _notIn
      || la.val =="!"
      || la.val == "\u2260"
      || la.val == "\u2264"
      || la.val == "\u2265";
}
bool IsShiftOp() {
  if (la.kind == _openAngleBracket) {
  } else if (la.kind == _closeAngleBracket) {
  } else {
    return false;
  }
  scanner.ResetPeek();
  var x = scanner.Peek();
  if (x.kind != la.kind) {
    return false;
  }
  return x.pos == la.pos + 1;  // return true only if the tokens are adjacent to each other
}
bool IsAddOp() {
  return la.val == "+" || la.val == "-";
}
bool IsMulOp() {
  return la.kind == _star || la.val == "/" || la.val == "%";
}
bool IsQSep() {
  return la.kind == _doublecolon || la.kind == _bullet;
}

bool IsNonFinalColon() {
  return la.kind == _colon && scanner.Peek().kind != _rbracket;
}
bool IsMapDisplay() {
  scanner.ResetPeek();
  return la.kind == _map && scanner.Peek().kind == _lbracket;
}
bool IsIMapDisplay() {
  scanner.ResetPeek();
  return la.kind == _imap && scanner.Peek().kind == _lbracket;
}
bool IsISetDisplay() {
  scanner.ResetPeek();
  return la.kind == _iset && scanner.Peek().kind == _lbrace;
}

bool IsSuffix() {
  return la.kind == _dot || la.kind == _lbracket || la.kind == _openparen;
}

string UnwildIdent(string x, bool allowWildcardId) {
  if (x.StartsWith("_")) {
    if (allowWildcardId && x.Length == 1) {
      return "_v" + anonymousIds++;
    } else {
      SemErr("cannot declare identifier beginning with underscore");
    }
  }
  return x;
}

bool IsLambda(bool allowLambda)
{
  if (!allowLambda) {
    return false;
  }
  scanner.ResetPeek();
  Token x;
  // peek at what might be a signature of a lambda expression
  if (la.kind == _ident) {
    // cool, that's the entire candidate signature
  } else if (la.kind != _openparen) {
    return false;  // this is not a lambda expression
  } else {
    int identCount = 0;
    x = scanner.Peek();
    while (x.kind != _closeparen) {
      if (identCount != 0) {
        if (x.kind != _comma) {
          return false;  // not the signature of a lambda
        }
        x = scanner.Peek();
      }
      if (x.kind != _ident) {
        return false;  // not a lambda expression
      }
      identCount++;
      x = scanner.Peek();
      if (x.kind == _colon) {
        // a colon belongs only in a lamdba signature, so this must be a lambda (or something ill-formed)
        return true;
      }
    }
  }
  // What we have seen so far could have been a lambda signature or could have been some
  // other expression (in particular, an identifier, a parenthesized identifier, or a
  // tuple all of whose subexpressions are identifiers).
  // It is a lambda expression if what follows is something that must be a lambda.
  x = scanner.Peek();
  return x.kind == _darrow || x.kind == _reads || x.kind == _requires;
}

bool IsIdentParen() {
  scanner.ResetPeek();
  Token x = scanner.Peek();
  return la.kind == _ident && x.kind == _openparen;
}

/* Used to disambiguate the LHS of a VarDeclStmt. If it looks like the start of a CasePattern,
 * we consider it to be a LetStmt. But if we are looking at a simple identifier, then we
 * consider it to be a VarDeclStmt.
 */
bool IsLetStmt() {
  return IsIdentParen() || la.kind == _openparen;
}

bool IsIdentColonOrBar() {
  Token x = scanner.Peek();
  return la.kind == _ident && (x.kind == _colon || x.kind == _verticalbar);
}

bool SemiFollowsCall(bool allowSemi, Expression e) {
  return allowSemi && la.kind == _semi && (e is ApplySuffix || (e is RevealExpr && (((RevealExpr)e).Expr is ApplySuffix)));
}

bool IsNotEndOfCase() {
  return la.kind != _EOF && la.kind != _rbrace && la.kind != _case;
}

/* The following is the largest lookahead there is. It needs to check if what follows
 * can be nothing but "<" Type { "," Type } ">".
 */
bool IsGenericInstantiation(bool inExpressionContext) {
  scanner.ResetPeek();
  if (!inExpressionContext) {
    return la.kind == _openAngleBracket;
  }
  IToken pt = la;
  if (!IsTypeList(ref pt)) {
    return false;
  }
  /* There are ambiguities in the parsing.  For example:
   *     F( a < b , c > (d) )
   * can either be a unary function F whose argument is a function "a" with type arguments "<b,c>" and
   * parameter "d", or can be a binary function F with the two boolean arguments "a < b" and "c > (d)".
   * To make the situation a little better, we (somewhat heuristically) look at the character that
   * follows the ">".  Note that if we, contrary to a user's intentions, pick "a<b,c>" out as a function
   * with a type instantiation, the user can disambiguate it by making sure the ">" sits inside some
   * parentheses, like:
   *     F( a < b , (c > (d)) )
   */
  switch (pt.kind) {
    case _dot:  // here, we're sure it must have been a type instantiation we saw, because an expression cannot begin with dot
    case _openparen:  // it was probably a type instantiation of a function/method
    case _lbracket:  // it is possible that it was a type instantiation
    case _lbrace:  // it was probably a type instantiation of a function/method
    // In the following cases, we're sure we must have read a type instantiation that just ended an expression
    case _closeparen:
    case _rbracket:
    case _rbrace:
    case _comma:
    case _semi:
    case _then:
    case _else:
    case _case:
    case _eq:
    case _neq:
    case _neqAlt:
    case _as:
    case _by:
    case _in:
    case _openAngleBracket:
    case _closeAngleBracket:
    case _EOF:
    // (specification clauses that can follow an expression)
    case _decreases:
    case _modifies:
    case _reads:
    case _requires:
    case _ensures:
    case _invariant:
    case _witness:
    // (top-level declarations that can follow an expression)
    case _function:
    case _predicate:
    case _inductive:
    case _twostate:
    case _lemma:
    case _copredicate:
    case _ghost:
    case _static:
    case _protected:
    case _import:
    case _export:
    case _class:
    case _trait:
    case _datatype:
    case _codatatype:
    case _var:
    case _const:
    case _newtype:
    case _type:
    case _iterator:
    case _method:
    case _colemma:
    case _constructor:
      return true;
    default:
      return false;
  }
}
/* Returns true if the next thing is of the form:
 *     "<" Type { "," Type } ">"
 */
bool IsTypeList(ref IToken pt) {
  if (pt.kind != _openAngleBracket) {
    return false;
  }
  pt = scanner.Peek();
  return IsTypeSequence(ref pt, _closeAngleBracket);
}
/* Returns true if the next thing is of the form:
 *     Type { "," Type }
 * followed by an endBracketKind.
 */
bool IsTypeSequence(ref IToken pt, int endBracketKind) {
  while (true) {
    if (!IsType(ref pt)) {
      return false;
    }
    if (pt.kind == endBracketKind) {
      // end of type list
      pt = scanner.Peek();
      return true;
    } else if (pt.kind == _comma) {
      // type list continues
      pt = scanner.Peek();
    } else {
      // not a type list
      return false;
    }
  }
}
bool IsType(ref IToken pt) {
  switch (pt.kind) {
    case _bool:
    case _char:
    case _nat:
    case _int:
    case _real:
    case _ORDINAL:
    case _string:
    case _object_q:
    case _object:
      pt = scanner.Peek();
      return true;
    case _arrayToken:
    case _arrayToken_q:
    case _bvToken:
    case _set:
    case _iset:
    case _multiset:
    case _seq:
    case _map:
    case _imap:
      pt = scanner.Peek();
      return pt.kind != _openAngleBracket || IsTypeList(ref pt);
    case _ident:
      while (true) {
        // invariant: next token is an ident
        pt = scanner.Peek();
        if (pt.kind == _openAngleBracket && !IsTypeList(ref pt)) {
          return false;
        }
        if (pt.kind != _dot) {
          // end of the type
          return true;
        }
        pt = scanner.Peek();  // get the _dot
        if (pt.kind != _ident) {
          return false;
        }
      }
    case _openparen:
      pt = scanner.Peek();
      if (pt.kind == _closeparen) {
        // end of type list
        pt = scanner.Peek();
        return true;
      }
      return IsTypeSequence(ref pt, _closeparen);
    default:
      return false;
  }
}


void ConvertKeywordTokenToIdent() {
  var oldKind = la.kind;
  la.kind = _ident;

  // call CheckLiteral with la
  var origT = t;
  t = la;
  scanner.CheckLiteral();
  t = origT;

  if (la.kind != _ident) {
    // it has been changed by CheckLiteral, which means it was a keyword
    la.kind = _ident;  // convert it to an ident
  } else {
    // la was something other than a keyword
    la.kind = oldKind;
  }
}

int StringToInt(string s, int defaultValue, string errString) {
  Contract.Requires(s != null);
  Contract.Requires(errString != null);
  try {
    if (s != "") {
      defaultValue = int.Parse(s);
    }
  } catch (System.OverflowException) {
    SemErr(string.Format("sorry, {0} ({1}) are not supported", errString, s));
  }
  return defaultValue;
}

/*--------------------------------------------------------------------------*/


	public Parser(Scanner/*!*/ scanner, Errors/*!*/ errors) {
		this.scanner = scanner;
		this.errors = errors;
		Token/*!*/ tok = new Token();
		tok.val = "";
		this.la = tok;
		this.t = new Token(); // just to satisfy its non-null constraint
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.filename, la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string/*!*/ msg) {
		Contract.Requires(msg != null);
		if (errDist >= minErrDist) errors.SemErr(t, msg);
		errDist = 0;
	}

	public void SemErr(IToken/*!*/ tok, string/*!*/ msg) {
	  Contract.Requires(tok != null);
	  Contract.Requires(msg != null);
	  errors.SemErr(tok, msg);
	}

	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}

	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}

	bool StartOf (int s) {
		return set[s, la.kind];
	}

	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}


	void Dafny() {
		List<MemberDecl/*!*/> membersDefaultClass = new List<MemberDecl/*!*/>();
		// to support multiple files, create a default module only if theModule is null
		DefaultModuleDecl defaultModule = (DefaultModuleDecl)((LiteralModuleDecl)theModule).ModuleDef;
		// theModule should be a DefaultModuleDecl (actually, the singular DefaultModuleDecl)
		Contract.Assert(defaultModule != null);

		while (la.kind == 90) {
			Get();
			Expect(24);
			{
			 string parsedFile = scanner.FullFilename;
			 bool isVerbatimString;
			 string includedFile = Util.RemoveParsedStringQuotes(t.val, out isVerbatimString);
			 includedFile = Util.RemoveEscaping(includedFile, isVerbatimString);
			 string fullPath = includedFile;
			 if (!Path.IsPathRooted(includedFile)) {
			   string basePath = Path.GetDirectoryName(parsedFile);
			   includedFile = Path.Combine(basePath, includedFile);
			   fullPath = Path.GetFullPath(includedFile);
			 }
			 defaultModule.Includes.Add(new Include(t, parsedFile, includedFile, fullPath));
			}

		}
		while (StartOf(1)) {
			TopDecl(defaultModule, membersDefaultClass, /* isTopLevel */ true, /* isAbstract */ false);
		}
		DefaultClassDecl defaultClass = null;
		foreach (TopLevelDecl topleveldecl in defaultModule.TopLevelDecls) {
		 defaultClass = topleveldecl as DefaultClassDecl;
		 if (defaultClass != null) {
		   defaultClass.Members.AddRange(membersDefaultClass);
		   break;
		 }
		}
		if (defaultClass == null) { // create the default class here, because it wasn't found
		 defaultClass = new DefaultClassDecl(defaultModule, membersDefaultClass);
		 defaultModule.TopLevelDecls.Add(defaultClass);
		}
		Expect(0);
	}

	void TopDecl(ModuleDefinition module, List<MemberDecl/*!*/> membersDefaultClass, bool isTopLevel, bool isAbstract ) {
		DeclModifierData dmod = new DeclModifierData(); ModuleDecl submodule;
		ClassDecl/*!*/ c; DatatypeDecl/*!*/ dt; TopLevelDecl td; IteratorDecl iter;
		TraitDecl/*!*/ trait;

		while (StartOf(2)) {
			DeclModifier(ref dmod);
		}
		switch (la.kind) {
		case 54: case 55: case 92: {
			SubModuleDecl(dmod, module, out submodule);
			var litmod = submodule as LiteralModuleDecl;
			if (litmod != null && litmod.ModuleDef.PrefixIds.Count != 0) {
			 var tup = new Tuple<List<IToken>, LiteralModuleDecl>(litmod.ModuleDef.PrefixIds, litmod);
			 module.PrefixNamedModules.Add(tup);
			} else {
			 module.TopLevelDecls.Add(submodule);
			}
			break;
		}
		case 56: {
			ClassDecl(dmod, module, out c);
			module.TopLevelDecls.Add(c);
			break;
		}
		case 58: case 59: {
			DatatypeDecl(dmod, module, out dt);
			module.TopLevelDecls.Add(dt);
			break;
		}
		case 62: {
			NewtypeDecl(dmod, module, out td);
			module.TopLevelDecls.Add(td);
			break;
		}
		case 63: {
			OtherTypeDecl(dmod, module, out td);
			module.TopLevelDecls.Add(td);
			break;
		}
		case 64: {
			IteratorDecl(dmod, module, out iter);
			module.TopLevelDecls.Add(iter);
			break;
		}
		case 57: {
			TraitDecl(dmod, module, out trait);
			module.TopLevelDecls.Add(trait);
			break;
		}
		case 46: case 47: case 48: case 49: case 50: case 51: case 60: case 61: case 65: case 66: case 67: case 105: {
			ClassMemberDecl(dmod, membersDefaultClass, false, true, !DafnyOptions.O.AllowGlobals,
!isTopLevel && DafnyOptions.O.IronDafny && isAbstract);
			break;
		}
		default: SynErr(158); break;
		}
	}

	void DeclModifier(ref DeclModifierData dmod) {
		if (la.kind == 91) {
			Get();
			dmod.IsAbstract = true;  CheckAndSetToken(ref dmod.AbstractToken);
		} else if (la.kind == 72) {
			Get();
			dmod.IsGhost = true;  CheckAndSetToken(ref dmod.GhostToken);
		} else if (la.kind == 52) {
			Get();
			dmod.IsStatic = true; CheckAndSetToken(ref dmod.StaticToken);
		} else if (la.kind == 53) {
			Get();
			dmod.IsProtected = true; CheckAndSetToken(ref dmod.ProtectedToken);
		} else SynErr(159);
	}

	void SubModuleDecl(DeclModifierData dmod, ModuleDefinition parent, out ModuleDecl submodule) {
		Attributes attrs = null;  IToken/*!*/ id; var prefixIds = new List<IToken>();
		List<MemberDecl/*!*/> namedModuleDefaultClassMembers = new List<MemberDecl>();;
		List<IToken> idPath, idExports;
		IToken idRefined = null;
		ModuleDefinition module;
		submodule = null; // appease compiler
		bool isAbstract = dmod.IsAbstract;
		bool isProtected = dmod.IsProtected;
		bool opened = false;
		CheckDeclModifiers(dmod, "Modules", AllowedDeclModifiers.Abstract | AllowedDeclModifiers.Protected);

		if (la.kind == 92) {
			Get();
			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			NoUSIdent(out id);
			while (la.kind == 32) {
				prefixIds.Add(id);
				Get();
				NoUSIdent(out id);
			}
			if (la.kind == 93) {
				Get();
				ModuleName(out idRefined);
			}
			module = new ModuleDefinition(id, id.val, prefixIds, isAbstract, isProtected, false, idRefined, parent, attrs, false); module.IsToBeVerified = theVerifyThisFile;
			Expect(75);
			module.BodyStartTok = t;
			while (StartOf(1)) {
				TopDecl(module, namedModuleDefaultClassMembers, /* isTopLevel */ false, isAbstract);
			}
			Expect(76);
			module.BodyEndTok = t;
			module.TopLevelDecls.Add(new DefaultClassDecl(module, namedModuleDefaultClassMembers));
			submodule = new LiteralModuleDecl(module, parent);
		} else if (la.kind == 54) {
			Get();
			if (la.kind == 94) {
				Get();
				opened = true;
			}
			ModuleName(out id);
			if (StartOf(3)) {
				idPath = new List<IToken>(); idExports = new List<IToken>();
				if (la.kind == 32 || la.kind == 33) {
					QualifiedModuleExportSuffix(idPath, idExports);
				}
				if (idPath.Count > 0)
				 SemErr(idPath[0], "Qualified imports must be given a name.");
				idPath.Insert(0, id);
				submodule = new AliasModuleDecl(idPath, id, parent, opened, idExports);

			} else if (la.kind == 95) {
				Get();
				QualifiedModuleExport(out idPath, out idExports);
				submodule = new AliasModuleDecl(idPath, id, parent, opened, idExports);
			} else if (la.kind == 25) {
				Get();
				QualifiedModuleExport(out idPath, out idExports);
				submodule = new ModuleFacadeDecl(idPath, id, parent, opened, idExports);
			} else SynErr(160);
			if (la.kind == 34) {
				while (!(la.kind == 0 || la.kind == 34)) {SynErr(161); Get();}
				Get();
				errors.Deprecated(t, "the semi-colon that used to terminate a sub-module declaration has been deprecated; in the new syntax, just leave off the semi-colon");
			}
		} else if (la.kind == 55) {
			IToken exportId;
			List<ExportSignature> exports = new List<ExportSignature>();;
			List<string> extends = new List<string>();
			bool provideAll = false;
			bool revealAll = false;
			bool isDefault = false;
			ExportSignature exsig;

			Get();
			exportId = t;
			if (la.kind == 1 || la.kind == 2) {
				ExportIdent(out exportId);
			}
			while (la.kind == 96 || la.kind == 97 || la.kind == 98) {
				if (la.kind == 96) {
					Get();
					if (la.kind == 1 || la.kind == 2) {
						ModuleExportSignature(true, out exsig);
						exports.Add(exsig);
						while (la.kind == 26) {
							Get();
							ModuleExportSignature(true, out exsig);
							exports.Add(exsig);
						}
					} else if (la.kind == 86) {
						Get();
						provideAll = true;
					} else SynErr(162);
				} else if (la.kind == 97) {
					Get();
					if (la.kind == 1 || la.kind == 2) {
						ModuleExportSignature(false, out exsig);
						exports.Add(exsig);
						while (la.kind == 26) {
							Get();
							ModuleExportSignature(false, out exsig);
							exports.Add(exsig);
						}
					} else if (la.kind == 86) {
						Get();
						revealAll = true;
					} else SynErr(163);
				} else {
					Get();
					ExportIdent(out id);
					extends.Add(id.val);
					while (la.kind == 26) {
						Get();
						ExportIdent(out id);
						extends.Add(id.val);
					}
				}
			}
			if (exportId.val == "export" || exportId.val == parent.Name) {
			 isDefault = true;
			}
			submodule = new ModuleExportDecl(exportId, parent, exports, extends, provideAll, revealAll, isDefault);

		} else SynErr(164);
	}

	void ClassDecl(DeclModifierData dmodClass, ModuleDefinition/*!*/ module, out ClassDecl/*!*/ c) {
		Contract.Requires(module != null);
		Contract.Ensures(Contract.ValueAtReturn(out c) != null);
		IToken/*!*/ id;
		Type trait = null;
		List<Type>/*!*/ traits = new List<Type>();
		Attributes attrs = null;
		List<TypeParameter/*!*/> typeArgs = new List<TypeParameter/*!*/>();
		List<MemberDecl/*!*/> members = new List<MemberDecl/*!*/>();
		IToken bodyStart;
		CheckDeclModifiers(dmodClass, "Classes", AllowedDeclModifiers.None);
		DeclModifierData dmod;

		while (!(la.kind == 0 || la.kind == 56)) {SynErr(165); Get();}
		Expect(56);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		if (la.kind == 81) {
			GenericParameters(typeArgs, true);
		}
		if (la.kind == 98) {
			Get();
			Type(out trait);
			traits.Add(trait);
			while (la.kind == 26) {
				Get();
				Type(out trait);
				traits.Add(trait);
			}
		}
		Expect(75);
		bodyStart = t;
		while (StartOf(4)) {
			dmod = new DeclModifierData();
			while (StartOf(2)) {
				DeclModifier(ref dmod);
			}
			ClassMemberDecl(dmod, members, true, false, false, false);
		}
		Expect(76);
		c = new ClassDecl(id, id.val, module, typeArgs, members, attrs, traits);
		c.BodyStartTok = bodyStart;
		c.BodyEndTok = t;

	}

	void DatatypeDecl(DeclModifierData dmod, ModuleDefinition/*!*/ module, out DatatypeDecl/*!*/ dt) {
		Contract.Requires(module != null);
		Contract.Ensures(Contract.ValueAtReturn(out dt)!=null);
		IToken/*!*/ id;
		Attributes attrs = null;
		List<TypeParameter/*!*/> typeArgs = new List<TypeParameter/*!*/>();
		List<DatatypeCtor/*!*/> ctors = new List<DatatypeCtor/*!*/>();
		IToken bodyStart = Token.NoToken;  // dummy assignment
		bool co = false;
		CheckDeclModifiers(dmod, "Datatypes or codatatypes", AllowedDeclModifiers.None);
		var members = new List<MemberDecl>();

		while (!(la.kind == 0 || la.kind == 58 || la.kind == 59)) {SynErr(166); Get();}
		if (la.kind == 58) {
			Get();
		} else if (la.kind == 59) {
			Get();
			co = true;
		} else SynErr(167);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		if (la.kind == 81) {
			GenericParameters(typeArgs, true);
		}
		Expect(95);
		bodyStart = t;
		if (la.kind == 27) {
			Get();
		}
		DatatypeMemberDecl(ctors);
		while (la.kind == 27) {
			Get();
			DatatypeMemberDecl(ctors);
		}
		if (la.kind == 75) {
			TypeMembers(module, members);
		}
		if (co) {
		 dt = new CoDatatypeDecl(id, id.val, module, typeArgs, ctors, members, attrs);
		} else {
		 dt = new IndDatatypeDecl(id, id.val, module, typeArgs, ctors, members, attrs);
		}
		dt.BodyStartTok = bodyStart;
		dt.BodyEndTok = t;

	}

	void NewtypeDecl(DeclModifierData dmod, ModuleDefinition module, out TopLevelDecl td) {
		IToken id, bvId;
		Attributes attrs = null;
		td = null;
		Type baseType = null;
		Expression constraint;
		Expression witness = null;
		bool witnessIsGhost = false;
		CheckDeclModifiers(dmod, "Newtypes", AllowedDeclModifiers.None);
		var members = new List<MemberDecl>();

		Expect(62);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		Expect(95);
		if (IsIdentColonOrBar()) {
			NoUSIdent(out bvId);
			if (la.kind == 25) {
				Get();
				Type(out baseType);
			}
			if (baseType == null) { baseType = new InferredTypeProxy(); }
			Expect(27);
			Expression(out constraint, false, true);
			if (IsWitness()) {
				if (la.kind == 72) {
					Get();
					witnessIsGhost = true;
				}
				Expect(73);
				Expression(out witness, false, true);
			}
			if (la.kind == 75) {
				TypeMembers(module, members);
			}
			var witnessKind = witness == null ? SubsetTypeDecl.WKind.None :
			witnessIsGhost ? SubsetTypeDecl.WKind.Ghost : SubsetTypeDecl.WKind.Compiled;
			td = new NewtypeDecl(id, id.val, module, new BoundVar(bvId, bvId.val, baseType), constraint, witnessKind, witness, members, attrs);
		} else if (StartOf(5)) {
			Type(out baseType);
			if (la.kind == 75) {
				TypeMembers(module, members);
			}
			td = new NewtypeDecl(id, id.val, module, baseType, members, attrs);
		} else SynErr(168);
	}

	void OtherTypeDecl(DeclModifierData dmod, ModuleDefinition module, out TopLevelDecl td) {
		IToken id, bvId;
		Attributes attrs = null;
		var characteristics = new TypeParameter.TypeParameterCharacteristics(false);
		var typeArgs = new List<TypeParameter>();
		td = null;
		Type ty = null;
		Expression constraint;
		Expression witness = null;
		bool witnessIsGhost = false;
		var kind = "Opaque type";

		Expect(63);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		while (la.kind == 79) {
			TypeParameterCharacteristics(ref characteristics);
		}
		if (la.kind == 81) {
			GenericParameters(typeArgs, true);
		}
		if (la.kind == 95) {
			Get();
			if (IsIdentColonOrBar()) {
				NoUSIdent(out bvId);
				if (la.kind == 25) {
					Get();
					Type(out ty);
				}
				if (ty == null) { ty = new InferredTypeProxy(); }
				Expect(27);
				Expression(out constraint, false, true);
				if (IsWitness()) {
					if (la.kind == 72) {
						Get();
						witnessIsGhost = true;
					}
					Expect(73);
					Expression(out witness, false, true);
				}
				var witnessKind = witness == null ? SubsetTypeDecl.WKind.None :
				 witnessIsGhost ? SubsetTypeDecl.WKind.Ghost : SubsetTypeDecl.WKind.Compiled;
				td = new SubsetTypeDecl(id, id.val, characteristics, typeArgs, module, new BoundVar(bvId, bvId.val, ty), constraint, witnessKind, witness, attrs);
				kind = "Subset type";

			} else if (StartOf(5)) {
				Type(out ty);
				td = new TypeSynonymDecl(id, id.val, characteristics, typeArgs, module, ty, attrs);
				kind = "Type synonym";

			} else SynErr(169);
		}
		if (td == null) {
		 if (module is DefaultModuleDecl) {
		   // opaque type declarations at the very outermost program scope get an automatic (!new)
		   characteristics.DisallowReferenceTypes = true;
		 }
		 td = new OpaqueTypeDecl(id, id.val, module, characteristics, typeArgs, attrs);
		}

		CheckDeclModifiers(dmod, kind, AllowedDeclModifiers.None);
		if (la.kind == 34) {
			while (!(la.kind == 0 || la.kind == 34)) {SynErr(170); Get();}
			Get();
			errors.Deprecated(t, "the semi-colon that used to terminate an opaque-type declaration has been deprecated; in the new syntax, just leave off the semi-colon");
		}
	}

	void IteratorDecl(DeclModifierData dmod, ModuleDefinition module, out IteratorDecl/*!*/ iter) {
		Contract.Ensures(Contract.ValueAtReturn(out iter) != null);
		IToken/*!*/ id;
		Attributes attrs = null;
		List<TypeParameter/*!*/>/*!*/ typeArgs = new List<TypeParameter/*!*/>();
		List<Formal/*!*/> ins = new List<Formal/*!*/>();
		List<Formal/*!*/> outs = new List<Formal/*!*/>();
		List<FrameExpression/*!*/> reads = new List<FrameExpression/*!*/>();
		List<FrameExpression/*!*/> mod = new List<FrameExpression/*!*/>();
		List<Expression/*!*/> decreases = new List<Expression>();
		List<MaybeFreeExpression/*!*/> req = new List<MaybeFreeExpression/*!*/>();
		List<MaybeFreeExpression/*!*/> ens = new List<MaybeFreeExpression/*!*/>();
		List<MaybeFreeExpression/*!*/> yieldReq = new List<MaybeFreeExpression/*!*/>();
		List<MaybeFreeExpression/*!*/> yieldEns = new List<MaybeFreeExpression/*!*/>();
		List<Expression/*!*/> dec = new List<Expression/*!*/>();
		Attributes readsAttrs = null;
		Attributes modAttrs = null;
		Attributes decrAttrs = null;
		BlockStmt body = null;
		IToken signatureEllipsis = null;
		IToken bodyStart = Token.NoToken;
		IToken bodyEnd = Token.NoToken;
		CheckDeclModifiers(dmod, "Iterators", AllowedDeclModifiers.None);

		while (!(la.kind == 0 || la.kind == 64)) {SynErr(171); Get();}
		Expect(64);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		if (la.kind == 79 || la.kind == 81) {
			if (la.kind == 81) {
				GenericParameters(typeArgs, true);
			}
			Formals(true, true, false, ins);
			if (la.kind == 100 || la.kind == 101) {
				if (la.kind == 100) {
					Get();
				} else {
					Get();
					SemErr(t, "iterators don't have a 'returns' clause; did you mean 'yields'?");
				}
				Formals(false, true, false, outs);
			}
		} else if (la.kind == 88) {
			Get();
			signatureEllipsis = t;
		} else SynErr(172);
		while (StartOf(6)) {
			IteratorSpec(reads, mod, decreases, req, ens, yieldReq, yieldEns, ref readsAttrs, ref modAttrs, ref decrAttrs);
		}
		if (la.kind == 75) {
			BlockStmt(out body, out bodyStart, out bodyEnd);
		}
		iter = new IteratorDecl(id, id.val, module, typeArgs, ins, outs,
		                       new Specification<FrameExpression>(reads, readsAttrs),
		                       new Specification<FrameExpression>(mod, modAttrs),
		                       new Specification<Expression>(decreases, decrAttrs),
		                       req, ens, yieldReq, yieldEns,
		                       body, attrs, signatureEllipsis);
		iter.BodyStartTok = bodyStart;
		iter.BodyEndTok = bodyEnd;

	}

	void TraitDecl(DeclModifierData dmodIn, ModuleDefinition/*!*/ module, out TraitDecl/*!*/ trait) {
		Contract.Requires(module != null);
		Contract.Ensures(Contract.ValueAtReturn(out trait) != null);
		CheckDeclModifiers(dmodIn, "Traits", AllowedDeclModifiers.None);
		IToken/*!*/ id;
		Attributes attrs = null;
		List<TypeParameter/*!*/> typeArgs = new List<TypeParameter/*!*/>(); //traits should not support type parameters at the moment
		List<MemberDecl/*!*/> members = new List<MemberDecl/*!*/>();
		IToken bodyStart;
		DeclModifierData dmod;

		while (!(la.kind == 0 || la.kind == 57)) {SynErr(173); Get();}
		Expect(57);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		if (la.kind == 81) {
			GenericParameters(typeArgs, true);
		}
		Expect(75);
		bodyStart = t;
		while (StartOf(4)) {
			dmod  = new DeclModifierData();
			while (StartOf(2)) {
				DeclModifier(ref dmod);
			}
			ClassMemberDecl(dmod, members, true, false, false, false);
		}
		Expect(76);
		trait = new TraitDecl(id, id.val, module, typeArgs, members, attrs);
		trait.BodyStartTok = bodyStart;
		trait.BodyEndTok = t;

	}

	void ClassMemberDecl(DeclModifierData dmod, List<MemberDecl> mm, bool allowConstructors, bool isValueType, bool moduleLevelDecl, bool isWithinAbstractModule) {
		Contract.Requires(cce.NonNullElements(mm));
		Method/*!*/ m;
		Function/*!*/ f;

		if (la.kind == 60) {
			if (moduleLevelDecl) {
			 SemErr(la, "fields are not allowed to be declared at the module level; instead, wrap the field in a 'class' declaration");
			 dmod.IsStatic = false;
			}

			FieldDecl(dmod, isValueType, mm);
		} else if (la.kind == 61) {
			ConstantFieldDecl(dmod, mm, moduleLevelDecl);
		} else if (IsFunctionDecl()) {
			if (moduleLevelDecl && dmod.StaticToken != null) {
			 errors.Warning(dmod.StaticToken, "module-level functions are always non-instance, so the 'static' keyword is not allowed here");
			 dmod.IsStatic = false;
			}

			FunctionDecl(dmod, isWithinAbstractModule, out f);
			mm.Add(f);
		} else if (StartOf(7)) {
			if (moduleLevelDecl && dmod.StaticToken != null) {
			 errors.Warning(dmod.StaticToken, "module-level methods are always non-instance, so the 'static' keyword is not allowed here");
			 dmod.IsStatic = false;
			}

			MethodDecl(dmod, allowConstructors, isWithinAbstractModule, out m);
			mm.Add(m);
		} else SynErr(174);
	}

	void Attribute(ref Attributes attrs) {
		IToken openBrace, closeBrace;
		IToken x = null;
		var args = new List<Expression>();

		Expect(74);
		openBrace = t;
		ConvertKeywordTokenToIdent();
		NoUSIdent(out x);
		if (StartOf(8)) {
			Expressions(args);
		}
		Expect(76);
		closeBrace = t;
		attrs = new UserSuppliedAttributes(x, openBrace, closeBrace, args, attrs);
	}

	void NoUSIdent(out IToken/*!*/ x) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null);
		Expect(1);
		x = t;
		if (x.val.StartsWith("_")) {
		 SemErr("cannot declare identifier beginning with underscore");
		}

	}

	void ModuleName(out IToken id) {
		Ident(out id);
	}

	void QualifiedModuleExportSuffix(List<IToken> ids, List<IToken> exports) {
		IToken id;
		if (la.kind == 32) {
			Get();
			ModuleName(out id);
			ids.Add(id);
			while (la.kind == 32) {
				Get();
				ModuleName(out id);
				ids.Add(id);
			}
		} else if (la.kind == 33) {
			Get();
			if (la.kind == 1 || la.kind == 2) {
				ExportIdent(out id);
				exports.Add(id);
			} else if (la.kind == 75) {
				Get();
				ExportIdent(out id);
				exports.Add(id);
				while (la.kind == 26) {
					Get();
					ExportIdent(out id);
					exports.Add(id);
				}
				Expect(76);
			} else SynErr(175);
		} else SynErr(176);
	}

	void QualifiedModuleExport(out List<IToken> ids, out List<IToken> exports) {
		IToken id; ids = new List<IToken>();
		List<IToken> sids = new List<IToken>(); exports = new List<IToken>();

		ModuleName(out id);
		ids.Add(id);
		if (la.kind == 32 || la.kind == 33) {
			QualifiedModuleExportSuffix(sids, exports);
		}
		ids.AddRange(sids);
	}

	void ExportIdent(out IToken id) {
		FuMe_Ident(out id);
	}

	void ModuleExportSignature(bool opaque, out ExportSignature exsig) {
		IToken prefix; IToken suffix = null;
		TypeNameOrCtorSuffix(out prefix);
		if (la.kind == 32) {
			Get();
			TypeNameOrCtorSuffix(out suffix);
		}
		if (suffix != null) {
		 exsig = new ExportSignature(prefix, prefix.val, suffix, suffix.val, opaque);
		} else {
		 exsig = new ExportSignature(prefix, prefix.val, opaque);
		}

	}

	void TypeNameOrCtorSuffix(out IToken id) {
		id = Token.NoToken;
		if (la.kind == 1) {
			Get();
			id = t;
		} else if (la.kind == 2) {
			Get();
			id = t;
		} else SynErr(177);
	}

	void Ident(out IToken/*!*/ x) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null);
		Expect(1);
		x = t;
	}

	void GenericParameters(List<TypeParameter/*!*/>/*!*/ typeArgs, bool allowVariance) {
		Contract.Requires(cce.NonNullElements(typeArgs));
		IToken/*!*/ id;
		TypeParameter.TypeParameterCharacteristics characteristics;
		TypeParameter.TPVarianceSyntax variance = TypeParameter.TPVarianceSyntax.NonVariant_Strict;  // assignment is to please compiler
		characteristics = new TypeParameter.TypeParameterCharacteristics(false);

		Expect(81);
		if (StartOf(9)) {
			Variance(out variance);
			if (!allowVariance) { SemErr(t, "type-parameter variance is not allowed to be specified in this context"); }
		}
		NoUSIdent(out id);
		while (la.kind == 79) {
			TypeParameterCharacteristics(ref characteristics);
		}
		typeArgs.Add(new TypeParameter(id, id.val, variance, characteristics));
		while (la.kind == 26) {
			Get();
			variance = TypeParameter.TPVarianceSyntax.NonVariant_Strict;
			characteristics = new TypeParameter.TypeParameterCharacteristics(false);

			if (StartOf(9)) {
				Variance(out variance);
				if (!allowVariance) { SemErr(t, "type-parameter variance is not allowed to be specified in this context"); }
			}
			NoUSIdent(out id);
			while (la.kind == 79) {
				TypeParameterCharacteristics(ref characteristics);
			}
			typeArgs.Add(new TypeParameter(id, id.val, variance, characteristics));
		}
		Expect(82);
	}

	void Type(out Type ty) {
		Contract.Ensures(Contract.ValueAtReturn(out ty) != null); IToken/*!*/ tok;
		TypeAndToken(out tok, out ty, false);
	}

	void FieldDecl(DeclModifierData dmod, bool isValueType, List<MemberDecl> mm) {
		Contract.Requires(cce.NonNullElements(mm));
		Attributes attrs = null;
		IToken/*!*/ id;  Type/*!*/ ty;
		CheckDeclModifiers(dmod, "Fields", AllowedDeclModifiers.Ghost);
		if (isValueType) {
		 // we're about to produce an error; put fields into a throw-away list, so we don't return them
		 mm = new List<MemberDecl>();
		}

		while (!(la.kind == 0 || la.kind == 60)) {SynErr(178); Get();}
		Expect(60);
		if (isValueType) {
		 SemErr(t, "mutable fields are now allowed in value types");
		}

		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		FIdentType(out id, out ty);
		mm.Add(new Field(id, id.val, dmod.IsGhost, ty, attrs));
		while (la.kind == 26) {
			Get();
			FIdentType(out id, out ty);
			mm.Add(new Field(id, id.val, dmod.IsGhost, ty, attrs));
		}
		OldSemi();
	}

	void ConstantFieldDecl(DeclModifierData dmod, List<MemberDecl/*!*/>/*!*/ mm, bool moduleLevelDecl) {
		Contract.Requires(cce.NonNullElements(mm));
		Attributes attrs = null;
		IToken/*!*/ id;  Type/*!*/ ty;
		Expression e = null;
		if (moduleLevelDecl && dmod.StaticToken != null) {
		 errors.Warning(dmod.StaticToken, "module-level const declarations are always non-instance, so the 'static' keyword is not allowed here");
		 dmod.IsStatic = false;
		}
		CheckDeclModifiers(dmod, "Fields", AllowedDeclModifiers.Ghost | AllowedDeclModifiers.Static);

		while (!(la.kind == 0 || la.kind == 61)) {SynErr(179); Get();}
		Expect(61);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		CIdentType(out id, out ty);
		if (ty == null) { ty = new InferredTypeProxy(); }
		if (la.kind == 29) {
			Get();
			Expression(out e, false, true);
		}
		if (e == null && ty is InferredTypeProxy) {
		 SemErr(id, "a const declaration must have a type or a RHS value");
		}
		mm.Add(new ConstantField(id, id.val, e, dmod.IsStatic, dmod.IsGhost, ty, attrs));

		OldSemi();
	}

	void FunctionDecl(DeclModifierData dmod, bool isWithinAbstractModule, out Function/*!*/ f) {
		Contract.Ensures(Contract.ValueAtReturn(out f)!=null);
		Attributes attrs = null;
		IToken/*!*/ id = Token.NoToken;  // to please compiler
		List<TypeParameter/*!*/> typeArgs = new List<TypeParameter/*!*/>();
		List<Formal/*!*/> formals = new List<Formal/*!*/>();
		Formal/*!*/ result = null;
		Type/*!*/ returnType = new BoolType();
		List<MaybeFreeExpression/*!*/> reqs = new List<MaybeFreeExpression/*!*/>();
		List<MaybeFreeExpression/*!*/> ens = new List<MaybeFreeExpression/*!*/>();
		List<FrameExpression/*!*/> reads = new List<FrameExpression/*!*/>();
		List<Expression/*!*/> decreases;
		Expression body = null;
		bool isPredicate = false; bool isIndPredicate = false; bool isCoPredicate = false;
		bool isFunctionMethod = false;
		IToken bodyStart = Token.NoToken;
		IToken bodyEnd = Token.NoToken;
		IToken signatureEllipsis = null;
		bool missingOpenParen;
		bool isTwoState = false;
		FixpointPredicate.KType kType = FixpointPredicate.KType.Unspecified;

		if (la.kind == 49) {
			Get();
			isTwoState = true;
		}
		if (la.kind == 46) {
			Get();
			if (la.kind == 65) {
				Get();
				if (isTwoState) { SemErr(t, "twostate functions are supported only as a ghosts, not as function methods"); }
				else { isFunctionMethod = true; }

			}
			AllowedDeclModifiers allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static;
			if (!isTwoState) { allowed |= AllowedDeclModifiers.Protected; }
			string caption = "Functions";
			if (isFunctionMethod) {
			 caption = "Function methods";
			}
			CheckDeclModifiers(dmod, caption, allowed);

			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			FuMe_Ident(out id);
			if (la.kind == 79 || la.kind == 81) {
				if (la.kind == 81) {
					GenericParameters(typeArgs, false);
				}
				Formals(true, isFunctionMethod, isTwoState, formals);
				Expect(25);
				if (FollowedByColon()) {
					Expect(79);
					IToken resultId;
					Type ty;
					bool isGhost;
					bool isOld;

					GIdentType(false, false, out resultId, out ty, out isGhost, out isOld);
					Contract.Assert(!isGhost && !isOld);
					result = new Formal(resultId, resultId.val, ty, false, false, false);

					Expect(80);
				} else if (StartOf(5)) {
					Type(out returnType);
				} else SynErr(180);
			} else if (la.kind == 88) {
				Get();
				signatureEllipsis = t;
			} else SynErr(181);
		} else if (la.kind == 47) {
			Get();
			isPredicate = true;
			if (la.kind == 65) {
				Get();
				if (isTwoState) { SemErr(t, "twostate predicates are supported only as a ghosts, not as predicate methods"); }
				else { isFunctionMethod = true; }

			}
			AllowedDeclModifiers allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static;
			if (!isTwoState) { allowed |= AllowedDeclModifiers.Protected; }
			string caption = "Predicates";
			if (isFunctionMethod) {
			 caption = "Predicate methods";
			}
			CheckDeclModifiers(dmod, caption, allowed);

			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			NoUSIdent(out id);
			if (StartOf(10)) {
				if (la.kind == 81) {
					GenericParameters(typeArgs, false);
				}
				missingOpenParen = true;
				if (la.kind == 79) {
					Formals(true, isFunctionMethod, isTwoState, formals);
					missingOpenParen = false;
				}
				if (missingOpenParen) { errors.Warning(t, "with the new support of higher-order functions in Dafny, parentheses-less predicates are no longer supported; in the new syntax, parentheses are required for the declaration and uses of predicates, even if the predicate takes no additional arguments"); }
				if (la.kind == 25) {
					Get();
					SemErr(t, "predicates do not have an explicitly declared return type; it is always bool");
				}
			} else if (la.kind == 88) {
				Get();
				signatureEllipsis = t;
			} else SynErr(182);
		} else if (la.kind == 48) {
			Contract.Assert(!isTwoState);  // the IsFunctionDecl check checks that "twostate" is not followed by "inductive"

			Get();
			Expect(47);
			isIndPredicate = true;
			CheckDeclModifiers(dmod, "Inductive predicates",
			 AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static | AllowedDeclModifiers.Protected);

			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			FuMe_Ident(out id);
			if (la.kind == 77 || la.kind == 79 || la.kind == 81) {
				if (la.kind == 81) {
					GenericParameters(typeArgs, false);
				}
				if (la.kind == 77) {
					KType(ref kType);
				}
				Formals(true, isFunctionMethod, false, formals);
				if (la.kind == 25) {
					Get();
					SemErr(t, "inductive predicates do not have an explicitly declared return type; it is always bool");
				}
			} else if (la.kind == 88) {
				Get();
				signatureEllipsis = t;
			} else SynErr(183);
		} else if (la.kind == 50) {
			Contract.Assert(!isTwoState);  // the IsFunctionDecl check checks that "twostate" is not followed by "copredicate"

			Get();
			isCoPredicate = true;
			CheckDeclModifiers(dmod, "Copredicates",
			 AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static | AllowedDeclModifiers.Protected);

			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			NoUSIdent(out id);
			if (la.kind == 77 || la.kind == 79 || la.kind == 81) {
				if (la.kind == 81) {
					GenericParameters(typeArgs, false);
				}
				if (la.kind == 77) {
					KType(ref kType);
				}
				Formals(true, isFunctionMethod, false, formals);
				if (la.kind == 25) {
					Get();
					SemErr(t, "copredicates do not have an explicitly declared return type; it is always bool");
				}
			} else if (la.kind == 88) {
				Get();
				signatureEllipsis = t;
			} else SynErr(184);
		} else SynErr(185);
		decreases = isIndPredicate || isCoPredicate ? null : new List<Expression/*!*/>();
		while (StartOf(11)) {
			FunctionSpec(reqs, reads, ens, decreases);
		}
		if (la.kind == 75) {
			FunctionBody(out body, out bodyStart, out bodyEnd);
		}
		if (!isWithinAbstractModule && DafnyOptions.O.DisallowSoundnessCheating && body == null && ens.Count > 0 &&
		   !Attributes.Contains(attrs, "axiom") && !Attributes.Contains(attrs, "imported")) {
		  SemErr(t, "a function with an ensures clause must have a body, unless given the :axiom attribute");
		}
		IToken tok = id;
		if (isTwoState && isPredicate) {
		  f = new TwoStatePredicate(tok, id.val, dmod.IsStatic, typeArgs, formals,
		                            reqs, reads, ens, new Specification<Expression>(decreases, null), body, attrs, signatureEllipsis);
		} else if (isTwoState) {
		  f = new TwoStateFunction(tok, id.val, dmod.IsStatic, typeArgs, formals, result, returnType,
		                           reqs, reads, ens, new Specification<Expression>(decreases, null), body, attrs, signatureEllipsis);
		} else if (isPredicate) {
		  f = new Predicate(tok, id.val, dmod.IsStatic, dmod.IsProtected, !isFunctionMethod, typeArgs, formals,
		                    reqs, reads, ens, new Specification<Expression>(decreases, null), body, Predicate.BodyOriginKind.OriginalOrInherited, attrs, signatureEllipsis);
		} else if (isIndPredicate) {
		  f = new InductivePredicate(tok, id.val, dmod.IsStatic, dmod.IsProtected, kType, typeArgs, formals,
		                             reqs, reads, ens, body, attrs, signatureEllipsis);
		} else if (isCoPredicate) {
		  f = new CoPredicate(tok, id.val, dmod.IsStatic, dmod.IsProtected, kType, typeArgs, formals,
		                      reqs, reads, ens, body, attrs, signatureEllipsis);
		} else {
		  f = new Function(tok, id.val, dmod.IsStatic, dmod.IsProtected, !isFunctionMethod, typeArgs, formals, result, returnType,
		                   reqs, reads, ens, new Specification<Expression>(decreases, null), body, attrs, signatureEllipsis);
		}
		f.BodyStartTok = bodyStart;
		f.BodyEndTok = bodyEnd;
		theBuiltIns.CreateArrowTypeDecl(formals.Count);
		if (isIndPredicate || isCoPredicate) {
		 // also create an arrow type for the corresponding prefix predicate
		 theBuiltIns.CreateArrowTypeDecl(formals.Count + 1);
		}

	}

	void MethodDecl(DeclModifierData dmod, bool allowConstructor, bool isWithinAbstractModule, out Method/*!*/ m) {
		Contract.Ensures(Contract.ValueAtReturn(out m) !=null);
		IToken/*!*/ id = Token.NoToken;
		bool hasName = false;  IToken keywordToken;
		Attributes attrs = null;
		List<TypeParameter/*!*/>/*!*/ typeArgs = new List<TypeParameter/*!*/>();
		List<Formal/*!*/> ins = new List<Formal/*!*/>();
		List<Formal/*!*/> outs = new List<Formal/*!*/>();
		List<MaybeFreeExpression/*!*/> req = new List<MaybeFreeExpression/*!*/>();
		List<FrameExpression/*!*/> mod = new List<FrameExpression/*!*/>();
		List<MaybeFreeExpression/*!*/> ens = new List<MaybeFreeExpression/*!*/>();
		List<Expression/*!*/> dec = new List<Expression/*!*/>();
		Attributes decAttrs = null;
		Attributes modAttrs = null;
		BlockStmt body = null;
		bool isPlainOlMethod = false;
		bool isLemma = false;
		bool isTwoStateLemma = false;
		bool isConstructor = false;
		bool isIndLemma = false;
		bool isCoLemma = false;
		IToken signatureEllipsis = null;
		IToken bodyStart = Token.NoToken;
		IToken bodyEnd = Token.NoToken;
		AllowedDeclModifiers allowed = AllowedDeclModifiers.None;
		string caption = "";
		FixpointPredicate.KType kType = FixpointPredicate.KType.Unspecified;

		while (!(StartOf(12))) {SynErr(186); Get();}
		switch (la.kind) {
		case 65: {
			Get();
			isPlainOlMethod = true; caption = "Methods";
			allowed = AllowedDeclModifiers.Ghost | AllowedDeclModifiers.Static;
			break;
		}
		case 51: {
			Get();
			isLemma = true; caption = "Lemmas";
			allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static
			 | AllowedDeclModifiers.Protected;
			break;
		}
		case 66: {
			Get();
			isCoLemma = true; caption = "Colemmas";
			allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static
			 | AllowedDeclModifiers.Protected;
			break;
		}
		case 105: {
			Get();
			isCoLemma = true; caption = "Comethods";
			allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static
			 | AllowedDeclModifiers.Protected;
			errors.Deprecated(t, "the 'comethod' keyword has been deprecated; it has been renamed to 'colemma'");

			break;
		}
		case 48: {
			Get();
			Expect(51);
			isIndLemma = true;  caption = "Inductive lemmas";
			allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static;
			break;
		}
		case 49: {
			Get();
			Expect(51);
			isTwoStateLemma = true; caption = "Two-state lemmas";
			allowed = AllowedDeclModifiers.AlreadyGhost | AllowedDeclModifiers.Static
			 | AllowedDeclModifiers.Protected;
			break;
		}
		case 67: {
			Get();
			if (allowConstructor) {
			 isConstructor = true;
			} else {
			 SemErr(t, "constructors are allowed only in classes");
			}
			caption = "Constructors";
			allowed = AllowedDeclModifiers.None;

			break;
		}
		default: SynErr(187); break;
		}
		keywordToken = t;
		CheckDeclModifiers(dmod, caption, allowed);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (la.kind == 1 || la.kind == 2) {
			FuMe_Ident(out id);
			hasName = true;
		}
		if (!hasName) {
		 id = keywordToken;
		 if (!isConstructor) {
		   SemErr(la, "a method must be given a name (expecting identifier)");
		 }
		}

		if (la.kind == 77 || la.kind == 79 || la.kind == 81) {
			if (la.kind == 81) {
				GenericParameters(typeArgs, false);
			}
			if (la.kind == 77) {
				KType(ref kType);
				if (!(isCoLemma || isIndLemma)) { SemErr(t, "type of _k can only be specified for inductive lemmas and co-lemmas"); }
			}
			var isCompilable = (isPlainOlMethod && !dmod.IsGhost) || isConstructor;
			Formals(true, isCompilable, isTwoStateLemma, ins);
			if (la.kind == 101) {
				Get();
				if (isConstructor) { SemErr(t, "constructors cannot have out-parameters"); }
				Formals(false, isCompilable, false, outs);
			}
		} else if (la.kind == 88) {
			Get();
			signatureEllipsis = t;
		} else SynErr(188);
		while (StartOf(13)) {
			MethodSpec(req, mod, ens, dec, ref decAttrs, ref modAttrs, caption, isConstructor);
		}
		if (la.kind == 75) {
			if (isConstructor) {
				DividedBlockStmt dividedBody;
				DividedBlockStmt(out dividedBody, out bodyStart, out bodyEnd);
				body = dividedBody;
			} else {
				BlockStmt(out body, out bodyStart, out bodyEnd);
			}
		}
		if (!isWithinAbstractModule && DafnyOptions.O.DisallowSoundnessCheating && body == null && ens.Count > 0 && !Attributes.Contains(attrs, "axiom") && !Attributes.Contains(attrs, "imported") && !Attributes.Contains(attrs, "decl") && theVerifyThisFile) {
		  SemErr(t, "a method with an ensures clause must have a body, unless given the :axiom attribute");
		}

		IToken tok = id;
		if (isConstructor) {
		 m = new Constructor(tok, hasName ? id.val : "_ctor", typeArgs, ins,
		                     req, new Specification<FrameExpression>(mod, modAttrs), ens, new Specification<Expression>(dec, decAttrs), (DividedBlockStmt)body, attrs, signatureEllipsis);
		} else if (isIndLemma) {
		 m = new InductiveLemma(tok, id.val, dmod.IsStatic, kType, typeArgs, ins, outs,
		                        req, new Specification<FrameExpression>(mod, modAttrs), ens, new Specification<Expression>(dec, decAttrs), body, attrs, signatureEllipsis);
		} else if (isCoLemma) {
		 m = new CoLemma(tok, id.val, dmod.IsStatic, kType, typeArgs, ins, outs,
		                 req, new Specification<FrameExpression>(mod, modAttrs), ens, new Specification<Expression>(dec, decAttrs), body, attrs, signatureEllipsis);
		} else if (isLemma) {
		 m = new Lemma(tok, id.val, dmod.IsStatic, typeArgs, ins, outs,
		               req, new Specification<FrameExpression>(mod, modAttrs), ens, new Specification<Expression>(dec, decAttrs), body, attrs, signatureEllipsis);
		} else if (isTwoStateLemma) {
		 m = new TwoStateLemma(tok, id.val, dmod.IsStatic, typeArgs, ins, outs,
		                       req, new Specification<FrameExpression>(mod, modAttrs),
		                       ens, new Specification<Expression>(dec, decAttrs), body, attrs, signatureEllipsis);
		} else {
		 m = new Method(tok, id.val, dmod.IsStatic, dmod.IsGhost, typeArgs, ins, outs,
		                req, new Specification<FrameExpression>(mod, modAttrs), ens, new Specification<Expression>(dec, decAttrs), body, attrs, signatureEllipsis);
		}
		m.BodyStartTok = bodyStart;
		m.BodyEndTok = bodyEnd;

	}

	void DatatypeMemberDecl(List<DatatypeCtor/*!*/>/*!*/ ctors) {
		Contract.Requires(cce.NonNullElements(ctors));
		Attributes attrs = null;
		IToken/*!*/ id;
		List<Formal/*!*/> formals = new List<Formal/*!*/>();

		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		NoUSIdent(out id);
		if (la.kind == 79) {
			FormalsOptionalIds(formals);
		}
		ctors.Add(new DatatypeCtor(id, id.val, formals, attrs));
	}

	void TypeMembers(ModuleDefinition/*!*/ module, List<MemberDecl> members ) {
		DeclModifierData dmod;

		Expect(75);
		while (StartOf(4)) {
			dmod = new DeclModifierData();
			while (StartOf(2)) {
				DeclModifier(ref dmod);
			}
			ClassMemberDecl(dmod, members, false, true, false, module.IsAbstract);
		}
		Expect(76);
	}

	void FormalsOptionalIds(List<Formal/*!*/>/*!*/ formals) {
		Contract.Requires(cce.NonNullElements(formals)); IToken/*!*/ id;  Type/*!*/ ty;  string/*!*/ name;  bool isGhost;
		Expect(79);
		if (StartOf(14)) {
			TypeIdentOptional(out id, out name, out ty, out isGhost);
			formals.Add(new Formal(id, name, ty, true, isGhost));
			while (la.kind == 26) {
				Get();
				TypeIdentOptional(out id, out name, out ty, out isGhost);
				formals.Add(new Formal(id, name, ty, true, isGhost));
			}
		}
		Expect(80);
	}

	void FIdentType(out IToken/*!*/ id, out Type/*!*/ ty) {
		Contract.Ensures(Contract.ValueAtReturn(out id) != null); Contract.Ensures(Contract.ValueAtReturn(out ty) != null);
		id = Token.NoToken;

		if (la.kind == 1) {
			WildIdent(out id, false);
		} else if (la.kind == 2) {
			Get();
			id = t;
		} else SynErr(189);
		Expect(25);
		Type(out ty);
	}

	void OldSemi() {
		if (la.kind == 34) {
			while (!(la.kind == 0 || la.kind == 34)) {SynErr(190); Get();}
			Get();
			errors.DeprecatedStyle(t, "deprecated style: a semi-colon is not needed here");
		}
	}

	void CIdentType(out IToken/*!*/ id, out Type ty) {
		Contract.Ensures(Contract.ValueAtReturn(out id) != null);
		id = Token.NoToken;
		ty = null;

		if (la.kind == 1) {
			WildIdent(out id, false);
		} else if (la.kind == 2) {
			Get();
			id = t;
		} else SynErr(191);
		if (la.kind == 25) {
			Get();
			Type(out ty);
		}
	}

	void Expression(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps = true) {
		Expression e0; IToken endTok;
		EquivExpression(out e, allowSemi, allowLambda, allowBitwiseOps);
		if (SemiFollowsCall(allowSemi, e)) {
			Expect(34);
			endTok = t;
			Expression(out e0, allowSemi, allowLambda);
			e = new StmtExpr(e.tok,
			     new UpdateStmt(e.tok, endTok, new List<Expression>(), new List<AssignmentRhs>() { new ExprRhs(e, null) }),
			     e0);

		}
	}

	void TypeParameterCharacteristics(ref TypeParameter.TypeParameterCharacteristics characteristics) {
		Expect(79);
		TPCharOption(ref characteristics);
		while (la.kind == 26) {
			Get();
			TPCharOption(ref characteristics);
		}
		Expect(80);
	}

	void GIdentType(bool allowGhostKeyword, bool allowNewKeyword, out IToken/*!*/ id, out Type/*!*/ ty, out bool isGhost, out bool isOld) {
		Contract.Ensures(Contract.ValueAtReturn(out id)!=null);
		Contract.Ensures(Contract.ValueAtReturn(out ty)!=null);
		isGhost = false; isOld = allowNewKeyword;
		while (la.kind == 72 || la.kind == 99) {
			if (la.kind == 72) {
				Get();
				if (allowGhostKeyword) { isGhost = true; } else { SemErr(t, "formal cannot be declared 'ghost' in this context"); }
			} else {
				Get();
				if (allowNewKeyword) { isOld = false; } else { SemErr(t, "formal cannot be declared 'new' in this context"); }
			}
		}
		IdentType(out id, out ty, true);
	}

	void IdentType(out IToken/*!*/ id, out Type/*!*/ ty, bool allowWildcardId) {
		Contract.Ensures(Contract.ValueAtReturn(out id) != null); Contract.Ensures(Contract.ValueAtReturn(out ty) != null);
		WildIdent(out id, allowWildcardId);
		Expect(25);
		Type(out ty);
	}

	void WildIdent(out IToken x, bool allowWildcardId) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null);
		Expect(1);
		x = t;
		t.val = UnwildIdent(t.val, allowWildcardId);

	}

	void LocalIdentTypeOptional(out LocalVariable var, bool isGhost) {
		IToken id;  Type ty;  Type optType = null;

		WildIdent(out id, true);
		if (la.kind == 25) {
			Get();
			Type(out ty);
			optType = ty;
		}
		var = new LocalVariable(id, id, id.val, optType == null ? new InferredTypeProxy() : optType, isGhost);
	}

	void IdentTypeOptional(out BoundVar var) {
		Contract.Ensures(Contract.ValueAtReturn(out var) != null);
		IToken id;  Type ty;  Type optType = null;

		WildIdent(out id, true);
		if (la.kind == 25) {
			Get();
			Type(out ty);
			optType = ty;
		}
		var = new BoundVar(id, id.val, optType == null ? new InferredTypeProxy() : optType);
	}

	void TypeIdentOptional(out IToken/*!*/ id, out string/*!*/ identName, out Type/*!*/ ty, out bool isGhost) {
		Contract.Ensures(Contract.ValueAtReturn(out id)!=null);
		Contract.Ensures(Contract.ValueAtReturn(out ty)!=null);
		Contract.Ensures(Contract.ValueAtReturn(out identName)!=null);
		string name = null; id = Token.NoToken; ty = new BoolType()/*dummy*/; isGhost = false;
		if (la.kind == 72) {
			Get();
			isGhost = true;
		}
		if (StartOf(5)) {
			TypeAndToken(out id, out ty, false);
			if (la.kind == 25) {
				Get();
				UserDefinedType udt = ty as UserDefinedType;
				if (udt != null && udt.TypeArgs.Count == 0) {
				 name = udt.Name;
				} else {
				 SemErr(id, "invalid formal-parameter name in datatype constructor");
				}

				Type(out ty);
			}
		} else if (la.kind == 2) {
			Get();
			id = t; name = id.val;
			Expect(25);
			Type(out ty);
		} else SynErr(192);
		if (name != null) {
		 identName = name;
		} else {
		 identName = "#" + anonymousIds++;
		}

	}

	void TypeAndToken(out IToken tok, out Type ty, bool inExpressionContext) {
		Contract.Ensures(Contract.ValueAtReturn(out tok)!=null); Contract.Ensures(Contract.ValueAtReturn(out ty) != null);
		tok = Token.NoToken;  ty = new BoolType();  /*keep compiler happy*/
		List<Type> gt; List<Type> tupleArgTypes = null;

		switch (la.kind) {
		case 8: {
			Get();
			tok = t;
			break;
		}
		case 9: {
			Get();
			tok = t;  ty = new CharType();
			break;
		}
		case 10: {
			Get();
			tok = t;  ty = new IntType();
			break;
		}
		case 11: {
			Get();
			tok = t;  ty = new UserDefinedType(tok, tok.val, null);
			break;
		}
		case 12: {
			Get();
			tok = t;  ty = new RealType();
			break;
		}
		case 13: {
			Get();
			tok = t;  ty = new BigOrdinalType();
			break;
		}
		case 7: {
			Get();
			tok = t;
			int w = StringToInt(tok.val.Substring(2), 0, "bitvectors that wide");
			ty = new BitvectorType(w);

			break;
		}
		case 17: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			if (gt != null && gt.Count > 1) {
			 SemErr("set type expects only one type argument");
			}
			ty = new SetType(true, gt != null ?gt[0] : null);

			break;
		}
		case 18: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			if (gt != null && gt.Count > 1) {
			 SemErr("set type expects only one type argument");
			}
			ty = new SetType(false, gt != null ? gt[0] : null);

			break;
		}
		case 19: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			if (gt != null && gt.Count > 1) {
			 SemErr("multiset type expects only one type argument");
			}
			ty = new MultiSetType(gt != null ? gt[0] : null);

			break;
		}
		case 20: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			if (gt != null && gt.Count > 1) {
			 SemErr("seq type expects only one type argument");
			}
			ty = new SeqType(gt != null ? gt[0] : null);

			break;
		}
		case 16: {
			Get();
			tok = t;  ty = new UserDefinedType(tok, tok.val, null);
			break;
		}
		case 14: {
			Get();
			tok = t;  ty = new UserDefinedType(tok, tok.val, null);
			break;
		}
		case 15: {
			Get();
			tok = t;  ty = new UserDefinedType(tok, tok.val, null);
			break;
		}
		case 21: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			if (gt == null) {
			 ty = new MapType(true, null, null);
			} else if (gt.Count != 2) {
			 SemErr("map type expects two type arguments");
			 ty = new MapType(true, gt[0], gt.Count == 1 ? new InferredTypeProxy() : gt[1]);
			} else {
			 ty = new MapType(true, gt[0], gt[1]);
			}

			break;
		}
		case 22: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			if (gt == null) {
			 ty = new MapType(false, null, null);
			} else if (gt.Count != 2) {
			 SemErr("imap type expects two type arguments");
			 ty = new MapType(false, gt[0], gt.Count == 1 ? new InferredTypeProxy() : gt[1]);
			} else {
			 ty = new MapType(false, gt[0], gt[1]);
			}

			break;
		}
		case 5: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			var dimString = tok.val.Substring(5);
			int dims = StringToInt(dimString, 1, "arrays of that many dimensions");
			ty = theBuiltIns.ArrayType(tok, dims, gt, true);

			break;
		}
		case 6: {
			Get();
			tok = t;
			OptGenericInstantiation(out gt, inExpressionContext);
			var dimString = tok.val.Substring(5);
			dimString = dimString.Substring(0, dimString.Length - 1);
			int dims = StringToInt(dimString, 1, "arrays of that many dimensions");
			ty = theBuiltIns.ArrayType(tok, dims, gt, true, true);

			break;
		}
		case 79: {
			Get();
			tok = t; tupleArgTypes = new List<Type>();
			if (StartOf(5)) {
				Type(out ty);
				tupleArgTypes.Add(ty);
				while (la.kind == 26) {
					Get();
					Type(out ty);
					tupleArgTypes.Add(ty);
				}
			}
			Expect(80);
			if (tupleArgTypes.Count == 1) {
			 // just return the type 'ty'
			} else {
			 var dims = tupleArgTypes.Count;
			 var tmp = theBuiltIns.TupleType(tok, dims, true);  // make sure the tuple type exists
			 ty = new UserDefinedType(tok, BuiltIns.TupleTypeName(dims), dims == 0 ? null : tupleArgTypes);
			}

			break;
		}
		case 1: {
			Expression e;
			NameSegmentForTypeName(out e, inExpressionContext);
			tok = t;
			while (la.kind == 32) {
				Get();
				TypeNameOrCtorSuffix(out tok);
				List<Type> typeArgs;
				OptGenericInstantiation(out typeArgs, inExpressionContext);
				e = new ExprDotName(tok, e, tok.val, typeArgs);
			}
			ty = new UserDefinedType(e.tok, e);
			break;
		}
		default: SynErr(193); break;
		}
		if (la.kind == 108 || la.kind == 109 || la.kind == 110) {
			int arrowKind = 0; /* 0: any, 1: partial, 2: total */
			Type t2;

			if (la.kind == 108) {
				Get();
				tok = t; arrowKind = 0;
			} else if (la.kind == 109) {
				Get();
				tok = t; arrowKind = 1;
			} else {
				Get();
				tok = t; arrowKind = 2;
			}
			Type(out t2);
			if (tupleArgTypes != null) {
			 gt = tupleArgTypes;
			} else {
			 gt = new List<Type>{ ty };
			}
			var arity = gt.Count;
			theBuiltIns.CreateArrowTypeDecl(arity);
			if (arrowKind == 0) {
			 ty = new ArrowType(tok, gt, t2);
			} else {
			 gt.Add(t2);
			 if (arrowKind == 1) {
			   ty = new UserDefinedType(tok, ArrowType.PartialArrowTypeName(arity), gt);
			 } else {
			   ty = new UserDefinedType(tok, ArrowType.TotalArrowTypeName(arity), gt);
			 }
			}

		}
	}

	void Formals(bool incoming, bool allowGhostKeyword, bool allowNewKeyword, List<Formal> formals) {
		Contract.Requires(cce.NonNullElements(formals));
		IToken id;
		Type ty;
		bool isGhost;
		bool isOld;

		Expect(79);
		if (la.kind == 1 || la.kind == 72 || la.kind == 99) {
			GIdentType(allowGhostKeyword, allowNewKeyword, out id, out ty, out isGhost, out isOld);
			formals.Add(new Formal(id, id.val, ty, incoming, isGhost, isOld));
			while (la.kind == 26) {
				Get();
				GIdentType(allowGhostKeyword, allowNewKeyword, out id, out ty, out isGhost, out isOld);
				formals.Add(new Formal(id, id.val, ty, incoming, isGhost, isOld));
			}
		}
		Expect(80);
	}

	void IteratorSpec(List<FrameExpression/*!*/>/*!*/ reads, List<FrameExpression/*!*/>/*!*/ mod, List<Expression/*!*/> decreases,
List<MaybeFreeExpression/*!*/>/*!*/ req, List<MaybeFreeExpression/*!*/>/*!*/ ens,
List<MaybeFreeExpression/*!*/>/*!*/ yieldReq, List<MaybeFreeExpression/*!*/>/*!*/ yieldEns,
ref Attributes readsAttrs, ref Attributes modAttrs, ref Attributes decrAttrs) {
		Expression/*!*/ e; FrameExpression/*!*/ fe; bool isFree = false; bool isYield = false; Attributes ensAttrs = null;
		IToken lbl = null;

		while (!(StartOf(15))) {SynErr(194); Get();}
		if (la.kind == 69) {
			Get();
			while (la.kind == 74) {
				Attribute(ref readsAttrs);
			}
			FrameExpression(out fe, false, false);
			reads.Add(fe);
			while (la.kind == 26) {
				Get();
				FrameExpression(out fe, false, false);
				reads.Add(fe);
			}
			OldSemi();
		} else if (la.kind == 68) {
			Get();
			while (la.kind == 74) {
				Attribute(ref modAttrs);
			}
			FrameExpression(out fe, false, false);
			mod.Add(fe);
			while (la.kind == 26) {
				Get();
				FrameExpression(out fe, false, false);
				mod.Add(fe);
			}
			OldSemi();
		} else if (StartOf(16)) {
			if (la.kind == 106) {
				Get();
				isFree = true;
				errors.Deprecated(t, "the 'free' keyword is soon to be deprecated");

			}
			if (la.kind == 107) {
				Get();
				isYield = true;
			}
			if (la.kind == 70) {
				Get();
				if (IsLabel(!isYield)) {
					LabelIdent(out lbl);
					Expect(25);
				}
				Expression(out e, false, false);
				OldSemi();
				AssertLabel al = lbl == null ? null : new AssertLabel(lbl, lbl.val);
				if (isYield) {
				 yieldReq.Add(new MaybeFreeExpression(e, isFree, al, null));
				} else {
				 req.Add(new MaybeFreeExpression(e, isFree, al, null));
				}

			} else if (la.kind == 71) {
				Get();
				while (la.kind == 74) {
					Attribute(ref ensAttrs);
				}
				Expression(out e, false, false);
				OldSemi();
				if (isYield) {
				 yieldEns.Add(new MaybeFreeExpression(e, isFree, ensAttrs));
				} else {
				 ens.Add(new MaybeFreeExpression(e, isFree, ensAttrs));
				}

			} else SynErr(195);
		} else if (la.kind == 44) {
			Get();
			while (la.kind == 74) {
				Attribute(ref decrAttrs);
			}
			DecreasesList(decreases, false, false);
			OldSemi();
		} else SynErr(196);
	}

	void BlockStmt(out BlockStmt/*!*/ block, out IToken bodyStart, out IToken bodyEnd) {
		Contract.Ensures(Contract.ValueAtReturn(out block) != null);
		List<Statement/*!*/> body = new List<Statement/*!*/>();

		Expect(75);
		bodyStart = t;
		while (StartOf(17)) {
			Stmt(body);
		}
		Expect(76);
		bodyEnd = t;
		block = new BlockStmt(bodyStart, bodyEnd, body);
	}

	void Variance(out TypeParameter.TPVarianceSyntax variance) {
		variance = TypeParameter.TPVarianceSyntax.NonVariant_Strict;  // never used; here just to please the C# compiler

		if (la.kind == 86) {
			Get();
			variance = TypeParameter.TPVarianceSyntax.Covariant_Permissive;
		} else if (la.kind == 102) {
			Get();
			variance = TypeParameter.TPVarianceSyntax.Covariant_Strict;
		} else if (la.kind == 103) {
			Get();
			variance = TypeParameter.TPVarianceSyntax.NonVariant_Permissive;
		} else if (la.kind == 104) {
			Get();
			variance = TypeParameter.TPVarianceSyntax.Contravariance;
		} else SynErr(197);
	}

	void TPCharOption(ref TypeParameter.TypeParameterCharacteristics characteristics) {
		if (la.kind == 83) {
			Get();
			characteristics.EqualitySupport = TypeParameter.EqualitySupportValue.Required;
		} else if (la.kind == 2) {
			Get();
			if (t.val == "0") {
			 characteristics.MustSupportZeroInitialization = true;
			} else {
			 SemErr(t, "unexpected TPCharOption");
			}

		} else if (la.kind == 103) {
			Get();
			Expect(99);
			characteristics.DisallowReferenceTypes = true;
		} else SynErr(198);
	}

	void FuMe_Ident(out IToken id) {
		id = Token.NoToken;
		if (la.kind == 1) {
			NoUSIdent(out id);
		} else if (la.kind == 2) {
			Get();
			id = t;
		} else SynErr(199);
	}

	void KType(ref FixpointPredicate.KType kType) {
		Expect(77);
		if (la.kind == 11) {
			Get();
			kType = FixpointPredicate.KType.Nat;
		} else if (la.kind == 13) {
			Get();
			kType = FixpointPredicate.KType.ORDINAL;
		} else SynErr(200);
		Expect(78);
	}

	void MethodSpec(List<MaybeFreeExpression> req, List<FrameExpression> mod, List<MaybeFreeExpression> ens,
List<Expression> decreases, ref Attributes decAttrs, ref Attributes modAttrs, string caption, bool performThisDeprecatedCheck) {
		Contract.Requires(cce.NonNullElements(req));
		Contract.Requires(cce.NonNullElements(mod));
		Contract.Requires(cce.NonNullElements(ens));
		Contract.Requires(cce.NonNullElements(decreases));
		Expression e;  FrameExpression fe;  bool isFree = false; Attributes ensAttrs = null; Attributes reqAttrs = null;
		IToken lbl = null;

		while (!(StartOf(18))) {SynErr(201); Get();}
		if (la.kind == 68) {
			Get();
			while (la.kind == 74) {
				Attribute(ref modAttrs);
			}
			FrameExpression(out fe, false, false);
			Util.AddFrameExpression(mod, fe, performThisDeprecatedCheck, errors);
			while (la.kind == 26) {
				Get();
				FrameExpression(out fe, false, false);
				Util.AddFrameExpression(mod, fe, performThisDeprecatedCheck, errors);
			}
			OldSemi();
		} else if (la.kind == 70 || la.kind == 71 || la.kind == 106) {
			if (la.kind == 106) {
				Get();
				isFree = true;
				errors.Deprecated(t, "the 'free' keyword is soon to be deprecated");

			}
			if (la.kind == 70) {
				Get();
				while (la.kind == 74) {
					Attribute(ref reqAttrs);
				}
				if (IsLabel(true)) {
					LabelIdent(out lbl);
					Expect(25);
				}
				Expression(out e, false, false);
				OldSemi();
				req.Add(new MaybeFreeExpression(e, isFree, lbl == null ? null : new AssertLabel(lbl, lbl.val), reqAttrs));
			} else if (la.kind == 71) {
				Get();
				while (la.kind == 74) {
					Attribute(ref ensAttrs);
				}
				Expression(out e, false, false);
				OldSemi();
				ens.Add(new MaybeFreeExpression(e, isFree, ensAttrs));
			} else SynErr(202);
		} else if (la.kind == 44) {
			Get();
			while (la.kind == 74) {
				Attribute(ref decAttrs);
			}
			DecreasesList(decreases, true, false);
			OldSemi();
		} else SynErr(203);
	}

	void DividedBlockStmt(out DividedBlockStmt body, out IToken bodyStart, out IToken bodyEnd) {
		Contract.Ensures(Contract.ValueAtReturn(out body) != null);
		List<Statement> bodyInit = new List<Statement>();
		IToken separatorTok = null;
		List<Statement> bodyProper = new List<Statement>();

		Expect(75);
		bodyStart = t;
		while (StartOf(17)) {
			Stmt(bodyInit);
		}
		if (la.kind == 99) {
			Get();
			separatorTok = t;
			Expect(34);
			while (StartOf(17)) {
				Stmt(bodyProper);
			}
		}
		Expect(76);
		bodyEnd = t;
		body = new DividedBlockStmt(bodyStart, bodyEnd, bodyInit, separatorTok, bodyProper);
	}

	void FrameExpression(out FrameExpression fe, bool allowSemi, bool allowLambda) {
		Contract.Ensures(Contract.ValueAtReturn(out fe) != null);
		Expression/*!*/ e;
		IToken/*!*/ id;
		string fieldName = null;  IToken feTok = null;
		fe = dummyFrameExpr;

		if (StartOf(8)) {
			Expression(out e, allowSemi, allowLambda);
			feTok = e.tok;
			if (la.kind == 33) {
				Get();
				Ident(out id);
				fieldName = id.val;  feTok = id;
			}
			fe = new FrameExpression(feTok, e, fieldName);
		} else if (la.kind == 33) {
			Get();
			Ident(out id);
			fieldName = id.val;
			fe = new FrameExpression(id, new ImplicitThisExpr(id), fieldName);
		} else SynErr(204);
	}

	void LabelIdent(out IToken id) {
		id = Token.NoToken;
		if (la.kind == 1) {
			NoUSIdent(out id);
		} else if (la.kind == 2) {
			Get();
			id = t;
		} else SynErr(205);
	}

	void DecreasesList(List<Expression> decreases, bool allowWildcard, bool allowLambda) {
		Expression e;
		PossiblyWildExpression(out e, allowLambda);
		if (!allowWildcard && e is WildcardExpr) {
		 SemErr(e.tok, "'decreases *' is allowed only on loops and tail-recursive methods");
		} else {
		 decreases.Add(e);
		}

		while (la.kind == 26) {
			Get();
			PossiblyWildExpression(out e, allowLambda);
			if (!allowWildcard && e is WildcardExpr) {
			 SemErr(e.tok, "'decreases *' is allowed only on loops and tail-recursive methods");
			} else {
			 decreases.Add(e);
			}

		}
	}

	void OptGenericInstantiation(out List<Type> gt, bool inExpressionContext) {
		gt = null;
		if (IsGenericInstantiation(inExpressionContext)) {
			gt = new List<Type>();
			GenericInstantiation(gt);
		}
	}

	void NameSegmentForTypeName(out Expression e, bool inExpressionContext) {
		IToken id;  List<Type> typeArgs;
		Ident(out id);
		OptGenericInstantiation(out typeArgs, inExpressionContext);
		e = new NameSegment(id, id.val, typeArgs);

	}

	void GenericInstantiation(List<Type> gt) {
		Contract.Requires(cce.NonNullElements(gt)); Type/*!*/ ty;
		Expect(81);
		Type(out ty);
		gt.Add(ty);
		while (la.kind == 26) {
			Get();
			Type(out ty);
			gt.Add(ty);
		}
		Expect(82);
	}

	void FunctionSpec(List<MaybeFreeExpression/*!*/>/*!*/ reqs, List<FrameExpression/*!*/>/*!*/ reads, List<MaybeFreeExpression/*!*/>/*!*/ ens, List<Expression/*!*/> decreases) {
		Contract.Requires(cce.NonNullElements(reqs));
		Contract.Requires(cce.NonNullElements(reads));
		Contract.Requires(decreases == null || cce.NonNullElements(decreases));
		Expression/*!*/ e;  FrameExpression/*!*/ fe;
		Attributes ensAttrs = null; Attributes reqAttrs = null;
		while (!(StartOf(19))) {SynErr(206); Get();}
		if (la.kind == 70) {
			Get();
			while (la.kind == 74) {
				Attribute(ref reqAttrs);
			}
			Expression(out e, false, false);
			OldSemi();
			reqs.Add(new MaybeFreeExpression(e, false, reqAttrs));
		} else if (la.kind == 69) {
			Get();
			PossiblyWildFrameExpression(out fe, false);
			reads.Add(fe);
			while (la.kind == 26) {
				Get();
				PossiblyWildFrameExpression(out fe, false);
				reads.Add(fe);
			}
			OldSemi();
		} else if (la.kind == 71) {
			Get();
			while (la.kind == 74) {
				Attribute(ref ensAttrs);
			}
			Expression(out e, false, false);
			OldSemi();
			ens.Add(new MaybeFreeExpression(e, false, ensAttrs));
		} else if (la.kind == 44) {
			Get();
			if (decreases == null) {
			 SemErr(t, "'decreases' clauses are meaningless for copredicates, so they are not allowed");
			 decreases = new List<Expression/*!*/>();
			}

			DecreasesList(decreases, false, false);
			OldSemi();
		} else SynErr(207);
	}

	void FunctionBody(out Expression/*!*/ e, out IToken bodyStart, out IToken bodyEnd) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null); e = dummyExpr;
		Expect(75);
		bodyStart = t;
		Expression(out e, true, true);
		Expect(76);
		bodyEnd = t;
	}

	void PossiblyWildFrameExpression(out FrameExpression fe, bool allowSemi) {
		Contract.Ensures(Contract.ValueAtReturn(out fe) != null); fe = dummyFrameExpr;
		if (la.kind == 86) {
			Get();
			fe = new FrameExpression(t, new WildcardExpr(t), null);
		} else if (StartOf(20)) {
			FrameExpression(out fe, allowSemi, false);
		} else SynErr(208);
	}

	void PossiblyWildExpression(out Expression e, bool allowLambda) {
		Contract.Ensures(Contract.ValueAtReturn(out e)!=null);
		e = dummyExpr;
		if (la.kind == 86) {
			Get();
			e = new WildcardExpr(t);
		} else if (StartOf(8)) {
			Expression(out e, false, allowLambda);
		} else SynErr(209);
	}

	void Stmt(List<Statement/*!*/>/*!*/ ss) {
		Statement/*!*/ s;

		OneStmt(out s);
		ss.Add(s);
	}

	void OneStmt(out Statement/*!*/ s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null); IToken/*!*/ x;  IToken/*!*/ id;  string label = null;
		s = dummyStmt;  /* to please the compiler */
		BlockStmt bs;
		IToken bodyStart, bodyEnd;
		int breakCount;

		while (!(StartOf(21))) {SynErr(210); Get();}
		switch (la.kind) {
		case 75: {
			BlockStmt(out bs, out bodyStart, out bodyEnd);
			s = bs;
			break;
		}
		case 119: {
			AssertStmt(out s, false);
			break;
		}
		case 36: {
			AssumeStmt(out s);
			break;
		}
		case 120: {
			PrintStmt(out s);
			break;
		}
		case 89: {
			RevealStmt(out s);
			break;
		}
		case 1: case 2: case 3: case 4: case 10: case 12: case 23: case 24: case 27: case 79: case 115: case 147: case 148: case 149: case 150: case 151: case 152: case 153: case 155: {
			UpdateStmt(out s);
			break;
		}
		case 60: case 72: {
			VarDeclStatement(out s);
			break;
		}
		case 116: {
			IfStmt(out s);
			break;
		}
		case 117: {
			WhileStmt(out s);
			break;
		}
		case 118: {
			NestedMatchStmt(out s);
			break;
		}
		case 121: case 122: {
			ForallStmt(out s);
			break;
		}
		case 37: {
			CalcStmt(out s);
			break;
		}
		case 123: {
			ModifyStmt(out s);
			break;
		}
		case 111: {
			Get();
			LabelIdent(out id);
			Expect(25);
			OneStmt(out s);
			s.Labels = new LList<Label>(new Label(id, id.val), s.Labels);
			break;
		}
		case 112: {
			Get();
			x = t; breakCount = 1; label = null;
			if (la.kind == 1 || la.kind == 2) {
				LabelIdent(out id);
				label = id.val;
			} else if (la.kind == 34 || la.kind == 112) {
				while (la.kind == 112) {
					Get();
					breakCount++;
				}
			} else SynErr(211);
			while (!(la.kind == 0 || la.kind == 34)) {SynErr(212); Get();}
			Expect(34);
			s = label != null ? new BreakStmt(x, t, label) : new BreakStmt(x, t, breakCount);
			break;
		}
		case 107: case 114: {
			ReturnStmt(out s);
			break;
		}
		case 88: {
			SkeletonStmt(out s);
			break;
		}
		default: SynErr(213); break;
		}
	}

	void AssertStmt(out Statement/*!*/ s, bool inExprContext) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null); IToken/*!*/ x;
		Expression e = dummyExpr; Attributes attrs = null;
		IToken dotdotdot = null;
		BlockStmt proof = null;
		IToken proofStart, proofEnd;
		IToken lbl = null;

		Expect(119);
		x = t;
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (StartOf(8)) {
			if (IsLabel(!inExprContext)) {
				LabelIdent(out lbl);
				Expect(25);
			}
			Expression(out e, false, true);
			if (la.kind == 42) {
				Get();
				BlockStmt(out proof, out proofStart, out proofEnd);
			} else if (la.kind == 34) {
				Get();
			} else SynErr(214);
		} else if (la.kind == 88) {
			Get();
			dotdotdot = t;
			Expect(34);
		} else SynErr(215);
		if (dotdotdot != null) {
		 s = new SkeletonStatement(new AssertStmt(x, t, new LiteralExpr(x, true), null, null, attrs), dotdotdot, null);
		} else {
		 s = new AssertStmt(x, t, e, proof, lbl == null ? null : new AssertLabel(lbl, lbl.val), attrs);
		}

	}

	void AssumeStmt(out Statement/*!*/ s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null); IToken/*!*/ x;
		Expression e = dummyExpr; Attributes attrs = null;
		IToken dotdotdot = null;

		Expect(36);
		x = t;
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (StartOf(8)) {
			Expression(out e, false, true);
		} else if (la.kind == 88) {
			Get();
			dotdotdot = t;
		} else SynErr(216);
		Expect(34);
		if (dotdotdot != null) {
		 s = new SkeletonStatement(new AssumeStmt(x, t, new LiteralExpr(x, true), attrs), dotdotdot, null);
		} else {
		 s = new AssumeStmt(x, t, e, attrs);
		}

	}

	void PrintStmt(out Statement s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null);
		IToken x;  Expression e;
		var args = new List<Expression>();

		Expect(120);
		x = t;
		Expression(out e, false, true);
		args.Add(e);
		while (la.kind == 26) {
			Get();
			Expression(out e, false, true);
			args.Add(e);
		}
		Expect(34);
		s = new PrintStmt(x, t, args);
	}

	void RevealStmt(out Statement s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null);
		IToken x; Expression e; var es = new List<Expression>();

		Expect(89);
		x = t;
		Expression(out e, false, true);
		es.Add(e);
		while (la.kind == 26) {
			Get();
			Expression(out e, false, true);
			es.Add(e);
		}
		Expect(34);
		s = new RevealStmt(x, t, es);
	}

	void UpdateStmt(out Statement/*!*/ s) {
		List<Expression> lhss = new List<Expression>();
		List<AssignmentRhs> rhss = new List<AssignmentRhs>();
		Expression e;
		AssignmentRhs r;
		IToken x = Token.NoToken;
		IToken endTok = Token.NoToken;
		Attributes attrs = null;
		IToken suchThatAssume = null;
		Expression suchThat = null;
		Expression exceptionExpr = null;

		if (StartOf(22)) {
			Lhs(out e);
			x = e.tok;
			if (la.kind == 34 || la.kind == 74) {
				while (la.kind == 74) {
					Attribute(ref attrs);
				}
				Expect(34);
				endTok = t; rhss.Add(new ExprRhs(e, attrs));
			} else if (StartOf(23)) {
				lhss.Add(e);
				while (la.kind == 26) {
					Get();
					Lhs(out e);
					lhss.Add(e);
				}
				if (la.kind == 29) {
					Get();
					x = t;
					Rhs(out r);
					rhss.Add(r);
					while (la.kind == 26) {
						Get();
						Rhs(out r);
						rhss.Add(r);
					}
				} else if (la.kind == 30) {
					Get();
					x = t;
					if (la.kind == _assume) {
						Expect(36);
						suchThatAssume = t;
					}
					Expression(out suchThat, false, true);
				} else if (la.kind == 115) {
					Get();
					x = t;
					Expression(out exceptionExpr, false, false);
				} else SynErr(217);
				Expect(34);
				endTok = t;
			} else if (la.kind == 25) {
				Get();
				SemErr(t, "invalid statement (did you forget the 'label' keyword?)");
			} else SynErr(218);
		} else if (la.kind == 115) {
			Get();
			x = t;
			Expression(out exceptionExpr, false, false);
			Expect(34);
			endTok = t;
		} else SynErr(219);
		if (suchThat != null) {
		 s = new AssignSuchThatStmt(x, endTok, lhss, suchThat, suchThatAssume, null);
		} else if (exceptionExpr != null) {
		 if (lhss.Count > 1) {
		   SemErr(x, "':-' assignments can only have one LHS");
		   lhss = new List<Expression>() { lhss[0] };
		 }
		 s = new AssignOrReturnStmt(x, endTok, lhss, exceptionExpr);
		} else {
		 if (lhss.Count == 0 && rhss.Count == 0) {
		   s = new BlockStmt(x, endTok, new List<Statement>()); // error, give empty statement
		 } else {
		   s = new UpdateStmt(x, endTok, lhss, rhss);
		 }
		}

	}

	void VarDeclStatement(out Statement/*!*/ s) {
		IToken x = null, assignTok = null;  bool isGhost = false;
		LocalVariable d;
		AssignmentRhs r;
		List<LocalVariable> lhss = new List<LocalVariable>();
		List<AssignmentRhs> rhss = new List<AssignmentRhs>();
		IToken suchThatAssume = null;
		Expression suchThat = null;
		Expression exceptionExpr = null;
		Attributes attrs = null;
		IToken endTok;
		s = dummyStmt;

		if (la.kind == 72) {
			Get();
			isGhost = true;  x = t;
		}
		Expect(60);
		if (!isGhost) { x = t; }
		if (!IsLetStmt()) {
			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			LocalIdentTypeOptional(out d, isGhost);
			lhss.Add(d); d.Attributes = attrs; attrs = null;
			while (la.kind == 26) {
				Get();
				while (la.kind == 74) {
					Attribute(ref attrs);
				}
				LocalIdentTypeOptional(out d, isGhost);
				lhss.Add(d); d.Attributes = attrs; attrs = null;
			}
			if (StartOf(24)) {
				if (la.kind == 29) {
					Get();
					assignTok = t;
					Rhs(out r);
					rhss.Add(r);
					while (la.kind == 26) {
						Get();
						Rhs(out r);
						rhss.Add(r);
					}
				} else if (la.kind == 30 || la.kind == 74) {
					while (la.kind == 74) {
						Attribute(ref attrs);
					}
					Expect(30);
					assignTok = t;
					if (la.kind == _assume) {
						Expect(36);
						suchThatAssume = t;
					}
					Expression(out suchThat, false, true);
				} else {
					Get();
					assignTok = t;
					Expression(out exceptionExpr, false, false);
				}
			}
			while (!(la.kind == 0 || la.kind == 34)) {SynErr(220); Get();}
			Expect(34);
			endTok = t;
			ConcreteUpdateStatement update;
			var lhsExprs = new List<Expression>();
			foreach (var lhs in lhss) {
			 lhsExprs.Add(new IdentifierExpr(lhs.Tok, lhs.Name));
			}
			if (suchThat != null) {
			 update = new AssignSuchThatStmt(assignTok, endTok, lhsExprs, suchThat, suchThatAssume, attrs);
			} else if (exceptionExpr != null) {
			   Contract.Assert(lhss.Count >= 1);
			   if (lhss.Count != 1) {
			     SemErr(assignTok, "':-' assignments can only have one LHS");
			     lhsExprs = new List<Expression>() { lhsExprs[0] };
			   }
			   update = new AssignOrReturnStmt(assignTok, endTok, lhsExprs, exceptionExpr);
			} else if (rhss.Count == 0) {
			 update = null;
			} else {
			 var ies = new List<Expression>();
			 foreach (var lhs in lhss) {
			   ies.Add(new AutoGhostIdentifierExpr(lhs.Tok, lhs.Name));
			 }
			 update = new UpdateStmt(assignTok, endTok, ies, rhss);
			}
			s = new VarDeclStmt(x, endTok, lhss, update);

		} else if (la.kind == 1 || la.kind == 79) {
			CasePattern<LocalVariable> pat;
			Expression e = dummyExpr;
			IToken id = t;

			CasePatternLocal(out pat, isGhost);
			if (la.kind == 29) {
				Get();
			} else if (la.kind == 30 || la.kind == 74) {
				while (la.kind == 74) {
					Attribute(ref attrs);
				}
				Expect(30);
				SemErr(pat.tok, "LHS of assign-such-that expression must be variables, not general patterns");
			} else SynErr(221);
			Expression(out e, false, true);
			Expect(34);
			s = new LetStmt(e.tok, e.tok, pat, e);
		} else SynErr(222);
	}

	void IfStmt(out Statement/*!*/ ifStmt) {
		Contract.Ensures(Contract.ValueAtReturn(out ifStmt) != null); IToken/*!*/ x;
		Expression guard = null;  IToken guardEllipsis = null;  bool isExistentialGuard = false;
		BlockStmt/*!*/ thn;
		BlockStmt/*!*/ bs;
		Statement/*!*/ s;
		Statement els = null;
		IToken bodyStart, bodyEnd, endTok;
		List<GuardedAlternative> alternatives;
		ifStmt = dummyIfStmt;  // to please the compiler
		bool usesOptionalBraces;

		Expect(116);
		x = t;
		if (IsAlternative()) {
			AlternativeBlock(true, out alternatives, out usesOptionalBraces, out endTok);
			ifStmt = new AlternativeStmt(x, endTok, alternatives, usesOptionalBraces);
		} else if (StartOf(25)) {
			if (IsExistentialGuard()) {
				ExistentialGuard(out guard, true);
				isExistentialGuard = true;
			} else if (StartOf(26)) {
				Guard(out guard);
			} else {
				Get();
				guardEllipsis = t;
			}
			BlockStmt(out thn, out bodyStart, out bodyEnd);
			endTok = thn.EndTok;
			if (la.kind == 40) {
				Get();
				if (la.kind == 116) {
					IfStmt(out s);
					els = s; endTok = s.EndTok;
				} else if (la.kind == 75) {
					BlockStmt(out bs, out bodyStart, out bodyEnd);
					els = bs; endTok = bs.EndTok;
				} else SynErr(223);
			}
			if (guardEllipsis != null) {
			 ifStmt = new SkeletonStatement(new IfStmt(x, endTok, isExistentialGuard, guard, thn, els), guardEllipsis, null);
			} else {
			 ifStmt = new IfStmt(x, endTok, isExistentialGuard, guard, thn, els);
			}

		} else SynErr(224);
	}

	void WhileStmt(out Statement stmt) {
		Contract.Ensures(Contract.ValueAtReturn(out stmt) != null); IToken x;
		Expression guard = null;  IToken guardEllipsis = null;

		List<MaybeFreeExpression> invariants = new List<MaybeFreeExpression>();
		List<Expression> decreases = new List<Expression>();
		Attributes decAttrs = null;
		Attributes modAttrs = null;
		List<FrameExpression> mod = null;

		BlockStmt body = null;  IToken bodyEllipsis = null;
		IToken bodyStart = null, bodyEnd = null, endTok = Token.NoToken;
		List<GuardedAlternative> alternatives;
		stmt = dummyStmt;  // to please the compiler
		bool isDirtyLoop = true;
		bool usesOptionalBraces;

		Expect(117);
		x = t;
		if (IsLoopSpec() || IsAlternative()) {
			while (StartOf(27)) {
				LoopSpec(invariants, decreases, ref mod, ref decAttrs, ref modAttrs);
			}
			AlternativeBlock(false, out alternatives, out usesOptionalBraces, out endTok);
			stmt = new AlternativeLoopStmt(x, endTok, invariants, new Specification<Expression>(decreases, decAttrs), new Specification<FrameExpression>(mod, modAttrs), alternatives, usesOptionalBraces);
		} else if (StartOf(25)) {
			if (StartOf(26)) {
				Guard(out guard);
				Contract.Assume(guard == null || cce.Owner.None(guard));
			} else {
				Get();
				guardEllipsis = t;
			}
			while (StartOf(27)) {
				LoopSpec(invariants, decreases, ref mod, ref decAttrs, ref modAttrs);
			}
			if (la.kind == _lbrace) {
				BlockStmt(out body, out bodyStart, out bodyEnd);
				endTok = body.EndTok; isDirtyLoop = false;
			} else if (la.kind == _ellipsis) {
				Expect(88);
				bodyEllipsis = t; endTok = t; isDirtyLoop = false;
			} else if (StartOf(28)) {
			} else SynErr(225);
			if (guardEllipsis != null || bodyEllipsis != null) {
			 if (mod != null) {
			   SemErr(mod[0].E.tok, "'modifies' clauses are not allowed on refining loops");
			 }
			 if (body == null && !isDirtyLoop) {
			   body = new BlockStmt(x, endTok, new List<Statement>());
			 }
			 stmt = new WhileStmt(x, endTok, guard, invariants, new Specification<Expression>(decreases, decAttrs), new Specification<FrameExpression>(null, null), body);
			 stmt = new SkeletonStatement(stmt, guardEllipsis, bodyEllipsis);
			} else {
			 // The following statement protects against crashes in case of parsing errors
			 if (body == null && !isDirtyLoop) {
			   body = new BlockStmt(x, endTok, new List<Statement>());
			 }
			 stmt = new WhileStmt(x, endTok, guard, invariants, new Specification<Expression>(decreases, decAttrs), new Specification<FrameExpression>(mod, modAttrs), body);
			}

		} else SynErr(226);
	}

	void NestedMatchStmt(out Statement/*!*/ s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null);
		IToken x;  Expression/*!*/ e;  NestedMatchCaseStmt/*!*/ c;
		List<NestedMatchCaseStmt/*!*/> cases = new List<NestedMatchCaseStmt/*!*/>();
		bool usesOptionalBraces = false;

		Expect(118);
		x = t;
		Expression(out e, true, true);
		if (la.kind == _lbrace) {
			Expect(75);
			usesOptionalBraces = true;
			while (la.kind == 38) {
				NestedCaseStatement(out c);
				cases.Add(c);
			}
			Expect(76);
		} else if (StartOf(28)) {
			while (la.kind == _case) {
				NestedCaseStatement(out c);
				cases.Add(c);
			}
		} else SynErr(227);
		s = new NestedMatchStmt(x, t, e, cases, usesOptionalBraces);
	}

	void ForallStmt(out Statement/*!*/ s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null);
		IToken/*!*/ x = Token.NoToken;
		List<BoundVar> bvars = null;
		Attributes attrs = null;
		Expression range = null;
		var ens = new List<MaybeFreeExpression/*!*/>();
		bool isFree;
		Expression/*!*/ e;
		BlockStmt block = null;
		IToken bodyStart, bodyEnd;
		IToken tok = Token.NoToken;

		if (la.kind == 121) {
			Get();
			x = t; tok = x;
		} else if (la.kind == 122) {
			Get();
			x = t;
			errors.Deprecated(t, "the 'parallel' keyword has been deprecated; the comprehension statement now uses the keyword 'forall' (and the parentheses around the bound variables are now optional)");

		} else SynErr(228);
		if (la.kind == _openparen) {
			Expect(79);
			if (la.kind == 1) {
				QuantifierDomain(out bvars, out attrs, out range);
			}
			Expect(80);
		} else if (StartOf(29)) {
			if (la.kind == _ident) {
				QuantifierDomain(out bvars, out attrs, out range);
			}
		} else SynErr(229);
		if (bvars == null) { bvars = new List<BoundVar>(); }
		if (range == null) { range = new LiteralExpr(x, true); }

		while (la.kind == 71 || la.kind == 106) {
			isFree = false;
			if (la.kind == 106) {
				Get();
				isFree = true;
				errors.Deprecated(t, "the 'free' keyword is soon to be deprecated");

			}
			Expect(71);
			Expression(out e, false, true);
			ens.Add(new MaybeFreeExpression(e, isFree));
			OldSemi();
			tok = t;
		}
		if (la.kind == _lbrace) {
			BlockStmt(out block, out bodyStart, out bodyEnd);
		}
		if (DafnyOptions.O.DisallowSoundnessCheating && block == null && 0 < ens.Count) {
		  SemErr(t, "a forall statement with an ensures clause must have a body");
		}

		if (block != null) {
		  tok = block.EndTok;
		}
		s = new ForallStmt(x, tok, bvars, attrs, range, ens, block);

	}

	void CalcStmt(out Statement s) {
		Contract.Ensures(Contract.ValueAtReturn(out s) != null);
		IToken x;
		Attributes attrs = null;
		CalcStmt.CalcOp op, userSuppliedOp = null, resOp = Microsoft.Dafny.CalcStmt.DefaultOp;
		var lines = new List<Expression>();
		var hints = new List<BlockStmt>();
		CalcStmt.CalcOp stepOp;
		var stepOps = new List<CalcStmt.CalcOp>();
		Expression e;
		IToken opTok;
		IToken danglingOperator = null;

		Expect(37);
		x = t;
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (StartOf(30)) {
			CalcOp(out opTok, out userSuppliedOp);
			if (userSuppliedOp.ResultOp(userSuppliedOp) == null) { // guard against non-transitive calcOp (like !=)
			 SemErr(opTok, "the main operator of a calculation must be transitive");
			} else {
			 resOp = userSuppliedOp;
			}

		}
		Expect(75);
		while (StartOf(8)) {
			Expression(out e, false, true);
			lines.Add(e); stepOp = null; danglingOperator = null;
			Expect(34);
			if (StartOf(30)) {
				CalcOp(out opTok, out op);
				var maybeOp = resOp.ResultOp(op);
				if (maybeOp == null) {
				 SemErr(opTok, "this operator cannot continue this calculation");
				} else {
				 stepOp = op;
				 resOp = maybeOp;
				 danglingOperator = opTok;
				}

			}
			stepOps.Add(stepOp);
			var subhints = new List<Statement>();
			IToken hintStart = la;  IToken hintEnd = hintStart;
			IToken t0, t1;
			BlockStmt subBlock; Statement subCalc;

			while (la.kind == _lbrace || la.kind == _calc) {
				if (la.kind == 75) {
					BlockStmt(out subBlock, out t0, out t1);
					hintEnd = subBlock.EndTok; subhints.Add(subBlock);
				} else if (la.kind == 37) {
					CalcStmt(out subCalc);
					hintEnd = subCalc.EndTok; subhints.Add(subCalc);
				} else SynErr(230);
			}
			var h = new BlockStmt(hintStart, hintEnd, subhints); // if the hint is empty, hintStart is the first token of the next line, but it doesn't matter because the block statement is just used as a container
			hints.Add(h);
			if (h.Body.Count != 0) { danglingOperator = null; }

		}
		Expect(76);
		if (danglingOperator != null) {
		 SemErr(danglingOperator, "a calculation cannot end with an operator");
		}
		if (lines.Count > 0) {
		 // Repeat the last line to create a dummy line for the dangling hint
		 lines.Add(lines[lines.Count - 1]);
		}
		s = new CalcStmt(x, t, userSuppliedOp, lines, hints, stepOps, attrs);

	}

	void ModifyStmt(out Statement s) {
		IToken tok;  IToken endTok = Token.NoToken;
		Attributes attrs = null;
		FrameExpression fe;  var mod = new List<FrameExpression>();
		BlockStmt body = null;  IToken bodyStart;
		IToken ellipsisToken = null;

		Expect(123);
		tok = t;
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (StartOf(20)) {
			FrameExpression(out fe, false, true);
			mod.Add(fe);
			while (la.kind == 26) {
				Get();
				FrameExpression(out fe, false, true);
				mod.Add(fe);
			}
		} else if (la.kind == 88) {
			Get();
			ellipsisToken = t;
		} else SynErr(231);
		if (la.kind == 75) {
			BlockStmt(out body, out bodyStart, out endTok);
		} else if (la.kind == 34) {
			while (!(la.kind == 0 || la.kind == 34)) {SynErr(232); Get();}
			Get();
			endTok = t;
		} else SynErr(233);
		s = new ModifyStmt(tok, endTok, mod, attrs, body);
		if (ellipsisToken != null) {
		 s = new SkeletonStatement(s, ellipsisToken, null);
		}

	}

	void ReturnStmt(out Statement/*!*/ s) {
		IToken returnTok = null;
		List<AssignmentRhs> rhss = null;
		AssignmentRhs r;
		bool isYield = false;

		if (la.kind == 114) {
			Get();
			returnTok = t;
		} else if (la.kind == 107) {
			Get();
			returnTok = t; isYield = true;
		} else SynErr(234);
		if (StartOf(31)) {
			Rhs(out r);
			rhss = new List<AssignmentRhs>(); rhss.Add(r);
			while (la.kind == 26) {
				Get();
				Rhs(out r);
				rhss.Add(r);
			}
		}
		Expect(34);
		if (isYield) {
		 s = new YieldStmt(returnTok, t, rhss);
		} else {
		 s = new ReturnStmt(returnTok, t, rhss);
		}

	}

	void SkeletonStmt(out Statement s) {
		List<IToken> names = null;
		List<Expression> exprs = null;
		IToken tok, dotdotdot, whereTok;
		Expression e;
		Expect(88);
		dotdotdot = t;
		if (la.kind == 113) {
			Get();
			names = new List<IToken>(); exprs = new List<Expression>(); whereTok = t;
			Ident(out tok);
			names.Add(tok);
			while (la.kind == 26) {
				Get();
				Ident(out tok);
				names.Add(tok);
			}
			Expect(29);
			Expression(out e, false, true);
			exprs.Add(e);
			while (la.kind == 26) {
				Get();
				Expression(out e, false, true);
				exprs.Add(e);
			}
			if (exprs.Count != names.Count) {
			 SemErr(whereTok, exprs.Count < names.Count ? "not enough expressions" : "too many expressions");
			 names = null; exprs = null;
			}

		}
		Expect(34);
		s = new SkeletonStatement(dotdotdot, t, names, exprs);
	}

	void Rhs(out AssignmentRhs r) {
		Contract.Ensures(Contract.ValueAtReturn<AssignmentRhs>(out r) != null);
		IToken/*!*/ x, newToken;  Expression/*!*/ e;
		Type ty = new InferredTypeProxy();
		List<Expression> ee = null;
		List<Expression> args = null;
		Expression arrayElementInit = null;
		List<Expression> display = null;
		r = dummyRhs;  // to please compiler
		Attributes attrs = null;

		if (la.kind == 99) {
			Get();
			newToken = t;
			if (la.kind == 77) {
				NewArray(out ee, out arrayElementInit, out display);
			} else if (StartOf(5)) {
				TypeAndToken(out x, out ty, false);
				if (la.kind == 77 || la.kind == 79) {
					if (la.kind == 77) {
						NewArray(out ee, out arrayElementInit, out display);
					} else {
						x = null; args = new List<Expression/*!*/>();
						Get();
						if (StartOf(8)) {
							Expressions(args);
						}
						Expect(80);
					}
				}
			} else SynErr(235);
			if (ee != null) {
			 if (display != null) {
			   r = new TypeRhs(newToken, ty, ee[0], display);
			 } else {
			   r = new TypeRhs(newToken, ty, ee, arrayElementInit);
			 }
			} else if (args != null) {
			 r = new TypeRhs(newToken, ty, args, false);
			} else {
			 r = new TypeRhs(newToken, ty);
			}

		} else if (la.kind == 86) {
			Get();
			r = new HavocRhs(t);
		} else if (StartOf(8)) {
			Expression(out e, false, true);
			r = new ExprRhs(e);
		} else SynErr(236);
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		r.Attributes = attrs;
	}

	void Lhs(out Expression e) {
		e = dummyExpr;  // the assignment is to please the compiler, the dummy value to satisfy contracts in the event of a parse error

		if (la.kind == 1) {
			NameSegment(out e);
			while (la.kind == 32 || la.kind == 77 || la.kind == 79) {
				Suffix(ref e);
			}
		} else if (StartOf(32)) {
			ConstAtomExpression(out e, false, false);
			Suffix(ref e);
			while (la.kind == 32 || la.kind == 77 || la.kind == 79) {
				Suffix(ref e);
			}
		} else SynErr(237);
	}

	void NewArray(out List<Expression> ee, out Expression arrayElementInit, out List<Expression> display ) {
		ee = new List<Expression>();
		arrayElementInit = null;
		display = null;
		IToken x;

		Expect(77);
		x = t;
		if (la.kind == 78) {
			Get();
			Expect(77);
			display = new List<Expression>();
			if (StartOf(8)) {
				Expressions(display);
			}
			Expect(78);
			ee.Add(new LiteralExpr(x, display.Count));

		} else if (StartOf(8)) {
			Expressions(ee);
			Expect(78);
			var tmp = theBuiltIns.ArrayType(ee.Count, new IntType(), true);

			if (la.kind == 77 || la.kind == 79) {
				if (la.kind == 79) {
					Get();
					Expression(out arrayElementInit, true, true);
					Expect(80);
				} else {
					Get();
					if (ee.Count > 1) {
					 SemErr(t, "An initializing element display is allowed only for 1-dimensional arrays");
					}
					display = new List<Expression>();

					if (StartOf(8)) {
						Expressions(display);
					}
					Expect(78);
				}
			}
		} else SynErr(238);
		if (ee.Count == 0) {
		 // an error occurred while parsing, but we still want to make sure to return a nonempty "ee"
		 ee.Add(new LiteralExpr(x, 0));
		}

	}

	void Expressions(List<Expression> args) {
		Expression e;
		Expression(out e, true, true);
		args.Add(e);
		while (la.kind == 26) {
			Get();
			Expression(out e, true, true);
			args.Add(e);
		}
	}

	void CasePatternLocal(out CasePattern<LocalVariable> pat, bool isGhost) {
		IToken id;  List<CasePattern<LocalVariable>> arguments;
		LocalVariable local;
		pat = null;

		if (IsIdentParen()) {
			Ident(out id);
			Expect(79);
			arguments = new List<CasePattern<LocalVariable>>();
			if (la.kind == 1 || la.kind == 79) {
				CasePatternLocal(out pat, isGhost);
				arguments.Add(pat);
				while (la.kind == 26) {
					Get();
					CasePatternLocal(out pat, isGhost);
					arguments.Add(pat);
				}
			}
			Expect(80);
			pat = new CasePattern<LocalVariable>(id, id.val, arguments);
		} else if (la.kind == 79) {
			Get();
			id = t;
			arguments = new List<CasePattern<LocalVariable>>();

			if (la.kind == 1 || la.kind == 79) {
				CasePatternLocal(out pat, isGhost);
				arguments.Add(pat);
				while (la.kind == 26) {
					Get();
					CasePatternLocal(out pat, isGhost);
					arguments.Add(pat);
				}
			}
			Expect(80);
			theBuiltIns.TupleType(id, arguments.Count, true); // make sure the tuple type exists
			string ctor = BuiltIns.TupleTypeCtorNamePrefix + arguments.Count;  //use the TupleTypeCtors
			pat = new CasePattern<LocalVariable>(id, ctor, arguments);

		} else if (la.kind == 1) {
			LocalIdentTypeOptional(out local, isGhost);
			pat = new CasePattern<LocalVariable>(local.Tok, local);

		} else SynErr(239);
		if (pat == null) {
		 pat = new CasePattern<LocalVariable>(t, "_ParseError", new List<CasePattern<LocalVariable>>());
		}

	}

	void AlternativeBlock(bool allowExistentialGuards, out List<GuardedAlternative> alternatives, out bool usesOptionalBraces, out IToken endTok) {
		alternatives = new List<GuardedAlternative>();
		endTok = null;
		usesOptionalBraces = false;
		GuardedAlternative alt;

		if (la.kind == 75) {
			Get();
			usesOptionalBraces = true;
			while (la.kind == 38) {
				AlternativeBlockCase(allowExistentialGuards, out alt);
				alternatives.Add(alt);
			}
			Expect(76);
		} else if (la.kind == 38) {
			AlternativeBlockCase(allowExistentialGuards, out alt);
			alternatives.Add(alt);
			while (la.kind == _case) {
				AlternativeBlockCase(allowExistentialGuards, out alt);
				alternatives.Add(alt);
			}
		} else SynErr(240);
		endTok = t;
	}

	void ExistentialGuard(out Expression e, bool allowLambda) {
		var bvars = new List<BoundVar>();
		BoundVar bv;  IToken x;
		Attributes attrs = null;
		Expression body;

		IdentTypeOptional(out bv);
		bvars.Add(bv); x = bv.tok;
		while (la.kind == 26) {
			Get();
			IdentTypeOptional(out bv);
			bvars.Add(bv);
		}
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		Expect(30);
		Expression(out body, true, allowLambda);
		e = new ExistsExpr(x, bvars, null, body, attrs);
	}

	void Guard(out Expression e) {
		Expression/*!*/ ee;  e = null;
		if (la.kind == 86) {
			Get();
			e = null;
		} else if (IsParenStar()) {
			Expect(79);
			Expect(86);
			Expect(80);
			e = null;
		} else if (StartOf(8)) {
			Expression(out ee, true, true);
			e = ee;
		} else SynErr(241);
	}

	void AlternativeBlockCase(bool allowExistentialGuards, out GuardedAlternative alt) {
		IToken x;
		Expression e; bool isExistentialGuard;
		List<Statement> body;

		Expect(38);
		x = t; isExistentialGuard = false; e = dummyExpr;
		if (allowExistentialGuards && IsExistentialGuard()) {
			ExistentialGuard(out e, false );
			isExistentialGuard = true;
		} else if (StartOf(8)) {
			Expression(out e, true, false);
		} else SynErr(242);
		Expect(35);
		body = new List<Statement>();
		while (!(StartOf(33))) {SynErr(243); Get();}
		while (IsNotEndOfCase()) {
			Stmt(body);
			while (!(StartOf(33))) {SynErr(244); Get();}
		}
		alt = new GuardedAlternative(x, isExistentialGuard, e, body);
	}

	void LoopSpec(List<MaybeFreeExpression> invariants, List<Expression> decreases, ref List<FrameExpression> mod, ref Attributes decAttrs, ref Attributes modAttrs) {
		Expression e; FrameExpression fe;
		bool isFree = false; Attributes attrs = null;

		if (la.kind == 45 || la.kind == 106) {
			while (!(la.kind == 0 || la.kind == 45 || la.kind == 106)) {SynErr(245); Get();}
			if (la.kind == 106) {
				Get();
				isFree = true; errors.Deprecated(t, "the 'free' keyword is soon to be deprecated");
			}
			Expect(45);
			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			Expression(out e, false, true);
			invariants.Add(new MaybeFreeExpression(e, isFree, attrs));
			OldSemi();
		} else if (la.kind == 44) {
			while (!(la.kind == 0 || la.kind == 44)) {SynErr(246); Get();}
			Get();
			while (la.kind == 74) {
				Attribute(ref decAttrs);
			}
			DecreasesList(decreases, true, true);
			OldSemi();
		} else if (la.kind == 68) {
			while (!(la.kind == 0 || la.kind == 68)) {SynErr(247); Get();}
			Get();
			mod = mod ?? new List<FrameExpression>();
			while (la.kind == 74) {
				Attribute(ref modAttrs);
			}
			FrameExpression(out fe, false, true);
			mod.Add(fe);
			while (la.kind == 26) {
				Get();
				FrameExpression(out fe, false, true);
				mod.Add(fe);
			}
			OldSemi();
		} else SynErr(248);
	}

	void ExtendedPattern(out ExtendedPattern pat) {
		IToken id; List<ExtendedPattern> arguments; pat = null; LiteralExpr lit; Expression te;

		if (StartOf(32)) {
			ConstAtomExpression(out te, false, false);
			if (te is LiteralExpr){
			lit = (LiteralExpr)te;
			pat = new LitPattern(lit.tok, lit);
			} else { SemErr(t, "invalid AtomConst used in pattern");
			        pat = null; }

		} else if (la.kind == 1) {
			Ident(out id);
			arguments = new List<ExtendedPattern>();
			if (la.kind == 79) {
				Get();
				ExtendedPattern(out pat);
				arguments.Add(pat);
				while (la.kind == 26) {
					Get();
					ExtendedPattern(out pat);
					arguments.Add(pat);
				}
				Expect(80);
				pat = new IdPattern(id, id.val, arguments);
			}
		} else SynErr(249);
		if (pat == null) {
		 pat = new IdPattern(t, "_ParseError", new List<ExtendedPattern>());
		}

	}

	void ConstAtomExpression(out Expression e, bool allowSemi, bool allowLambda) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		IToken/*!*/ x;  BigInteger n;   Basetypes.BigDec d;
		e = dummyExpr;  Type toType = null;

		switch (la.kind) {
		case 147: {
			Get();
			e = new LiteralExpr(t, false);
			break;
		}
		case 148: {
			Get();
			e = new LiteralExpr(t, true);
			break;
		}
		case 149: {
			Get();
			e = new LiteralExpr(t);
			break;
		}
		case 2: case 3: {
			Nat(out n);
			e = new LiteralExpr(t, n);
			break;
		}
		case 4: {
			Dec(out d);
			e = new LiteralExpr(t, d);
			break;
		}
		case 23: {
			Get();
			e = new CharLiteralExpr(t, t.val.Substring(1, t.val.Length - 2));
			break;
		}
		case 24: {
			Get();
			bool isVerbatimString;
			string s = Util.RemoveParsedStringQuotes(t.val, out isVerbatimString);
			e = new StringLiteralExpr(t, s, isVerbatimString);

			break;
		}
		case 150: {
			Get();
			e = new ThisExpr(t);
			break;
		}
		case 151: {
			Get();
			x = t;
			Expect(79);
			Expression(out e, true, true);
			Expect(80);
			e = new UnaryOpExpr(x, UnaryOpExpr.Opcode.Fresh, e);
			break;
		}
		case 152: {
			Get();
			x = t;
			Expect(79);
			Expression(out e, true, true);
			Expect(80);
			e = new UnaryOpExpr(x, UnaryOpExpr.Opcode.Allocated, e);
			break;
		}
		case 153: {
			Get();
			x = t; FrameExpression fe; var mod = new List<FrameExpression>(); IToken oldAt = null;
			if (la.kind == 154) {
				Get();
				LabelIdent(out oldAt);
			}
			Expect(79);
			FrameExpression(out fe, false, false);
			mod.Add(fe);
			while (la.kind == 26) {
				Get();
				FrameExpression(out fe, false, false);
				mod.Add(fe);
			}
			Expect(80);
			e = new UnchangedExpr(x, mod, oldAt?.val);
			break;
		}
		case 155: {
			Get();
			x = t; IToken oldAt = null;
			if (la.kind == 154) {
				Get();
				LabelIdent(out oldAt);
			}
			Expect(79);
			Expression(out e, true, true);
			Expect(80);
			e = new OldExpr(x, e, oldAt?.val);
			break;
		}
		case 27: {
			Get();
			x = t;
			Expression(out e, true, true, false);
			e = new UnaryOpExpr(x, UnaryOpExpr.Opcode.Cardinality, e);
			Expect(27);
			break;
		}
		case 10: case 12: {
			if (la.kind == 10) {
				Get();
				x = t; toType = new IntType();
			} else {
				Get();
				x = t; toType = new RealType();
			}
			errors.Deprecated(t, string.Format("the syntax \"{0}(expr)\" for type conversions has been deprecated; the new syntax is \"expr as {0}\"", x.val));
			Expect(79);
			Expression(out e, true, true);
			Expect(80);
			e = new ConversionExpr(x, e, toType);
			break;
		}
		case 79: {
			ParensExpression(out e, allowSemi, allowLambda);
			break;
		}
		default: SynErr(250); break;
		}
	}

	void NestedCaseStatement(out NestedMatchCaseStmt/*!*/ c) {
		Contract.Ensures(Contract.ValueAtReturn(out c) != null);
		IToken/*!*/ x;
		ExtendedPattern/*!*/ pat = null;
		ExtendedPattern/*!*/ tpat;
		var body = new List<Statement/*!*/>();

		Expect(38);
		x = t;
		if (la.kind == _openparen) {
			Expect(79);
			List<ExtendedPattern> arguments = new List<ExtendedPattern>();
			ExtendedPattern(out tpat);
			arguments.Add(tpat);
			while (la.kind == 26) {
				Get();
				ExtendedPattern(out tpat);
				arguments.Add(tpat);
			}
			Expect(80);
			pat = new IdPattern(t, "", arguments);
		} else if (StartOf(22)) {
			ExtendedPattern(out pat);

		} else SynErr(251);
		Expect(35);
		while (!(StartOf(33))) {SynErr(252); Get();}
		while (IsNotEndOfCase()) {
			Stmt(body);
			while (!(StartOf(33))) {SynErr(253); Get();}
		}
		c = new NestedMatchCaseStmt(x, pat, body);
	}

	void QuantifierDomain(out List<BoundVar> bvars, out Attributes attrs, out Expression range) {
		bvars = new List<BoundVar>();
		BoundVar/*!*/ bv;
		attrs = null;
		range = null;

		IdentTypeOptional(out bv);
		bvars.Add(bv);
		while (la.kind == 26) {
			Get();
			IdentTypeOptional(out bv);
			bvars.Add(bv);
		}
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (la.kind == _verticalbar) {
			Expect(27);
			Expression(out range, true, true);
		}
	}

	void CalcOp(out IToken x, out CalcStmt.CalcOp/*!*/ op) {
		var binOp = BinaryExpr.Opcode.Eq; // Returns Eq if parsing fails because it is compatible with any other operator
		Expression k = null;
		x = null;

		switch (la.kind) {
		case 83: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Eq;
			if (la.kind == 124) {
				Get();
				Expect(77);
				Expression(out k, true, true);
				Expect(78);
			}
			break;
		}
		case 81: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Lt;
			break;
		}
		case 82: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Gt;
			break;
		}
		case 125: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Le;
			break;
		}
		case 126: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Ge;
			break;
		}
		case 84: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Neq;
			break;
		}
		case 85: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Neq;
			break;
		}
		case 127: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Le;
			break;
		}
		case 128: {
			Get();
			x = t;  binOp = BinaryExpr.Opcode.Ge;
			break;
		}
		case 129: case 130: {
			EquivOp();
			x = t;  binOp = BinaryExpr.Opcode.Iff;
			break;
		}
		case 131: case 132: {
			ImpliesOp();
			x = t;  binOp = BinaryExpr.Opcode.Imp;
			break;
		}
		case 133: case 134: {
			ExpliesOp();
			x = t;  binOp = BinaryExpr.Opcode.Exp;
			break;
		}
		default: SynErr(254); break;
		}
		if (k == null) {
		 op = new Microsoft.Dafny.CalcStmt.BinaryCalcOp(binOp);
		} else {
		 op = new Microsoft.Dafny.CalcStmt.TernaryCalcOp(k);
		}

	}

	void EquivOp() {
		if (la.kind == 129) {
			Get();
		} else if (la.kind == 130) {
			Get();
		} else SynErr(255);
	}

	void ImpliesOp() {
		if (la.kind == 131) {
			Get();
		} else if (la.kind == 132) {
			Get();
		} else SynErr(256);
	}

	void ExpliesOp() {
		if (la.kind == 133) {
			Get();
		} else if (la.kind == 134) {
			Get();
		} else SynErr(257);
	}

	void AndOp() {
		if (la.kind == 135) {
			Get();
		} else if (la.kind == 136) {
			Get();
		} else SynErr(258);
	}

	void OrOp() {
		if (la.kind == 137) {
			Get();
		} else if (la.kind == 138) {
			Get();
		} else SynErr(259);
	}

	void NegOp() {
		if (la.kind == 103) {
			Get();
		} else if (la.kind == 139) {
			Get();
		} else SynErr(260);
	}

	void Forall() {
		if (la.kind == 121) {
			Get();
		} else if (la.kind == 140) {
			Get();
		} else SynErr(261);
	}

	void Exists() {
		if (la.kind == 141) {
			Get();
		} else if (la.kind == 142) {
			Get();
		} else SynErr(262);
	}

	void QSep() {
		if (la.kind == 28) {
			Get();
		} else if (la.kind == 31) {
			Get();
		} else SynErr(263);
	}

	void EquivExpression(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;
		ImpliesExpliesExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
		while (IsEquivOp()) {
			EquivOp();
			x = t;
			ImpliesExpliesExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
			e0 = new BinaryExpr(x, BinaryExpr.Opcode.Iff, e0, e1);
		}
	}

	void ImpliesExpliesExpression(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;
		LogicalExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
		if (IsImpliesOp() || IsExpliesOp()) {
			if (la.kind == 131 || la.kind == 132) {
				ImpliesOp();
				x = t;
				ImpliesExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, BinaryExpr.Opcode.Imp, e0, e1);
			} else if (la.kind == 133 || la.kind == 134) {
				ExpliesOp();
				x = t;
				LogicalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, BinaryExpr.Opcode.Exp, e1, e0);
				while (IsExpliesOp()) {
					ExpliesOp();
					x = t;
					LogicalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
					e0 = new BinaryExpr(x, BinaryExpr.Opcode.Exp, e1, e0);

				}
			} else SynErr(264);
		}
	}

	void LogicalExpression(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;
		Expression first;
		e0 = dummyExpr; /* mute the warning */

		if (la.kind == 135 || la.kind == 136) {
			AndOp();
			x = t;
			RelationalExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
			first = e0;
			while (IsAndOp()) {
				AndOp();
				x = t;
				RelationalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, BinaryExpr.Opcode.And, e0, e1);
			}
			if (e0 == first) {
			 // There was only one conjunct. To make sure that the type checker still checks it to
			 // be a boolean, we conjoin "true" to its left.
			 e0 = new BinaryExpr(x, BinaryExpr.Opcode.And, new LiteralExpr(x, true), e0);
			}

		} else if (la.kind == 137 || la.kind == 138) {
			OrOp();
			x = t;
			RelationalExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
			first = e0;
			while (IsOrOp()) {
				OrOp();
				x = t;
				RelationalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, BinaryExpr.Opcode.Or, e0, e1);
			}
			if (e0 == first) {
			 // There was only one disjunct. To make sure that the type checker still checks it to
			 // be a boolean, we disjoin [sic] "false" to its left.
			 e0 = new BinaryExpr(x, BinaryExpr.Opcode.Or, new LiteralExpr(x, false), e0);
			}

		} else if (StartOf(34)) {
			RelationalExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
			if (IsAndOp() || IsOrOp()) {
				if (la.kind == 135 || la.kind == 136) {
					AndOp();
					x = t;
					RelationalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
					e0 = new BinaryExpr(x, BinaryExpr.Opcode.And, e0, e1);
					while (IsAndOp()) {
						AndOp();
						x = t;
						RelationalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
						e0 = new BinaryExpr(x, BinaryExpr.Opcode.And, e0, e1);
					}
				} else if (la.kind == 137 || la.kind == 138) {
					OrOp();
					x = t;
					RelationalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
					e0 = new BinaryExpr(x, BinaryExpr.Opcode.Or, e0, e1);
					while (IsOrOp()) {
						OrOp();
						x = t;
						RelationalExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
						e0 = new BinaryExpr(x, BinaryExpr.Opcode.Or, e0, e1);
					}
				} else SynErr(265);
			}
		} else SynErr(266);
	}

	void ImpliesExpression(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;
		LogicalExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
		if (IsImpliesOp()) {
			ImpliesOp();
			x = t;
			ImpliesExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
			e0 = new BinaryExpr(x, BinaryExpr.Opcode.Imp, e0, e1);
		}
	}

	void RelationalExpression(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		IToken x = null;  Expression e0, e1 = null;  BinaryExpr.Opcode op;
		List<Expression> chain = null;
		List<BinaryExpr.Opcode> ops = null;
		List<IToken> opLocs = null;
		List<Expression/*?*/> prefixLimits = null;
		Expression k;
		int kind = 0;  // 0 ("uncommitted") indicates chain of ==, possibly with one !=
		              // 1 ("ascending")   indicates chain of ==, <, <=, possibly with one !=
		              // 2 ("descending")  indicates chain of ==, >, >=, possibly with one !=
		              // 3 ("illegal")     indicates illegal chain
		              // 4 ("disjoint")    indicates chain of disjoint set operators
		bool hasSeenNeq = false;

		ShiftTerm(out e0, allowSemi, allowLambda, allowBitwiseOps);
		e = e0;
		if (IsRelOp()) {
			RelOp(out x, out op, out k);
			ShiftTerm(out e1, allowSemi, allowLambda, allowBitwiseOps);
			if (k == null) {
			 e = new BinaryExpr(x, op, e0, e1);
			} else {
			 Contract.Assert(op == BinaryExpr.Opcode.Eq || op == BinaryExpr.Opcode.Neq);
			 e = new TernaryExpr(x, op == BinaryExpr.Opcode.Eq ? TernaryExpr.Opcode.PrefixEqOp : TernaryExpr.Opcode.PrefixNeqOp, k, e0, e1);
			}

			while (IsRelOp()) {
				if (chain == null) {
				 chain = new List<Expression>();
				 ops = new List<BinaryExpr.Opcode>();
				 opLocs = new List<IToken>();
				 prefixLimits = new List<Expression>();
				 chain.Add(e0); ops.Add(op); opLocs.Add(x); prefixLimits.Add(k); chain.Add(e1);
				 switch (op) {
				   case BinaryExpr.Opcode.Eq:
				     kind = 0;  break;
				   case BinaryExpr.Opcode.Neq:
				     kind = 0;  hasSeenNeq = true;  break;
				   case BinaryExpr.Opcode.Lt:
				   case BinaryExpr.Opcode.Le:
				     kind = 1;  break;
				   case BinaryExpr.Opcode.Gt:
				   case BinaryExpr.Opcode.Ge:
				     kind = 2;  break;
				   case BinaryExpr.Opcode.Disjoint:
				     kind = 4;  break;
				   default:
				     kind = 3;  break;
				 }
				}

				RelOp(out x, out op, out k);
				switch (op) {
				 case BinaryExpr.Opcode.Eq:
				   if (kind != 0 && kind != 1 && kind != 2) { SemErr(x, "chaining not allowed from the previous operator"); kind = 3; }
				   break;
				 case BinaryExpr.Opcode.Neq:
				   if (hasSeenNeq) { SemErr(x, "a chain cannot have more than one != operator"); kind = 3; }
				   else if (kind != 0 && kind != 1 && kind != 2) { SemErr(x, "this operator cannot continue this chain"); kind = 3; }
				   hasSeenNeq = true;  break;
				 case BinaryExpr.Opcode.Lt:
				 case BinaryExpr.Opcode.Le:
				   if (kind == 0) { kind = 1; }
				   else if (kind != 1) { SemErr(x, "this operator chain cannot continue with an ascending operator"); kind = 3; }
				   break;
				 case BinaryExpr.Opcode.Gt:
				 case BinaryExpr.Opcode.Ge:
				   if (kind == 0) { kind = 2; }
				   else if (kind != 2) { SemErr(x, "this operator chain cannot continue with a descending operator"); kind = 3; }
				   break;
				 case BinaryExpr.Opcode.Disjoint:
				   if (kind != 4) { SemErr(x, "can only chain disjoint (!!) with itself."); kind = 3; }
				   break;
				 default:
				   SemErr(x, "this operator cannot be part of a chain");
				   kind = 3;  break;
				}

				ShiftTerm(out e1, allowSemi, allowLambda, allowBitwiseOps);
				ops.Add(op); opLocs.Add(x); prefixLimits.Add(k); chain.Add(e1);

			}
		}
		if (chain != null && kind != 3) {
		 e = new ChainingExpression(opLocs[0], chain, ops, opLocs, prefixLimits);
		}

	}

	void ShiftTerm(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null);
		IToken x = Token.NoToken;  Expression e1;  BinaryExpr.Opcode op = BinaryExpr.Opcode.LeftShift/*(dummy)*/;

		Term(out e0, allowSemi, allowLambda, allowBitwiseOps);
		while (IsShiftOp()) {
			if (la.kind == 81) {
				Get();
				x = t;  op = BinaryExpr.Opcode.LeftShift;
				Expect(81);
				x.val = "<<";  Contract.Assert(t.pos == x.pos + 1);
			} else if (la.kind == 82) {
				Get();
				x = t;  op = BinaryExpr.Opcode.RightShift;
				Expect(82);
				x.val = "<<";  Contract.Assert(t.pos == x.pos + 1);
			} else SynErr(267);
			Term(out e1, allowSemi, allowLambda, allowBitwiseOps);
			e0 = new BinaryExpr(x, op, e0, e1);
		}
	}

	void RelOp(out IToken/*!*/ x, out BinaryExpr.Opcode op, out Expression k) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null);
		x = Token.NoToken;  op = BinaryExpr.Opcode.Add/*(dummy)*/;
		IToken y;
		k = null;

		switch (la.kind) {
		case 83: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Eq;
			if (la.kind == 124) {
				Get();
				Expect(77);
				Expression(out k, true, true);
				Expect(78);
			}
			break;
		}
		case 81: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Lt;
			break;
		}
		case 82: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Gt;
			break;
		}
		case 125: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Le;
			break;
		}
		case 126: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Ge;
			break;
		}
		case 84: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Neq;
			if (la.kind == 124) {
				Get();
				Expect(77);
				Expression(out k, true, true);
				Expect(78);
			}
			break;
		}
		case 43: {
			Get();
			x = t;  op = BinaryExpr.Opcode.In;
			break;
		}
		case 87: {
			Get();
			x = t;  op = BinaryExpr.Opcode.NotIn;
			break;
		}
		case 103: {
			Get();
			x = t;  y = Token.NoToken;
			if (la.val == "!") {
				Expect(103);
				y = t;
			}
			if (y == Token.NoToken) {
			 SemErr(x, "invalid RelOp");
			} else if (y.pos != x.pos + 1) {
			 SemErr(x, "invalid RelOp (perhaps you intended \"!!\" with no intervening whitespace?)");
			} else {
			 x.val = "!!";
			 op = BinaryExpr.Opcode.Disjoint;
			}

			break;
		}
		case 85: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Neq;
			break;
		}
		case 127: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Le;
			break;
		}
		case 128: {
			Get();
			x = t;  op = BinaryExpr.Opcode.Ge;
			break;
		}
		default: SynErr(268); break;
		}
	}

	void Term(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;  BinaryExpr.Opcode op;
		Factor(out e0, allowSemi, allowLambda, allowBitwiseOps);
		while (IsAddOp()) {
			AddOp(out x, out op);
			Factor(out e1, allowSemi, allowLambda, allowBitwiseOps);
			e0 = new BinaryExpr(x, op, e0, e1);
		}
	}

	void Factor(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;  BinaryExpr.Opcode op;
		BitvectorFactor(out e0, allowSemi, allowLambda, allowBitwiseOps);
		while (IsMulOp()) {
			MulOp(out x, out op);
			BitvectorFactor(out e1, allowSemi, allowLambda, allowBitwiseOps);
			e0 = new BinaryExpr(x, op, e0, e1);
		}
	}

	void AddOp(out IToken x, out BinaryExpr.Opcode op) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null); x = Token.NoToken;  op=BinaryExpr.Opcode.Add/*(dummy)*/;
		if (la.kind == 102) {
			Get();
			x = t;  op = BinaryExpr.Opcode.Add;
		} else if (la.kind == 104) {
			Get();
			x = t;  op = BinaryExpr.Opcode.Sub;
		} else SynErr(269);
	}

	void BitvectorFactor(out Expression e0, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e0) != null); IToken/*!*/ x;  Expression/*!*/ e1;  BinaryExpr.Opcode op;
		AsExpression(out e0, allowSemi, allowLambda, allowBitwiseOps);
		if (allowBitwiseOps && IsBitwiseOp()) {
			if (la.kind == 145) {
				op = BinaryExpr.Opcode.BitwiseAnd;
				Get();
				x = t;
				AsExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, op, e0, e1);
				while (IsBitwiseAndOp()) {
					Expect(145);
					x = t;
					AsExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
					e0 = new BinaryExpr(x, op, e0, e1);
				}
			} else if (la.kind == 27) {
				op = BinaryExpr.Opcode.BitwiseOr;
				Get();
				x = t;
				AsExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, op, e0, e1);
				while (IsBitwiseOrOp()) {
					Expect(27);
					x = t;
					AsExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
					e0 = new BinaryExpr(x, op, e0, e1);
				}
			} else if (la.kind == 146) {
				op = BinaryExpr.Opcode.BitwiseXor;
				Get();
				x = t;
				AsExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
				e0 = new BinaryExpr(x, op, e0, e1);
				while (IsBitwiseXorOp()) {
					Expect(146);
					x = t;
					AsExpression(out e1, allowSemi, allowLambda, allowBitwiseOps);
					e0 = new BinaryExpr(x, op, e0, e1);
				}
			} else SynErr(270);
		}
	}

	void MulOp(out IToken x, out BinaryExpr.Opcode op) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null); x = Token.NoToken;  op = BinaryExpr.Opcode.Add/*(dummy)*/;
		if (la.kind == 86) {
			Get();
			x = t;  op = BinaryExpr.Opcode.Mul;
		} else if (la.kind == 143) {
			Get();
			x = t;  op = BinaryExpr.Opcode.Div;
		} else if (la.kind == 144) {
			Get();
			x = t;  op = BinaryExpr.Opcode.Mod;
		} else SynErr(271);
	}

	void AsExpression(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		IToken tok; IToken x; Type toType;
		UnaryExpression(out e, allowSemi, allowLambda, allowBitwiseOps);
		while (IsAs()) {
			Expect(41);
			tok = t;
			TypeAndToken(out x, out toType, true);
			e = new ConversionExpr(tok, e, toType);
		}
	}

	void UnaryExpression(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null); IToken/*!*/ x;  e = dummyExpr;
		if (la.kind == 104) {
			Get();
			x = t;
			UnaryExpression(out e, allowSemi, allowLambda, allowBitwiseOps);
			e = new NegationExpression(x, e);
		} else if (la.kind == 103 || la.kind == 139) {
			NegOp();
			x = t;
			UnaryExpression(out e, allowSemi, allowLambda, allowBitwiseOps);
			e = new UnaryOpExpr(x, UnaryOpExpr.Opcode.Not, e);
		} else if (IsMapDisplay()) {
			Expect(21);
			x = t;
			MapDisplayExpr(x, true, out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (IsIMapDisplay()) {
			Expect(22);
			x = t;
			MapDisplayExpr(x, false, out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (IsISetDisplay()) {
			Expect(18);
			x = t;
			ISetDisplayExpr(x, false, out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (IsLambda(allowLambda)) {
			LambdaExpression(out e, allowSemi, allowBitwiseOps);
		} else if (StartOf(35)) {
			EndlessExpression(out e, allowSemi, allowLambda, allowBitwiseOps);
		} else if (la.kind == 1) {
			NameSegment(out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (la.kind == 75 || la.kind == 77) {
			DisplayExpr(out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (la.kind == 19) {
			MultiSetExpr(out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (la.kind == 20) {
			SeqConstructionExpr(out e);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else if (StartOf(32)) {
			ConstAtomExpression(out e, allowSemi, allowLambda);
			while (IsSuffix()) {
				Suffix(ref e);
			}
		} else SynErr(272);
	}

	void MapDisplayExpr(IToken/*!*/ mapToken, bool finite, out Expression e) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		List<ExpressionPair/*!*/>/*!*/ elements= new List<ExpressionPair/*!*/>() ;
		e = dummyExpr;

		Expect(77);
		if (StartOf(8)) {
			MapLiteralExpressions(out elements);
		}
		e = new MapDisplayExpr(mapToken, finite, elements);
		Expect(78);
	}

	void Suffix(ref Expression e) {
		Contract.Requires(e != null); Contract.Ensures(e!=null);
		IToken id, x;
		Expression e0 = null;  Expression e1 = null;  Expression ee;  bool anyDots = false;
		List<Expression> multipleLengths = null; bool takeRest = false; // takeRest is relevant only if multipleLengths is non-null
		List<Expression> multipleIndices = null;
		List<Tuple<IToken, string, Expression>> updates;
		Expression v;

		if (la.kind == 32) {
			Get();
			if (la.kind == 79) {
				Get();
				x = t; updates = new List<Tuple<IToken, string, Expression>>();
				MemberBindingUpdate(out id, out v);
				updates.Add(Tuple.Create(id, id.val, v));
				while (la.kind == 26) {
					Get();
					MemberBindingUpdate(out id, out v);
					updates.Add(Tuple.Create(id, id.val, v));
				}
				Expect(80);
				e = new DatatypeUpdateExpr(x, e, updates);
			} else if (StartOf(36)) {
				DotSuffix(out id, out x);
				if (x != null) {
				 // process id as a Suffix in its own right
				 e = new ExprDotName(id, e, id.val, null);
				 id = x;  // move to the next Suffix
				}
				IToken openParen = null;  List<Type> typeArgs = null;  List<Expression> args = null;

				if (IsGenericInstantiation(true)) {
					typeArgs = new List<Type>();
					GenericInstantiation(typeArgs);
				} else if (la.kind == 124) {
					HashCall(id, out openParen, out typeArgs, out args);
				} else if (StartOf(37)) {
				} else SynErr(273);
				e = new ExprDotName(id, e, id.val, typeArgs);
				if (openParen != null) {
				 e = new ApplySuffix(openParen, e, args);
				}

			} else SynErr(274);
		} else if (la.kind == 77) {
			Get();
			x = t;
			if (StartOf(8)) {
				Expression(out ee, true, true);
				e0 = ee;
				if (la.kind == 156) {
					Get();
					anyDots = true;
					if (StartOf(8)) {
						Expression(out ee, true, true);
						e1 = ee;
					}
				} else if (la.kind == 29) {
					Get();
					Expression(out ee, true, true);
					e1 = ee;
				} else if (la.kind == 25) {
					Get();
					multipleLengths = new List<Expression>();
					multipleLengths.Add(e0);  // account for the Expression read before the colon
					takeRest = true;

					if (StartOf(8)) {
						Expression(out ee, true, true);
						multipleLengths.Add(ee); takeRest = false;
						while (IsNonFinalColon()) {
							Expect(25);
							Expression(out ee, true, true);
							multipleLengths.Add(ee);
						}
						if (la.kind == 25) {
							Get();
							takeRest = true;
						}
					}
				} else if (la.kind == 26 || la.kind == 78) {
					while (la.kind == 26) {
						Get();
						Expression(out ee, true, true);
						if (multipleIndices == null) {
						 multipleIndices = new List<Expression>();
						 multipleIndices.Add(e0);
						}
						multipleIndices.Add(ee);

					}
				} else SynErr(275);
			} else if (la.kind == 156) {
				Get();
				anyDots = true;
				if (StartOf(8)) {
					Expression(out ee, true, true);
					e1 = ee;
				}
			} else SynErr(276);
			if (multipleIndices != null) {
			 e = new MultiSelectExpr(x, e, multipleIndices);
			 // make sure an array class with this dimensionality exists
			 var tmp = theBuiltIns.ArrayType(multipleIndices.Count, new IntType(), true);
			} else {
			 if (!anyDots && e0 == null) {
			   /* a parsing error occurred */
			   e0 = dummyExpr;
			 }
			 Contract.Assert(anyDots || e0 != null);
			 if (anyDots) {
			   //Contract.Assert(e0 != null || e1 != null);
			   e = new SeqSelectExpr(x, false, e, e0, e1);
			 } else if (multipleLengths != null) {
			   Expression prev = null;
			   List<Expression> seqs = new List<Expression>();
			    foreach (var len in multipleLengths) {
			      var end = prev == null ? len : new BinaryExpr(x, BinaryExpr.Opcode.Add, prev, len);
			      seqs.Add(new SeqSelectExpr(x, false, e, prev, end));
			      prev = end;
			    }
			   if (takeRest) {
			     seqs.Add(new SeqSelectExpr(x, false, e, prev, null));
			   }
			   e = new SeqDisplayExpr(x, seqs);
			 } else if (e1 == null) {
			   Contract.Assert(e0 != null);
			   e = new SeqSelectExpr(x, true, e, e0, null);
			 } else {
			   Contract.Assert(e0 != null);
			   e = new SeqUpdateExpr(x, e, e0, e1);
			 }
			}

			Expect(78);
		} else if (la.kind == 79) {
			Get();
			IToken openParen = t; var args = new List<Expression>();
			if (StartOf(8)) {
				Expressions(args);
			}
			Expect(80);
			e = new ApplySuffix(openParen, e, args);
		} else SynErr(277);
	}

	void ISetDisplayExpr(IToken/*!*/ setToken, bool finite, out Expression e) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		List<Expression> elements = new List<Expression/*!*/>();;
		e = dummyExpr;

		Expect(75);
		if (StartOf(8)) {
			Expressions(elements);
		}
		e = new SetDisplayExpr(setToken, finite, elements);
		Expect(76);
	}

	void LambdaExpression(out Expression e, bool allowSemi, bool allowBitwiseOps) {
		IToken x = Token.NoToken;
		IToken id;  BoundVar bv;
		var bvs = new List<BoundVar>();
		FrameExpression fe;  Expression ee;
		var reads = new List<FrameExpression>();
		Expression req = null;
		Expression body = null;

		if (la.kind == 1) {
			WildIdent(out id, true);
			x = t; bvs.Add(new BoundVar(id, id.val, new InferredTypeProxy()));
		} else if (la.kind == 79) {
			Get();
			x = t;
			if (la.kind == 1) {
				IdentTypeOptional(out bv);
				bvs.Add(bv);
				while (la.kind == 26) {
					Get();
					IdentTypeOptional(out bv);
					bvs.Add(bv);
				}
			}
			Expect(80);
		} else SynErr(278);
		while (la.kind == 69 || la.kind == 70) {
			if (la.kind == 69) {
				Get();
				PossiblyWildFrameExpression(out fe, true);
				reads.Add(fe);
				while (la.kind == 26) {
					Get();
					PossiblyWildFrameExpression(out fe, true);
					reads.Add(fe);
				}
			} else {
				Get();
				Expression(out ee, true, false);
				req = req == null ? ee : new BinaryExpr(req.tok, BinaryExpr.Opcode.And, req, ee);
			}
		}
		LambdaArrow();
		Expression(out body, allowSemi, true, allowBitwiseOps);
		e = new LambdaExpr(x, bvs, req, reads, body);
		theBuiltIns.CreateArrowTypeDecl(bvs.Count);

	}

	void EndlessExpression(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		IToken/*!*/ x;
		Expression e0, e1;
		Statement s;
		bool isExistentialGuard = false;
		e = dummyExpr;

		switch (la.kind) {
		case 116: {
			Get();
			x = t;
			if (IsExistentialGuard()) {
				ExistentialGuard(out e, true);
				isExistentialGuard = true;
			} else if (StartOf(8)) {
				Expression(out e, true, true);
			} else SynErr(279);
			Expect(39);
			Expression(out e0, true, true, true);
			Expect(40);
			Expression(out e1, allowSemi, allowLambda, allowBitwiseOps);
			if (isExistentialGuard) {
			 var exists = (ExistsExpr) e;
			 var LHSs = new List<CasePattern<BoundVar>>();
			 foreach (var v in exists.BoundVars) {
			   LHSs.Add(new CasePattern<BoundVar>(e.tok, v));
			 }
			 e0 = new LetExpr(e.tok, LHSs, new List<Expression>() { exists.Term }, e0, false);
			}
			e = new ITEExpr(x, isExistentialGuard, e, e0, e1);

			break;
		}
		case 118: {
			NestedMatchExpression(out e, allowSemi, allowLambda, allowBitwiseOps);
			break;
		}
		case 121: case 140: case 141: case 142: {
			QuantifierGuts(out e, allowSemi, allowLambda);
			break;
		}
		case 17: {
			Get();
			x = t;
			SetComprehensionExpr(x, true, out e, allowSemi, allowLambda, allowBitwiseOps);
			break;
		}
		case 18: {
			Get();
			x = t;
			SetComprehensionExpr(x, false, out e, allowSemi, allowLambda, true);
			break;
		}
		case 36: case 37: case 119: {
			StmtInExpr(out s);
			Expression(out e, allowSemi, allowLambda, allowBitwiseOps);
			e = new StmtExpr(s.Tok, s, e);
			break;
		}
		case 60: case 72: case 115: {
			LetExpr(out e, allowSemi, allowLambda, allowBitwiseOps);
			break;
		}
		case 21: {
			Get();
			x = t;
			MapComprehensionExpr(x, true, out e, allowSemi, allowLambda, allowBitwiseOps);
			break;
		}
		case 22: {
			Get();
			x = t;
			MapComprehensionExpr(x, false, out e, allowSemi, allowLambda, true);
			break;
		}
		case 89: {
			Get();
			Expression(out e, false, false, allowBitwiseOps);
			e = new RevealExpr(e.tok, e);
			break;
		}
		case 111: {
			NamedExpr(out e, allowSemi, allowLambda, allowBitwiseOps);
			break;
		}
		default: SynErr(280); break;
		}
	}

	void NameSegment(out Expression e) {
		IToken id;
		IToken openParen = null;  List<Type> typeArgs = null;  List<Expression> args = null;

		Ident(out id);
		if (IsGenericInstantiation(true)) {
			typeArgs = new List<Type>();
			GenericInstantiation(typeArgs);
		} else if (la.kind == 124) {
			HashCall(id, out openParen, out typeArgs, out args);
		} else if (StartOf(37)) {
		} else SynErr(281);
		e = new NameSegment(id, id.val, typeArgs);
		if (openParen != null) {
		 e = new ApplySuffix(openParen, e, args);
		}

	}

	void DisplayExpr(out Expression e) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		IToken x;  List<Expression> elements;
		e = dummyExpr;

		if (la.kind == 75) {
			Get();
			x = t;  elements = new List<Expression/*!*/>();
			if (StartOf(8)) {
				Expressions(elements);
			}
			e = new SetDisplayExpr(x, true, elements);
			Expect(76);
		} else if (la.kind == 77) {
			Get();
			x = t;  elements = new List<Expression/*!*/>();
			if (StartOf(8)) {
				Expressions(elements);
			}
			e = new SeqDisplayExpr(x, elements);
			Expect(78);
		} else SynErr(282);
	}

	void MultiSetExpr(out Expression e) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		IToken/*!*/ x = null;  List<Expression/*!*/>/*!*/ elements;
		e = dummyExpr;

		Expect(19);
		x = t;
		if (la.kind == 75) {
			Get();
			elements = new List<Expression/*!*/>();
			if (StartOf(8)) {
				Expressions(elements);
			}
			e = new MultiSetDisplayExpr(x, elements);
			Expect(76);
		} else if (la.kind == 79) {
			Get();
			x = t;  elements = new List<Expression/*!*/>();
			Expression(out e, true, true);
			e = new MultiSetFormingExpr(x, e);
			Expect(80);
		} else SynErr(283);
	}

	void SeqConstructionExpr(out Expression e) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		IToken x = null;
		Expression n, f;
		e = dummyExpr;

		Expect(20);
		x = t;
		Expect(79);
		Expression(out n, true, true);
		Expect(26);
		Expression(out f, true, true);
		Expect(80);
		e = new SeqConstructionExpr(x, n, f);
	}

	void Nat(out BigInteger n) {
		n = BigInteger.Zero;
		string S;

		if (la.kind == 2) {
			Get();
			S = Util.RemoveUnderscores(t.val);
			try {
			 n = BigIntegerParser.Parse(S);
			} catch (System.FormatException) {
			 SemErr("incorrectly formatted number");
			 n = BigInteger.Zero;
			}

		} else if (la.kind == 3) {
			Get();
			S = Util.RemoveUnderscores(t.val.Substring(2));
			try {
			 // note: leading 0 required when parsing positive hex numbers
			 n = BigIntegerParser.Parse("0" + S, System.Globalization.NumberStyles.HexNumber);
			} catch (System.FormatException) {
			 SemErr("incorrectly formatted number");
			 n = BigInteger.Zero;
			}

		} else SynErr(284);
	}

	void Dec(out Basetypes.BigDec d) {
		d = Basetypes.BigDec.ZERO;
		Expect(4);
		var S = Util.RemoveUnderscores(t.val);
		try {
		 d = Basetypes.BigDec.FromString(S);
		} catch (System.FormatException) {
		 SemErr("incorrectly formatted number");
		 d = Basetypes.BigDec.ZERO;
		}

	}

	void ParensExpression(out Expression e, bool allowSemi, bool allowLambda) {
		IToken x;
		var args = new List<Expression>();

		Expect(79);
		x = t;
		if (StartOf(8)) {
			Expressions(args);
		}
		Expect(80);
		if (args.Count == 1) {
		 e = new ParensExpression(x, args[0]);
		} else {
		 // make sure the corresponding tuple type exists
		 var tmp = theBuiltIns.TupleType(x, args.Count, true);
		 e = new DatatypeValue(x, BuiltIns.TupleTypeName(args.Count), BuiltIns.TupleTypeCtorNamePrefix + args.Count, args);
		}

	}

	void LambdaArrow() {
		Expect(35);
	}

	void MapLiteralExpressions(out List<ExpressionPair> elements) {
		Expression/*!*/ d, r;
		elements = new List<ExpressionPair/*!*/>();
		Expression(out d, true, true);
		Expect(29);
		Expression(out r, true, true);
		elements.Add(new ExpressionPair(d,r));
		while (la.kind == 26) {
			Get();
			Expression(out d, true, true);
			Expect(29);
			Expression(out r, true, true);
			elements.Add(new ExpressionPair(d,r));
		}
	}

	void MapComprehensionExpr(IToken mapToken, bool finite, out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null);
		BoundVar bv;
		List<BoundVar> bvars = new List<BoundVar>();
		Expression range = null;
		Expression bodyLeft = null;
		Expression bodyRight;
		Attributes attrs = null;

		IdentTypeOptional(out bv);
		bvars.Add(bv);
		while (la.kind == 26) {
			Get();
			IdentTypeOptional(out bv);
			bvars.Add(bv);
		}
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		if (la.kind == 27) {
			Get();
			Expression(out range, true, true, true);
		}
		QSep();
		Expression(out bodyRight, allowSemi, allowLambda, allowBitwiseOps);
		if (IsGets()) {
			bodyLeft = bodyRight;
			Expect(29);
			Expression(out bodyRight, allowSemi, allowLambda, allowBitwiseOps);
		}
		if (bodyLeft == null && bvars.Count != 1) {
		 SemErr(t, "a map comprehension with more than one bound variable must have a term expression of the form 'Expr := Expr'");
		 e = dummyExpr;
		} else {
		 e = new MapComprehension(mapToken, finite, bvars, range ?? new LiteralExpr(mapToken, true), bodyLeft, bodyRight, attrs);
		}

	}

	void NestedMatchExpression(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out e) != null); IToken/*!*/ x;  NestedMatchCaseExpr/*!*/ c;
		List<NestedMatchCaseExpr/*!*/> cases = new List<NestedMatchCaseExpr/*!*/>();
		bool usesOptionalBraces = false;

		Expect(118);
		x = t;
		Expression(out e, allowSemi, allowLambda, allowBitwiseOps);
		if (la.kind == _lbrace) {
			Expect(75);
			usesOptionalBraces = true;
			while (la.kind == 38) {
				NestedCaseExpression(out c, true, true, allowBitwiseOps);
				cases.Add(c);
			}
			Expect(76);
		} else if (StartOf(38)) {
			while (la.kind == _case) {
				NestedCaseExpression(out c, allowSemi, allowLambda, allowBitwiseOps);
				cases.Add(c);
			}
		} else SynErr(285);
		e = new NestedMatchExpr(x, e, cases, usesOptionalBraces);
	}

	void QuantifierGuts(out Expression q, bool allowSemi, bool allowLambda) {
		Contract.Ensures(Contract.ValueAtReturn(out q) != null); IToken/*!*/ x = Token.NoToken;
		bool univ = false;
		List<BoundVar/*!*/> bvars;
		Attributes attrs;
		Expression range;
		Expression/*!*/ body;

		if (la.kind == 121 || la.kind == 140) {
			Forall();
			x = t;  univ = true;
		} else if (la.kind == 141 || la.kind == 142) {
			Exists();
			x = t;
		} else SynErr(286);
		QuantifierDomain(out bvars, out attrs, out range);
		QSep();
		Expression(out body, allowSemi, allowLambda);
		if (univ) {
		 q = new ForallExpr(x, bvars, range, body, attrs);
		} else {
		 q = new ExistsExpr(x, bvars, range, body, attrs);
		}

	}

	void SetComprehensionExpr(IToken setToken, bool finite, out Expression q, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out q) != null);
		BoundVar bv;
		List<BoundVar/*!*/> bvars = new List<BoundVar>();
		Expression range;
		Expression body = null;
		Attributes attrs = null;

		IdentTypeOptional(out bv);
		bvars.Add(bv);
		while (la.kind == 26) {
			Get();
			IdentTypeOptional(out bv);
			bvars.Add(bv);
		}
		while (la.kind == 74) {
			Attribute(ref attrs);
		}
		Expect(27);
		Expression(out range, allowSemi, allowLambda, allowBitwiseOps);
		if (IsQSep()) {
			QSep();
			Expression(out body, allowSemi, allowLambda, allowBitwiseOps);
		}
		if (body == null && bvars.Count != 1) {
		 SemErr(t, "a set comprehension with more than one bound variable must have a term expression");
		 q = dummyExpr;
		} else {
		 q = new SetComprehension(setToken, finite, bvars, range, body, attrs);
		}

	}

	void StmtInExpr(out Statement s) {
		s = dummyStmt;
		if (la.kind == 119) {
			AssertStmt(out s, true);
		} else if (la.kind == 36) {
			AssumeStmt(out s);
		} else if (la.kind == 37) {
			CalcStmt(out s);
		} else SynErr(287);
	}

	void LetExpr(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		e = dummyExpr;
		if (la.kind == 60 || la.kind == 72) {
			LetExprWithLHS(out e, allowSemi, allowLambda, allowBitwiseOps);
		} else if (la.kind == 115) {
			LetExprWithoutLHS(out e, allowSemi, allowLambda, allowBitwiseOps);
		} else SynErr(288);
	}

	void NamedExpr(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		IToken/*!*/ x, d;
		e = dummyExpr;
		Expression expr;

		Expect(111);
		x = t;
		NoUSIdent(out d);
		Expect(25);
		Expression(out e, allowSemi, allowLambda, allowBitwiseOps);
		expr = e;
		e = new NamedExpr(x, d.val, expr);
	}

	void LetExprWithLHS(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		IToken x = null;
		bool isGhost = false;
		var letLHSs = new List<CasePattern<BoundVar>>();
		var letRHSs = new List<Expression>();
		CasePattern<BoundVar> pat;
		bool exact = true;
		bool isLetOrFail = false;
		Attributes attrs = null;
		e = dummyExpr;

		if (la.kind == 72) {
			Get();
			isGhost = true;  x = t;
		}
		Expect(60);
		if (!isGhost) { x = t; }
		CasePattern(out pat);
		if (isGhost) { pat.Vars.Iter(bv => bv.IsGhost = true); }
		letLHSs.Add(pat);

		while (la.kind == 26) {
			Get();
			CasePattern(out pat);
			if (isGhost) { pat.Vars.Iter(bv => bv.IsGhost = true); }
			letLHSs.Add(pat);

		}
		if (la.kind == 29) {
			Get();
		} else if (la.kind == 30 || la.kind == 74) {
			while (la.kind == 74) {
				Attribute(ref attrs);
			}
			Expect(30);
			exact = false;
			foreach (var lhs in letLHSs) {
			 if (lhs.Arguments != null) {
			   SemErr(lhs.tok, "LHS of let-such-that expression must be variables, not general patterns");
			 }
			}

		} else if (la.kind == 115) {
			Get();
			isLetOrFail = true;
		} else SynErr(289);
		Expression(out e, false, true);
		letRHSs.Add(e);
		while (la.kind == 26) {
			Get();
			Expression(out e, false, true);
			letRHSs.Add(e);
		}
		Expect(34);
		Expression(out e, allowSemi, allowLambda, allowBitwiseOps);
		if (isLetOrFail) {
		 CasePattern<BoundVar> lhs = null;
		 Contract.Assert(letLHSs.Count > 0);
		 if (letLHSs.Count == 1) {
		   lhs = letLHSs[0];
		 } else {
		   SemErr("':-' can have at most one left-hand side");
		 }
		 Expression rhs = null;
		 Contract.Assert(letRHSs.Count > 0);
		 if (letRHSs.Count == 1) {
		   rhs = letRHSs[0];
		 } else {
		   SemErr("':-' must have exactly one right-hand side");
		 }
		 e = new LetOrFailExpr(x, lhs, rhs, e);
		} else {
		 e = new LetExpr(x, letLHSs, letRHSs, e, exact, attrs);
		}

	}

	void LetExprWithoutLHS(out Expression e, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		IToken x;
		Expression rhs;
		Expression body;

		Expect(115);
		x = t;
		Expression(out rhs, false, true);
		Expect(34);
		Expression(out body, allowSemi, allowLambda, allowBitwiseOps);
		e = new LetOrFailExpr(x, null, rhs, body);
	}

	void CasePattern(out CasePattern<BoundVar> pat) {
		IToken id;  List<CasePattern<BoundVar>> arguments;
		BoundVar bv;
		pat = null;

		if (IsIdentParen()) {
			Ident(out id);
			Expect(79);
			arguments = new List<CasePattern<BoundVar>>();
			if (la.kind == 1 || la.kind == 79) {
				CasePattern(out pat);
				arguments.Add(pat);
				while (la.kind == 26) {
					Get();
					CasePattern(out pat);
					arguments.Add(pat);
				}
			}
			Expect(80);
			pat = new CasePattern<BoundVar>(id, id.val, arguments);
		} else if (la.kind == 79) {
			Get();
			id = t;
			arguments = new List<CasePattern<BoundVar>>();

			if (la.kind == 1 || la.kind == 79) {
				CasePattern(out pat);
				arguments.Add(pat);
				while (la.kind == 26) {
					Get();
					CasePattern(out pat);
					arguments.Add(pat);
				}
			}
			Expect(80);
			theBuiltIns.TupleType(id, arguments.Count, true); // make sure the tuple type exists
			string ctor = BuiltIns.TupleTypeCtorNamePrefix + arguments.Count;  //use the TupleTypeCtors
			pat = new CasePattern<BoundVar>(id, ctor, arguments);

		} else if (la.kind == 1) {
			IdentTypeOptional(out bv);
			pat = new CasePattern<BoundVar>(bv.tok, bv);

		} else SynErr(290);
		if (pat == null) {
		 pat = new CasePattern<BoundVar>(t, "_ParseError", new List<CasePattern<BoundVar>>());
		}

	}

	void NestedCaseExpression(out NestedMatchCaseExpr c, bool allowSemi, bool allowLambda, bool allowBitwiseOps) {
		Contract.Ensures(Contract.ValueAtReturn(out c) != null); IToken/*!*/ x;
		ExtendedPattern/*!*/ pat = null;
		ExtendedPattern/*!*/ tpat;
		Expression/*!*/ body;

		Expect(38);
		x = t;
		if (la.kind == _openparen) {
			Expect(79);
			List<ExtendedPattern> arguments = new List<ExtendedPattern>();
			ExtendedPattern(out tpat);
			arguments.Add(tpat);
			while (la.kind == 26) {
				Get();
				ExtendedPattern(out tpat);
				arguments.Add(tpat);
			}
			Expect(80);
			pat = new IdPattern(t, "", arguments);
		} else if (StartOf(22)) {
			ExtendedPattern(out pat);

		} else SynErr(291);
		Expect(35);
		Expression(out body, allowSemi, allowLambda, allowBitwiseOps);
		c = new NestedMatchCaseExpr(x, pat, body);
	}

	void HashCall(IToken id, out IToken openParen, out List<Type> typeArgs, out List<Expression> args) {
		Expression k; args = new List<Expression>(); typeArgs = null;
		Expect(124);
		id.val = id.val + "#";
		if (la.kind == 81) {
			typeArgs = new List<Type>();
			GenericInstantiation(typeArgs);
		}
		Expect(77);
		Expression(out k, true, true);
		Expect(78);
		args.Add(k);
		Expect(79);
		openParen = t;
		if (StartOf(8)) {
			Expressions(args);
		}
		Expect(80);
	}

	void MemberBindingUpdate(out IToken id, out Expression e) {
		id = Token.NoToken; e = dummyExpr;
		if (la.kind == 1) {
			Get();
			id = t;
		} else if (la.kind == 2) {
			Get();
			id = t;
		} else SynErr(292);
		Expect(29);
		Expression(out e, true, true);
	}

	void DotSuffix(out IToken x, out IToken y) {
		Contract.Ensures(Contract.ValueAtReturn(out x) != null);
		x = Token.NoToken;
		y = null;

		if (la.kind == 1) {
			Get();
			x = t;
		} else if (la.kind == 2) {
			Get();
			x = t;
		} else if (la.kind == 4) {
			Get();
			x = t;
			int exponent = x.val.IndexOf('e');
			if (0 <= exponent) {
			 // this is not a legal field/destructor name
			 SemErr(x, "invalid DotSuffix");
			} else {
			 int dot = x.val.IndexOf('.');
			 if (0 <= dot) {
			   y = new Token();
			   y.pos = x.pos + dot + 1;
			   y.val = x.val.Substring(dot + 1);
			   x.val = x.val.Substring(0, dot);
			   y.col = x.col + dot + 1;
			   y.line = x.line;
			   y.filename = x.filename;
			   y.kind = x.kind;
			 }
			}

		} else if (la.kind == 70) {
			Get();
			x = t;
		} else if (la.kind == 69) {
			Get();
			x = t;
		} else SynErr(293);
	}



	public void Parse() {
		la = new Token();
		la.val = "";
		Get();
		Dafny();
		Expect(0);

		Expect(0);
	}

	static readonly bool[,]/*!*/ set = {
		{_T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_T,_x, _T,_T,_T,_x, _x,_x,_x,_x, _T,_T,_x,_x, _T,_T,_x,_T, _x,_x,_x,_x, _T,_T,_T,_T, _T,_T,_x,_x, _T,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_T,_T,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _T,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_T,_T,_T, _T,_x,_x,_T, _T,_x,_x,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_T,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_T,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_T,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_T,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_T,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_T,_T,_T, _T,_T,_T,_T, _T,_x,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_T, _x,_T,_x,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_x,_x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _x,_x,_x,_T, _T,_x,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_x,_T,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x},
		{_T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_T,_T, _T,_T,_T,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_T, _T,_x,_x},
		{_T,_T,_T,_T, _T,_x,_x,_x, _x,_x,_T,_x, _T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_x,_T, _x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_T, _T,_x,_x,_x, _x,_x,_x,_T, _x,_x,_T,_T, _T,_T,_T,_T, _x,_x,_x,_T, _T,_x,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _x,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_T,_x, _x,_x,_x,_T, _T,_T,_T,_T, _T,_T,_T,_T, _T,_T,_x,_T, _T,_x,_x}

	};
} // end Parser


public class Errors {
	public int count = 0;                                    // number of errors detected
	public System.IO.TextWriter/*!*/ errorStream = Console.Out;   // error messages go to this stream
	public string errMsgFormat = "{0}({1},{2}): error: {3}"; // 0=filename, 1=line, 2=column, 3=text
	public string warningMsgFormat = "{0}({1},{2}): warning: {3}"; // 0=filename, 1=line, 2=column, 3=text

	public void SynErr(string filename, int line, int col, int n) {
		SynErr(filename, line, col, GetSyntaxErrorString(n));
	}

	public virtual void SynErr(string filename, int line, int col, string/*!*/ msg) {
		Contract.Requires(msg != null);
		errorStream.WriteLine(errMsgFormat, filename, line, col, msg);
		count++;
	}

	string GetSyntaxErrorString(int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "digits expected"; break;
			case 3: s = "hexdigits expected"; break;
			case 4: s = "decimaldigits expected"; break;
			case 5: s = "arrayToken expected"; break;
			case 6: s = "arrayToken_q expected"; break;
			case 7: s = "bvToken expected"; break;
			case 8: s = "bool expected"; break;
			case 9: s = "char expected"; break;
			case 10: s = "int expected"; break;
			case 11: s = "nat expected"; break;
			case 12: s = "real expected"; break;
			case 13: s = "ORDINAL expected"; break;
			case 14: s = "object expected"; break;
			case 15: s = "object_q expected"; break;
			case 16: s = "string expected"; break;
			case 17: s = "set expected"; break;
			case 18: s = "iset expected"; break;
			case 19: s = "multiset expected"; break;
			case 20: s = "seq expected"; break;
			case 21: s = "map expected"; break;
			case 22: s = "imap expected"; break;
			case 23: s = "charToken expected"; break;
			case 24: s = "stringToken expected"; break;
			case 25: s = "colon expected"; break;
			case 26: s = "comma expected"; break;
			case 27: s = "verticalbar expected"; break;
			case 28: s = "doublecolon expected"; break;
			case 29: s = "gets expected"; break;
			case 30: s = "boredSmiley expected"; break;
			case 31: s = "bullet expected"; break;
			case 32: s = "dot expected"; break;
			case 33: s = "backtick expected"; break;
			case 34: s = "semi expected"; break;
			case 35: s = "darrow expected"; break;
			case 36: s = "assume expected"; break;
			case 37: s = "calc expected"; break;
			case 38: s = "case expected"; break;
			case 39: s = "then expected"; break;
			case 40: s = "else expected"; break;
			case 41: s = "as expected"; break;
			case 42: s = "by expected"; break;
			case 43: s = "in expected"; break;
			case 44: s = "decreases expected"; break;
			case 45: s = "invariant expected"; break;
			case 46: s = "function expected"; break;
			case 47: s = "predicate expected"; break;
			case 48: s = "inductive expected"; break;
			case 49: s = "twostate expected"; break;
			case 50: s = "copredicate expected"; break;
			case 51: s = "lemma expected"; break;
			case 52: s = "static expected"; break;
			case 53: s = "protected expected"; break;
			case 54: s = "import expected"; break;
			case 55: s = "export expected"; break;
			case 56: s = "class expected"; break;
			case 57: s = "trait expected"; break;
			case 58: s = "datatype expected"; break;
			case 59: s = "codatatype expected"; break;
			case 60: s = "var expected"; break;
			case 61: s = "const expected"; break;
			case 62: s = "newtype expected"; break;
			case 63: s = "type expected"; break;
			case 64: s = "iterator expected"; break;
			case 65: s = "method expected"; break;
			case 66: s = "colemma expected"; break;
			case 67: s = "constructor expected"; break;
			case 68: s = "modifies expected"; break;
			case 69: s = "reads expected"; break;
			case 70: s = "requires expected"; break;
			case 71: s = "ensures expected"; break;
			case 72: s = "ghost expected"; break;
			case 73: s = "witness expected"; break;
			case 74: s = "lbracecolon expected"; break;
			case 75: s = "lbrace expected"; break;
			case 76: s = "rbrace expected"; break;
			case 77: s = "lbracket expected"; break;
			case 78: s = "rbracket expected"; break;
			case 79: s = "openparen expected"; break;
			case 80: s = "closeparen expected"; break;
			case 81: s = "openAngleBracket expected"; break;
			case 82: s = "closeAngleBracket expected"; break;
			case 83: s = "eq expected"; break;
			case 84: s = "neq expected"; break;
			case 85: s = "neqAlt expected"; break;
			case 86: s = "star expected"; break;
			case 87: s = "notIn expected"; break;
			case 88: s = "ellipsis expected"; break;
			case 89: s = "reveal expected"; break;
			case 90: s = "\"include\" expected"; break;
			case 91: s = "\"abstract\" expected"; break;
			case 92: s = "\"module\" expected"; break;
			case 93: s = "\"refines\" expected"; break;
			case 94: s = "\"opened\" expected"; break;
			case 95: s = "\"=\" expected"; break;
			case 96: s = "\"provides\" expected"; break;
			case 97: s = "\"reveals\" expected"; break;
			case 98: s = "\"extends\" expected"; break;
			case 99: s = "\"new\" expected"; break;
			case 100: s = "\"yields\" expected"; break;
			case 101: s = "\"returns\" expected"; break;
			case 102: s = "\"+\" expected"; break;
			case 103: s = "\"!\" expected"; break;
			case 104: s = "\"-\" expected"; break;
			case 105: s = "\"comethod\" expected"; break;
			case 106: s = "\"free\" expected"; break;
			case 107: s = "\"yield\" expected"; break;
			case 108: s = "\"~>\" expected"; break;
			case 109: s = "\"-->\" expected"; break;
			case 110: s = "\"->\" expected"; break;
			case 111: s = "\"label\" expected"; break;
			case 112: s = "\"break\" expected"; break;
			case 113: s = "\"where\" expected"; break;
			case 114: s = "\"return\" expected"; break;
			case 115: s = "\":-\" expected"; break;
			case 116: s = "\"if\" expected"; break;
			case 117: s = "\"while\" expected"; break;
			case 118: s = "\"match\" expected"; break;
			case 119: s = "\"assert\" expected"; break;
			case 120: s = "\"print\" expected"; break;
			case 121: s = "\"forall\" expected"; break;
			case 122: s = "\"parallel\" expected"; break;
			case 123: s = "\"modify\" expected"; break;
			case 124: s = "\"#\" expected"; break;
			case 125: s = "\"<=\" expected"; break;
			case 126: s = "\">=\" expected"; break;
			case 127: s = "\"\\u2264\" expected"; break;
			case 128: s = "\"\\u2265\" expected"; break;
			case 129: s = "\"<==>\" expected"; break;
			case 130: s = "\"\\u21d4\" expected"; break;
			case 131: s = "\"==>\" expected"; break;
			case 132: s = "\"\\u21d2\" expected"; break;
			case 133: s = "\"<==\" expected"; break;
			case 134: s = "\"\\u21d0\" expected"; break;
			case 135: s = "\"&&\" expected"; break;
			case 136: s = "\"\\u2227\" expected"; break;
			case 137: s = "\"||\" expected"; break;
			case 138: s = "\"\\u2228\" expected"; break;
			case 139: s = "\"\\u00ac\" expected"; break;
			case 140: s = "\"\\u2200\" expected"; break;
			case 141: s = "\"exists\" expected"; break;
			case 142: s = "\"\\u2203\" expected"; break;
			case 143: s = "\"/\" expected"; break;
			case 144: s = "\"%\" expected"; break;
			case 145: s = "\"&\" expected"; break;
			case 146: s = "\"^\" expected"; break;
			case 147: s = "\"false\" expected"; break;
			case 148: s = "\"true\" expected"; break;
			case 149: s = "\"null\" expected"; break;
			case 150: s = "\"this\" expected"; break;
			case 151: s = "\"fresh\" expected"; break;
			case 152: s = "\"allocated\" expected"; break;
			case 153: s = "\"unchanged\" expected"; break;
			case 154: s = "\"@\" expected"; break;
			case 155: s = "\"old\" expected"; break;
			case 156: s = "\"..\" expected"; break;
			case 157: s = "??? expected"; break;
			case 158: s = "invalid TopDecl"; break;
			case 159: s = "invalid DeclModifier"; break;
			case 160: s = "invalid SubModuleDecl"; break;
			case 161: s = "this symbol not expected in SubModuleDecl"; break;
			case 162: s = "invalid SubModuleDecl"; break;
			case 163: s = "invalid SubModuleDecl"; break;
			case 164: s = "invalid SubModuleDecl"; break;
			case 165: s = "this symbol not expected in ClassDecl"; break;
			case 166: s = "this symbol not expected in DatatypeDecl"; break;
			case 167: s = "invalid DatatypeDecl"; break;
			case 168: s = "invalid NewtypeDecl"; break;
			case 169: s = "invalid OtherTypeDecl"; break;
			case 170: s = "this symbol not expected in OtherTypeDecl"; break;
			case 171: s = "this symbol not expected in IteratorDecl"; break;
			case 172: s = "invalid IteratorDecl"; break;
			case 173: s = "this symbol not expected in TraitDecl"; break;
			case 174: s = "invalid ClassMemberDecl"; break;
			case 175: s = "invalid QualifiedModuleExportSuffix"; break;
			case 176: s = "invalid QualifiedModuleExportSuffix"; break;
			case 177: s = "invalid TypeNameOrCtorSuffix"; break;
			case 178: s = "this symbol not expected in FieldDecl"; break;
			case 179: s = "this symbol not expected in ConstantFieldDecl"; break;
			case 180: s = "invalid FunctionDecl"; break;
			case 181: s = "invalid FunctionDecl"; break;
			case 182: s = "invalid FunctionDecl"; break;
			case 183: s = "invalid FunctionDecl"; break;
			case 184: s = "invalid FunctionDecl"; break;
			case 185: s = "invalid FunctionDecl"; break;
			case 186: s = "this symbol not expected in MethodDecl"; break;
			case 187: s = "invalid MethodDecl"; break;
			case 188: s = "invalid MethodDecl"; break;
			case 189: s = "invalid FIdentType"; break;
			case 190: s = "this symbol not expected in OldSemi"; break;
			case 191: s = "invalid CIdentType"; break;
			case 192: s = "invalid TypeIdentOptional"; break;
			case 193: s = "invalid TypeAndToken"; break;
			case 194: s = "this symbol not expected in IteratorSpec"; break;
			case 195: s = "invalid IteratorSpec"; break;
			case 196: s = "invalid IteratorSpec"; break;
			case 197: s = "invalid Variance"; break;
			case 198: s = "invalid TPCharOption"; break;
			case 199: s = "invalid FuMe_Ident"; break;
			case 200: s = "invalid KType"; break;
			case 201: s = "this symbol not expected in MethodSpec"; break;
			case 202: s = "invalid MethodSpec"; break;
			case 203: s = "invalid MethodSpec"; break;
			case 204: s = "invalid FrameExpression"; break;
			case 205: s = "invalid LabelIdent"; break;
			case 206: s = "this symbol not expected in FunctionSpec"; break;
			case 207: s = "invalid FunctionSpec"; break;
			case 208: s = "invalid PossiblyWildFrameExpression"; break;
			case 209: s = "invalid PossiblyWildExpression"; break;
			case 210: s = "this symbol not expected in OneStmt"; break;
			case 211: s = "invalid OneStmt"; break;
			case 212: s = "this symbol not expected in OneStmt"; break;
			case 213: s = "invalid OneStmt"; break;
			case 214: s = "invalid AssertStmt"; break;
			case 215: s = "invalid AssertStmt"; break;
			case 216: s = "invalid AssumeStmt"; break;
			case 217: s = "invalid UpdateStmt"; break;
			case 218: s = "invalid UpdateStmt"; break;
			case 219: s = "invalid UpdateStmt"; break;
			case 220: s = "this symbol not expected in VarDeclStatement"; break;
			case 221: s = "invalid VarDeclStatement"; break;
			case 222: s = "invalid VarDeclStatement"; break;
			case 223: s = "invalid IfStmt"; break;
			case 224: s = "invalid IfStmt"; break;
			case 225: s = "invalid WhileStmt"; break;
			case 226: s = "invalid WhileStmt"; break;
			case 227: s = "invalid NestedMatchStmt"; break;
			case 228: s = "invalid ForallStmt"; break;
			case 229: s = "invalid ForallStmt"; break;
			case 230: s = "invalid CalcStmt"; break;
			case 231: s = "invalid ModifyStmt"; break;
			case 232: s = "this symbol not expected in ModifyStmt"; break;
			case 233: s = "invalid ModifyStmt"; break;
			case 234: s = "invalid ReturnStmt"; break;
			case 235: s = "invalid Rhs"; break;
			case 236: s = "invalid Rhs"; break;
			case 237: s = "invalid Lhs"; break;
			case 238: s = "invalid NewArray"; break;
			case 239: s = "invalid CasePatternLocal"; break;
			case 240: s = "invalid AlternativeBlock"; break;
			case 241: s = "invalid Guard"; break;
			case 242: s = "invalid AlternativeBlockCase"; break;
			case 243: s = "this symbol not expected in AlternativeBlockCase"; break;
			case 244: s = "this symbol not expected in AlternativeBlockCase"; break;
			case 245: s = "this symbol not expected in LoopSpec"; break;
			case 246: s = "this symbol not expected in LoopSpec"; break;
			case 247: s = "this symbol not expected in LoopSpec"; break;
			case 248: s = "invalid LoopSpec"; break;
			case 249: s = "invalid ExtendedPattern"; break;
			case 250: s = "invalid ConstAtomExpression"; break;
			case 251: s = "invalid NestedCaseStatement"; break;
			case 252: s = "this symbol not expected in NestedCaseStatement"; break;
			case 253: s = "this symbol not expected in NestedCaseStatement"; break;
			case 254: s = "invalid CalcOp"; break;
			case 255: s = "invalid EquivOp"; break;
			case 256: s = "invalid ImpliesOp"; break;
			case 257: s = "invalid ExpliesOp"; break;
			case 258: s = "invalid AndOp"; break;
			case 259: s = "invalid OrOp"; break;
			case 260: s = "invalid NegOp"; break;
			case 261: s = "invalid Forall"; break;
			case 262: s = "invalid Exists"; break;
			case 263: s = "invalid QSep"; break;
			case 264: s = "invalid ImpliesExpliesExpression"; break;
			case 265: s = "invalid LogicalExpression"; break;
			case 266: s = "invalid LogicalExpression"; break;
			case 267: s = "invalid ShiftTerm"; break;
			case 268: s = "invalid RelOp"; break;
			case 269: s = "invalid AddOp"; break;
			case 270: s = "invalid BitvectorFactor"; break;
			case 271: s = "invalid MulOp"; break;
			case 272: s = "invalid UnaryExpression"; break;
			case 273: s = "invalid Suffix"; break;
			case 274: s = "invalid Suffix"; break;
			case 275: s = "invalid Suffix"; break;
			case 276: s = "invalid Suffix"; break;
			case 277: s = "invalid Suffix"; break;
			case 278: s = "invalid LambdaExpression"; break;
			case 279: s = "invalid EndlessExpression"; break;
			case 280: s = "invalid EndlessExpression"; break;
			case 281: s = "invalid NameSegment"; break;
			case 282: s = "invalid DisplayExpr"; break;
			case 283: s = "invalid MultiSetExpr"; break;
			case 284: s = "invalid Nat"; break;
			case 285: s = "invalid NestedMatchExpression"; break;
			case 286: s = "invalid QuantifierGuts"; break;
			case 287: s = "invalid StmtInExpr"; break;
			case 288: s = "invalid LetExpr"; break;
			case 289: s = "invalid LetExprWithLHS"; break;
			case 290: s = "invalid CasePattern"; break;
			case 291: s = "invalid NestedCaseExpression"; break;
			case 292: s = "invalid MemberBindingUpdate"; break;
			case 293: s = "invalid DotSuffix"; break;

			default: s = "error " + n; break;
		}
		return s;
	}

	public void SemErr(IToken/*!*/ tok, string/*!*/ msg) {  // semantic errors
		Contract.Requires(tok != null);
		Contract.Requires(msg != null);
		SemErr(tok.filename, tok.line, tok.col, msg);
	}

	public virtual void SemErr(string filename, int line, int col, string/*!*/ msg) {
		Contract.Requires(msg != null);
		errorStream.WriteLine(errMsgFormat, filename, line, col, msg);
		count++;
	}

	public void Warning(IToken/*!*/ tok, string/*!*/ msg) {  // warnings
		Contract.Requires(tok != null);
		Contract.Requires(msg != null);
		Warning(tok.filename, tok.line, tok.col, msg);
	}

	public virtual void Warning(string filename, int line, int col, string msg) {
		Contract.Requires(msg != null);
		errorStream.WriteLine(warningMsgFormat, filename, line, col, msg);
	}
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}


}