// RUN: %dafny /compile:3 /spillTargetCode:3 /print:"%t.print" /dprint:"%t.dprint" "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

trait TT
{
  function method Plus(x:int, y:int) : int
    requires x > y
  {
     x + y
  }
  function method Times(x:int, y:int) : int
    requires x > y
  static function method StaticMinus(x:int, y:int) : int
    requires x > y
  {
     x - y
  }

  method Double(x: int)
  {
    print "d=", x+x, "\n";
  }
  method AddFive(x: int)
  static method StaticTriple(x: int)
  {
    print "t=", 3*x, "\n";
  }
}

class CC extends TT
{
  function method Times(x:int, y:int) : int
    requires x>y
  {
    x * y
  }
  method AddFive(x: int)
  {
    print "a5=", x+5, "\n";
  }
}

method Client(t: TT)
{
  var x := t.Plus(10, 2);
  print "x=", x, "\n";
  t.Double(400);
  t.AddFive(402);
  t.StaticTriple(404);
}

method Main()
{
  var c := new CC;
  var y := c.Plus(100, 20);
  print "y=", y, "\n";
  Client(c);
  var z := TT.StaticMinus(50, 20);
  print "z=", z, "\n";
  var z' := CC.StaticMinus(50, 20);
  print "z'=", z', "\n";
  var w := c.StaticMinus(50, 20);
  print "w=", w, "\n";

  c.Double(500);
  { var cc: CC := new CC; cc.AddFive(502); }
  c.StaticTriple(504);
  TT.StaticTriple(504);
  CC.StaticTriple(505);

  var seven := OtherModule.Y.F(15);
  assert seven == 7;
  var b := OtherModule.Y.P(seven as real);
  print "From OtherModule.Y: ", seven, " and ", b, "\n";
  seven := OtherModule.X.F(15);
  assert seven == 7;
  b := OtherModule.X.P(seven as real);
  print "From OtherModule.X: ", seven, " and ", b, "\n";

  TestFields.Test();

  Generics.Test();
}

module OtherModule {
  trait Y {
    static function method F(x: int): int
    { x / 2 }
    static method P(t: real) returns (f: bool)
    {
      print "This is OtherModule.P(", t, ")\n";
      f := t < 10.0;
    }
  }
  class X extends Y {
  }
}


module TestFields {
  trait J {
    var f: int
  }

  class C extends J {
  }

  method Test() {
    var c := new C;
    var j: J := c;

    c.f := 15;
    assert j.f == 15;
    print "c.f= ", c.f, " j.f= ", j.f, "\n";
    j.f := 18;
    assert c.f == 18;
    print "c.f= ", c.f, " j.f= ", j.f, "\n";
  }
}

module Generics {
  trait Identity {
    method Call<T>(x: T) returns (r: T)
  }   

  class IdentityImpl extends Identity {
    method Call<T>(x: T) returns (r: T) {
      r := x;
    }
  } 

  // TODO-RS: Call this something else: Closure? Method?
  trait Function<T, R> {
    method Call(t: T) returns (r: R)
    // method Compose<S>(f: Function<S, T>) returns (res: Function<S, R>) {
    //   res := new ComposedFunction(f, this);
    // }
  }

  type IntFunction<T> = Function<T, int>

  // class ComposedFunction<S, T, R> extends Function<S, R> {
  //   const first: Function<S, T>
  //   const second: Function<T, R>

  //   constructor(first: Function<S, T>, second: Function<T, R>) {
  //     this.first := first;
  //     this.second := second;
  //   }

  //   method Call(s: S) returns (r: R) {
  //     var t := first.Call(s);
  //     r := second.Call(t);
  //   }
  // }

  class Triple extends Function<int, int> {
    constructor() {}
    method Call(t: int) returns (r: int) {
      r := 3*t;
    }
  }

  method Test() {
    var tripler := new Triple();
    var x := tripler.Call(42);
    print "x=", x, "\n";
  }
}
