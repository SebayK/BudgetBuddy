namespace BudgetBuddy.Models.DTO
{
    public class CreateIncomeDto
    {
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
    }
}