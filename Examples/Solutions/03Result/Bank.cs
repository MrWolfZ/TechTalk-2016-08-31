using System.Collections.Generic;
using Functional.Solutions._03Result;

namespace Examples.Solutions._03Result
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

    public Result<Bank, string> Deposit(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(a => a.Deposit(amount))
          .Map(this.SetAccount);

    public Result<Bank, string> Withdraw(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(a => a.Withdraw(amount))
          .Map(this.SetAccount);

    private Bank SetAccount(Account account)
    {
      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { [account.Id] = account };
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

    public class Account
    {
      public Account(long id, double balance)
      {
        this.Id = id;
        this.Balance = balance;
      }

      public long Id { get; }
      public double Balance { get; }

      public override string ToString()
      {
        return $"[Account] Id: {this.Id}, Balance: {this.Balance}";
      }

      public Result<Account, string> Withdraw(double amount)
      {
        if (amount < 0)
        {
          return $"amount cannot be negative: {amount}";
        }

        if (this.Balance < amount)
        {
          return $"not enough funds to withdraw {amount}: {this.Balance}";
        }

        return new Account(this.Id, this.Balance - amount);
      }

      public Result<Account, string> Deposit(double amount)
      {
        if (amount < 0)
        {
          return $"amount cannot be negative: {amount}";
        }

        return new Account(this.Id, this.Balance + amount);
      }
    }
  }
}
