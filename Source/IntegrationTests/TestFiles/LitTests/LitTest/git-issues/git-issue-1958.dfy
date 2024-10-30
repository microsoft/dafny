// RUN: %exits-with 4 %verify --type-system-refresh --general-traits=datatype "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module Example0 {
  datatype BaseType = Ctor(x: int)
  {
    predicate i()
  }

  type R = r: BaseType | r.i() witness *

  function F0(v: nat): R { // anonymous result value
    var o: R := Ctor(v); // error: this BaseType might not be an R
    o
  }

  function F1(v: nat): (r: R) { // named result value
    var o: R := Ctor(v); // error: this BaseType might not be an R
    o
  }
}

module Example1 {
  datatype BaseType = Ctor(x: int)
  {
    predicate i()
  }

  type R = r: BaseType | r.i() witness *

  function G0(v: nat): R { // anonymous result value
    Ctor(v) // error: this BaseType might not be an R
  }

  function G1(v: nat): (r: R) { // named result value
    Ctor(v) // error: this BaseType might not be an R
  }
}

module Example2 {
  type BaseType(!new)
  {
    predicate i()
  }

  type R = r: BaseType | r.i() witness *

  ghost function Ctor(x: nat): R
    requires exists b: BaseType :: b.i()
  {
    var b: BaseType :| b.i();
    b
  }

  ghost function F(v: nat): R {
    var o: R := Ctor(v); // error: precondition failure (***) as reported in issue 1958, this was once not reported
    o
  }

  ghost function G0(v: nat): R { // anonymous result value
    Ctor(v) // error: precondition failure
  }

  ghost function G1(v: nat): (r: R) { // named result value
    Ctor(v) // error: precondition failure (***) as reported in issue 1958, this was once not reported
  }

  method MF(v: nat) returns (ghost r: R) {
    ghost var o: R;
    o := Ctor(v); // error: precondition failure
    return o;
  }

  predicate IsR(r: R) { true }

  method AssignSuchThat() {
    var r: R :| IsR(r); // error: cannot establish existence of r
  }
   
  ghost function LetSuchThat(): R {
    var r: R :| IsR(r); // error: cannot establish existence of r
    r
  }
}

module Example3 {
  datatype BaseType = BaseType
  {
    predicate i() {
      false
    }
  }

  type R = r: BaseType | r.i() witness *

  function Ctor(x: nat): R
    requires exists b: BaseType :: b.i()
  {
    var b: BaseType :| b.i();
    b
  }

  function F(v: nat): R {
    var o: R := Ctor(v); // error: precondition failure (***) as reported in issue 1958, this was once not reported
    o
  }

  method Main() {
    var r := F(15);
    print r, "\n";
  }
}
