namespace BudgetBuddy.Models;

public class Invoice
{
    public int Id { get; set; }
    public required string FilePath { get; set; }
    public DateTime UploadDate { get; set; }

    public int ExpenseId { get; set; }
    public required Expense Expense { get; set; }
}