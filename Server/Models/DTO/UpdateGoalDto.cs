namespace BudgetBuddy.Models.DTO
{
    public class UpdateGoalDto
    {
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public int BudgetId { get; set; }
    }
}