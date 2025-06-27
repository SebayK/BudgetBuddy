namespace BudgetBuddy.Models.DTO
{
    public class CreateBudgetDto
    {
        public decimal TotalAmount { get; set; }

        // Lista użytkowników do przypisania do budżetu (np. po wybraniu w UI)
        public List<string> UserIds { get; set; }
    }
}