using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Models;

namespace BudgetBuddy.Infrastructure
{
    public class BudgetContext : DbContext
    {
        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .AddInterceptors(new ConnectionInterceptor());
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
    }
}
