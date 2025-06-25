namespace BudgetBuddy.Models.DTO;

public class CreateBudgetDto
{
    public decimal TotalAmount { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<UserBudgetAssignmentDto> Users { get; set; } = new();
}