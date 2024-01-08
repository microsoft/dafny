// RUN: %testDafnyForEachCompiler "%s" -- --relax-definite-assignment --type-system-refresh --general-newtypes

method Main() {
  Numerics.Test();
  SimpleBoolAndint.Test();
  TrivialConstraint.Test();
  BaseTypesThatAreSubsetTypes.Test();
  TypeDescriptors.Test();
  PrintChars.Test();
}

module Numerics {
  newtype i32 = x: int | -0x8000_0000 <= x < 0x8000_0000

  newtype j32 = i32

  newtype k32 = j: j32 | true

  newtype nn = x: int | 0 <= x
  newtype byte = n: nn | n < 256

  method Test() {
    var i: i32 := 100;
    var j: j32 := 100;

    print i, " ", j, "\n"; // 100 100
    print i == i, " ", j == j, "\n"; // true true
    print i == j as i32, " ", i as j32 == j, "\n"; // true true
    print i as int == j as int, "\n"; // true

    var k: k32 := 99;
    var n: nn := 99;
    var b: byte := 99;

    print k, " ", n, " ", b, "\n"; // 99 99
    print k == k, " ", n == n, " ", b == b, "\n"; // true true true
    print k == n as k32, " ", n == b as nn, " ", b == k as byte, "\n"; // true true true
  }
}

module SimpleBoolAndint {
  export
    provides Test

  newtype MyBool = bool
  newtype MyInt = int

  method M(x: MyBool) returns (y: MyBool)
  {
    y := x;
    var z := x && !x ==> x;
    assert z;
  }

  method Test() {
    var b: MyBool := true;
    var c := M(b);
    var d: bool := c as bool;
    var e := M((2 < 19 < 17 < 19 <==> true as bool) as MyBool);
    var f := M((2 < 19 < 17 < 19 <==> true) as MyBool);
    var g := M(2 < 19 < 17 < 19);
    print b, " ", c, " ", e, " ", f, " ", g, "\n"; // true true false false false
    TestMore();
    Operators(true);
  }

  method TestMore() {
    var b: bool;
    var c: MyBool;
    c := b as MyBool;
    b := c as bool;

    var x: int;
    var y: MyInt;
    y := x as MyInt;
    x := y as int;

    print b, " ", c, " ", x, " ", y, "\n"; // false false 0 0

    b := x as MyInt == y;
    c := x == y as int;
    print b, " ", c, "\n"; // true true

    b := x as MyInt < y;
    c := x < y as int;
    print b, " ", c, "\n"; // false false
  }

  method Operators(b: MyBool) {
    print b == b && !(b != b) && (b <==> b) && (b ==> b) && (!b ==> b) && (b <== b) && (b <== !b) && (b && b) && (b || b), "\n"; // true
  }
}

module TrivialConstraint {
  export
    provides Test

  newtype MyBool = b: bool | true
  newtype MyInt = int

  method M(x: MyBool) returns (y: MyBool)
  {
    y := x;
    var z := x && !x ==> x;
    assert z;
  }

  method Test() {
    var b: MyBool := true;
    var c := M(b);
    var d: bool := c as bool;
    var e := M((2 < 19 < 17 < 19 <==> true as bool) as MyBool);
    var f := M((2 < 19 < 17 < 19 <==> true) as MyBool);
    var g := M(2 < 19 < 17 < 19);
    print b, " ", c, " ", e, " ", f, " ", g, "\n"; // true true false false false
    TestMore();
    Operators(true);
  }

  method TestMore() {
    var b: bool;
    var c: MyBool;
    c := b as MyBool;
    b := c as bool;

    var x: int;
    var y: MyInt;
    y := x as MyInt;
    x := y as int;

    print b, " ", c, " ", x, " ", y, "\n"; // false false 0 0

    b := x as MyInt == y;
    c := x == y as int;
    print b, " ", c, "\n"; // true true

    b := x as MyInt < y;
    c := x < y as int;
    print b, " ", c, "\n"; // false false
  }

  method Operators(b: MyBool) {
    print b == b && !(b != b) && (b <==> b) && (b ==> b) && (!b ==> b) && (b <== b) && (b <== !b) && (b && b) && (b || b), "\n"; // true
  }
}

module BaseTypesThatAreSubsetTypes {
  type X = x: int | true
  type Y = y: X | true

