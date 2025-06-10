using BudgetBuddy.Enums;

namespace BudgetBuddy.Models.DTO;

public class UserBudgetDto
{
  public string UserId { get; set; }
  public int BudgetId { get; set; }
  public UserBudgetRole Role { get; set; }
}