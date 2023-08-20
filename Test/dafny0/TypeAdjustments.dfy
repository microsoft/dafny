// RUN: %exits-with 4 %dafny /compile:0 /typeSystemRefresh:1 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

type Even = u | u % 2 == 0

method M0(n: nat, e: Even) {
  var x; // nat
  x := n;
  x := n;

  var y; // int
  y := e;
  y := n;

  var z; // int
  z := n + n;

  x, y, z := *, *, *;
  if {
  case true =>
    assert 0 <= x;
  case true =>
    assert 0 <= y; // error: y is an int
  case true =>
    assert y % 2 == 0; // error: y is an int
  case true =>
    assert 0 <= z; // error: z is an int
  }
}

method M1() {
  var arr;
  arr := new [100];
  var y;
  y := arr[0] + 15;

  arr, y := *, *;
  var obj: object? := arr;
  assert obj != null;
  assert 0 <= y; // error: y is an int
}

method M2(ms: multiset<real>, m0: map<bool, nat>, m1: imap<real, nat>)
  requires true in m0.Keys
  requires 5.90 in m1.Keys
{
  var z;
  var nrr := new nat[100];
  z := nrr[0];
  var matrix := new nat[100, 100];
  z := matrix[2, 3];
  z := ms[3.19];
  z := m0[true];
  z := m1[5.90];

  z := *;
  assert 0 <= z;
}

method M3(s: seq<nat>, arr: array<nat>)
  requires 10 <= |s| && 10 <= arr.Length
{
  // variables a, b, c, x, y, z, w have type seq<nat>
  var a;
  a := s[..10];
  var b;
  b := s[0..];
  var c;
  c := s[0..10];

  var x;
  x := arr[..10];
  var y;
  y := arr[0..];
  var z;
  z := arr[0..10];
  var w;
  w := arr[..];

  var k;
  k := a[0];
  k := b[0];
  k := c[0];
  k := x[0];
  k := y[0];
  k := z[0];
  k := w[0];
  k := *;
  assert 0 <= k;
}

method M4(i: int, n: nat, b: bool) {
  var x; // nat
  x := if b then n else n;
  var y; // int
  y := if b then i else n;
  var z; // int
  z := if b then n else i;

  x, y, z := *, *, *;
  if {
    case true =>
      assert 0 <= x;
    case true =>
      assert 0 <= y; // error: y is int
    case true =>
      assert 0 <= z; // error: z is int
  }
}

datatype List = Nil | Cons(head: nat, tail: List)

type NatA = x: nat | 10 <= x < 20 witness *
type NatB = x: nat | 20 <= x < 30 witness *
type NatC = x: nat | 30 <= x < 40 witness *

method M5(list: List, a: NatA, b: NatB, c: NatC) {
  var x; // nat
  match list {
    case Nil =>
      x := a;
    case Cons(_, Nil) =>
      x := b;
    case _ =>
      x := c;
  }

  x := *;
  if {
    case true =>
      assert 0 <= x;
    case true =>
      assert 10 <= x; // error: x is nat
    case true =>
      assert 20 <= x; // error: x is nat
    case true =>
      assert 30 <= x; // error: x is nat
  }
}

method M6(list: List, a: NatA, b: NatB, c: NatC) {
  var x; // nat
  x :=
    match list
    case Nil => a
    case Cons(_, Nil) => b
    case _ => c;

  x := *;
  if {
    case true =>
      assert 0 <= x;
    case true =>
      assert 10 <= x; // error: x is nat
    case true =>
      assert 20 <= x; // error: x is nat
    case true =>
      assert 30 <= x; // error: x is nat
  }
}

class Cell {
  var data: nat
  function F(): nat {
    2
  }
  method M() returns (x: nat, y: int) {
  }
}

class CellX<X> {
  constructor (u: X) {
    data := u;
  }
  const data: X
  function F(): X {
    data
  }
  method M() returns (x: X, y: int) {
    x, y := data, 12;
  }
}

