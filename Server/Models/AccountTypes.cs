namespace BudgetBuddy.Models;

public class AccountTypes {
  public int Id { get; set; }
  public string AccountType { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public ICollection<Accounts> Accounts { get; set; }
}