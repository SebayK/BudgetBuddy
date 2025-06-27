namespace BudgetBuddy.Models.DTO
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public int? InvoiceId { get; set; } // Jeśli Invoice ma Id, warto przekazywać tylko referencję
        // Opcjonalnie: public string CategoryName { get; set; }
    }
}