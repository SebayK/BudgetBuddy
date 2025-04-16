namespace BudgetBuddy.Models;

public class Incomes
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
}
