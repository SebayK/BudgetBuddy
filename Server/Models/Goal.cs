namespace BudgetBuddy.Models;

public class Goal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public int BudgetId { get; set; }
    public Budget Budget { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
}