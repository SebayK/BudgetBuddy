namespace BudgetBuddy.Models;

public class User
{
    public int Id { get; set; }
    public int AccountId { get; set; }
   
    public string Name { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Goal> Goal { get; set; }
    public ICollection<Account> Accounts { get; set; }
}