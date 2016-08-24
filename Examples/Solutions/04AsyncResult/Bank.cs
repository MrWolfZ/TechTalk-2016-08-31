using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Functional;
using Functional.Solutions._03Result;
using Functional.Solutions._04AsyncResult;

namespace Examples.Solutions._04AsyncResult
{
  public class Bank
  {
    public static readonly Bank Empty = new Bank(new Dictionary<long, Account>());
    private static readonly Random Random = new Random();

    private Bank(IDictionary<long, Account> accounts)
    {
      this.Accounts = accounts;
    }

    private IDictionary<long, Account> Accounts { get; }

    public override string ToString()
    {
      return $"[Bank] Accounts: [{string.Join(", ", this.Accounts.Values)}]";
    }

    public async Task<Result<Bank, string>> CreateAccount(long id)
    {
      if (this.Accounts.ContainsKey(id))
      {
        return $"account {id} already exists";
      }

      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { { id, new Account(id, 0) } };
      return await SomeExternalSystemCall().Map(res => new Bank(updatedAccounts));
    }

    public Task<Result<Bank, string>> Deposit(long accountId, double amount) =>
      this.UpdateAccount(accountId, a => a.Deposit(amount));

    public Task<Result<Bank, string>> Withdraw(long accountId, double amount) =>
      this.UpdateAccount(accountId, a => a.Withdraw(amount));

    private static async Task<Result<Unit, string>> SomeExternalSystemCall()
    {
      await Task.Delay(Random.Next(100, 800));
      return Unit.Default;
    }

    private Task<Result<Bank, string>> UpdateAccount(long accountId, Func<Account, Task<Result<Account, string>>> f) =>
      this.FindAccount(accountId)
          .Bind(f)
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

      public async Task<Result<Account, string>> Withdraw(double amount)
      {
        if (amount < 0)
        {
          return $"amount cannot be negative: {amount}";
        }

        if (this.Balance < amount)
        {
          return $"not enough funds to withdraw {amount}: {this.Balance}";
        }

        // Option 1: map on task
        return await SomeExternalSystemCall().Map(res => new Account(this.Id, this.Balance - amount));
      }

      public async Task<Result<Account, string>> Deposit(double amount)
      {
        if (amount < 0)
        {
          return $"amount cannot be negative: {amount}";
        }

        // Option 2: await, then map on result
        var res = await SomeExternalSystemCall();
        return res.Map(r => new Account(this.Id, this.Balance + amount));
      }
    }
  }
}
