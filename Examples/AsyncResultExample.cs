using System;
using System.Threading.Tasks;
using Examples.Solutions._04AsyncResult;
using Functional;
using Functional.Solutions._02Option;
using Functional.Solutions._03Result;
using Functional.Solutions._04AsyncResult;

namespace Examples
{
  public static class AsyncResultExample
  {
    public static async Task<Unit> Run()
    {
      var bank = Bank.Empty;
      while (true)
      {
        Console.Write("Which operation do you want to perform?\n1. Create Account\n2. Deposit\n3. Withdraw\n4. Show bank\n5. Quit\nAnswer: ");
        var opt = Console.ReadLine()
                         .TryParseInt()
                         .Bind(ChooseOperation(bank));

        if (opt.IsNone)
        {
          break;
        }

        await opt.ToResult("").Map(
          t => t.Match(
            b => bank = b,
            err => Console.Error.WriteLine($"An error occured during the operation: {err}")));
      }

      Console.WriteLine("Ended example with bank in state '{0}'.", bank);
      return Unit.Default;
    }

    private static Func<int, Option<Task<Result<Bank, string>>>> ChooseOperation(Bank bank) => op =>
    {
      switch (op)
      {
        case 1:
          return CreateAccount(bank);
        case 2:
          return Deposit(bank);
        case 3:
          return Withdraw(bank);
        case 4:
          Console.WriteLine(bank);
          return Option.Some(Task.FromResult(Result.Success<Bank, string>(bank)));
        case 5:
          return Option.None;
        default:
          return Option.Some(Task.FromResult(Result.Failure<Bank, string>($"unknown operation: {op}")));
      }
    };

    private static async Task<Result<Bank, string>> CreateAccount(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var res = Console.ReadLine()
                       .TryParseLong()
                       .ToResult("could not parse account ID")
                       .Bind(bank.CreateAccount);

      return (await res).DoIfSuccess(b => Console.WriteLine("Created account!"));
    }

    private static async Task<Result<Bank, string>> Deposit(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var accountIdInput = Console.ReadLine();
      Console.Write("How much do you want to deposit?\nAnswer: ");
      var amountInput = Console.ReadLine();

      var res = TryParseAccountIdAndAmount(accountIdInput, amountInput)
        .Bind(t => bank.Deposit(t.Item1, t.Item2));

      return (await res).DoIfSuccess(b => Console.WriteLine("Deposited amount!"));
    }

    private static async Task<Result<Bank, string>> Withdraw(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var accountIdInput = Console.ReadLine();
      Console.Write("How much do you want to withdraw?\nAnswer: ");
      var amountInput = Console.ReadLine();

      var res = TryParseAccountIdAndAmount(accountIdInput, amountInput)
        .Bind(t => bank.Withdraw(t.Item1, t.Item2));

      return (await res).DoIfSuccess(b => Console.WriteLine("Withdrew amount!"));
    }

    private static Result<Tuple<long, double>, string> TryParseAccountIdAndAmount(string accountId, string amount) =>
      accountId.TryParseLong()
               .ToResult("could not parse account ID")
               .Bind(id => amount.TryParseDouble().ToResult("could not parse amount").Map(a => Tuple.Create(id, a)));
  }
}