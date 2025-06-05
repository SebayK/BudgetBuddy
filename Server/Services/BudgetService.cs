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

  public async Task<IEnumerable<Budget>> GetAllBudgetsAsync(string userId) {
    return await _context.Budget
      .AsNoTracking()
      .Where(b => b.UserBudgets.Any(ub => ub.UserId == userId))
      .ToListAsync();
  }

  public async Task<BudgetDto?> GetBudgetByIdAsync(int id, string userId) {
    return await _context.Budget
      .AsNoTracking()
      .Where(b => b.UserBudgets.Any(ub => ub.UserId == userId))
      .Select(b => new BudgetDto {
        Id = b.Id,
        TotalAmount = b.TotalAmount,
        Goals = [],
        Transactions = [],
        UserBudgets = []
      })
      .FirstOrDefaultAsync(b => b.Id == id);
  }

  public async Task<bool> UpdateBudgetAsync(int id, Budget budget) {
    if (budget == null)
      throw new ArgumentNullException(nameof(budget));
    if (id != budget.Id)
      return false;

    _context.Entry(budget).State = EntityState.Modified;

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
    budget.UserBudgets = budget.UserBudgets ?? new List<UserBudget>();
    budget.UserBudgets.Add(new UserBudget { UserId = userId, Budget = budget, Role = "Owner" });
    _context.Budget.Add(budget);
    await _context.SaveChangesAsync();
    return budget;
  }

  public async Task<bool> DeleteBudgetAsync(int id) {
    var budget = await _context.Budget.FindAsync(id);
    if (budget == null)
      return false;

    _context.Budget.Remove(budget);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> BudgetExistsAsync(int id) {
    return await _context.Budget.AnyAsync(b => b.Id == id);
  }
}