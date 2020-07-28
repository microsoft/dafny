// RUN: %dafny /compile:3 /spillTargetCode:2 /compileTarget:cs "%s" > "%t"
// RUN: %dafny /compile:3 /spillTargetCode:2 /compileTarget:js "%s" >> "%t"
// RUN: %dafny /compile:3 /spillTargetCode:2 /compileTarget:go "%s" >> "%t"
// RUN: %dafny /compile:3 /spillTargetCode:2 /compileTarget:java "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

method Main() {
  AssignSuchThat();
  LetSuchThat();
  Quantifier();
  MapComprehension();
  OutParamsUnderLambdas();  // white-box testing
  AltControlFlow();
  Sequences();
  SetComprehension();
  GoNil();
}

predicate method Thirteen(x: int) { x == 13 }
predicate method Even(y: int) { y % 2 == 1 }
function method FourMore(x: int): int { x + 4 }

method AssignSuchThat() {
  var x, y;
  assert Thirteen(13);
  x, y :| 12 <= x < y && Thirteen(x);
  print "x=", x, " y=", y, "\n";
  var b;
  x, b, y :| 12 <= x < y && Thirteen(x) && b;
  print "x=", x, " y=", y, " b=", if b then "yes" else "no", "\n";
}

method LetSuchThat() {
  assert Thirteen(13);
  var p := var x, y :| 12 <= x < y < 15 && Thirteen(x); (x, y);
  print "p=", p, "\n";
  var q := var x, b, y :| 12 <= x < y < 15 && Thirteen(x) && b; (x, y, if b then "yes" else "no");
  print "q=", q, "\n";
}

method Quantifier() {
  var s := [0, 1, 1, 2, 3, 5, 8, 13];
  print forall x :: x in s ==> x < 20, " ";  // true
  print forall x :: x in s ==> x < 10, "\n";  // false
  print exists x :: x in s && x == 3, " ";  // true
  print exists x :: x in s && x == 4, "\n";  // false
}

method MapComprehension() {
  // var m := map x,y | 12 <= x < y < 17 && Thirteen(x) && Even(y) :: x := y;
  var m := map x | 12 <= x < 15 :: x / 2;
  print m, "\n";
  m := map x | 12 <= x < 15 :: FourMore(x) := x;
  print m, "\n";
}

method OutParamsUnderLambdas() {
  var x, b := XP();
  print "XP returned: ", x, " ", b, "\n";
  var m := XM();
  print "XM returned: ", m, "\n";
}

method XP() returns (x: int, b: bool) {
  var s := {2, 4};
  b := exists y :: y in s && y < x;
}


method XM() returns (x: int) {
  var before, after;

  var f := () => x;
  before := f();
  x := 2;
  after := f();
  print "after: ", f(), " ", "before: ", f(), "\n";

  f := () => x;
  before := f();
  x := 16;
  after := f();
  print "after: ", f(), " ", "before: ", f(), "\n";
}

method AltControlFlow() {
  var s := [2, 29, 34, 35, 36, 59, 104, 106, 107, 107, 108, 2700];
  var lo, hi, Lo, Hi;

  lo, hi := FindRange(s, 0, 3000);
  Lo, Hi := FindRange(s, 35, 107);
  print lo, " ", hi, " ", Lo, " ", Hi, "\n";

  lo, hi := FindRangeIf(s, 0, 3000);
  Lo, Hi := FindRangeIf(s, 35, 107);
  print lo, " ", hi, " ", Lo, " ", Hi, "\n";

  lo, hi := FindRangeBindingGuard(s, 0, 3000);
  Lo, Hi := FindRangeBindingGuard(s, 35, 107);
  print lo, " ", hi, " ", Lo, " ", Hi, "\n";

  lo, hi := FindRangeBindingGuardAlt(s, 0, 3000);
  Lo, Hi := FindRangeBindingGuardAlt(s, 35, 107);
  print lo, " ", hi, " ", Lo, " ", Hi, "\n";
}

