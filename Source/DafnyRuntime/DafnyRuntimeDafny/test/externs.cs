using System;
using System.Numerics;

namespace AtomicBoxes {

  public class AtomicBox<T> where T : class {

    private volatile T value;

    public static AtomicBox<T> Make(T value) {
      var result = new AtomicBox<T>();
      result.Put(value);
      return result;
    }

    public T Get() => value;
    public void Put(T t) => value = t;
  }
}

namespace Arrays {

  public class __default {
    public static Arrays.Array<T> NewArray<T>(BigInteger length) {
      return new CsharpArray<T>((int)length);
    }
  }

  public class CsharpArray<T> : Arrays.Array<T>, Arrays.ImmutableArray<T> {
    private readonly T[] values;
    private readonly int length;

    public CsharpArray(int length) {
      this.values = new T[length];
      this.length = length;
    }
    
    public CsharpArray(T[] values, int length) {
      this.values = values;
      this.length = length;
    }

    public BigInteger Length() {
      return length;
    }

    public T Read(BigInteger i) {
      return values[(int)i];
    }
    
    public T At(BigInteger i) {
      return values[(int)i];
    }

    public void Write(BigInteger i, T t) {
      values[(int)i] = t;
    }

    public void WriteRangeArray(BigInteger start, Arrays.ImmutableArray<T> other) {
      var csharpArray = other as CsharpArray<T>;
      Array.Copy(csharpArray.values, 0, values, (int)start, csharpArray.length);
    }

    public Arrays.ImmutableArray<T> Freeze(BigInteger size) {
      return new CsharpArray<T>(values, (int)size);
    }
  }
}