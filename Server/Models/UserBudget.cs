using System.Text.Json.Serialization;

namespace BudgetBuddy.Models
{
    public class UserBudget
    {
        public string UserId { get; set; }

        [JsonIgnore] 
        public User User { get; set; }

        public int BudgetId { get; set; }

        [JsonIgnore] 
        public Budget Budget { get; set; }

        public string Role { get; set; } // Np. "Owner", "Editor", "Viewer"
    }
}