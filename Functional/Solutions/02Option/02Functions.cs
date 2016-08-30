using System;
using Functional.Solutions._03Result;

namespace Functional.Solutions._02Option
{
  public static class OptionExtensions
  {
    public static TResult Match<TOption, TResult>(this Option<TOption> opt, Func<TOption, TResult> ifSome, Func<TResult> ifNone)
      => opt.IsSome ? ifSome(opt.Value) : ifNone();

    public static void Match<TOption>(this Option<TOption> opt, Action<TOption> ifSome, Action ifNone)
    {
      if (opt.IsSome)
      {
        ifSome(opt.Value);
        return;
      }

      ifNone();
    }

    public static Option<TOption> DoIfSome<TOption>(this Option<TOption> opt, Action<TOption> ifSome)
    {
      if (opt.IsSome)
      {
        ifSome(opt.Value);
      }

      return opt;
    }

    public static Option<TOption> DoIfNone<TOption>(this Option<TOption> opt, Action ifNone)
    {
      if (!opt.IsSome)
      {
        ifNone();
      }

      return opt;
    }

    public static TOption GetOrElse<TOption>(this Option<TOption> opt, Func<TOption> ifNone)
      => opt.IsSome ? opt.Value : ifNone();

    public static TOption GetOrElse<TOption>(this Option<TOption> opt, TOption ifNone) => opt.GetOrElse(() => ifNone);

    public static Option<TResult> Bind<TSource, TResult>(this Option<TSource> opt, Func<TSource, Option<TResult>> f)
      => opt.Match(f, () => Option.None);

    public static Option<TResult> Map<TSource, TResult>(this Option<TSource> opt, Func<TSource, TResult> f)
      => opt.Bind(o => Option.Some(f(o)));

    public static Option<TSource> Filter<TSource>(this Option<TSource> opt, Func<TSource, bool> f)
      => opt.Bind(o => f(o) ? Option.Some(o) : Option.None);

    public static Result<TOption, TFailure> ToResult<TOption, TFailure>(this Option<TOption> opt, Func<TFailure> ifNone) =>
      opt.Match<TOption, Result<TOption, TFailure>>(res => res, () => ifNone());

    public static Result<TOption, TFailure> ToResult<TOption, TFailure>(this Option<TOption> opt, TFailure ifNone) =>
      opt.ToResult(() => ifNone);
  }
}
