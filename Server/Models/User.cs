using BudgetBuddy.Enums;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace BudgetBuddy.Models;

public class User : IdentityUser
{
    public int AccountId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [JsonIgnore]
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    [JsonIgnore]
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [JsonIgnore]
    public ICollection<Goal> Goal { get; set; } = new List<Goal>();

    [JsonIgnore]
    public ICollection<Account> Accounts { get; set; } = new List<Account>();

    public UserRole Role { get; set; }

    [JsonIgnore]
    public ICollection<UserBudget> UserBudgets { get; set; } = new List<UserBudget>();

    [JsonIgnore]
    public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
}