using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Models;

public class ShareBudgets {
  public int Id { get; set; }

  [StringLength(128)] public required string Name { get; set; }

  // ❌ Stara linia powodująca błąd (brak kolumny w bazie):
  // public string UserId { get; set; }

  // Zgodne z bazą danych:
  public string OwnerUserId { get; set; } = string.Empty;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public ICollection<UserShareBudget> UserShareBudgets { get; set; } = new List<UserShareBudget>();
}