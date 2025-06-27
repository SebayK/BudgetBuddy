using System.Text.Json.Serialization;

namespace BudgetBuddy.Models;

public class AccountType
{
    public int Id { get; set; }

    public required string Name { get; set; }

    [JsonIgnore] //Ignorujemy, aby uniknąć zapętlenia przy serializacji
    public List<Account> Accounts { get; set; } = new();
}