using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Models;

public class ShareBudgets {
  public int Id { get; set; }

  [StringLength(128)] public required string Name { get; set; }

  public ICollection<User> Users { get; set; } = new List<User>();
}