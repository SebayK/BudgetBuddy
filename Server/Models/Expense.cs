namespace BudgetBuddy.Models;

public class Expense
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public int? InvoiceId { get; set; }

    // WYMAGANE właściwości – nie mogą być nullable
    public Category Category { get; set; } = null!;
    public User User { get; set; } = null!;
    public Invoice Invoice { get; set; } = null!;
    public int BudgetId { get; set; }
}