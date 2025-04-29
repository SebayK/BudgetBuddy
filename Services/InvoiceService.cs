using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services
{
    public class InvoiceService
    {
        private readonly BudgetContext _context;

        public InvoiceService(BudgetContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
        {
            return await _context.Invoice.AsNoTracking().ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _context.Invoice.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> UpdateInvoiceAsync(int id, Invoice invoice)
        {
            if (id != invoice.Id)
                return false;

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InvoiceExistsAsync(id))
                    return false;

                throw;
            }
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
                return false;

            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> InvoiceExistsAsync(int id)
        {
            return await _context.Invoice.AnyAsync(e => e.Id == id);
        }
    }
}