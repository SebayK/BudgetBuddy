using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class IncomeService {
  private readonly BudgetContext _context;

  public IncomeService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Income>> GetAllIncomesAsync() {
    return await _context.Incomes.AsNoTracking().ToListAsync();
  }

  public async Task<Income?> GetIncomeByIdAsync(int id) {
    return await _context.Incomes.FindAsync(id);
  }

  public async Task<bool> UpdateIncomeAsync(int id, Income income) {
    if (id != income.Id) return false;

    _context.Entry(income).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await IncomeExistsAsync(id)) return false;
      throw;
    }
  }

  public async Task<Income> CreateIncomeAsync(Income income) {
    _context.Incomes.Add(income);
    await _context.SaveChangesAsync();
    return income;
  }

  public async Task<bool> DeleteIncomeAsync(int id) {
    var income = await _context.Incomes.FindAsync(id);
    if (income == null) return false;

    _context.Incomes.Remove(income);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> IncomeExistsAsync(int id) {
    return await _context.Incomes.AnyAsync(e => e.Id == id);
  }
}