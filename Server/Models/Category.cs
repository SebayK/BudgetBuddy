namespace BudgetBuddy.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string ColorHex { get; set; } = "#FFFFFF"; 

    public string UserId { get; set; } = string.Empty; 

    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Incomes> Incomes { get; set; } = new List<Incomes>();
}
