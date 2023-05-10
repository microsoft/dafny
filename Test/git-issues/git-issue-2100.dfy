// RUN: %dafny /compile:0 "%s" > "%t"
// RUN: %diff "%s.expect" "%t"

module Library {
  export
    provides Cl

  trait Tr { }
  class Cl extends Tr { }
}

module Client {
  import Library

  // The mention of the export-provided Cl below (in the case where
  // its parent type, Tr, is not exported) once generated malformed
  // Boogie. In particular, Cl should be treated as an abstract type in
  // this Client module, but in some places in the translation to Boogie,
  // the type was still treated as a non-abstract type.
  method Test(cl: Library.Cl) {
  }
}