  newtype A = a: int | true
  newtype B = b: A | true

  newtype M = m: X | true
  newtype N = n: Y | true

  method Test() {
    var w: int := 99;

    var x: X := 100;
    var y: Y := 101;
    
    var a: A := 102;
    var b: B := 103;

    var m: M := 104;
    var n: N := 105;

    print w + x + y + a as int + b as int + m as X + n as X, "\n"; // 714
  }
}

module Char {
  newtype MyChar = char
  newtype UpperCase = ch | 'A' <= ch <= 'Z' witness 'D'

  method Test() {
    var c, u, r := Comparisons();
    print c, " ", u, " ", r, "\n"; // 'e' 'E' true
    MyString([u, u, u]);
    MyString("hello");
    Mix();
    GoodOl'Char('B');
  }
  
  method Comparisons() returns (c: MyChar, u: UpperCase, r: bool) {
    c := 'e';
    u := 'E';

    expect c == c;
    expect !(u != u);

    r := c < c;
    expect !r;
    r := c <= c;
    expect r;
    r := c >= c;
    expect r;
    r := c > c;
    expect !r;

    r := u <= u <= u;
    expect r;

    if c == 'f' && u == 'D' {
      expect false;
    }
  }

  method MyString(s: seq<UpperCase>) {
    if s != [] {
      var ch := s[|s| / 2];
      var charDiff := ch as char - 'A' + 'B';
      expect ch == 'Z' || (charDiff as UpperCase > ch);
      expect charDiff as int == ch as char as int + 1;
      print ch, " ", charDiff, "\n";
    }
  }

  newtype MyBool = bool
  newtype MyInt = int

  method Mix() {
    var c: MyChar := 'A';
    var u: UpperCase := 'B';
    var one: MyInt := 1;

    var b: MyBool := (u as MyChar - c) as char == one as int as char;
    // Note, in the following line, `[c]` prints as "['A']", not just "A", because the type is not seq<char> but seq<MyChar>
    print [c], " ", u, " ", one, " ", b, "\n"; // ['A'] 'B' 1 true
  }
  
  method GoodOl'Char(ch: char)
    requires 'A' <= ch < 'Z'
  {
    print ch, " "; // 'B'

    var c0 := ch as char;
    var c1 := c0 - 'A';
    var charDiff := c1 + 'B';
    print charDiff, " "; // 'C'

    var uc := charDiff as char;
    print uc, " "; // 'C'

    print uc > ch, "\n"; // true
  }
}

module TypeDescriptors {
  newtype UpperCase = ch | 'A' <= ch <= 'Z' witness 'F'
  newtype MyInt = x: int | true witness 365
  newtype Word = x: int | -3 <= x < 0x8000_0000 witness 400
  type SubChar = ch | 'a' <= ch <= 'z' witness 'g'

  method Test() {
    var d0, y0 := P<char>('a', 'a', 'A', 3, 4, 5, 'b');
    var d1, y1 := P<UpperCase>('A', 'a', 'A', 3, 4, 5, 'b');
    var d2, y2 := P<int>(3, 'a', 'A', 3, 4, 5, 'b');
    var d3, y3 := P<MyInt>(4, 'a', 'A', 3, 4, 5, 'b');
    var d4, y4 := P<Word>(5, 'a', 'A', 3, 4, 5, 'b');
    var d5, y5 := P<SubChar>('b', 'a', 'A', 3, 4, 5, 'b');

    print "(";
    print d0, " ", y0, ") (";
    print d1, " ", y1, ") (";
    print d2, " ", y2, ") (";
    print d3, " ", y3, ") (";
    print d4, " ", y4, ") (";
    print d5, " ", y5, ")\n";
  }

  method P<X(0)>(x: X, ch: char, uc: UpperCase, i: int, m: MyInt, w: Word, s: SubChar) returns (d: X, y: X) {
    print ch, " ", uc, " ", i, " ", m, " ", w, " ", s, " -- ", x, "\n";
    d, y := *, x;
  }
}

module PrintChars {
  newtype MyChar = char

  method Test() {
    var ch: char := 'c';
    var my: MyChar := 'm';

    print ch, " ", my, "\n"; // 'c' 'm'
    Print(ch, my);
  }

  method Print<X, Y>(x: X, y: Y) {
    print x, " ", y, "\n"; // 'c' 'm'
  }
}
