using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class ShareBudgetService {
  private readonly BudgetContext _context;

  public ShareBudgetService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<ShareBudget>> GetAllShareBudgetsAsync() {
    return await _context.ShareBudget.AsNoTracking().ToListAsync();
  }

  public async Task<ShareBudget?> GetShareBudgetByIdAsync(int id) {
    return await _context.ShareBudget.AsNoTracking().FirstOrDefaultAsync(sb => sb.Id == id);
  }

  public async Task<bool> UpdateShareBudgetAsync(int id, ShareBudget shareBudget) {
    if (id != shareBudget.Id)
      return false;

    _context.Entry(shareBudget).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await ShareBudgetExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<ShareBudget> CreateShareBudgetAsync(ShareBudget shareBudget) {
    _context.ShareBudget.Add(shareBudget);
    await _context.SaveChangesAsync();
    return shareBudget;
  }

  public async Task<bool> DeleteShareBudgetAsync(int id) {
    var shareBudget = await _context.ShareBudget.FindAsync(id);
    if (shareBudget == null)
      return false;

    _context.ShareBudget.Remove(shareBudget);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> ShareBudgetExistsAsync(int id) {
    return await _context.ShareBudget.AnyAsync(sb => sb.Id == id);
  }
}