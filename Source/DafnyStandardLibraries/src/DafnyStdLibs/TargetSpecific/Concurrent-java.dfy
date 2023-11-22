module {:extern "DafnyStdLibsExterns", "Concurrent"} DafnyStdLibs.Concurrent refines ConcurrentInterface {

  class {:extern} MutableMap<K(==), V(==)> ... {

    constructor {:extern} {:axiom} (ghost inv: (K, V) -> bool)
      ensures this.inv == inv

    ghost predicate Valid()
    {
      true
    }

    method {:extern} {:axiom} Keys() returns (keys: set<K>)

    method {:extern} {:axiom} HasKey(k: K) returns (used: bool)

    method {:extern} {:axiom} Values() returns (values: set<V>)

    method {:extern} {:axiom} Items() returns (items: set<(K,V)>)

    method {:extern} {:axiom} Put(k: K, v: V)

    method {:extern} {:axiom} Get(k: K) returns (r: Option<V>)

    method {:extern} {:axiom} Remove(k: K)

    method {:extern} {:axiom} Size() returns (c: nat)

  }

  class {:extern} AtomicBox<T> ... {

    constructor {:extern} {:axiom} (ghost inv: T -> bool, t: T)
      requires inv(t)
      ensures this.inv == inv

    ghost predicate Valid() { true }

    method {:extern} {:axiom} Get() returns (t: T)

    method {:extern} {:axiom} Put(t: T)

  }

  class {:extern "Lock"} Lock ... {

    constructor {:extern} () {}

    method {:extern} Lock() {}

    method {:extern} Unlock() {}

  }
}