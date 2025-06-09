namespace BudgetBuddy.Models.DTO;

public class UserBudgetAssignmentDto
{
    public string UserId { get; set; } = string.Empty;
    public string Role { get; set; } = "Owner"; // domyślna rola
}