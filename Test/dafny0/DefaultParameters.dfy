// RUN: %dafny "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module Actuals {
  datatype List<T> = Nil | Cons(T, tail: List<T> := Nil)

  method M(a: int, b: int) returns (r: int)
    ensures r == a + 2 * b
  {
    r := a + 2 * b;
  }

  function method F(a: int, b: int, c: int): int
  {
    a + 2 * b + 3 * c
  }

  class Cell<U> {
    var value: U
    constructor (u: U)
      ensures value == u
    {
      value := u;
    }
  }

  iterator Iter(a: int, b: int) yields (z: int) {
  }

  method ParsingActualBindings() {
    var xs0 := Cons(5, tail := Cons(7, Nil));
    var tuple0 := (1 := 10, 0 := 300);
    var r0 := M(100, b := 60);
    var x0 := F(200, c := 20, b := 10);
    var c0 := new Cell(u := 75);
    var iter0 := new Iter(10, b := 20);

    var xs1 := Cons(5, Cons(7, Nil));
    var tuple1 := (300, 10);
    var r1 := M(100, 60);
    var x1 := F(200, 10, 20);
    var c1 := new Cell(75);
    var iter1 := new Iter(10, 20);

    assert xs0 == xs1;
    assert tuple0 == tuple1;
    assert r0 == r1;
    assert x0 == x1;
    assert c0.value == c1.value;
    assert iter0.a == iter1.a && iter0.b == iter1.b;
  }
}

module Termination {
  function method R(n: nat := R(0)): nat { n }
}

module TwoState {
  class C { }

  twostate predicate P0(a: C, b: C := a)
  twostate predicate P1(a: C, new b: C := a)
  twostate predicate P2(new a: C, b: C := a) // error: 'b' needs to have been allocated already in the old state
  twostate predicate P3(new a: C, new b: C := a)
  twostate lemma L0(a: C, b: C := a)
  twostate lemma L1(a: C, new b: C := a)
  twostate lemma L2(new a: C, b: C := a) // error: 'b' needs to have been allocated already in the old state
  twostate lemma L3(new a: C, new b: C := a)
}

module A {
  function method F(x: int := 5): (r: int)
    ensures r == x
  method M(x: int := 5) returns (r: int)
    ensures r == x

  lemma Lemma0()
    ensures F() == 5
  {
  }
}

module B refines A {
  function method F(x: int): int { x }
  method M(x: int) returns (r: int) { r := x; }

  function method F'(x: int := 6): int { x }
  method M'(x: int := 6) returns (r: int) ensures r == x { r := x; }

  lemma Lemma1()
    ensures F() == 6
  { // error: postcondition violation
  }

  method TestLemmas() {
    var y := F();
    if * {
      Lemma0();
      assert y == 5;
    } else if * {
      Lemma0();
      assert y == 6; // error
    } else if * {
      Lemma1();
      assert F() == 5 && F() == 6; // fine, since F() promises one and Lemma1() the other
    } else if * {
      assert F() == 5;
      var r := M();
      assert r == 5;
      assert r == 7; // error
    } else {
      assert F'() == 6;
      var r := M'();
      assert r == 6;
    }
  }
}

module Wellformedness {
  class C {
    var u: int
    const v: int

    function T0(x: int := this.u): int // reads clause is checked only at call sites

    function T1(x: int := this.v): int

    function T2(c: C, x: int := c.u): int
      reads c

    function T3(c: C, x: int := c.u): int // reads clause is checked only at call sites
      requires c == this
      reads this

    function T4(x: int := 10, z: int := 10 / x): int // error: division by zero

    function T5(x: int, z: int := 10 / x): int // error: division by zero (despite precondition)
      requires x == 10

    function T6(x: int := 3 / y, y: int := 10): int // error: division by zero (despite precondition)
      requires y == 10

    function T7(x: int := 3, y: int := 10): int
      requires y == 8
      requires 1 / x == 2000 // error: division by zero
  }

