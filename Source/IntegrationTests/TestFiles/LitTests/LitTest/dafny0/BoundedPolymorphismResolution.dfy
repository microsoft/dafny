// RUN: %testDafnyForEachResolver --expect-exit-code=2 "%s" -- --general-traits=datatype

module BadTypeNames {
  datatype DT<W extends Unknown> = Dt(w: W) // error: what is Unknown?

  type Syn<X extends Unknown> = DT<X> // error: what is Unknown?

  class Class<Y extends Unknown> { // error: what is Unknown?
  }

  trait AnotherTrait<Z extends Unknown> { // error: what is Unknown?
  }

  type Abstract<V extends Unknown> // error: what is Unknown?

  trait Generic<Q> { }

  codatatype Mutual<A extends Generic<Z>, B extends Generic<A>> = More(Mutual) // error: what is Z?

  function Function<K extends Unknown>(k: K): int { 2 } // error: what is Unknown?

  method Method<L extends Unknown>(l: L) { } // error: what is Unknown?

  least predicate Least<M extends Unknown>(m: M) { true } // error: what is Unknown?

  greatest lemma Greatest<N extends Unknown>(n: N) { } // error: what is Unknown?

  iterator Iter<O extends Unknown>(o: O) { } // error: what is Unknown?
}

module AsIsResolve0 {
  trait Trait extends object {
    const n: int
  }
  trait AnotherTrait { }

  method M<X extends Trait>(x: X) returns (b: bool, t: Trait, u: int) {
    t := x; // (legacy error)
    b := x is Trait; // (legacy error)
    b := x is AnotherTrait; // error: AnotherTrait is not compatible with X
    b := x is object; // (legacy error)
    t := x as Trait; // (legacy error)
    u := t.n;
    assert x == t; // (legacy error)
  }
}

module AsIsResolve1 {
  trait Trait {
    const n: int
  }

  method Q<Y(==) extends Trait>(y: Y) returns (t: Trait, u: int) {
    t := y as Trait; // (legacy error)
    u := (y as Trait).n; // (legacy error)
    u := y.n; // error: y needs to be cast to Trait before .n can be accessed
  }

  method Nullable<Z extends object>(z: Z) returns (b: bool) {
    var n;
    n := z;
    b := (z as object) as object? == null; // (legacy error)
    n := null; // error: although n is a reference type, there is no type Z?
  }

  method T<Z extends object?>(z: Z) {
    var n;
    n := z;
    n := null; // error: although n extends object?, Z may have constraints that exclude "null"
  }

  trait XTrait<X> { }

  method U<T extends XTrait<int>>(t: T) {
    var a := t as XTrait<int>; // (legacy error)
    var b := t as XTrait<bool>; // error: non-compatible types
  }
}
  
module AsIsResolve2 {
  trait Trait {
    const n: int
  }

  method A<X(==) extends Trait>(x: X, t: Trait) returns (b: bool) {
    b := x is X;
    b := x is Trait; // (legacy error)
    b := t is X; // error: not allowed in compiled code
    b := t is Trait;
  }

  method B<X(==) extends Trait>(x: X, t: Trait) returns (ghost g: bool, b: bool) {
    b := x is X;
    b := x is Trait; // (legacy error)
    g := t is X; // (legacy error)
    b := t is Trait;
  }

  method N<X extends Trait>(x: X) returns (b: bool, t: Trait) {
    t := x as Trait; // (legacy error)
    assert x == t; // (legacy error)
    ghost var gb := t is X; // (legacy error)
    b := t is X; // error: this is not allowed in compiled code
  }
}

module AsIsResolve3 {
  trait Trait {
    const n: int
  }

  method P<Y extends Trait>(y: Y) returns (b: bool, t: Trait) {
    t := y as Trait; // (legacy error)
    assert y == t; // (legacy error)
    b := y == t; // error: we don't know if y supports equality
  }

  method R<Y(==) extends Trait>(y: Y) returns (b: bool, t: Trait) {
    t := y as Trait; // (legacy error)
    b := y == t; // error: Trait not known to support equality
  }

  method S<Y extends object>(y: Y) returns (b: bool, obj: object) {
    obj := y as object; // (legacy error)
    b := y == obj; // error: Y not known to support equality
  }

  method Finally<Y(==) extends object>(y: Y) returns (b: bool, obj: object) {
    obj := y as object; // (legacy error)
    b := y == obj; // both Y and object support equality, so we're good // (legacy error)
  }
}

module AsIsResolve4 {
  trait Trait { }
  class Class extends Trait { }
  
  method M<X extends Trait>(x: X) {
    var a0 := x as X; // (legacy error)
    var a1 := x as Trait; // (legacy error)
    var a2 := x as Class; // error
    var a3 := x as int; // error
  }
}

module BoundMustBeTrait {
  trait Trait extends object {
    const n: int
  }

  datatype Color = Brown | Peach
  class Class { }
  type Synonym = Trait
  type Subset = t: Trait | t.n == 8 witness *

  datatype D0<W extends int> = Make(w: W) // error: bound must be a trait
  datatype D1<W extends array<Trait>> = Make(w: W) // error: bound must be a trait
  datatype D2<W extends Color> = Make(w: W) // error: bound must be a trait
  datatype D3<W extends Class?> = Make(w: W) // error: bound must be a trait
  datatype D4<W extends Synonym> = Make(w: W)
  datatype D5<W extends Subset> = Make(w: W)
  datatype D6<W extends Trait?> = Make(w: W)
  datatype D7<W extends Trait> = Make(w: W)
}

module Refinement {
  module AA {
    trait Trait { }
    trait GenericTrait<X> { }

