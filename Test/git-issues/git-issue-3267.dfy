// RUN: %exits-with 1 %baredafny zzzz "%s" > "%t"
// RUN: %exits-with 1 %baredafny -zzzz "%s" >> "%t"
// RUN: %exits-with 1 %baredafny test.d "%s" >> "%t"
// RUN: %exits-with 1 %baredafny resolve --use-basename-for-filename --zzzz "%s" >> "%t"
// RUN: %exits-with 1 %baredafny resolve --use-basename-for-filename test "%s" >> "%t"
// RUN: %exits-with 1 %baredafny resolve --use-basename-for-filename test.d "%s" >> "%t"
// RUN: %exits-with 1 %baredafny /useBaseNameForFileName test.d "%s" >> "%t"
// RUN: %exits-with 1 %baredafny /useBaseNameForFileName /zzzz "%s" >> "%t"
// RUN: %exits-with 1 %baredafny /useBaseNameForFileName -zzzz "%s" >> "%t"
// RUN: %diff "%s.expect" "%t"

module A{}
