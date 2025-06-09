namespace BudgetBuddy.Models.DTO;

public class CreateBudgetDto
{
    public decimal TotalAmount { get; set; }

    // Lista użytkowników wraz z rolami
    public List<UserBudgetAssignmentDto> Users { get; set; } = new();
}