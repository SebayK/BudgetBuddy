using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase {
  private readonly ReportService _reportService;

  public ReportController(ReportService reportService) {
    _reportService = reportService;
  }

  // GET: api/Report
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Report>>> GetAllReportsAsync() {
    var report = await _reportService.GetAllReportsAsync();
    return Ok(report);
  }

  // GET: api/Report/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Report>> GetReport(int id) {
    var report = await _reportService.GetReportByIdAsync(id);

    if (report == null)
      return NotFound();

    return Ok(report);
  }

  // PUT: api/Report/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutReport(int id, Report report) {
    var success = await _reportService.UpdateReportAsync(id, report);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Report
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Report>> PostReport(Report report) {
    var createdReport = await _reportService.CreateReportAsync(report);
    return CreatedAtAction(nameof(GetReport), new { id = createdReport.Id }, createdReport);
  }

  // DELETE: api/Report/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteReport(int id) {
    var success = await _reportService.DeleteReportAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }

  [HttpGet("convert")]
  public async Task<IActionResult> Convert(decimal amount, string from, string to,
    [FromServices] CurrencyConverterService converter) {
    var result = await _reportService.ConvertAsync(amount, from, to, converter);
    return Ok(result);
  }
}