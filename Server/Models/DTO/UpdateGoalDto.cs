namespace BudgetBuddy.Models.Dto
{
    public class UpdateGoalDto : BaseDto
    {
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }
    }
}
