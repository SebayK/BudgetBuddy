using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase {
  private readonly BudgetContext _context;

  public ReportController(BudgetContext context) {
    _context = context;
  }

  // GET: api/Report
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Report>>> GetReport() {
    return await _context.Report.ToListAsync();
  }

  // GET: api/Report/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Report>> GetReport(int id) {
    var report = await _context.Report.FindAsync(id);

    if (report == null) return NotFound();

    return report;
  }

  // PUT: api/Report/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutReport(int id, Report report) {
    if (id != report.Id) return BadRequest();

    _context.Entry(report).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) {
      if (!ReportExists(id)) return NotFound();

      throw;
    }

    return NoContent();
  }

  // POST: api/Report
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Report>> PostReport(Report report) {
    _context.Report.Add(report);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetReport", new { id = report.Id }, report);
  }

  // DELETE: api/Report/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteReport(int id) {
    var report = await _context.Report.FindAsync(id);
    if (report == null) return NotFound();

    _context.Report.Remove(report);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool ReportExists(int id) {
    return _context.Report.Any(e => e.Id == id);
  }
}