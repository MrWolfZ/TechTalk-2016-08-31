using System;
using Functional.Solutions._02Option;

namespace Functional.Templates._03Result
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

    public static Result<TSuccess, TFailure> DoIfSuccess<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Action<TSuccess> ifSuccess)
    {
      if (res.IsSuccess)
      {
        ifSuccess(res.Success);
      }

      return res;
    }

    public static Result<TSuccess, TFailure> DoIfFailure<TSuccess, TFailure>(this Result<TSuccess, TFailure> res, Action<TFailure> ifFailure)
    {
      if (!res.IsSuccess)
      {
        ifFailure(res.Failure);
      }

      return res;
    }

    // Bind
    public static Result<TResult, TFailure> Bind<TSuccess, TFailure, TResult>(this Result<TSuccess, TFailure> res, Func<TSuccess, Result<TResult, TFailure>> f)
    {
      throw new NotImplementedException();
    }

    // Map

    // MapFailure

    // Filter

    public static Option<TSuccess> ToOption<TSuccess, TFailure>(this Result<TSuccess, TFailure> res)
      => res.IsSuccess ? Option.Some(res.Success) : Option.None;
  }
}