method M7(n: nat) {
  var cell := new Cell;

  var x;
  x := cell.data;
  var y;
  y := cell.F();
  var z;
  var w;
  z, w := cell.M();
  var ff := cell.F;
  ff := *;
  var u;
  u := ff();

  x, y, z, w, u := *, *, *, *, *;
  assert 0 <= x;
  assert 0 <= y;
  assert 0 <= z;
  assert 0 <= w; // error: w is int
  assert ff.requires();
  assert 0 <= u;
}

method M8(n: nat) {
  var cell;
  cell := new CellX<nat>(n);

  var x;
  x := cell.data;
  var y;
  y := cell.F();
  var z;
  var w;
  z, w := cell.M();
  var ff := cell.F;
  ff := *;
  var u;
  u := ff();

  x, y, z, w, u := *, *, *, *, *;
  assert 0 <= x;
  assert 0 <= y;
  assert 0 <= z;
  assert 0 <= w; // error: w is int
  assert ff.requires();
  assert 0 <= u;
}

method M9(n: nat) {
  if
  case true =>
    var cell := new CellX<nat>(n);
    assert 0 <= cell.data;
  case true =>
    var cell := new CellX(n);
    assert 0 <= cell.data;
  case true =>
    var cell := new CellX<int>(n);
    assert 0 <= cell.data; // error: cell.data is int
  case true =>
    var xx: CellX<nat>;
    var cell := new CellX<int>(n);
    xx := cell; // error: types of xx and cell don't match
  case true =>
    var xx: CellX<int>;
    var cell := new CellX<nat>(n);
    xx := cell; // error: types of xx and cell don't match
}

module TypeParameters {
  datatype List<+Y> = Nil | Cons(head: Y, List<Y>)

  class Class<A(0)> {
    var data: A
    method InstanceMethod(cc: Class<A>) returns (a: A) {
      a := data;
    }
    function InstanceFunction(cc: Class<A>): A
  }

  method MFitToAnything<G>(g: G) returns (r: G) {
    return g;
  }

  method MFitToList<G(0)>(g: List<G>) returns (r: G)

  function FFitToAnything<G>(g: G): G

  function FFitToList<G(0)>(g: List<G>): G

  method M(c: Class<nat>, xs: List<nat>, ys: List<int>, n: nat) {
    var d := c;
    var i := d.InstanceMethod(c);
    assert 0 <= i;

    var g0 := MFitToAnything(c);
    var g1 := MFitToAnything(xs);
    var g2 := MFitToAnything(n);
    var g3 := MFitToAnything((n, n));
    assert 0 <= g0.data;
    assert g1.Cons? ==> 0 <= g1.head;
    assert 0 <= g2;
    assert 0 <= g3.0 && 0 <= g3.1;

    var x; // nat
    x := MFitToList(xs);
    var y; // int
    y := MFitToList(ys);
    assert 0 <= x;
    assert 0 <= y; // error: y is int
  }

  method F(c: Class<nat>, xs: List<nat>, ys: List<int>, n: nat) {
    var d := c;
    var i := d.InstanceFunction(c);
    assert 0 <= i;

    var g0 := FFitToAnything(c);
    var g1 := FFitToAnything(xs);
    var g2 := FFitToAnything(n);
    var g3 := FFitToAnything((n, n));
    assert 0 <= g0.data;
    assert g1.Cons? ==> 0 <= g1.head;
    assert 0 <= g2;
    assert 0 <= g3.0 && 0 <= g3.1;

    var x; // nat
    x := FFitToList(xs);
    var y; // int
    y := FFitToList(ys);
    assert 0 <= x;
    assert 0 <= y; // error: y is int
  }

  method Tuples(n: nat) {
    var p;
    p := (n, n);
    p := *;
    assert 0 <= p.0;
    assert 0 <= p.1;
  }
}
