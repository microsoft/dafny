// RUN: ! %baredafny run %args --target=cs --unicode-char=1 "%s" > "%t"
// RUN: ! %baredafny run %args --target=go --unicode-char=1 "%s" >> "%t"
// RUN: ! %baredafny run %args --target=java --unicode-char=1 "%s" >> "%t"
// RUN: ! %baredafny run %args --target=js --unicode-char=1 "%s" >> "%t"
// RUN: ! %baredafny run %args --target=py --unicode-char=1 "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"
include "../../expectations/ExpectWithMessage.dfy"
