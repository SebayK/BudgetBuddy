namespace BudgetBuddy.Models.Dto
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ColorHex { get; set; } = "#FFFFFF";
    }
}
