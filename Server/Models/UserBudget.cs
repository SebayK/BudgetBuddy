using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetBuddy.Models
{
    public class UserBudget
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int BudgetId { get; set; }
        public Budget Budget { get; set; }

        public string Role { get; set; } // Np. "Owner", "Editor", "Viewer"
    }
}
