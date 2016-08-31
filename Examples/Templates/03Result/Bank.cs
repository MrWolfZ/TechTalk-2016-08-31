using System;
using System.Collections.Generic;
using Functional.Solutions._03Result;

namespace Examples.Templates._03Result
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

    public Result<Bank, string> CreateAccount(long id)
    {
      if (this.Accounts.ContainsKey(id))
      {
        return $"account {id} already exists";
      }

      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { { id, new Account(id, 0) } };
      return new Bank(updatedAccounts);
    }

    private Result<Account, string> FindAccount(long accountId)
    {
      if (this.Accounts.ContainsKey(accountId))
      {
        return this.Accounts[accountId];
      }

      return $"account {accountId} does not exist";
    }

    public Result<Bank, string> Deposit(long accountId, double amount)
    {
      throw new NotImplementedException();
    }

    private static Func<Account, Result<Account, string>> Deposit(double amount) => a =>
    {
      if (amount < 0)
      {
        return $"amount cannot be negative: {amount}";
      }

      return a.With(balance: a.Balance + amount);
    };

    public Result<Bank, string> Withdraw(long accountId, double amount)
    {
      throw new NotImplementedException();
    }

    private static Func<Account, Result<Account, string>> Withdraw(double amount) => a =>
    {
      if (amount < 0)
      {
        return $"amount cannot be negative: {amount}";
      }

      if (a.Balance < amount)
      {
        return $"not enough funds to withdraw {amount}: {a.Balance}";
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