using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetsController : ControllerBase {
  private readonly BudgetService _budgetService;

  public BudgetsController(BudgetService budgetService) {
    _budgetService = budgetService;
  }

  // GET: api/Budget
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Budget>>> GetAllBudgetsAsync() {
    var budget = await _budgetService.GetAllBudgetsAsync();
    return Ok(budget);
  }

  // GET: api/Budget/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Budget>> GetBudget(int id) {
    var budget = await _budgetService.GetBudgetByIdAsync(id);

    if (budget == null)
      return NotFound();

    return Ok(budget);
  }

  // PUT: api/Budget/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutBudget(int id, Budget budget) {
    var success = await _budgetService.UpdateBudgetAsync(id, budget);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Budget
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Budget>> PostBudget(Budget budget) {
    var createdBudget = await _budgetService.CreateBudgetAsync(budget);
    return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.Id }, createdBudget);
  }

  // DELETE: api/Budget/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteBudget(int id) {
    var success = await _budgetService.DeleteBudgetAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}