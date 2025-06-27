public class CreateExpenseDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public int CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int BudgetId { get; set; } 
    public int? InvoiceId { get; set; }
}