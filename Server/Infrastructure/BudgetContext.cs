using BudgetBuddy.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Infrastructure;

public class BudgetContext(DbContextOptions<BudgetContext> options) : IdentityDbContext<User>(options)
{
  public DbSet<Expense> Expenses { get; set; }
  public DbSet<Income> Incomes { get; set; }
  public DbSet<Budget> Budget { get; set; }
  public DbSet<Category> Category { get; set; }
  public DbSet<Goal> Goal { get; set; }
  public DbSet<Invoice> Invoice { get; set; }
  public DbSet<Report> Report { get; set; }
  public DbSet<Transaction> Transaction { get; set; }
  public DbSet<User> User { get; set; }
  public DbSet<Account> Accounts { get; set; }
  public DbSet<AccountType> AccountTypes { get; set; }
  public DbSet<Notifications> Notifications { get; set; }
  public DbSet<ShareBudgets> ShareBudgets { get; set; }
  public DbSet<UserShareBudget> UserShareBudgets { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder
      .LogTo(Console.WriteLine, LogLevel.Information)
      .EnableSensitiveDataLogging()
      .AddInterceptors(new ConnectionInterceptor());
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Budget>(entity =>
    {
      entity.Property(b => b.TotalAmount)
        .HasPrecision(18, 2);
    });

    modelBuilder.Entity<Expense>()
      .HasOne(e => e.Category)
      .WithMany(c => c.Expenses)
      .HasForeignKey(e => e.CategoryId);

    modelBuilder.Entity<Expense>(entity =>
    {
      entity.Property(e => e.Amount).HasPrecision(18, 2);
    });

    modelBuilder.Entity<Expense>()
      .HasOne(e => e.User)
      .WithMany(u => u.Expenses)
      .HasForeignKey(e => e.UserId);

    modelBuilder.Entity<Transaction>()
      .HasOne(t => t.User)
      .WithMany(u => u.Transactions)
      .HasForeignKey(t => t.UserId);

    modelBuilder.Entity<Goal>()
      .HasOne(g => g.Budget)
      .WithMany(b => b.Goals)
      .HasForeignKey(g => g.BudgetId);

    modelBuilder.Entity<Goal>(entity =>
    {
      entity.Property(g => g.TargetAmount)
        .HasPrecision(18, 2);
    });

    modelBuilder.Entity<Transaction>()
      .HasOne(t => t.Budget)
      .WithMany(b => b.Transactions)
      .HasForeignKey(t => t.BudgetId);

    modelBuilder.Entity<Transaction>()
      .HasOne(t => t.User)
      .WithMany(u => u.Transactions)
      .HasForeignKey(t => t.UserId);

    modelBuilder.Entity<Transaction>()
      .HasOne(t => t.Budget)
      .WithMany(b => b.Transactions)
      .HasForeignKey(t => t.BudgetId);

    modelBuilder.Entity<Transaction>(entity =>
    {
      entity.Property(t => t.Amount)
        .HasPrecision(18, 2);
    });

    modelBuilder.Entity<Invoice>()
      .HasOne(i => i.Expense)
      .WithOne(e => e.Invoice)
      .HasForeignKey<Invoice>(i => i.ExpenseId);

    modelBuilder.Entity<Income>(entity =>
    {
      entity.Property(i => i.Amount)
        .HasPrecision(18, 2);
    });

    modelBuilder.Entity<Account>()
      .HasOne(a => a.User)
      .WithMany(u => u.Accounts)
      .HasForeignKey(a => a.UserId);

    modelBuilder.Entity<AccountType>()
      .HasMany(at => at.Accounts)
      .WithOne(a => a.AccountType)
      .HasForeignKey(a => a.AccountTypesId);

    modelBuilder.Entity<UserBudget>()
      .HasKey(ub => new { ub.UserId, ub.BudgetId });

    modelBuilder.Entity<UserBudget>()
      .HasOne(ub => ub.User)
      .WithMany(u => u.UserBudgets)
      .HasForeignKey(ub => ub.UserId);

    modelBuilder.Entity<UserBudget>()
      .HasOne(ub => ub.Budget)
      .WithMany(b => b.UserBudgets)
      .HasForeignKey(ub => ub.BudgetId);

    modelBuilder.Entity<Notifications>()
      .HasOne(n => n.User)
      .WithMany(u => u.Notifications)
      .HasForeignKey(n => n.UserId);

    modelBuilder.Entity<Notifications>()
      .Property(n => n.UserId)
      .IsRequired()
      .HasMaxLength(450);

    modelBuilder.Entity<Notifications>()
      .Property(n => n.Message)
      .IsRequired()
      .HasMaxLength(256);

    // 🔄 Nowa relacja: User <-> ShareBudgets (z rolą)
    modelBuilder.Entity<UserShareBudget>()
      .HasKey(usb => new { usb.UserId, usb.ShareBudgetId });

    modelBuilder.Entity<UserShareBudget>()
      .HasOne(usb => usb.User)
      .WithMany(u => u.UserShareBudgets)
      .HasForeignKey(usb => usb.UserId);

    modelBuilder.Entity<UserShareBudget>()
      .HasOne(usb => usb.ShareBudget)
      .WithMany(sb => sb.UserShareBudgets)
      .HasForeignKey(usb => usb.ShareBudgetId);
  }
}
