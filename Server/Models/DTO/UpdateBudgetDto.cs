namespace BudgetBuddy.Models.DTO
{
    public class UpdateBudgetDto
    {
        public decimal TotalAmount { get; set; }

        // Opcjonalnie możesz aktualizować listę użytkowników
        public List<string> UserIds { get; set; }
    }
}