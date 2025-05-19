using System.Collections.Generic;

namespace BudgetBuddy.Models;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public List<Expense> Expenses { get; set; } = new();

    public List<Income> Incomes { get; set; } = new();
}
