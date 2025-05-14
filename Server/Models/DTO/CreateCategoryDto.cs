namespace BudgetBuddy.Models.Dto
{
    public class CreateCategoryDto : BaseDto
    {
        public string ColorHex { get; set; } = "#FFFFFF";
        public string UserId { get; set; } = string.Empty;
    }
}
