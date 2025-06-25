using BudgetBuddy.Enums;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class BudgetService {
  private readonly BudgetContext _context;

  public BudgetService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<BudgetDto>> GetAllBudgetsAsync(string userId) {
    var budgets = await _context.Budgets
      .AsNoTracking()
      .Where(b => b.UserBudgets.Any(ub => ub.UserId == userId))
      .ToListAsync();

    return budgets.Any() ? budgets.Select(MapToBudgetDto).ToList() : new List<BudgetDto>();
  }

  public async Task<BudgetDto?> GetBudgetByIdAsync(int id, string userId) {
    var budget = await _context.Budgets
      .AsNoTracking()
      .Where(b => b.UserBudgets.Any(ub => ub.UserId == userId))
      .FirstOrDefaultAsync(b => b.Id == id);

    return budget == null ? null : MapToBudgetDto(budget);
  }

  public async Task<bool> UpdateBudgetAsync(int id, Budget budget, string userId) {
    if (budget == null)
      throw new ArgumentNullException(nameof(budget));
    var existingBudget = await _context.Budgets
      .Where(b => b.Id == id && b.UserBudgets.Any(ub => ub.UserId == userId))
      .FirstOrDefaultAsync();
    if (existingBudget == null)
      return false;

    budget.Id = existingBudget.Id;
    _context.Entry(existingBudget).CurrentValues.SetValues(budget);

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await BudgetExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<Budget> CreateBudgetAsync(Budget budget, string userId) {
    if (budget == null)
      throw new ArgumentNullException(nameof(budget));
    if (string.IsNullOrEmpty(userId))
      throw new ArgumentNullException(nameof(userId));
    if (budget.UserBudgets.All(ub => ub.Role != UserBudgetRole.Owner)) {
      budget.UserBudgets = new List<UserBudget>();
      budget.UserBudgets.Add(new UserBudget { UserId = userId, Budget = budget, Role = UserBudgetRole.Owner });
    }

    if (budget.UserBudgets.Count(ub => ub.Role == UserBudgetRole.Owner) > 1)
      throw new InvalidOperationException("Budżet może mieć tylko jednego ownera. Dodaj użytkownika z inną rolą.");


    _context.Budgets.Add(budget);
    await _context.SaveChangesAsync();
    return budget;
  }

  public async Task<bool> DeleteBudgetAsync(int id, string userId) {
    var budget = await _context.Budgets
      .Where(b => b.Id == id && b.UserBudgets.Any(ub => ub.UserId == userId))
      .FirstOrDefaultAsync();
    if (budget == null)
      return false;

    _context.Budgets.Remove(budget);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> BudgetExistsAsync(int id) {
    return await _context.Budgets.AnyAsync(b => b.Id == id);
  }

  private static BudgetDto MapToBudgetDto(Budget b) {
    return new BudgetDto {
      Id = b.Id,
      TotalAmount = b.TotalAmount,
      Goals = MapGoals(b.Goals),
      Transactions = MapTransactions(b.Transactions),
      UserBudgets = MapUserBudgets(b.UserBudgets)
    };
  }

  private static GoalDto[] MapGoals(ICollection<Goal> goals) {
    if (!goals.Any())
      return Array.Empty<GoalDto>();
    return goals.Select(g => new GoalDto {
      Id = g.Id,
      Name = g.Name,
      TargetAmount = g.TargetAmount,
    }).ToArray();
  }

  private static TransactionDto[] MapTransactions(ICollection<Transaction> transactions) {
    if (!transactions.Any())
      return Array.Empty<TransactionDto>();
    return transactions.Select(t => new TransactionDto {
      Id = t.Id,
      Amount = t.Amount,
      Type = t.Type,
      Description = t.Description,
      Date = t.Date,
      IsRecurring = t.IsRecurring,
      RecurrenceInterval = t.RecurrenceInterval,
      NextOccurrenceDate = t.NextOccurrenceDate,
    }).ToArray();
  }

  private static UserBudgetDto[] MapUserBudgets(ICollection<UserBudget> userBudgets) {
    if (!userBudgets.Any())
      return Array.Empty<UserBudgetDto>();
    return userBudgets.Select(ub => new UserBudgetDto {
      UserId = ub.UserId,
      BudgetId = ub.BudgetId,
      Role = ub.Role
    }).ToArray();
  }
}