using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomeController : ControllerBase {
  private readonly IncomeService _incomeService;

  public IncomeController(IncomeService incomeService) {
    _incomeService = incomeService;
  }

  // GET: api/Income
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Income>>> GetAllIncomesAsync() {
    var income = await _incomeService.GetAllIncomesAsync();
    return Ok(income);
  }

  // GET: api/Income/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Income>> GetIncome(int id) {
    var income = await _incomeService.GetIncomeByIdAsync(id);

    if (income == null)
      return NotFound();

    return Ok(income);
  }

  // PUT: api/Income/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutIncome(int id, Income income) {
    var success = await _incomeService.UpdateIncomeAsync(id, income);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Income
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Income>> PostIncome(Income income) {
    var createdIncome = await _incomeService.CreateIncomeAsync(income);
    return CreatedAtAction(nameof(GetIncome), new { id = createdIncome.Id }, createdIncome);
  }

  // DELETE: api/Income/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteIncome(int id) {
    var success = await _incomeService.DeleteIncomeAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}