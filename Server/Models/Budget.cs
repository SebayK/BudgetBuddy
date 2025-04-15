namespace BudgetBuddy.Models;

public class Budget
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<Goal> Goals { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}