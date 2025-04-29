﻿namespace BudgetBuddy.Models;

public class AccountType {
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public ICollection<Account> Accounts { get; set; }
}