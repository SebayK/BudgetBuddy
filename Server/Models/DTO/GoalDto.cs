namespace BudgetBuddy.Models.Dto;

public class GoalDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime Deadline { get; set; }
}