    type SoonSubsetType0<A>
    type SoonSubsetType1<A>
    type SoonSubsetType2<A extends Trait>

    type AbstractType0<A>
    type AbstractType1<A extends Trait>
    type AbstractType2<A>

    type AbstractType3<A extends Trait extends object>
    type AbstractType4<A extends Trait extends object>
    type AbstractType5<A extends Trait extends object>
    type AbstractType6<A extends Trait extends object>

    type AbstractType7<A extends GenericTrait<B>, B extends GenericTrait<A>>

    type ToBeReplaced0<A extends Trait>
    type ToBeReplaced1<A extends Trait>
    type ToBeReplaced2<A extends Trait>
    type ToBeReplaced3<A extends Trait>
    type ToBeReplaced4<A extends Trait>
    type ToBeReplaced5<A extends Trait>

    method M0<A extends Trait>(u: int)
    method M1<A extends Trait>(u: int)
  }

  module BB refines AA {
    type SoonSubsetType0<B> = int // here, the name is allowed to be changed
    type SoonSubsetType1<B extends Trait> = int // error: name change not allowed
    type SoonSubsetType2<B> = int // error: name change not allowed

    type AbstractType0<B> // error: name change not allowed
    type AbstractType1<B> // error: name change not allowed
    type AbstractType2<B extends Trait> // error: name change not allowed

    type AbstractType3<A extends Trait> // error: wrong number of type bounds
    type AbstractType4<A extends Trait extends object>
    type AbstractType5<A extends Trait extends Trait> // error: mismatched bound
    type AbstractType6<A extends object extends Trait> // error (x2): the order has to be the same (because the checking is rather simplistic)

    type AbstractType7<A extends GenericTrait<B>, B extends GenericTrait<A>>

    datatype ToBeReplaced0<A extends Trait> = Record(a: A)
    datatype ToBeReplaced1<A extends object> = Record(a: A) // error: mismatched bound
    type ToBeReplaced2<A extends Trait> = int
    type ToBeReplaced3<A> = int // error: mismatched bound
    class ToBeReplaced4<A extends Trait> { }
    class ToBeReplaced5<A extends object> { } // error: mismatched bound

    method M0<A extends object>(u: int) // error: mismatched bound
    method M1<A extends Trait>(u: int)
  }
}

module CheckArguments {
  trait Trait<X> {
    function Value(): X
  }

  datatype Dt extends Trait<string> = Dt(s: string)
  {
    function Value(): string { s }
  }

  class RandomClass<R> extends Trait<R> {
    const r: R
    constructor (r: R) {
      this.r := r;
    }
    function Value(): R { r }
  }

  method MyMethod<Y extends Trait<string>>(y0: Y)
  function MyFunction<Y extends Trait<string>>(y1: Y): int
  class MyClass<Y extends Trait<string>> {
    constructor (y2: Y)
  }

  method Test() {
    var m := new RandomClass(3.14);
    MyMethod(m); // error: type parameter is RandomClass<real>, which is not a Trait<string> as required by type bound

    var n := new RandomClass('x');
    var _ := MyFunction(n); // error: type parameter is RandomClass<char>, which is not a Trait<string> as required by type bound

    var o := new RandomClass(500);
    var oo := new MyClass(o); // error: type parameter is RandomClass<int>, which is not a Trait<string> as required by type bound
  }
}

module Overrides {
  trait Base {
    function F<X extends object>(): int
    method M<X extends object>()
  }

  class Class extends Base {
    function F<X extends object extends object>(): int { 2 } // error: number of bounds is different
    method M<X extends object?>() { } // error: bound is different
  }

  type ObjectSynonym = object

  type AbstractType extends Base {
    function F<Y extends object>(): int { 2 } // error: type parameter has been renamed
    method M<X extends ObjectSynonym>() { } // the synonym here is okay
  }

  // ---

  trait GenericBound<Z> { }

  trait Parent {
    function F<X extends GenericBound<Y>, Y extends GenericBound<X>>(): int
    method M<X extends GenericBound<Y>, Y extends GenericBound<X>>()
    greatest lemma L<X extends GenericBound<int>, Y extends IntGenericBound>()
  }

  type IntGenericBound = GenericBound<int>

  codatatype SomeGood extends Parent = More
  {
    function F<X extends GenericBound<Y>, Y extends GenericBound<X>>(): int { 2 }
    method M<X extends GenericBound<Y>, Y extends GenericBound<int>>() { } // error: bound of Y is different
    greatest lemma L<X extends IntGenericBound, Y extends GenericBound<int>>() { }
  }

  datatype CoSomeGood extends Parent = More
  {
    function F<Y extends GenericBound<X>, X extends GenericBound<Y>>(): int { 2 } // error (x2): type parameters have been renamed
    method M<X extends IntGenericBound, Y extends GenericBound<X>>() { } // error: bound is different
    greatest lemma L<X extends GenericBound<X>, Y extends IntGenericBound>() { } // error: bound is different
  }

  trait AnotherTrait extends Parent {
    greatest lemma L<X extends GenericBound<int>, Y extends GenericBound<X>>() { } // error: bound is different
  }
}

module VariousBounds {
  trait Parent { }
  trait XTrait extends Parent { }
  trait YTrait extends Parent { }

  datatype Dt<X extends XTrait> = Dt(x: X)
  {
    method M<Y extends YTrait>(y: Y) {
      var parent: Parent;
      parent := x; // (legacy error)
      assert parent is XTrait; // (legacy error)
      parent := y; // (legacy error)
      assert parent is YTrait; // (legacy error)
    }
  }
}
