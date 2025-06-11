namespace BudgetBuddy.Models;

public class Invoice
{
    public int Id { get; set; }

    public string FilePath { get; set; } = string.Empty;

    public DateTime UploadDate { get; set; }

    public int ExpenseId { get; set; }

    // Poprawka: bez required, z opcją null
    public Expense? Expense { get; set; }
}