  method M0(x: int := 8, y: int := 10 / x) // error: division by zero

  method M1(x: int, z: int := 10 / x) // error: division by zero (despite precondition)
    requires x == 10

  method M2(x: int := 3 / y, y: int := 10) // error: division by zero (despite precondition)
    requires y == 10

  method M3(x: int := 3, y: int := 10)
    requires y == 8
    requires 1 / x == 2000 // error: division by zero

  iterator Iter0(x: int := 8, y: int := 10 / x) // error: division by zero (reported twice)

  iterator Iter1(x: int, z: int := 10 / x) // error: division by zero (despite precondition) (reported twice)
    requires x == 10

  iterator Iter2(x: int := 3 / y, y: int := 10) // error: division by zero (despite precondition) (reported twice)
    requires y == 10

  iterator Iter3(x: int := 3, y: int := 10)
    requires y == 8
    requires 1 / x == 2000 // error: division by zero (reported twice)

  datatype Dt = Dt(x: int := 8, y: int := 10 / x) // error: division by zero (reported twice)

  function method Int(): int
  function method Nat(): int
    ensures 0 <= Nat()

  function SubrangeF0(x: nat := Int()): int // error: Int() may not be a "nat"
  method SubrangeM0(x: nat := Int()) // error: Int() may not be a "nat"
  iterator SubrangeI0(x: nat := Int()) // error: Int() may not be a "nat"
  datatype SubrangeD0 = D0(x: nat := Int()) // error: Int() may not be a "nat"

  function SubrangeF1(x: nat := Nat()): int
  method SubrangeM1(x: nat := Nat())
  iterator SubrangeI1(x: nat := Nat())
  datatype SubrangeD1 = D1(x: nat := Nat())

  iterator DependencyRegression(x: nat)
    // if the call graph dependencies are set up correctly, then
    requires assert 0 <= Nat(); 3 < 10  // there should be no complaints about this assertion
}

module Nested {
  function F(xt: int, yt: int := G(xt)): int
  function G(x: int, y: int := x): int {
    if x <= 0 then 0 else
      F(x - 1) // expands to F(x-1, G(x-1, x-1))
  }

  function F'(xt: int, yt: int := G'(xt)): int
    decreases 5
  {
    G'(xt) // error: cannot prove termination
  }
  function G'(x: int, y: int := x): int
    decreases 6
  {
    F'(y) // error: expands to F'(y, G'(y, y)), and cannot prove termination for call to G'
  }

  function K(xt: nat, yt: nat := if xt == 0 then 6 else L(xt - 1)): nat
    decreases xt, 0
  function L(x: nat, y: nat := x): nat
    decreases x, 1
  {
    K(x) // should expand to: K(x, if x == 0 then 6 else L(x - 1))
  }

  function A(x: nat := B()): nat
  function B(x: nat := C()): nat
  function C(): nat
    decreases 7
  {
    ABC0() + ABC1() + ABC2()
  }
  function ABC0(): nat
    decreases 6
  {
    A(B(C())) // error: call to C may not terminate
  }
  function ABC1(): nat
    decreases 6
  {
    // the following expression expands to A(B(C()))
    A(B()) // error: call to C may not terminate
  }
  function ABC2(): nat
    decreases 6
  {
    // the following expression expands to A(B(C()))
    A() // error: call to C may not terminate
  }
}

module ReadsAndDecreases {
  // reads and decreases are not checked directly on default-valued expressions. Instead,
  // those are checked at call sites.
  class C {
    var data: int
    // The following function has an empty reads clause. Still, it's fine for the default
    // value of the parameter to read "this.data".
    function M(x: int := data): int { x }

    // The following function has a decreases clause that would not allow recursive calls.
    // Still, it's fine for the default value of the parameter to call N.
    function NA(): int
      decreases 3
    {
      NCaller1(2, this)
    }
    function NB(x: int, y: int := NA()): int
      decreases x
    {
      NCaller0(x, this) + NCaller1(x, this)
    }

