// RUN: %dafny /compile:0 /print:"%t.print" /dprint:"%t.dprint" "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module Basics {
  method M() {
    var k := 0;
    forall i | 0 <= i < 20 {
      k := k + i;  // error: not allowed to assign to local k, since k is not declared inside forall block
    }

    forall i | 0 <= i < 20
      ensures true
    {
      var x := i;
      x := x + 1;
    }

    ghost var y;
    var z;
    forall i | 0 <= i
      ensures true
    {
      var x := i;
      x := x + 1;
      y := 18;  // error: assigning to a (ghost) variable inside a ghost forall block
      z := 20;  // error: assigning to a (non-ghost) variable inside a ghost forall block
    }
  }
}

module Misc {
  class C { }

  method M0(IS: set<int>) {
    forall i | 0 <= i < 20 {
      i := i + 1;  // error: not allowed to assign to bound variable
    }

    forall i | 0 <= i
      ensures true
    {
      ghost var x := i;
      x := x + 1;  // cool
    }

    var ca := new C[20];
    forall i | 0 <= i < 20 {
      ca[i] := new C;  // error: new allocation not allowed
    }
  }

  method M1() {
    forall i | 0 <= i < 20 {
      assert i < 100;
      if i == 17 { break; }  // error: nothing to break out of
    }

    var m := 0;
    label OutsideLoop:
    while m < 20 {
      forall i | 0 <= i < 20 {
        if i == 17 { break; }  // error: not allowed to break out of loop that sits outside forall
        if i == 8 { break break; }  // error: ditto (also: attempt to break too far)
        if i == 9 { break OutsideLoop; }  // error: ditto
      }
      m := m + 1;
    }

    forall i | 0 <= i < 20 {
      var j := 0;
      while j < i {
        if j == 6 { break; }  // fine
        if j % 7 == 4 { break break; }  // error: attempt to break out too far
        if j % 7 == 4 { break OutsideLoop; }  // error: attempt to break to place not in enclosing scope
        j := j + 1;
      }
    }
  }
}

module AnotherModule {
  class C {
    var data: int
    ghost var gdata: int

    ghost method Init_ModifyNothing() { }

    ghost method Init_ModifyThis()
      modifies this
    {
      data := 6;  // error: assignment to a non-ghost field
      gdata := 7;
    }

    ghost method Init_ModifyStuff(c: C)
      modifies this, c
    {
    }

    method NonGhostMethod() {
      print "hello\n";
    }

    ghost method GhostMethodWithModifies(x: int)
      modifies this
    {
      gdata := gdata + x;
    }
  }

  method TestNew() {
    forall i | 0 <= i < 20
      ensures true
    {
      var c := new C;  // error: 'new' not allowed in proof context
      var d := new C.Init_ModifyNothing();  // error (x2): 'new' not allowed in ghost context
      var e := new C.Init_ModifyThis();  // error (x2): 'new' not allowed in ghost context
      var f := new C.Init_ModifyStuff(e);  // error (x2): 'new' not allowed in ghost context
      c.Init_ModifyStuff(d);  // error: not allowed to call method with modifies clause in ghost context
      c.NonGhostMethod();  // error: only allowed to call ghost methods (because of possible 'print' statements, sigh)
    }
  }

  method M2() {
    var a := new int[100];
    forall x | 0 <= x < 100
      ensures true
    {
      a[x] :| assume a[x] > 0;  // error: not allowed to update heap location in a proof-forall statement
    }
  }

  method M3(c: C) {
    forall x {
      c.GhostMethodWithModifies(x);  // error: not allowed to call method with nonempty modifies clause
    }
    forall x
      ensures true
    {
      c.GhostMethodWithModifies(x);  // error: not allowed to call method with nonempty modifies clause
    }
  }

  method AggregatePrinting() {
    forall i | 0 <= i < 1000
      ensures true
    {
      print "here we go\n"; // error: cannot print in ghost context
    }
  }
}

module UpdatesInOuterScope {
  method M() {
    ghost var x := 0;
    forall i | 0 <= i < 100
      ensures true
    {
      // AssignStmt
      x := 10;
      // AssignSuchThatStmt
      x :| x == 10;
      // CallStmt
      x := P();
    }
  }

  method P() returns (x: int)
}

module MoreReturn {
  method M() {
    forall i | 0 <= i < 20
      ensures true
    {
      if i == 8 { return; }  // error: return not allowed inside forall block
    }
  }
}

// ---------- return inside modify statement

module ModifyStmtReturn0 {
  class Cell {
    var data: int
  }

  method M(c: Cell)
    modifies c
    ensures c.data == 10
  {
    modify c;
    c.data := 10;
  }

  method W(c: Cell)
    modifies c
    ensures c.data == 10
  {
    var i := 0;
    while i < 10
      invariant i == 0 || c.data == 10
    {
      modify c;
      c.data := 10;
      i := i + 1;
    }
  }
}

module ModifyStmtReturn1 refines ModifyStmtReturn0 {
  method M... {
    modify ... {
      return; // error: a "return" here would cause a problem with the refinement
    }
    ...;
  }
}

module ModifyStmtReturn2 {
  class C { }

  method M(a: int, b: int) {
    var c := new C;
    var x := 0;
    label Outer:
    for i := 0 to 100 {
      modify c {
        var y := 0;
        if i == a {
          return; // error: return not allowed inside "modify"
        } else {
          y := y + x;
          x := x + 2; // error: not allowed to modify variable declared outside "modify" statement
        }

        label Inner:
        for j := 0 to i {
          if 5 * i == a {
            break;
          } else if 5 * i == b {
            break Inner;
          } else if 5 * i == a + b {
            return; // error: return is not allowed here either
          } else {
            y := y + x;
            x := x + 2; // error: not allowed to modify variable declared outside "modify" statement
          }
        }
      }
    }
  }
}

// ---------- break inside modify statement

module ModifyStmtBreak0 {
  class Cell {
    var data: int
  }

  method W(c: Cell)
    modifies c
    ensures c.data == 10
  {
    var i := 0;
    label L:
    while i < 10
      invariant i == 0 || c.data == 10
    {
      modify c;
      c.data := 10;
      i := i + 1;
    }
  }
}

module ModifyStmtBreak1 refines ModifyStmtBreak0 {
  method W... {
    ...;
    while ... {
      modify ... {
        if * {
          break; // error: a "break" here would cause a problem with the refinement
        } else {
          break L; // error: a "break" here would cause a problem with the refinement
        }
      }
      ...;
    }
  }
}

module ModifyStmtBreak2 {
  class C { }

  method M(a: int, b: int) {
    var c := new C;
    label Outer:
    for i := 0 to 100 {
      modify c {
        var y := 0;
        if i == a {
          break; // error: break to outside "modify" is not allowed
        } else if i == b {
          break Outer; // error: break to outside "modify" is not allowed
        }

        label Inner:
        for j := 0 to i {
          if 5 * i == a {
            break;
          } else if 5 * i == b {
            break Inner;
          }
        }
      }
    }
  }
}
