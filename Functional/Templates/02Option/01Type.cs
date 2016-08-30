namespace Functional.Templates._02Option
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
  }
}
