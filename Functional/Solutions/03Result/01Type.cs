namespace Functional.Solutions._03Result
{
  public static class Result
  {
    public static Result<TSuccess, TFailure> Success<TSuccess, TFailure>(TSuccess t) => new Result<TSuccess, TFailure>(t);
    public static Result<TSuccess, TFailure> Failure<TSuccess, TFailure>(TFailure err) => new Result<TSuccess, TFailure>(err);
  }

  public struct Result<TSuccess, TFailure>
  {
    public Result(TSuccess result)
    {
      this.IsSuccess = true;
      this.Success = result;
      this.Failure = default(TFailure);
    }

    public Result(TFailure failure)
    {
      this.IsSuccess = false;
      this.Success = default(TSuccess);
      this.Failure = failure;
    }

    public bool IsSuccess { get; }

    internal TFailure Failure { get; }
    internal TSuccess Success { get; }

    public static implicit operator Result<TSuccess, TFailure>(TSuccess res) => Result.Success<TSuccess, TFailure>(res);
    public static implicit operator Result<TSuccess, TFailure>(TFailure err) => Result.Failure<TSuccess, TFailure>(err);
  }
}
