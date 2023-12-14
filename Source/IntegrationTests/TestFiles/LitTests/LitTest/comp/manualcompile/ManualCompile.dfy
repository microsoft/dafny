// NONUNIFORM: Highly target language specific

// RUN: %translate cs --include-runtime "%s" > "%t"
// RUN: dotnet build %S/ManualCompile.csproj
// RUN: dotnet %S/bin/Debug/net6.0/ManualCompile.dll >> "%t"

// RUN: %translate js --include-runtime "%s" >> "%t"
// RUN: node %S/ManualCompile.js >> "%t"

// RUN: %translate go --include-runtime "%s" >> "%t"
// RUN: env GO111MODULE=auto GOPATH=%S/ManualCompile-go go run %S/ManualCompile-go/src/ManualCompile.go >> "%t"

// RUN: %translate java --include-runtime "%s" >> "%t"
// RUN: javac %S/ManualCompile-java/ManualCompile.java %S/ManualCompile-java/**/*.java
// RUN: java -cp %S/ManualCompile-java ManualCompile >> "%t"

// RUN: %translate cpp --unicode-char=false --include-runtime "%s" >> "%t"
// RUN: g++ -g -Wall -Wextra -Wpedantic -Wno-unused-variable -std=c++17 -I %binaryDir -o %S/ManualCompile.exe %S/ManualCompile.cpp
// RUN: %S/ManualCompile.exe >> "%t"

// RUN: %translate py --include-runtime "%s" >> "%t"
// RUN: python3 %S/ManualCompile-py >> "%t"

// RUN: %diff "%s.expect" "%t"

method Main() {
  print "hello, Dafny\n";
}
