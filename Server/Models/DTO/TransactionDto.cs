namespace BudgetBuddy.Models.DTO
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        public DateTime? NextOccurrenceDate { get; set; }
        public int CategoryId { get; set; }
        public int BudgetId { get; set; }
    }
}