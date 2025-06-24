namespace BudgetBuddy.Models;

public class Report {
  public int Id { get; set; }
  public int UserId { get; set; }
  public string Title { get; set; }
  public DateTime GeneratedDate { get; set; }
  public DateTime From { get; set; }
  public DateTime To { get; set; }
  public string Content { get; set; }
  public decimal TotalExpenses { get; set; }
  public decimal TotalIncomes { get; set; }
  public decimal Balance => TotalIncomes - TotalExpenses;
}