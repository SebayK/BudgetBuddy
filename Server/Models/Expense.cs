namespace BudgetBuddy.Models;
public class Expense
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public int CategoryId { get; set; }
    public required Category Category { get; set; }

    public string UserId { get; set; }
    public required User User { get; set; }

    public required Invoice Invoice { get; set; }
}