method FindRange(s: seq<int>, from: int, to: int) returns (lo: int, hi: int)
  requires forall i,j :: 0 <= i < j < |s| ==> s[i] <= s[j]
  requires from <= to
  ensures 0 <= lo <= hi <= |s|
  ensures forall i :: 0 <= i < |s| ==> (from <= s[i] < to <==> lo <= i < hi)
{
  lo, hi := 0, |s|;
  while
    invariant lo <= hi <= |s|
    invariant forall i :: 0 <= i < lo ==> s[i] < from
    invariant forall i :: hi <= i < |s| ==> to <= s[i]
    decreases hi - lo
  {
  case lo < |s| && s[lo] < from =>
    lo := lo + 1;
  case 0 < hi && to <= s[hi-1] =>
    hi := hi - 1;
  }
}

method FindRangeIf(s: seq<int>, from: int, to: int) returns (lo: int, hi: int)
  requires forall i,j :: 0 <= i < j < |s| ==> s[i] <= s[j]
  requires from <= to
  ensures 0 <= lo <= hi <= |s|
  ensures forall i :: 0 <= i < |s| ==> (from <= s[i] < to <==> lo <= i < hi)
{
  lo, hi := 0, |s|;
  while lo < hi
    invariant lo <= hi <= |s|
    invariant forall i :: 0 <= i < lo ==> s[i] < from
    invariant forall i :: hi <= i < |s| ==> to <= s[i]
    decreases hi - lo
  {
    if
    case s[lo] < from =>
      lo := lo + 1;
    case to <= s[hi-1] =>
      hi := hi - 1;
    case from <= s[lo] && s[hi-1] < to =>
      break;
  }
}

method FindRangeBindingGuard(s: seq<int>, from: int, to: int) returns (lo: int, hi: int)
  requires forall i,j :: 0 <= i < j < |s| ==> s[i] <= s[j]
  requires from <= to
  ensures 0 <= lo <= hi <= |s|
  ensures forall i :: 0 <= i < |s| ==> (from <= s[i] < to <==> lo <= i < hi)
{
  lo, hi := 0, |s|;
  while lo < hi
    invariant lo <= hi <= |s|
    invariant forall i :: 0 <= i < lo ==> s[i] < from
    invariant forall i :: hi <= i < |s| ==> to <= s[i]
    decreases hi - lo
  {
    if j :| lo <= j < |s| && s[j] < from {
      lo := j + 1;
    } else if j :| 0 <= j < hi && to <= s[j] {
      hi := j;
    } else {
      break;
    }
  }
}

method FindRangeBindingGuardAlt(s: seq<int>, from: int, to: int) returns (lo: int, hi: int)
  requires forall i,j :: 0 <= i < j < |s| ==> s[i] <= s[j]
  requires from <= to
  ensures 0 <= lo <= hi <= |s|
  ensures forall i :: 0 <= i < |s| ==> (from <= s[i] < to <==> lo <= i < hi)
{
  lo, hi := 0, |s|;
  while lo < hi
    invariant lo <= hi <= |s|
    invariant forall i :: 0 <= i < lo ==> s[i] < from
    invariant forall i :: hi <= i < |s| ==> to <= s[i]
    decreases hi - lo
  {
    if
    case j :| lo <= j < |s| && s[j] < from =>
      lo := j + 1;
    case j :| 0 <= j < hi && to <= s[j] =>
      hi := j;
    case forall j :: lo <= j < hi ==> from <= s[j] < to =>
      break;
  }
}

method Sequences() {
  var four1s := [1, 1, 1, 1];
  var twelve1s := seq(12, _ => 1);
  assert twelve1s == four1s + four1s + four1s;

  var squares := seq(8, i => i*i);
  assert |squares| == 8;
  assert squares[6] == 36;

  var nats := seq(8, i => i);

  print four1s, "\n";
  print twelve1s, "\n";
  print squares, "\n";
  print nats, "\n";
}

