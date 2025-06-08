using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class ExpenseService {
  private readonly BudgetContext _context;

  public ExpenseService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Expense>> GetAllExpensesAsync() {
    return await _context.Expenses.AsNoTracking().ToListAsync();
  }

  public async Task<Expense?> GetExpenseByIdAsync(int id) {
    return await _context.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
  }

  public async Task<bool> UpdateExpenseAsync(int id, Expense expense) {
    if (id != expense.Id)
      return false;

    _context.Entry(expense).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await ExpenseExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<Expense> CreateExpenseAsync(Expense expense) {
    _context.Expenses.Add(expense);
    await _context.SaveChangesAsync();
    return expense;
  }

  public async Task<bool> DeleteExpenseAsync(int id) {
    var expense = await _context.Expenses.FindAsync(id);
    if (expense == null)
      return false;

    _context.Expenses.Remove(expense);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> ExpenseExistsAsync(int id) {
    return await _context.Expenses.AnyAsync(e => e.Id == id);
  }
}