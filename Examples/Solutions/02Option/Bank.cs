using System.Collections.Generic;
using Functional.Solutions._02Option;

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
        return Option.None;
      }

      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { { id, new Account(id, 0) } };
      return new Bank(updatedAccounts);
    }

    public Option<Bank> Deposit(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(a => a.Deposit(amount))
          .Map(this.SetAccount);

    public Option<Bank> Withdraw(long accountId, double amount) =>
      this.FindAccount(accountId)
          .Bind(a => a.Withdraw(amount))
          .Map(this.SetAccount);

    private Bank SetAccount(Account account)
    {
      var updatedAccounts = new Dictionary<long, Account>(this.Accounts) { [account.Id] = account };
      return new Bank(updatedAccounts);
    }

    private Option<Account> FindAccount(long accountId) =>
      this.Accounts.ContainsKey(accountId) ? Option.Some(this.Accounts[accountId]) : Option.None;

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

      public Option<Account> Withdraw(double amount)
      {
        if (amount < 0 || this.Balance < amount)
        {
          return Option.None;
        }

        return new Account(this.Id, this.Balance - amount);
      }

      public Option<Account> Deposit(double amount)
      {
        if (amount < 0)
        {
          return Option.None;
        }

        return new Account(this.Id, this.Balance + amount);
      }
    }
  }
}
