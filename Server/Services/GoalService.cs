using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class GoalService {
  private readonly BudgetContext _context;

  public GoalService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Goal>> GetAllGoalsAsync() {
    return await _context.Goal.AsNoTracking().ToListAsync();
  }

  public async Task<Goal?> GetGoalByIdAsync(int id) {
    return await _context.Goal.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
  }

  public async Task<bool> UpdateGoalAsync(int id, Goal goal) {
    if (id != goal.Id)
      return false;

    _context.Entry(goal).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await GoalExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<Goal> CreateGoalAsync(Goal goal) {
    _context.Goal.Add(goal);
    await _context.SaveChangesAsync();
    return goal;
  }

  public async Task<bool> DeleteGoalAsync(int id) {
    var goal = await _context.Goal.FindAsync(id);
    if (goal == null)
      return false;

    _context.Goal.Remove(goal);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> GoalExistsAsync(int id) {
    return await _context.Goal.AnyAsync(g => g.Id == id);
  }
}