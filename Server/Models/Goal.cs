namespace BudgetBuddy.Models;

public class Goal
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal TargetAmount { get; set; }  // decimal OK dla SQL Server

    public DateTime TargetDate { get; set; }   // dodaj, jeśli używasz go w kontrolerze

    public int BudgetId { get; set; }

    public string UserId { get; set; } = string.Empty;

    // Poprawione: NIE wymagane (bez `required`)
    public Budget? Budget { get; set; }

    public User? User { get; set; }
}