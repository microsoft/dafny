// RUN: %exits-with 4 %verify --cores 1 --progress "%s" > "%t".raw
// RUN: %sed 's/time: \d*ms/redacted/g' "%t".raw > %t
// RUN: %diff "%s.expect" "%t"

// These tests make sure that the built-in arrow types are taken into account when
// generating proof obligations.

ghost function CheckReads(f: int ~> int, x: int): int
{ // 3 proof obligations
  assert true;
  f(x)  // error: reads and precondition
}

ghost function CheckPre(f: int --> int, x: int): int
{ // 2 proof obligations
  assert true;
  f(x)  // error: precondition
}

ghost function CheckReadsTot(f: int -> int, x: int): int
{ // 1 proof obligations
  assert true;
  f(x)
}

ghost function CheckReadsParens(f: int -> int, x: int): int
{ // 1 proof obligations
  assert true;
  (f)(x)
}

ghost function CheckReadsLambdaGeneral(x: int): int
{ // 3 proof obligations
  assert true;
  (y reads {} requires true => y)(x)
}

ghost function CheckReadsLambdaPartial(x: int): int
{ // 2 proof obligations
  assert true;
  (y requires true => y)(x)
}

ghost function CheckReadsLambdaTotal(x: int): int
{ // 1 proof obligations
  assert true;
  (y => y)(x)
}
