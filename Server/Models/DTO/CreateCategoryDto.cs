namespace BudgetBuddy.Models.DTO
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;

        // "Expense" lub "Income"
        public string Type { get; set; } = "Expense";

        // Id użytkownika przypisującego kategorię
        public string UserId { get; set; } = string.Empty;
    }
}