namespace BudgetBuddy.Models.Dto
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ColorHex { get; set; } = "#FFFFFF";
        public string UserId { get; set; } = string.Empty;
    }
}
