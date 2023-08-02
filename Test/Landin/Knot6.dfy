// RUN: %exits-with 2 %dafny /compile:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

class Ref {
  ghost var hogp: () ~> int
}

method LogicKnot1(r1: Ref,r2: Ref)
  modifies r1
  ensures r1.hogp == (() reads r2, r2.hogp.reads() => if r2.hogp.requires() then 1 + r2.hogp() else 0)
{
  r1.hogp := () reads r2, r2.hogp.reads() => if r2.hogp.requires() then 1 + r2.hogp() else 0;
}

method LogicKnot2(r1: Ref,r2: Ref)
  modifies r2
  ensures r2.hogp == (() reads r1, r1.hogp.reads() => if r1.hogp.requires() then 1 + r1.hogp() else 0)
{
  r2.hogp := () reads r1, r1.hogp.reads() => if r1.hogp.requires() then 1 + r1.hogp() else 0;
}

method Main()
  ensures false
{
  var r1 := new Ref;
  var r2 := new Ref;
  LogicKnot1(r1,r2);
  LogicKnot2(r1,r2);
}
