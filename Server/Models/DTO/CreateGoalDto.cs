namespace BudgetBuddy.Models.Dto;

public class CreateGoalDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal TargetAmount { get; set; }
    public DateTime Deadline { get; set; }
    public int BudgetId { get; set; }
}
