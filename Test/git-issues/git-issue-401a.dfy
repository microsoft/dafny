// RUN: not %dafny /compile:0 /z3exe:"%S"/../../Binaries/z3/binz/z3 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

method m() {
  assert 1 + 1 == 2;
}
