namespace BudgetBuddy.Models;

public class Account {
  public int Id { get; set; }
  public int UserId { get; set; }
  public int AccountNumber { get; set; }
  public AccountType AccountType { get; set; } 
  public string CurrencyId { get; set; } = string.Empty;
  public User User { get; set; }
  public int AccountTypesId { get; set; }
}