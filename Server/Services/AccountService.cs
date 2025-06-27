using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class AccountService {
  private readonly BudgetContext _context;

  public AccountService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Account>> GetAllAccountsAsync() {
    return await _context.Accounts.AsNoTracking().ToListAsync();
  }

  public async Task<Account?> GetAccountByIdAsync(int id) {
    return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
  }

  public async Task<bool> UpdateAccountAsync(int id, Account account) {
    if (id != account.Id) return false;

    _context.Entry(account).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await AccountExistsAsync(id)) return false;
      throw;
    }
  }

  public async Task<Account> CreateAccountAsync(Account account) {
    _context.Accounts.Add(account);
    await _context.SaveChangesAsync();
    return account;
  }

  public async Task<bool> DeleteAccountAsync(int id) {
    var account = await _context.Accounts.FindAsync(id);
    if (account == null) return false;

    _context.Accounts.Remove(account);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> AccountExistsAsync(int id) {
    return await _context.Accounts.AnyAsync(a => a.Id == id);
  }
}