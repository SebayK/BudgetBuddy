namespace BudgetBuddy.Models;

public class Account {
  public int Id { get; set; }
    public string UserId { get; set; }
    public int AccountNumber { get; set; }
    public required AccountType AccountType { get; set; }
    public required string CurrencyId { get; set; }
    public required User User { get; set; }
    public int AccountTypesId { get; set; }
}