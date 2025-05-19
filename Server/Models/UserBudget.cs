using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBuddy.Models
{
    public class UserBudget
    {
        public string UserId { get; set; }
    public required User User { get; set; }
    public int BudgetId { get; set; }
    public required Budget Budget { get; set; }
    public required string Role { get; set; } // Np. "Owner", "Editor", "Viewer"
    }
}
