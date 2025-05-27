namespace BudgetBuddy.Models;

public class Goal
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public decimal TargetAmount { get; set; } // zmienione z float na decimal, nie działa z SQL server

    public int BudgetId { get; set; }
    public required Budget Budget { get; set; }

    public string UserId { get; set; }
    public required User User { get; set; }
}
