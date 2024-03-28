// RUN: %verify "%s" > "%t"
// RUN: %exits-with 3 %dafny /noVerify /compile:4 --target cs "%s" --args csharp 1 >> "%t"
// RUN: %diff "%s.expect" "%t"

method Main(args: array<string>, dummy: int) {
  print "ok", dummy;
}
