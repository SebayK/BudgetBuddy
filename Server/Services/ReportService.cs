using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class ReportService {
  private readonly BudgetContext _context;

  public ReportService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Report>> GetAllReportsAsync() {
    return await _context.Report.AsNoTracking().ToListAsync();
  }

  public async Task<Report?> GetReportByIdAsync(int id) {
    return await _context.Report.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
  }

  public async Task<bool> UpdateReportAsync(int id, Report report) {
    if (id != report.Id)
      return false;

    _context.Entry(report).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await ReportExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<Report> CreateReportAsync(Report report) {
    _context.Report.Add(report);
    await _context.SaveChangesAsync();
    return report;
  }

  public async Task<bool> DeleteReportAsync(int id) {
    var report = await _context.Report.FindAsync(id);
    if (report == null)
      return false;

    _context.Report.Remove(report);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> ReportExistsAsync(int id) {
    return await _context.Report.AnyAsync(r => r.Id == id);
  }

  public async Task<decimal> ConvertAsync(decimal amount, string from, string to, CurrencyConverterService converter) {
    return await converter.ConvertAsync(amount, from, to);
  }
}