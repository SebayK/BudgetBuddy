namespace BudgetBuddy.Models;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime GeneratedDate { get; set; }
    public string Content { get; set; }
}