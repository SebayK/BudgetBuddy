namespace BudgetBuddy.Models.Dto
{
    public class CreateGoalDto : BaseDto
    {
        public decimal TargetAmount { get; set; }
        public DateTime Deadline { get; set; }
        public int BudgetId { get; set; }
        public int UserId { get; set; }
    }
}
