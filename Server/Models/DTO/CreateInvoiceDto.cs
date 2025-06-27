namespace BudgetBuddy.Models.DTO
{
    public class CreateInvoiceDto
    {
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public int ExpenseId { get; set; }
    }
}