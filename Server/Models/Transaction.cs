namespace BudgetBuddy.Models;

public class Transaction
{
     public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string UserId { get; set; }
    public required User User { get; set; }
    public int BudgetId { get; set; }
    public required Budget Budget { get; set; }
}