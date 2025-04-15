namespace BudgetBuddy.Models;

public class Invoice
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }
    public int ExpenseId { get; set; }
    public Expense Expense { get; set; }
}