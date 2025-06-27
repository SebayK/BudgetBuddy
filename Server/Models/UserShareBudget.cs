namespace BudgetBuddy.Models
{
    public class UserShareBudget
    {
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public int ShareBudgetId { get; set; }
        public ShareBudgets ShareBudget { get; set; } = null!;

        public string Role { get; set; } = "Viewer"; // np. "Viewer", "Editor"
    }
}