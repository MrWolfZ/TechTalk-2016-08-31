using System;
using System.Threading.Tasks;
using Functional.Solutions._02Option;
using Functional.Solutions._03Result;

namespace Functional.Solutions._04AsyncResult
{
  public static class AsyncResultExtensions
  {
    public static async Task<TResult> Match<TSuccess, TFailure, TResult>(
      this Task<Result<TSuccess, TFailure>> task,
      Func<TSuccess, TResult> ifSuccess,
      Func<TFailure, TResult> ifFailure)
    {
      var res = await task;
      return res.Match(ifSuccess, ifFailure);
    }

    public static async Task<TResult> Match<TSuccess, TFailure, TResult>(
      this Task<Result<TSuccess, TFailure>> task,
      Func<TSuccess, Task<TResult>> ifSuccess,
      Func<TFailure, Task<TResult>> ifFailure)
    {
      var res = await task;
      return await res.Match(ifSuccess, ifFailure);
    }

    public static Task<Result<TResult, TFailure>> Bind<TSuccess, TFailure, TResult>(
      this Result<TSuccess, TFailure> res,
      Func<TSuccess, Task<Result<TResult, TFailure>>> f) =>
        res.Match(f, err => Task.FromResult(Result.Failure<TResult, TFailure>(err)));

    public static async Task<Result<TResult, TFailure>> Bind<TSuccess, TFailure, TResult>(
      this Task<Result<TSuccess, TFailure>> task,
      Func<TSuccess, Result<TResult, TFailure>> f)
    {
      var res = await task;
      return res.Match(f, err => err);
    }

    public static async Task<Result<TResult, TFailure>> Bind<TSuccess, TFailure, TResult>(
      this Task<Result<TSuccess, TFailure>> task,
      Func<TSuccess, Task<Result<TResult, TFailure>>> f)
    {
      var res = await task;
      return await res.Match(f, err => Task.FromResult(Result.Failure<TResult, TFailure>(err)));
    }

    public static Task<Result<TResult, TFailure>> Map<TSuccess, TFailure, TResult>(this Task<Result<TSuccess, TFailure>> res, Func<TSuccess, Task<TResult>> f)
      => res.Bind<TSuccess, TFailure, TResult>(async o => await f(o));

    public static Task<Result<TResult, TFailure>> Map<TSuccess, TFailure, TResult>(this Result<TSuccess, TFailure> res, Func<TSuccess, Task<TResult>> f)
      => res.Bind<TSuccess, TFailure, TResult>(async o => await f(o));

    public static Task<Result<TResult, TFailure>> Map<TSuccess, TFailure, TResult>(this Task<Result<TSuccess, TFailure>> res, Func<TSuccess, TResult> f)
      => res.Bind<TSuccess, TFailure, TResult>(o => f(o));

    public static Task<Result<TSuccess, TResult>> MapFailure<TSuccess, TFailure, TResult>(
      this Task<Result<TSuccess, TFailure>> res,
      Func<TFailure, Task<TResult>> f)
      => res.Match<TSuccess, TFailure, Result<TSuccess, TResult>>(suc => Task.FromResult(Result.Success<TSuccess, TResult>(suc)), async err => await f(err));

    public static Task<Result<TSuccess, TResult>> MapFailure<TSuccess, TFailure, TResult>(
      this Result<TSuccess, TFailure> res,
      Func<TFailure, Task<TResult>> f)
      =>
        res.Match<TSuccess, TFailure, Task<Result<TSuccess, TResult>>>(
          suc => Task.FromResult(Result.Success<TSuccess, TResult>(suc)),
          async err => await f(err));

    public static Task<Result<TSuccess, TResult>> MapFailure<TSuccess, TFailure, TResult>(
      this Task<Result<TSuccess, TFailure>> res,
      Func<TFailure, TResult> f)
      => res.Match<TSuccess, TFailure, Result<TSuccess, TResult>>(suc => suc, err => f(err));

    public static async Task<Result<TSuccess, TFailure>> Filter<TSuccess, TFailure>(
      this Task<Result<TSuccess, TFailure>> task,
      Func<TSuccess, Option<TFailure>> f)
    {
      var res = await task;
      return res.Filter(f);
    }

    public static async Task<Result<TSuccess, TFailure>> Filter<TSuccess, TFailure>(
      this Result<TSuccess, TFailure> res,
      Func<TSuccess, Task<Option<TFailure>>> f)
    {
      if (!res.IsSuccess)
      {
        return res.Failure;
      }

      var opt = await f(res.Success);
      return opt.Match<TFailure, Result<TSuccess, TFailure>>(err => err, () => res.Success);
    }

    public static async Task<Result<TSuccess, TFailure>> Filter<TSuccess, TFailure>(
      this Task<Result<TSuccess, TFailure>> task,
      Func<TSuccess, Task<Option<TFailure>>> f)
    {
      var res = await task;
      return await res.Filter(f);
    }

    public static async Task<Option<TSuccess>> ToOption<TSuccess, TFailure>(this Task<Result<TSuccess, TFailure>> task)
    {
      var res = await task;
      return res.IsSuccess ? Option.Some(res.Success) : Option.None;
    }
  }
}
