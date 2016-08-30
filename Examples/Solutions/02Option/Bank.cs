using System;
using System.Collections.Generic;
using Functional.Solutions._02Option;
using static Functional.Solutions._02Option.Option;

namespace Examples.Solutions._02Option
{
  public class Bank
  {
    public static Bank Empty = new Bank(new Dictionary<long, Account>());

    private Bank(IDictionary<long, Account> accounts)
    {
      this.Accounts = accounts;
    }

    private IDictionary<long, Account> Accounts { get; }

    public override string ToString()
    {
      return $"[Bank] Accounts: [{string.Join(", ", this.Accounts.Values)}]";
    }

    public Option<Bank> CreateAccount(long id)
    {
      if (this.Accounts.ContainsKey(id))
      {
        return None;
      }

      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { { id, new Account(id, 0) } };
      return new Bank(updatedAccounts);
    }

    private Option<Account> FindAccount(long accountId) =>
      this.Accounts.ContainsKey(accountId) ? Some(this.Accounts[accountId]) : None;

    public Option<Bank> Deposit(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(Deposit(amount))
          .Map(this.SetAccount);

    private static Func<Account, Option<Account>> Deposit(double amount) => a =>
    {
      if (amount < 0)
      {
        return None;
      }

      return a.With(balance: a.Balance + amount);
    };

    public Option<Bank> Withdraw(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(Withdraw(amount))
          .Map(this.SetAccount);

    private static Func<Account, Option<Account>> Withdraw(double amount) => a =>
    {
      if (amount < 0 || a.Balance < amount)
      {
        return None;
      }

      return a.With(balance: a.Balance - amount);
    };

    private Bank SetAccount(Account account)
    {
      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { [account.Id] = account };
      return new Bank(updatedAccounts);
    }
  }
}
