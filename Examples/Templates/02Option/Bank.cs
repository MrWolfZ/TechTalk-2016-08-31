using System;
using System.Collections.Generic;
using Functional.Templates._02Option;

namespace Examples.Templates._02Option
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
      throw new NotImplementedException();
    }

    private Option<Account> FindAccount(long accountId)
    {
      throw new NotImplementedException();
    }

    public Option<Bank> Deposit(long accountId, double amount)
    {
      throw new NotImplementedException();
    }

    private static Func<Account, Option<Account>> Deposit(double amount) => a =>
    {
      throw new NotImplementedException();
    };

    public Option<Bank> Withdraw(long accountId, double amount)
    {
      throw new NotImplementedException();
    }

    private static Func<Account, Option<Account>> Withdraw(double amount) => a =>
    {
      throw new NotImplementedException();
    };

    private Bank SetAccount(Account account)
    {
      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { [account.Id] = account };
      return new Bank(updatedAccounts);
    }
  }
}
