using System;
using Examples.Solutions._02Option;
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
        Console.Write("Which operation do you want to perform?\n1. Create Account\n2. Deposit\n3. Withdraw\n4. Quit\nAnswer: ");
        var res = Console.ReadLine()
                         .TryParseInt()
                         .Bind(ChooseOperation(bank));

        bank = res.IfNone(bank);

        if (res.IsNone)
        {
          break;
        }
      }

      Console.WriteLine("Ended example with bank in state '{0}'.", bank);
      return Unit.Default;
    }

    private static Func<int, Option<Bank>> ChooseOperation(Bank bank) => op =>
    {
      switch (op)
      {
        case 1:
          return Option.Some(CreateAccount(bank));
        case 2:
          return Option.Some(Deposit(bank));
        case 3:
          return Option.Some(Withdraw(bank));
        case 4:
          return Option.None;
        default:
          return Option.Some(bank);
      }
    };

    private static Bank CreateAccount(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var res = Console.ReadLine()
                       .TryParseLong()
                       .Bind(bank.CreateAccount);

      res.Match(
        b => Console.WriteLine("Created account!"),
        () => Console.WriteLine("Could not create account!"));

      return res.Match(b => b, () => bank);
    }

    private static Bank Deposit(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var accountIdInput = Console.ReadLine();
      Console.Write("How much do you want to deposit?\nAnswer: ");
      var amountInput = Console.ReadLine();

      var res = TryParseAccountIdAndAmount(accountIdInput, amountInput)
        .Bind(t => bank.Deposit(t.Item1, t.Item2));

      res.Match(
        b => Console.WriteLine("Deposited amount!"),
        () => Console.WriteLine("Could not deposit amount!"));

      return res.Match(b => b, () => bank);
    }

    private static Bank Withdraw(Bank bank)
    {
      Console.Write("What is the account ID?\nAnswer: ");
      var accountIdInput = Console.ReadLine();
      Console.Write("How much do you want to withdraw?\nAnswer: ");
      var amountInput = Console.ReadLine();

      var res = TryParseAccountIdAndAmount(accountIdInput, amountInput)
        .Bind(t => bank.Withdraw(t.Item1, t.Item2));

      res.Match(
        b => Console.WriteLine("Withdrew amount!"),
        () => Console.WriteLine("Could not withdraw amount!"));

      return res.Match(b => b, () => bank);
    }

    private static Option<Tuple<long, double>> TryParseAccountIdAndAmount(string accountId, string amount) =>
      accountId.TryParseLong().Bind(id => amount.TryParseDouble().Map(a => Tuple.Create(id, a)));
  }
}
