// Dafny program the_program compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in net6.0 should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;

namespace DafnyToRustCompilerCoverage.RASTCoverage {

  public partial class __default {
    public static void TestNoOptimize(RAST._IExpr e)
    {
    }
    public static void TestOptimizeToString()
    {
      RAST._IExpr _1674_x;
      _1674_x = RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"));
      RAST._IExpr _1675_y;
      _1675_y = RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("y"));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), RAST.Expr.create_Call(RAST.Expr.create_Select(_1674_x, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements()), DAST.Format.UnaryOpFormat.create_NoFormat())).Optimize(), RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), _1674_x, DAST.Format.UnaryOpFormat.create_NoFormat())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), RAST.Expr.create_Call(RAST.Expr.create_Select(_1674_x, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("clone")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements(_1675_y)), DAST.Format.UnaryOpFormat.create_NoFormat()));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), _1674_x, _1675_y, DAST.Format.BinaryOpFormat.create_NoFormat()), DAST.Format.UnaryOpFormat.create_CombineFormat())).Optimize(), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!="), _1674_x, _1675_y, DAST.Format.BinaryOpFormat.create_NoFormat())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _1674_x, _1675_y, DAST.Format.BinaryOpFormat.create_NoFormat()), DAST.Format.UnaryOpFormat.create_CombineFormat())).Optimize(), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">="), _1674_x, _1675_y, DAST.Format.BinaryOpFormat.create_NoFormat())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _1674_x, _1675_y, DAST.Format.BinaryOpFormat.create_ReverseFormat()), DAST.Format.UnaryOpFormat.create_CombineFormat())).Optimize(), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _1675_y, _1674_x, DAST.Format.BinaryOpFormat.create_NoFormat())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("1")))))).Optimize(), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/*optimized*/1"))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("1"), false))))).Optimize(), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/*optimized*/1"))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize((RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements(_1674_x)))).Optimize());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize((RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("1")), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("2")))))).Optimize());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_None()), RAST.Expr.create_StmtExpr(RAST.Expr.create_AssignVar(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), _1675_y), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return"))))).Optimize(), RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1675_y)), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return")))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_None()), RAST.Expr.create_StmtExpr(RAST.Expr.create_AssignVar(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("w"), _1675_y), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return")))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(_1674_x);
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(_1674_x, _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_Match(_1674_x, Dafny.Sequence<RAST._IMatchCase>.FromElements()), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_StructBuild(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"), Dafny.Sequence<RAST._IAssignIdentifier>.FromElements()), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_Tuple(Dafny.Sequence<RAST._IExpr>.FromElements()), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), _1674_x, DAST.Format.UnaryOpFormat.create_NoFormat()), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&&"), _1674_x, _1674_x, DAST.Format.BinaryOpFormat.create_NoFormat()), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_TypeAscription(_1674_x, RAST.Type.create_I128()), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("1")), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("1"), true), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoOptimize(RAST.Expr.create_StmtExpr(RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), _1674_x), _1674_x));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_None()), RAST.Expr.create_StmtExpr(RAST.Expr.create_AssignVar(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), _1675_y), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return"))))).Optimize(), RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("z"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_Some(_1675_y)), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("return")))));
      Dafny.ISequence<RAST._IExpr> _1676_coverageExpression;
      _1676_coverageExpression = Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Expr.create_Match(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), Dafny.Sequence<RAST._IMatchCase>.FromElements(RAST.MatchCase.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))))), RAST.Expr.create_StmtExpr(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("panic!()")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("a"))), RAST.Expr.create_Block(RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"))), RAST.Expr.create_StructBuild(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dummy"), Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("foo"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("bar"))))), RAST.Expr.create_StructBuild(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dummy"), Dafny.Sequence<RAST._IAssignIdentifier>.FromElements(RAST.AssignIdentifier.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("foo"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("bar"))), RAST.AssignIdentifier.create(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("foo2"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("bar"))))), RAST.Expr.create_Tuple(Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")))), RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), DAST.Format.UnaryOpFormat.create_NoFormat()), RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("+"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("y")), DAST.Format.BinaryOpFormat.create_NoFormat()), RAST.Expr.create_TypeAscription(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Type.create_I128()), RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("322")), RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), true), RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), false), RAST.Expr.create_ConversionNum(RAST.Type.create_I128(), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_ConversionNum(RAST.__default.RawType(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("X")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_None()), RAST.Expr.create_DeclareVar(RAST.DeclareType.create_CONST(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")))), RAST.Expr.create_AssignVar(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_IfExpr(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_Loop(Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_For(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_Labelled(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))), RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None()), RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("l"))), RAST.Expr.create_Continue(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None()), RAST.Expr.create_Continue(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_Some(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("l"))), RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_None()), RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_Some(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")))), RAST.Expr.create_Call(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements()), RAST.Expr.create_Call(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), Dafny.Sequence<RAST._IType>.FromElements(RAST.Type.create_I128(), RAST.Type.create_I32()), Dafny.Sequence<RAST._IExpr>.FromElements(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("y")))), RAST.Expr.create_Select(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc")), RAST.Expr.create_MemberSelect(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc")));
      BigInteger _hi4 = new BigInteger((_1676_coverageExpression).Count);
      for (BigInteger _1677_i = BigInteger.Zero; _1677_i < _hi4; _1677_i++) {
        RAST._IExpr _1678_c;
        _1678_c = (_1676_coverageExpression).Select(_1677_i);
        RAST._IPrintingInfo _1679___v0;
        _1679___v0 = (_1678_c).printingInfo;
        RAST._IExpr _1680___v1;
        _1680___v1 = (_1678_c).Optimize();
        Dafny.IMap<RAST._IExpr,Dafny.ISequence<Dafny.Rune>> _1681___v2;
        _1681___v2 = Dafny.Map<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>.FromElements(new Dafny.Pair<RAST._IExpr, Dafny.ISequence<Dafny.Rune>>(_1678_c, (_1678_c)._ToString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))));
        RAST._IExpr _1682___v3;
        _1682___v3 = (RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_None()), _1678_c)).Optimize();
        RAST._IExpr _1683___v4;
        _1683___v4 = (RAST.Expr.create_StmtExpr(RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), Std.Wrappers.Option<RAST._IType>.create_Some(RAST.Type.create_I128()), Std.Wrappers.Option<RAST._IExpr>.create_None()), RAST.Expr.create_StmtExpr(RAST.Expr.create_AssignVar(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), _1678_c), RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(""))))).Optimize();
        RAST._IExpr _1684___v5;
        _1684___v5 = (RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), _1678_c, DAST.Format.UnaryOpFormat.create_NoFormat())).Optimize();
        RAST._IExpr _1685___v6;
        _1685___v6 = (RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), _1678_c, DAST.Format.UnaryOpFormat.create_NoFormat())).Optimize();
        RAST._IExpr _1686___v7;
        _1686___v7 = (RAST.Expr.create_ConversionNum(RAST.Type.create_U8(), _1678_c)).Optimize();
        RAST._IExpr _1687___v8;
        _1687___v8 = (RAST.Expr.create_ConversionNum(RAST.Type.create_U8(), RAST.Expr.create_Call(_1678_c, Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements()))).Optimize();
        RAST._IExpr _1688___v9;
        _1688___v9 = (RAST.Expr.create_ConversionNum(RAST.Type.create_U8(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(_1678_c, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements()))).Optimize();
        RAST._IExpr _1689___v10;
        _1689___v10 = (RAST.Expr.create_ConversionNum(RAST.Type.create_U8(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(_1678_c, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements()))).Optimize();
        RAST._IExpr _1690___v11;
        _1690___v11 = (RAST.Expr.create_ConversionNum(RAST.Type.create_U8(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(_1678_c, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements()))).Optimize();
        RAST._IExpr _1691___v12;
        _1691___v12 = (RAST.Expr.create_ConversionNum(RAST.Type.create_U8(), RAST.Expr.create_Call(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_MemberSelect(RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("dafny_runtime")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("DafnyInt")), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("from")), Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements(_1678_c)))).Optimize();
        Std.Wrappers._IOption<Dafny.ISequence<Dafny.Rune>> _1692___v13;
        _1692___v13 = (_1678_c).RightMostIdentifier();
      }
    }
    public static void TestPrintingInfo()
    {
      RAST._IExpr _1693_x;
      _1693_x = RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"));
      RAST._IExpr _1694_y;
      _1694_y = RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("y"));
      DAST.Format._IBinaryOpFormat _1695_bnf;
      _1695_bnf = DAST.Format.BinaryOpFormat.create_NoFormat();
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(((RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))).printingInfo).is_UnknownPrecedence);
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((_1693_x).printingInfo, RAST.PrintingInfo.create_Precedence(BigInteger.One)));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_LiteralInt(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("3"))).printingInfo, RAST.PrintingInfo.create_Precedence(BigInteger.One)));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_LiteralString(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("abc"), true)).printingInfo, RAST.PrintingInfo.create_Precedence(BigInteger.One)));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_SuffixPrecedence(new BigInteger(5))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_Precedence(new BigInteger(6))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_Precedence(new BigInteger(6))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_Precedence(new BigInteger(6))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_Precedence(new BigInteger(6))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&mut"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_Precedence(new BigInteger(6))));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_UnaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!!"), _1693_x, DAST.Format.UnaryOpFormat.create_NoFormat())).printingInfo, RAST.PrintingInfo.create_UnknownPrecedence()));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_Select(_1693_x, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("name"))).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_MemberSelect(_1693_x, Dafny.Sequence<Dafny.Rune>.UnicodeFromString("name"))).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_Call(_1693_x, Dafny.Sequence<RAST._IType>.FromElements(), Dafny.Sequence<RAST._IExpr>.FromElements())).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(2), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_TypeAscription(_1693_x, RAST.Type.create_I128())).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(10), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(20), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(20), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("%"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(20), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("+"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(30), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(30), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<<"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(40), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">>"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(40), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(50), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("^"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(60), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("|"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(70), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("=="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("!="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(80), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&&"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(90), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("||"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(100), RAST.Associativity.create_LeftToRight())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(".."), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("..="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("+="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("-="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("*="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("/="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("%="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("&="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("|="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("^="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("<<="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(">>="), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(new BigInteger(110), RAST.Associativity.create_RightToLeft())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_BinaryOp(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("?!?"), _1693_x, _1694_y, _1695_bnf)).printingInfo, RAST.PrintingInfo.create_PrecedenceAssociativity(BigInteger.Zero, RAST.Associativity.create_RequiresParentheses())));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(object.Equals((RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None())).printingInfo, RAST.PrintingInfo.create_UnknownPrecedence()));
    }
    public static void TestExpr()
    {
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestOptimizeToString();
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestPrintingInfo();
      DafnyToRustCompilerCoverage.RASTCoverage.__default.TestNoExtraSemicolonAfter();
    }
    public static void AssertCoverage(bool x)
    {
    }
    public static void TestNoExtraSemicolonAfter()
    {
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage((RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString(";"))).NoExtraSemicolonAfter());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(!((RAST.Expr.create_RawExpr(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("a"))).NoExtraSemicolonAfter()));
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage((RAST.Expr.create_Return(Std.Wrappers.Option<RAST._IExpr>.create_None())).NoExtraSemicolonAfter());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage((RAST.Expr.create_Continue(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None())).NoExtraSemicolonAfter());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage((RAST.Expr.create_Break(Std.Wrappers.Option<Dafny.ISequence<Dafny.Rune>>.create_None())).NoExtraSemicolonAfter());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage((RAST.Expr.create_AssignVar(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"), RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("y")))).NoExtraSemicolonAfter());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage((RAST.Expr.create_DeclareVar(RAST.DeclareType.create_MUT(), Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"), Std.Wrappers.Option<RAST._IType>.create_None(), Std.Wrappers.Option<RAST._IExpr>.create_None())).NoExtraSemicolonAfter());
      DafnyToRustCompilerCoverage.RASTCoverage.__default.AssertCoverage(!((RAST.Expr.create_Identifier(Dafny.Sequence<Dafny.Rune>.UnicodeFromString("x"))).NoExtraSemicolonAfter()));
    }
  }
} // end of namespace DafnyToRustCompilerCoverage.RASTCoverage