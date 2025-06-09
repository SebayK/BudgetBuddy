using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services
{
    public class IncomeService
    {
        private readonly BudgetContext _context;

        public IncomeService(BudgetContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Incomes>> GetAllIncomesAsync()
        {
            return await _context.Incomes
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Incomes?> GetIncomeByIdAsync(int id)
        {
            return await _context.Incomes
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Incomes> CreateIncomeAsync(Incomes income)
        {
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return income;
        }

        public async Task<bool> UpdateIncomeAsync(int id, Incomes updatedIncome)
        {
            if (id != updatedIncome.Id)
                return false;

            
            var existing = await _context.Incomes.FindAsync(id);
            if (existing == null)
                return false;
            
            existing.Name = updatedIncome.Name;
            existing.Source = updatedIncome.Source;
            existing.Amount = updatedIncome.Amount;
            existing.Date = updatedIncome.Date;
            existing.UserId = updatedIncome.UserId;
            existing.CategoryId = updatedIncome.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteIncomeAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
                return false;

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> IncomeExistsAsync(int id)
        {
            return await _context.Incomes.AnyAsync(e => e.Id == id);
        }
    }
}
