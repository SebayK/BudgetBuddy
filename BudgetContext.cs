using Microsoft.EntityFrameworkCore;
using BudgetBuddyAPI.Models;

namespace BudgetBuddyAPI.Data
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
    }
}
