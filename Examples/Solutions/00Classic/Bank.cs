using System;
using System.Collections.Generic;

namespace Examples.Solutions._00Classic
{
  public class Bank
  {
    private IDictionary<long, Account> Accounts { get; } = new Dictionary<long, Account>();

    public override string ToString()
    {
      return $"[Bank] Accounts: [{string.Join(", ", this.Accounts.Values)}]";
    }

    public void CreateAccount(long id)
    {
      if (this.Accounts.ContainsKey(id))
      {
        throw new ArgumentException($"account {id} already exists");
      }

      this.Accounts.Add(id, new Account(id, 0));
    }

    public void Deposit(long accountId, double amount)
    {
      var a = this.FindAccount(accountId);
      a.Deposit(amount);
    }

    public void Withdraw(long accountId, double amount)
    {
      var a = this.FindAccount(accountId);
      a.Withdraw(amount);
    }

    private Account FindAccount(long accountId)
    {
      if (!this.Accounts.ContainsKey(accountId))
      {
        throw new ArgumentException($"account {accountId} does not exist");
      }

      return this.Accounts[accountId];
    }

    public class Account
    {
      public Account(long id, double balance)
      {
        this.Id = id;
        this.Balance = balance;
      }

      public long Id { get; }
      public double Balance { get; private set; }

      public override string ToString()
      {
        return $"[Account] Id: {this.Id}, Balance: {this.Balance}";
      }

      public void Withdraw(double amount)
      {
        if (amount < 0)
        {
          throw new ArgumentException($"amount cannot be negative: {amount}");
        }

        if (this.Balance < amount)
        {
          throw new ArgumentException($"not enough funds to withdraw {amount}: {this.Balance}");
        }

        this.Balance -= amount;
      }

      public void Deposit(double amount)
      {
        if (amount < 0)
        {
          throw new ArgumentException($"amount cannot be negative: {amount}");
        }

        this.Balance += amount;
      }
    }
  }
}
