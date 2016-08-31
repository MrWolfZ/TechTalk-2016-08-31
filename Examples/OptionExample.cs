using System;
using Examples.Templates._02Option;
using Functional;
using Functional.Solutions._02Option;

namespace Examples
{
  public static class OptionExample
  {
    public static Unit Run()
    {
      var bank = Bank.Empty;
      while (true)
      {
        Console.Write("Which operation do you want to perform?\n1. Create Account\n2. Deposit\n3. Withdraw\n4. Show bank\n5. Quit\nAnswer: ");
        var res = Console.ReadLine()
                         .TryParseInt()
                         .Bind(ChooseOperation(bank));

        if (res.IsNone)
        {
          break;
        }

        bank = res.GetOrElse(bank)
                  .DoIfNone(() => Console.WriteLine("An error occured during the operation!"))
                  .GetOrElse(bank);
      }

      Console.WriteLine("Ended example with bank in state '{0}'.", bank);
      return Unit.Default;
    }

    private static Func<int, Option<Option<Bank>>> ChooseOperation(Bank bank) => op =>
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
          return Option.Some(bank);
        case 5:
          return Option.None;
        default:
          return Option.Some<Option<Bank>>(Option.None);
      }
    };

    private static Option<Bank> CreateAccount(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      return Console.ReadLine()
                    .TryParseLong()
                    .Bind(bank.CreateAccount)
                    .DoIfSome(b => Console.WriteLine("Created account!"));
    }

    private static Option<Bank> Deposit(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var accountIdInput = Console.ReadLine();
      Console.Write("How much do you want to deposit?\nAnswer: ");
      var amountInput = Console.ReadLine();

      return TryParseAccountIdAndAmount(accountIdInput, amountInput)
        .Bind(t => bank.Deposit(t.Item1, t.Item2))
        .DoIfSome(b => Console.WriteLine("Deposited amount!"));
    }

    private static Option<Bank> Withdraw(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var accountIdInput = Console.ReadLine();
      Console.Write("How much do you want to withdraw?\nAnswer: ");
      var amountInput = Console.ReadLine();

      return TryParseAccountIdAndAmount(accountIdInput, amountInput)
        .Bind(t => bank.Withdraw(t.Item1, t.Item2))
        .DoIfSome(b => Console.WriteLine("Withdrew amount!"));
    }

    private static Option<Tuple<long, double>> TryParseAccountIdAndAmount(string accountId, string amount) =>
      accountId.TryParseLong().Bind(id => amount.TryParseDouble().Map(a => Tuple.Create(id, a)));
  }
}