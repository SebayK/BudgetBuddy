namespace BudgetBuddy.Models.DTO
{
    public class IncomesDto
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        // Opcjonalnie: public string CategoryName { get; set; }
    }
}