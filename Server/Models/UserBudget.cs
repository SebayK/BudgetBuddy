using System.Text.Json.Serialization;

using BudgetBuddy.Enums;

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

        public UserBudgetRole Role { get; set; } // Np. "Owner", "Editor", "Viewer"
    }
}