    // The following function has a division-by-zero error in the default-value expression
    // for "y". That's not allowed (even if all call sites pass in "x" as non-0), and it's
    // checked here.
    function O(x: int, y: int := 3 / x): int // error: division by zero (reported twice, see comment below in OCaller1)
    {
      x + y
    }
  }

  function MCaller0(c: C): int {
    c.M() // error: reads violation
  }
  function MCaller1(c: C): int
    reads c
  {
    c.M()
  }
  function MCaller2(c: C): int {
    c.M(2)
  }

  function NCaller0(x: int, c: C): int
    decreases x, 0
  {
    if x <= 0 then 0 else c.NB(x - 1, 0)
  }
  function NCaller1(x: int, c: C): int
    decreases x, 0
  {
    if x <= 0 then 0 else c.NB(x - 1) // error: this defaults to c.NB(x - 1, NA()), and NA() may not terminate
  }

  function OCaller0(c: C): int {
    c.O(1) + c.O(0, 2)
  }
  function OCaller1(c: C): int {
    // The following line causes a division-by-zero error to be reported (again) at the default-value expression.
    // It's unfortunate that the error is shown there. However, in a correct program, the default-value
    // expression needs to be fixed anywhere, so this kind of double-error is not likely to be either common
    // or too confusing.
    c.O(0) // error: division by zero (reported at declaration of O)
  }
  function OCaller2(x: int, c: C): int
    requires x != 0
  {
    c.O(x)
  }

  function method J(): int
  lemma AboutJ()
    ensures J() != 0
  method Jx(x: int := AboutJ(); 2 / J()) // lemma ensures no div-by-zero
  method Jy() {
    Jx(); // no div-by-zero reported here, either (because lemma is copied as part of the default-value expression)
  }

  lemma Lemma(x: int)
    requires x == 3
  function BadLemmaCall(y: int := Lemma(2); 5): int // error: precondition violation in lemma call
  method BadLemmaCaller() {
    var z := BadLemmaCall(); // error: precondition violation in lemma call (reported at the position of the default value 2 lines above)
  }

  function MoreReads(a: array<int>, m: array2<int>,
    x: int := if 0 < a.Length then a[0] else 3,
    y: int := if 0 < m.Length0 && 0 < m.Length1 then m[0, 0] else 3): int
  function ReadA0(a: array<int>, m: array2<int>): int
    requires m.Length0 == 0
  {
    MoreReads(a, m) // error: reads violation for a
  }
  function ReadA1(a: array<int>, m: array2<int>): int
    requires m.Length0 == 0
    reads a
  {
    MoreReads(a, m)
  }
  function ReadM0(a: array<int>, m: array2<int>): int
    requires a.Length == 0
  {
    MoreReads(a, m) // error: reads violation for m
  }
  function ReadM1(a: array<int>, m: array2<int>): int
    requires a.Length == 0
    reads m
  {
    MoreReads(a, m)
  }
  function ReadNeither(a: array<int>, m: array2<int>): int
    requires a.Length == 0 == m.Length0
  {
    MoreReads(a, m)
  }
}

module TickRegressions {
  lemma X()
  // The uses of X() in the following declarations once caused malformed Boogie, because
  // the $Tick variable wasn't used in the necessary Boogie modifies clauess.
  const u: nat := (X(); -3) // error: -3 is not a nat
  datatype S = S(x: nat := X(); -3) // error: -3 is not a nat
}

module StmtExprCallPreconditionRegression {
  function G(): int
    // Amazingly, the well-formedness check for G was once omitted for this function.
    requires (X(); true) // error: precondition violation

  lemma X()
    requires false
  { }
}

module IteratorFrameRegression {
  ghost method X()
  // The following once caused malformed Boogie, because the $_Frame variable had not been declared.
  iterator Iter()
    requires (X(); 3) == 3
  // The following once caused malformed Boogie, because the $_Frame variable had not been declared.
  iterator Iter'(x: int := X(); 3)
}

