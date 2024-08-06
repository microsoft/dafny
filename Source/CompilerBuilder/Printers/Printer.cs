﻿// See https://aka.ms/new-console-template for more information

namespace CompilerBuilder;

record IndentW<T>(Printer<T> Inner, int Amount) : Printer<T> {
  public Document? Print(T value) {
    var innerValue = Inner.Print(value);
    if (innerValue != null) {
      return new IndentD(innerValue, Amount);
    }

    return null;
  }
}
public interface Printer<in T> : Printer {

  public Document? Print(T value);
}

class FailW<T> : Printer<T> {
  public Document? Print(T value) {
    return null;
  }
}

class RecursiveW<T>(Func<Printer<T>> get) : Printer<T> {

  public Printer<T>? inner;
  public Printer<T> Inner => inner ??= get();
  
  public Document? Print(T value) {
    return Inner.Print(value);
  }
}

class ChoiceW<T>(Printer<T> first, Printer<T> second): Printer<T> {
  public Document? Print(T value) {
    return first.Print(value) ?? second.Print(value);
  }
}

class Cast<T, U>(Printer<T> printer) : Printer<U> {
  public Document? Print(U value) {
    if (value is T t) {
      return printer.Print(t);
    }

    return null;
  }
}

public interface Printer;

public interface VoidPrinter : Printer {
  
  public Document Print();
}

class EmptyW : VoidPrinter {
  public static readonly EmptyW Instance = new();

  private EmptyW() { }
  public Document Print() {
    return new Empty();
  }
}

class VerbatimW : Printer<string> {
  public static readonly Printer<string> Instance = new VerbatimW();

  private VerbatimW() { }
  public Document Print(string value) {
    return new Verbatim(value);
  }
}

class MapW<T, U>(Printer<T> printer, Func<U, T?> map) : Printer<U> {
  public Document? Print(U value) {
    var newValue = map(value);
    if (newValue == null) {
      return null;
    }

    return printer.Print(newValue);
  }
}

class IgnoreW<T>(VoidPrinter printer) : Printer<T> {
  public Document Print(T value) {
    return printer.Print();
  }
}

// TODO rename TextW and VerbatimW to make the difference more clear?
internal class TextW(string value) : VoidPrinter {
  public Document Print() {
    return new Verbatim(value);
  }
}

// TODO replace by map and VerbatimW?
internal class NumberW : Printer<int> {
  public Document? Print(int value) {
    return new Verbatim(value.ToString());
  }
}

class SequenceW<TFirst, TSecond, T>(Printer<TFirst> first, Printer<TSecond> second, 
  Func<T, (TFirst, TSecond)?> destruct, Orientation orientation) : Printer<T> {
  public Document? Print(T value) {
    var t = destruct(value);
    if (t == null) {
      return null;
    }

    var (firstValue, secondValue) = t.Value;
    var firstDoc = first.Print(firstValue);
    var secondDoc = second.Print(secondValue);
    if (firstDoc == null || secondDoc == null) {
      return null;
    }

    return firstDoc.Then(secondDoc, orientation);
  }
}

class SkipLeftW<T>(VoidPrinter first, Printer<T> second, Orientation orientation) : Printer<T> {
  public Document? Print(T value) {
    var secondValue = second.Print(value);
    if (secondValue == null) {
      return null;
    }
    return first.Print().Then(secondValue, orientation);
  }
}

class SkipRightW<T>(Printer<T> first, VoidPrinter second, Orientation orientation) : Printer<T> {
  public Document? Print(T value) {
    var firstValue = first.Print(value);
    if (firstValue == null) {
      return null;
    }
    return firstValue.Then(second.Print(), orientation);
  }
}

public static class PrinterExtensions {
  public static Printer<U> Map<T, U>(this Printer<T> printer, Func<U,T?> map) {
    return new MapW<T, U>(printer, map);
  }
}