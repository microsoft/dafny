namespace CompilerBuilder;

public interface ParseResult;

public interface ParseResult<T> : ParseResult {
  internal ParseResult<TU> Continue<TU>(Func<ConcreteSuccess<T>, ParseResult<TU>> f);

  internal ConcreteResult<T>? Concrete => Success as ConcreteResult<T> ?? Failure;

  public ConcreteSuccess<T>? Success { get; }
  public ConcreteSuccess<T> ForceSuccess {
    get {
      if (Success != null) {
        return Success;
      }

      throw new InvalidOperationException(Failure!.Message + $", at ({Failure.Location.Line},{Failure.Location.Column})");
    }
  }
  
  public FailureResult<T>? Failure { get; }
  internal IEnumerable<IFoundRecursion<T>> Recursions { get; }
  
  internal ParseResult<T> Combine(ParseResult<T> other) {
    ConcreteSuccess<T>? concreteSuccess = null;
    if (Success != null && other.Success != null) {
      concreteSuccess = Success.Remainder.Offset > other.Success.Remainder.Offset ? Success : other.Success;
    }

    concreteSuccess ??= Success ?? other.Success;

    FailureResult<T>? failure = null;
    if (Failure != null && other.Failure != null) {
      failure = Failure.Location.Offset >= other.Failure.Location.Offset ? Failure : other.Failure;
    }

    failure ??= Failure ?? other.Failure;
    if (concreteSuccess != null && failure != null && failure.Location.Offset <= concreteSuccess.Remainder.Offset) {
      // TODO consider keeping the failure if it's at the same offset, because it might have been how you wanted to continue
      failure = null;
    }

    return new Aggregate<T>(concreteSuccess, failure, Recursions.Concat(other.Recursions));
  }
}

public interface ConcreteResult<T> : ParseResult<T>;

internal record Aggregate<T>(ConcreteSuccess<T>? Success, 
  FailureResult<T>? Failure, 
  IEnumerable<IFoundRecursion<T>> Recursions) : ParseResult<T> {
  public ParseResult<U> Continue<U>(Func<ConcreteSuccess<T>, ParseResult<U>> f) {
    var newRecursions = Recursions.Select(r => (IFoundRecursion<U>)r.Continue(f));
    var newFailure = Failure == null ? null : new FailureResult<U>(Failure.Message, Failure.Location);
    var noConcrete = new Aggregate<U>(null, newFailure, newRecursions);
    if (Success != null) {
      var concreteResult = Success.Continue(f);
      return concreteResult.Combine(noConcrete);
    }

    return noConcrete;
  }
}

public record ConcreteSuccess<T>(T Value, ITextPointer Remainder) : ConcreteResult<T> {
  public ParseResult<TB> Continue<TB>(Func<ConcreteSuccess<T>, ParseResult<TB>> f) {
    var result = f(this);
    return result;
  }

  public ConcreteSuccess<T>? Success => this;
  public FailureResult<T>? Failure => null;
  IEnumerable<IFoundRecursion<T>> ParseResult<T>.Recursions => [];
}

interface IFoundRecursion<T> : ParseResult<T> {
  Parser Parser { get; }
  ParseResult<T> Apply(object value, ITextPointer remainder);
}

/// <summary>
/// Parsers that do not have recursive children have the same result regardless of whether they're a recursion on the stack.
/// A parser that has a recursive child, will have a different result if that child is already on its stack.
///
/// TODO do a little analysis to see how often a particular parser is in a cache with different seens
/// 
/// </summary>
record FoundRecursion<TA, TB>(Parser Parser, Func<ConcreteSuccess<TA>, ParseResult<TB>> Recursion) : IFoundRecursion<TB> {
  public ParseResult<TC> Continue<TC>(Func<ConcreteSuccess<TB>, ParseResult<TC>> f) {
    return new FoundRecursion<TA, TC>(Parser, concrete => {
      var inner = Recursion(concrete);
      var continued = inner.Continue(f);
      return continued;
    });
  }

  public ConcreteSuccess<TB>? Success => null;
  public FailureResult<TB>? Failure => null;
  public IEnumerable<IFoundRecursion<TB>> Recursions => new IFoundRecursion<TB>[]{ this };

  public ParseResult<TB> Apply(object value, ITextPointer remainder) {
    return Recursion(new ConcreteSuccess<TA>((TA)value, remainder));
  }
}

public record FailureResult<T> : ConcreteResult<T> {
  public FailureResult(string Message, ITextPointer Location) {
    this.Message = Message;
    this.Location = Location;
  }

  public FailureResult<U> Cast<U>() {
    return new FailureResult<U>(Message, Location);
  }

  public ParseResult<TU> Continue<TU>(Func<ConcreteSuccess<T>, ParseResult<TU>> f) {
    return new FailureResult<TU>(Message, Location);
  }

  public ConcreteSuccess<T>? Success => null;
  public FailureResult<T> Failure => this;

  IEnumerable<IFoundRecursion<T>> ParseResult<T>.Recursions => [];
  public string Message { get; init; }
  public ITextPointer Location { get; init; }

  public void Deconstruct(out string Message, out ITextPointer Location) {
    Message = this.Message;
    Location = this.Location;
  }
}