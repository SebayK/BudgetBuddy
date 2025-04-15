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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(b => b.TotalAmount)
                    .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId);
            modelBuilder.Entity<Expense>(entity => { entity.Property(e => e.Amount).HasPrecision(18, 2); });

            // Expense -> User (Many-to-One)
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId);

            // User -> Transaction (One-to-Many)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            /* User -> Notification (One-to-Many)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);*/

            // Budget -> Goal (One-to-Many)
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.Budget)
                .WithMany(b => b.Goals)
                .HasForeignKey(g => g.BudgetId);
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(g => g.TargetAmount)
                    .HasPrecision(18, 2);
            });

            // Budget -> Transaction (One-to-Many)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Budget)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BudgetId);

            // Goal -> Budget (Many-to-One)
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.Budget)
                .WithMany(b => b.Goals)
                .HasForeignKey(g => g.BudgetId);

            // Transaction -> User (Many-to-One)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            // Transaction -> Budget (Many-to-One)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Budget)
                .WithMany(b => b.Transactions)
                .HasForeignKey(t => t.BudgetId);
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(t => t.Amount)
                    .HasPrecision(18, 2); 
            });

            // Invoice -> Expense (One-to-One)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Expense)
                .WithOne(e => e.Invoice)
                .HasForeignKey<Invoice>(i => i.ExpenseId);
            modelBuilder.Entity<Incomes>(entity =>
            {
                entity.Property(i => i.Amount)
                    .HasPrecision(18, 2);
            });
        }
       

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Incomes> Incomes { get; set; }
        public DbSet<Budget> Budget { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Goal> Goal { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }
    }
}