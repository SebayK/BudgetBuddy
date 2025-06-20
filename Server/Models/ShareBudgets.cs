using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Models
{
    public class ShareBudgets
    {
        public int Id { get; set; }

        [StringLength(128)]
        public required string Name { get; set; }

        public string OwnerUserId { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserShareBudget> UserShareBudgets { get; set; } = new List<UserShareBudget>();
    }
}