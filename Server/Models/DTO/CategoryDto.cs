namespace BudgetBuddy.Models.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Opcjonalnie: ilość wydatków/przychodów w tej kategorii
        // public int ExpenseCount { get; set; }
        // public int IncomeCount { get; set; }
    }
}