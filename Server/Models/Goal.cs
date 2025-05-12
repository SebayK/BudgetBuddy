namespace BudgetBuddy.Models;

public class Goal
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal TargetAmount { get; set; }

    public decimal CurrentAmount { get; set; } = 0;

    public DateTime Deadline { get; set; } 

    public int BudgetId { get; set; }
    public Budget Budget { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}
