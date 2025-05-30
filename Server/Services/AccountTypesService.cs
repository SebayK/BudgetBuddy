using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class AccountTypeService {
  private readonly BudgetContext _context;

  public AccountTypeService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<AccountType>> GetAllAccountTypesAsync() {
    return await _context.AccountTypes.AsNoTracking().ToListAsync();
  }

  public async Task<AccountType?> GetAccountTypeByIdAsync(int id) {
    return await _context.AccountTypes.AsNoTracking().FirstOrDefaultAsync(at => at.Id == id);
  }

  public async Task<bool> UpdateAccountTypeAsync(int id, AccountType accountType) {
    if (id != accountType.Id) return false;

    _context.Entry(accountType).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await AccountTypeExistsAsync(id)) return false;
      throw;
    }
  }

  public async Task<AccountType> CreateAccountTypeAsync(AccountType accountType) {
    _context.AccountTypes.Add(accountType);
    await _context.SaveChangesAsync();
    return accountType;
  }

  public async Task<bool> DeleteAccountTypeAsync(int id) {
    var accountType = await _context.AccountTypes.FindAsync(id);
    if (accountType == null) return false;

    _context.AccountTypes.Remove(accountType);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> AccountTypeExistsAsync(int id) {
    return await _context.AccountTypes.AnyAsync(at => at.Id == id);
  }
}