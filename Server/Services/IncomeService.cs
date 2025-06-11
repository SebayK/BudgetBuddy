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

        public async Task<IEnumerable<Income>> GetAllIncomesAsync()
        {
            return await _context.Incomes
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<Income?> GetIncomeByIdAsync(int id)
        {
            // Można łatwo podmienić na wersję z DTO: return _mapper.Map<IncomeDTO>(await ...)
            return await _context.Incomes
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(i => i.Id == id);

            // Wersja alternatywna z drugiego brancha:
            // return await _context.Incomes.FindAsync(id);
        }

        public async Task<Income> CreateIncomeAsync(Income income)
        {
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return income;
        }

        public async Task<bool> UpdateIncomeAsync(int id, Income updatedIncome)
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await IncomeExistsAsync(id))
                    return false;

                throw;
            }

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
