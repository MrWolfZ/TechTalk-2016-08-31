using System;
using Functional.Solutions._02Option;

namespace Functional.Solutions._03Result
{
  public static class ResultExtensions
  {
    public static TResult Match<TSuccess, TFailure, TResult>(
      this Result<TSuccess, TFailure> res,
      Func<TSuccess, TResult> ifSuccess,
      Func<TFailure, TResult> ifFailure)
      => res.IsSuccess ? ifSuccess(res.Success) : ifFailure(res.Failure);

    public static void Match<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Action<TSuccess> ifSuccess, Action<TFailure> ifFailure)
    {
      if (res.IsSuccess)
      {
        ifSuccess(res.Success);
        return;
      }

      ifFailure(res.Failure);
    }

    public static Result<TSuccess, TFailure> IfSuccess<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Action<TSuccess> ifSuccess)
    {
      if (res.IsSuccess)
      {
        ifSuccess(res.Success);
      }

      return res;
    }

    public static TSuccess IfFailure<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Func<TFailure, TSuccess> ifFailure)
      => res.IsSuccess ? res.Success : ifFailure(res.Failure);

    public static TSuccess IfFailure<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, TSuccess ifFailure) => res.IfFailure(err => ifFailure);

    public static Result<TSuccess, TFailure> IfFailure<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Action<TFailure> ifFailure)
    {
      if (!res.IsSuccess)
      {
        ifFailure(res.Failure);
      }

      return res;
    }

    public static Result<TResult, TFailure> Bind<TSuccess, TFailure, TResult>(this Result<TSuccess, TFailure> res, Func<TSuccess, Result<TResult, TFailure>> f)
      => res.Match(f, err => err);

    public static Result<TResult, TFailure> Map<TSuccess, TFailure, TResult>(this Result<TSuccess, TFailure> res, Func<TSuccess, TResult> f)
      => res.Bind<TSuccess, TFailure, TResult>(o => f(o));

    public static Result<TSuccess, TResult> MapFailure<TSuccess, TFailure, TResult>(this Result<TSuccess, TFailure> res, Func<TFailure, TResult> f)
      => res.Match<TSuccess, TFailure, Result<TSuccess, TResult>>(suc => suc, err => f(err));

    public static Result<TSuccess, TFailure> Filter<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Func<TSuccess, Option<TFailure>> f)
      => res.Bind<TSuccess, TFailure, TSuccess>(o => f(o).IfNone(res.Failure));

    public static Option<TSuccess> ToOption<TSuccess, TFailure>(this Result<TSuccess, TFailure> res)
      => res.IsSuccess ? Option.Some(res.Success) : Option.None;
  }
}
