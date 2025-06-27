namespace BudgetBuddy.Models.DTO
{
    public class ShareBudgetCreateDto
    {
        public string Name { get; set; } = string.Empty;
        
        public List<UserShareBudgetCreateDto> Users { get; set; } = new();
    }
}