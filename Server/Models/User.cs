using BudgetBuddy.Enums;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Models;

public class User : IdentityUser
{
    public int AccountId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Expense> Expenses { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
    public ICollection<Goal> Goal { get; set; }
    public ICollection<Account> Accounts { get; set; }
    public UserRole Role { get; set; }
    public ICollection<UserBudget> UserBudgets { get; set; }
}
