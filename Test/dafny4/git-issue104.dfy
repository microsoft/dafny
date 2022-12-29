// RUN: %baredafny verify %args "%s" > "%t"
// RUN: %baredafny run %args --no-verify --target=cs "%s" >> "%t"
// RUN: %baredafny run %args --no-verify --target=java "%s" >> "%t"
// RUN: %baredafny run %args --no-verify --target=js "%s" >> "%t"
// RUN: %baredafny run %args --no-verify --target=go "%s" >> "%t"
// RUN: %baredafny run %args --no-verify --target=py "%s" "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

predicate method bug(a: array<int>)
  reads a
{
  forall i, j | 0 <= i <= j < a.Length :: a[i] <= a[j]
}

method Main() {
  var a := new int[25](i => 2*i + 3);
  var b := new int[25](i => var u := 2*i + 3; if i == 7 then 2 else u);
  var c0 := bug(a);
  var c1 := bug(b);
  print c0, " ", c1, "\n"; // true false
}
