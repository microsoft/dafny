// RUN: %testDafnyForEachCompiler "%s" -- --type-system-refresh --general-newtypes --general-traits=datatype --reads-clauses-on-methods

method Main() {
  Set.Test();
  Set.TestSpecialCase();
  Iset.Test();
  Frames.CallEm();
  Decreases.Test();
  Multiset.Test();
}

module Set {
  newtype IntSet = s: set<int> | true

  method Test() {
    var s: IntSet;
    s := {};
    print s, " "; // {}
    s := {6, 19};
    print |s|, " "; // 2
    s := set x: int | 6 <= x < 10 && 2 * x < 300;
    print |s|, " ", 7 in s, " "; // 4 true
    s := set x: int | 6 <= x < 10 :: 2 * x;
    print |s|, " ", 7 in s, "\n"; // 4 false

    var bb := [5 in s, 12 in s, 19 !in s];

    var t: IntSet := s;

    s := s + t;
    s := s - t;
    s := s * t;
    var disjoint := s !! t;

    print bb, " ", s, " ", disjoint, "\n"; // [false, true, true] {} true

    var u: set<int>;
    u := s as set<int>;
    s := u as IntSet;
    var b0 := s is set<int>;
    var b1 := u is IntSet;

    print |s|, " ", |u|, " ", b0, " ", b1, "\n"; // 0 0 true true

    b0 := s <= t;
    b1 := s < t;
    var b2 := s > s;
    var b3 := s >= s;
    print b0, " ", b1, " ", b2, " ", b3, "\n"; // true true false true

    b0 := s == t;
    b1 := s != t;
    print b0, " ", b1, "\n"; // false true
  }

  method TestSpecialCase() {
    var dt := Dt;
    print |dt.FFF(15)|, "\n"; // 15
  }

  // auto-accumulator tail recursive function in trait (this has a special case in the compiler code)
  trait Trait {
    function FFF(n: nat): IntSet {
      if n == 0 then {} else {n} + FFF(n - 1)
    }
  }

  datatype Dt extends Trait = Dt
}

module Iset {
  newtype IntIset = s: iset<int> | true

  method Test() {
    var s: IntIset;
    s := iset{};
    print "iset: ", s, " "; // {}
    s := iset{6, 19};
    print s - iset{6}, " "; // {19}
    s := iset x: int | 6 <= x < 10 && 2 * x < 300;
    s := iset x: int | 6 <= x < 10 :: 2 * x;

    var t: IntIset := s;

    s := s + t;
    s := s - t;
    s := s * t;
    var disjoint := s !! t;

    print s, " ", disjoint, "\n"; // {} true

    var u: iset<int>;
    u := s as iset<int>;
    s := u as IntIset;
    var b0 := s is iset<int>;
    var b1 := u is IntIset;

    print s, " ", u, " ", b0, " ", b1, "\n"; // {} {} true true

    b0 := s <= t;
    b1 := s < t;
    var b2 := s > s;
    var b3 := s >= s;
    print b0, " ", b1, " ", b2, " ", b3, "\n"; // true true false true

    b0 := s == t;
    b1 := s != t;
    print b0, " ", b1, "\n"; // false true
  }
}

module Frames {
  method CallEm() {
    var o := new object;
//    label Recently:
//    M({o}, iset{o}, multiset{o, o}, [o, o, o]);
//    var u := F({o}, iset{o}, multiset{o, o}, [o, o, o]);
//    ghost var b := P2@Recently({o}, iset{o}, multiset{o, o}, [o, o, o]);
  }
  
  newtype ObjectSet = s: set<object> | true
  newtype ObjectISet = ss: iset<object> | true
  newtype ObjectMultiset = m: multiset<object> | true
  /*new*/type ObjectSeq = q: seq<object> | true

  function R(x: int): ObjectSet {
    {}
  }

  method M(s: ObjectSet, ss: ObjectISet, m: ObjectMultiset, q: ObjectSeq)
    modifies s
    modifies ss
    modifies m
    modifies q
    reads s
    reads ss
    reads m
    reads q
  {
    assert unchanged(s);
    assert unchanged(ss);
    assert unchanged(m);
    assert unchanged(q);
    modify s;
    modify ss;
    modify m;
    modify q;
    for i := 0 to 100
      modifies s
      modifies ss
      modifies m
      modifies q
    {
    }
    ghost var g: bool;
    g := fresh(s);
    g := fresh(ss);
    g := fresh(q);
  }

  function F(s: ObjectSet, ss: ObjectISet, m: ObjectMultiset, q: ObjectSeq): int
    reads s
    reads ss
    reads m
    reads q
    reads R
  {
    6
  }

  twostate predicate P2(s: ObjectSet, ss: ObjectISet, m: ObjectMultiset, q: ObjectSeq)
    reads s
    reads ss
    reads m
    reads q
    reads R
  {
    true
  }
}

module Decreases {
  newtype MyInt = int
  newtype BoundedInt = x: int | 0 <= x < 10_000
  newtype BoolSet = s: set<bool> | true

  method Test() {
    A(100);
    K({true, true});
  }

  method A(x: int)
    requires x < 10_000
    decreases x
  {
    if 0 < x {
      B((x - 1) as MyInt);
    } else if x == 0 {
      print "Ends in A\n";
    }
  }

  method B(y: MyInt)
    requires y < 10_000
    decreases y
  {
    if 0 < y {
      C((y - 1) as BoundedInt);
    } else if y == 0 {
      print "Ends in B\n";
    }
  }

  method C(z: BoundedInt)
    decreases z
  {
    if 0 < z {
      A((z - 1) as int);
    } else if z == 0 {
      print "Ends in C\n";
    }
  }

  method K(s: set<bool>)
    decreases s
  {
    if s == {} {
      print "K is done\n";
    } else {
      L(s as BoolSet);
    }
  }

  method L(t: BoolSet)
    requires t != {}
    decreases t, 0 as MyInt
  {
    var b :| b in t;
    var t' := t - {b};
    K(t' as set<bool>);
  }
}

module Multiset {
  newtype IntMultiset = s: multiset<int> | true

  method Test() {
    var s: IntMultiset;
    s := multiset{};
    print s, " "; // multiset{}
    s := multiset{6, 19, 6};
    print |s|, " "; // 3
    print 7 in s, " ", 6 in s, "\n"; // false true
    print s[6], " ", s[19], " ", s[20], "\n"; // 2 1 0
    s := s[17 := 3][1800 := 0][6 := 1];
    print s[6], " ", s[17], " ", s[20], "\n"; // 1 3 0

    var t: IntMultiset := s;

    s := s + t;
    s := s - t;
    s := s * t;
    expect s == t;
    print |s|, "\n"; // 5
    s := s - t;

    var disjoint := s !! t;
    print s, " ", disjoint, "\n"; // multiset{} true

    var u: multiset<int>;
    u := s as multiset<int>;
    s := u as IntMultiset;
    var b0 := s is multiset<int>;
    var b1 := u is IntMultiset;

    print |s|, " ", |u|, " ", b0, " ", b1, "\n"; // 0 0 true true

    b0 := s <= t;
    b1 := s < t;
    var b2 := s > s;
    var b3 := s >= s;
    print b0, " ", b1, " ", b2, " ", b3, "\n"; // true true false true

    b0 := s == t;
    b1 := s != t;
    print b0, " ", b1, "\n"; // false true
  }
}
