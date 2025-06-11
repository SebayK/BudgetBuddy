using System.Collections.Generic;

namespace BudgetBuddy.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    // Dodane właściwości:
    public string Type { get; set; } = "Expense"; // np. "Income" lub "Expense"
    public string UserId { get; set; } = string.Empty;

    public List<Expense> Expenses { get; set; } = new();
    public List<Income> Incomes { get; set; } = new();
}