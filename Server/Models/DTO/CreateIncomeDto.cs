namespace BudgetBuddy.Models.DTO;

public class CreateIncomeDto
{
    public string Name { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}