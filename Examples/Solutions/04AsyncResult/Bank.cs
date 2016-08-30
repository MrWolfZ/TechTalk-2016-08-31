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

    private Result<Account, string> FindAccount(long accountId)
    {
      if (this.Accounts.ContainsKey(accountId))
      {
        return this.Accounts[accountId];
      }

      return $"account {accountId} does not exist";
    }

    public Task<Result<Bank, string>> Deposit(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(Deposit(amount))
          .Map(this.SetAccount);

    private static Func<Account, Task<Result<Account, string>>> Deposit(double amount) => async a =>
    {
      if (amount < 0)
      {
        return $"amount cannot be negative: {amount}";
      }

      // Variant 2: await, then map on result
      var res = await SomeExternalSystemCall();
      return res.Map(r => a.With(balance: a.Balance + amount));
    };

    public Task<Result<Bank, string>> Withdraw(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(Withdraw(amount))
          .Map(this.SetAccount);

    private static Func<Account, Task<Result<Account, string>>> Withdraw(double amount) => async a =>
    {
      if (amount < 0)
      {
        return $"amount cannot be negative: {amount}";
      }

      if (a.Balance < amount)
      {
        return $"not enough funds to withdraw {amount}: {a.Balance}";
      }

      // Variant 2: map on task
      return await SomeExternalSystemCall().Map(res => a.With(balance: a.Balance - amount));
    };

    private static async Task<Result<Unit, string>> SomeExternalSystemCall()
    {
      await Task.Delay(Random.Next(100, 800));
      return Unit.Default;
    }

    private Bank SetAccount(Account account)
    {
      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { [account.Id] = account };
      return new Bank(updatedAccounts);
    }
  }
}
