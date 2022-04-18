// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %dafny /noVerify /compile:4 /compileTarget:py "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

method LinearSearch(a: array<int>, key: int) returns (n: nat)
  ensures 0 <= n <= a.Length
  ensures n == a.Length || a[n] == key
{
  n := 0;
  while n < a.Length
    invariant n <= a.Length 
  {
    if a[n] == key {
      return;
    }
    n := n + 1;
  }
}

method PrintArray<A>(a: array?<A>) {
  if (a == null) {
    print "It's null\n";
  } else {
    var i := 0;
    while i < a.Length {
      print a[i], " ";
      i := i + 1;
    }
    print "\n";
  }
}

method Main() {
  var a := new int[23];
  var i := 0;
    while i < 23 {
      a[i] := i;
      i := i + 1;
    }
    PrintArray(a);
    var n := LinearSearch(a, 17);
    print n, "\n";
    var s : seq<int> := a[..];
    print s, "\n";
    s := a[2..16];
    print s, "\n";
    s := a[20..];
    print s, "\n";
    s := a[..8];
    print s, "\n";
    
    // Conversion to sequence should copy elements (sequences are immutable!)
    a[0] := 42;
    print s, "\n";
    MultipleDimensions();
    
    
   }
   
   method MultipleDimensions() {
     var matrix := new int[2,8];
     }
