namespace BudgetBuddy.Models.DTO
{
    public class CreateGoalDto
    {
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public int BudgetId { get; set; }
        public string UserId { get; set; }
    }
}