namespace BudgetBuddy.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }

    public string Type { get; set; } = "expense"; // "income" lub "expense"

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string UserId { get; set; } = string.Empty;

    public bool IsRecurring { get; set; } = false;

    public string? RecurrenceInterval { get; set; } // np. "Monthly", "Weekly", "Yearly"

    public DateTime? NextOccurrenceDate { get; set; }

    // relacje - mogą być null, jeśli tylko podajesz ID
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int BudgetId { get; set; }
    public Budget? Budget { get; set; }

    public User? User { get; set; }
}