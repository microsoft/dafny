// RUN: %exits-with 2 %dafny /compile:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

class Y {
  const f: Y -> nat // Type error here? because since Y can be accessed, -> should be ~>
  constructor(f: Y -> nat)
    ensures this.f == f
  {
    this.f := f;
  }
}

method Main()
  ensures false
{
  var knot := new Y((x: Y) => 1 + x.f(x)); // Why doesn't it have a reads clause? Because f can pretend that it does not
  var a := knot.f(knot);
}