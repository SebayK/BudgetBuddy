namespace BudgetBuddy.Models.Dto;

public class TransactionExportDto
{
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // "income" lub "expense"
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public int BudgetId { get; set; }
}
