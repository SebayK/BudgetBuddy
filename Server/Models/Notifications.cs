namespace BudgetBuddy.Models;

public class Notifications {
  public int Id { get; set; }
  public required string UserId { get; set; }
  public required User User { get; set; }
  public required string Message { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? RemindAt { get; set; }
  public bool IsRead { get; set; }
}