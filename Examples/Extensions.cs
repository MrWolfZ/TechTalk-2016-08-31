using Functional.Solutions._02Option;

namespace Examples
{
  public static class Extensions
  {
    public static Option<int> TryParseInt(this string input)
    {
      int result;
      if (int.TryParse(input, out result))
      {
        return result;
      }

      return Option.None;
    }

    public static Option<long> TryParseLong(this string input)
    {
      long result;
      if (long.TryParse(input, out result))
      {
        return result;
      }

      return Option.None;
    }

    public static Option<double> TryParseDouble(this string input)
    {
      double result;
      if (double.TryParse(input, out result))
      {
        return result;
      }

      return Option.None;
    }
  }
}