trait NothingInParticular { }
class ClassA { }
class ClassB extends NothingInParticular { }

method SetComprehension() {
  SetComprehension0();
  SetComprehension1();
  SetComprehension2();
  SetComprehension3();
}

method SetComprehension0() {
  var w, x, y, z := new ClassA, new ClassA, new ClassB, new ClassB;
  var s := {w, x, y, z};
  // The following set comprehension picks att elements in s:
  var all := set o: object | o in s;
  // The next set comprehension picks out 2 of the elements in s:
  var aa := set o: ClassA | o in s;
  // The next set comprehension picks out the other 2 of the elements in s:
  var bb := set o: ClassB | o in s;
  // The following picks out the same elements as in bb:
  var nn := set o: NothingInParticular | o in s;

  print |s|, " ", |all|, " ";           // 4 4
  print |aa|, " ", |bb|, " ";           // 2 2
  print |aa * bb|, " ", |aa + bb|, " "; // 0 4
  print |nn|, " ", bb == nn, "\n";      // 2 true
}

// SetComprehension1 is like SetComprehension0, but also adds "null" to "s".
method SetComprehension1() {
  var w, x, y, z := new ClassA, new ClassA, new ClassB, new ClassB;
  var s := {w, x, y, z, null};
  // The following set comprehension picks att elements in s:
  var all := set o: object | o in s;
  // The next set comprehension picks out 2 of the elements in s:
  var aa := set o: ClassA | o in s;
  // The next set comprehension picks out the other 2 of the elements in s:
  var bb := set o: ClassB | o in s;
  // The following picks out the same elements as in bb:
  var nn := set o: NothingInParticular | o in s;

  print |s|, " ", |all|, " ";           // 5 4
  print |aa|, " ", |bb|, " ";           // 2 2
  print |aa * bb|, " ", |aa + bb|, " "; // 0 4
  print |nn|, " ", bb == nn, "\n";      // 2 true
}

// SetComprehension2 is like SetComprehension1, but uses maybe-null types in comprehensions
method SetComprehension2() {
  var w, x, y, z := new ClassA, new ClassA, new ClassB, new ClassB;
  var s := {w, x, y, z, null};
  // The following set comprehension picks att elements in s:
  var all := set o: object? | o in s;
  // The next set comprehension picks out 2 of the elements in s:
  var aa := set o: ClassA? | o in s;
  // The next set comprehension picks out the other 2 of the elements in s:
  var bb := set o: ClassB? | o in s;
  // The following picks out the same elements as in bb:
  var nn := set o: NothingInParticular? | o in s;

  print |s|, " ", |all|, " ";           // 5 5
  print |aa|, " ", |bb|, " ";           // 3 3
  print |aa * bb|, " ", |aa + bb|, " "; // 1 5
  print |nn|, " ", bb == nn, "\n";      // 3 true
}

datatype Color = Red | Green | Blue

predicate method True<G>(g: G) { true }

method SetComprehension3() {
  var s: set<bool> := {false, true};
  // The following set comprehension picks att elements in s:
  var all := set o: bool | o in s;
  var aa := set o: bool | o in s && !o;
  var bb := set o: bool | o in s && o;

  print |s|, " ", |all|, " ";           // 2 2
  print |aa|, " ", |bb|, " ";           // 1 1
  print |aa * bb|, " ", |aa + bb|, " "; // 0 2
  print aa == all, " ", aa <= all, "\n"; // false true

  var d := set z: Color | True(z);
  var e := set z: Color | z in d;
  print |d|, " ", |e|, "\n"; // 3 3
}

trait ICell { var data: int }
class CellA extends ICell { }
class CellB extends ICell { }

method GoNil() {
  var a, b := new CellA, new CellB;
  var aa := {a, null};
  var bb := {b, null};
  var cc := aa * bb;
  var dd := aa + bb;
  print "the intersection is ", cc, "\n";  // {null}
  print "there are ", |dd|, " elements in the union\n";  // 3
}
