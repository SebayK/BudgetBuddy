namespace BudgetBuddy.Models;

public class Notifications {
  public int Id { get; set; }
  public string UserId { get; set; }
  public User User { get; set; }
  public string Message { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? RemindAt { get; set; }
  public bool IsRead { get; set; }
}