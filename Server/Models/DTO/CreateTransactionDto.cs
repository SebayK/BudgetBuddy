namespace BudgetBuddy.Models.DTO;

public class CreateTransactionDto {
  public decimal Amount { get; set; }
  public string Type { get; set; } = "expense";
  public string Description { get; set; } = string.Empty;
  public DateTime Date { get; set; }
  public string UserId { get; set; }
  public bool IsRecurring { get; set; } = false;
  public string? RecurrenceInterval { get; set; }
  public DateTime? NextOccurrenceDate { get; set; }
  public int CategoryId { get; set; }
  public int BudgetId { get; set; }

  public required string Currency { get; set; } = "PLN";
}