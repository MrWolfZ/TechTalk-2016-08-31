namespace Examples
{
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

    public Account With(double? balance) => new Account(this.Id, balance ?? this.Balance);
  }
}