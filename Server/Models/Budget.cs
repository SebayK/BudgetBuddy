namespace BudgetBuddy.Models;

public class Budget {
  public int Id { get; set; }

  public string Name { get; set; } = string.Empty;

  public decimal TotalAmount { get; set; }

  public string Currency { get; set; } = string.Empty;

  public ICollection<Goal> Goals { get; set; } = [];
  public ICollection<Transaction> Transactions { get; set; } = [];
  public ICollection<UserBudget> UserBudgets { get; set; } = [];
}