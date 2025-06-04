namespace BudgetBuddy.Models;

public class ShareBudget {
  public int Id { get; set; }
  public string Name { get; set; }
  public ICollection<User> Users { get; set; }
}