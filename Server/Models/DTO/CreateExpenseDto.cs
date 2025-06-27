namespace BudgetBuddy.Models.DTO
{
    public class CreateExpenseDto
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public int InvoiceId { get; set; }
    }
}