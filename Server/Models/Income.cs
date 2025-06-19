namespace BudgetBuddy.Models;

public class Income
{
    public int Id { get; set; }

    public required string Name { get; set; }  // dodane
    public required string Source { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public required string UserId { get; set; }  // dodane

    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int BudgetId { get; set; }
}