﻿namespace Functional.Solutions._02Option
{
  public static class Option
  {
    public static NoneImpl None = new NoneImpl();
    public static Option<T> Some<T>(T t) => new Option<T>(t);

    public struct NoneImpl
    {
    }
  }

  public struct Option<T>
  {
    internal T Value { get; }

    public Option(T value)
    {
      this.IsSome = true;
      this.Value = value;
    }

    public bool IsSome { get; }
    public bool IsNone => !this.IsSome;

    // implicit conversions for ease of use
    public static implicit operator Option<T>(T res) => Option.Some(res);
    public static implicit operator Option<T>(Option.NoneImpl o) => default(Option<T>);
  }
}
