namespace BudgetBuddy.Models.Dto
{
    public abstract class BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
