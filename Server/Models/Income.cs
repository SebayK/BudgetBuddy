namespace BudgetBuddy.Models;

public class Income
{
    public int Id { get; set; }
    public required string Source { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public required Category Category { get; set; }
    public int CategoryId { get; set; }
}

