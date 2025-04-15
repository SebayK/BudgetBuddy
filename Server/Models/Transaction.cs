namespace BudgetBuddy.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int BudgetId { get; set; }
    public Budget Budget { get; set; }
}