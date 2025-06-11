using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class AccountService
{
    private readonly BudgetContext _context;

    public AccountService(BudgetContext context)
    {
        _context = context;
    }

    // Pobiera wszystkie konta
    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _context.Accounts
            .Include(a => a.AccountType) // jeśli chcesz mieć dostęp do AccountType.Name
            .AsNoTracking()
            .ToListAsync();
    }

    // Pobiera jedno konto po ID
    public async Task<Account?> GetAccountByIdAsync(int id)
    {
        return await _context.Accounts
            .Include(a => a.AccountType) // jeśli chcesz mieć dostęp do AccountType.Name
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    // Aktualizuje konto
    public async Task<bool> UpdateAccountAsync(int id, Account account)
    {
        if (id != account.Id)
            return false;

        _context.Entry(account).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AccountExistsAsync(id))
                return false;

            throw;
        }
    }

    // Tworzy nowe konto
    public async Task<Account> CreateAccountAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }

    // Usuwa konto
    public async Task<bool> DeleteAccountAsync(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
            return false;

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }

    // Sprawdza, czy konto istnieje
    private async Task<bool> AccountExistsAsync(int id)
    {
        return await _context.Accounts.AnyAsync(a => a.Id == id);
    }

    // Pobiera użytkownika po ID (np. "abc123")
    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    // Pobiera typ konta po ID (np. 1 = "Oszczędnościowe")
    public async Task<AccountType?> GetAccountTypeByIdAsync(int id)
    {
        return await _context.AccountTypes.FindAsync(id);
    }
}
