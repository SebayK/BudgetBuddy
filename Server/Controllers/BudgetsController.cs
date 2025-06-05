using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetsController : ControllerBase {
  private readonly BudgetService _budgetService;
  private readonly IHttpContextAccessor _httpContextAccessor;
  
  private string? UserId => _httpContextAccessor.HttpContext?.Items["UserId"] as string;

  public BudgetsController (BudgetService budgetService, IHttpContextAccessor httpContextAccessor) {
    _budgetService = budgetService;
    _httpContextAccessor = httpContextAccessor;
  }

  // GET: api/Budget
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Budget>>> GetAllBudgetsAsync() {
    var budget = await _budgetService.GetAllBudgetsAsync(UserId);
    return Ok(budget);
  }

  // GET: api/Budget/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Budget>> GetBudget(int id) {
    var budget = await _budgetService.GetBudgetByIdAsync(id, UserId);

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
    var userId = UserId;
    var createdBudget = await _budgetService.CreateBudgetAsync(budget, userId);
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