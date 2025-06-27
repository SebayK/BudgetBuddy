using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class ShareBudgetsService(BudgetContext context) {
  public async Task<IEnumerable<ShareBudgets>> GetAllShareBudgetsAsync(string userId) {
    return await context.ShareBudgets
      .AsNoTracking()
      .Where(sb => sb.Users.Any(u => u.Id == userId))
      .Include(sb => sb.Users)
      .ToListAsync();
  }

  public async Task<ShareBudgets?> GetShareBudgetByIdAsync(int id, string userId) {
    return await context.ShareBudgets
      .AsNoTracking()
      .Where(sb => sb.Users.Any(u => u.Id == userId))
      .Include(sb => sb.Users)
      .FirstOrDefaultAsync(sb => sb.Id == id);
  }

  public async Task<bool> UpdateShareBudgetAsync(int id, ShareBudgets shareBudgets, string userId) {
    if (id != shareBudgets.Id)
      return false;

    context.Entry(shareBudgets).State = EntityState.Modified;
    var existingShareBudgets = await context.ShareBudgets
      .Where(sb => sb.Id == id && sb.Users.Any(u => u.Id == userId))
      .FirstOrDefaultAsync();
    if (existingShareBudgets == null)
      return false;

    shareBudgets.Id = existingShareBudgets.Id;
    context.Entry(existingShareBudgets).CurrentValues.SetValues(shareBudgets);

    try {
      await context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await ShareBudgetExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<ShareBudgets> CreateShareBudgetAsync(ShareBudgets shareBudgets, string userId) {
    if (shareBudgets == null)
      throw new ArgumentNullException(nameof(shareBudgets));
    if (string.IsNullOrEmpty(userId))
      throw new ArgumentNullException(nameof(userId));
    var user = await context.Users.FindAsync(userId);

    if (user == null)
      throw new InvalidOperationException("Nie znaleziono użytkownika o podanym userId.");
    shareBudgets.Users = new List<User> { user };
    context.ShareBudgets.Add(shareBudgets);
    await context.SaveChangesAsync();
    return shareBudgets;
  }

  public async Task<bool> DeleteShareBudgetAsync(int id, string userId) {
    var shareBudget = await context.ShareBudgets
      .Where(sb => sb.Id == id && sb.Users.Any(u => u.Id == userId))
      .FirstOrDefaultAsync();

    if (shareBudget == null)
      return false;

    context.ShareBudgets.Remove(shareBudget);
    await context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> ShareBudgetExistsAsync(int id) {
    return await context.ShareBudgets.AnyAsync(sb => sb.Id == id);
  }
}