namespace BudgetBuddy.Models;

public class Accounts {
  public int Id { get; set; }
  public int UserId { get; set; }
  public int AccountNumber { get; set; }
  public AccountTypes AccountTypes { get; set; } 
  public string CurrencyId { get; set; } = string.Empty;
  public User User { get; set; }
  public int AccountTypesId { get; set; }
}