// RUN: %exits-with -any %baredafny verify %args "%S/dfyconfig.toml" > "%t"
// RUN: %diff "%s.expect" "%t"

module X {
  method {:only} VerifyMe() {
    assert false; // Should display an error
  }
  method DontVerifyMe() {
    assert false; // Should not show any error
  }
}