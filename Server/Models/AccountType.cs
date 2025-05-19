namespace BudgetBuddy.Models;

public class AccountType
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<Account> Accounts { get; set; } = new();
}
