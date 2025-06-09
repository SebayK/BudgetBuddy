namespace BudgetBuddy.Models.DTO
{
    public class CreateGoalDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal TargetAmount { get; set; }

        public DateTime TargetDate { get; set; }  // Dodane — potrzebne w kontrolerze

        public int BudgetId { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}