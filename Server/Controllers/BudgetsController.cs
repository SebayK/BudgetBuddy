using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using BudgetBuddy.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetsController : ControllerBase {
  private readonly BudgetService _budgetService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  private string? UserId => _httpContextAccessor.HttpContext?.Items["UserId"] as string;

  public BudgetsController(BudgetService budgetService, IHttpContextAccessor httpContextAccessor) {
    _budgetService = budgetService;
    _httpContextAccessor = httpContextAccessor;
  }

  // ✅ GET: api/Budgets (zwraca uproszczoną listę)
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<object>>> GetAllBudgetsAsync() {
    var budgets = await _budgetService.GetAllBudgetsAsync(UserId);

    var simplified = budgets.Select(b => new {
      id = b.Id,
      name = b.Name ?? $"Konto {b.Id}"
    });

    return Ok(simplified);
  }

  // GET: api/Budgets/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Budget>> GetBudget(int id) {
    var budget = await _budgetService.GetBudgetByIdAsync(id, UserId);

    if (budget == null)
      return NotFound();

    return Ok(budget);
  }

  // PUT: api/Budgets/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutBudget(int id, Budget budget) {
    var success = await _budgetService.UpdateBudgetAsync(id, budget, UserId);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Budgets
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Budget>> PostBudget([FromBody] CreateBudgetDto dto) {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var newBudget = new Budget {
      TotalAmount = dto.TotalAmount,
      Name = dto.Name,
      UserBudgets = dto.Users.Select(u => new UserBudget {
        UserId = u.UserId,
        Role = Enum.TryParse<UserBudgetRole>(u.Role, true, out var parsedRole)
          ? parsedRole
          : UserBudgetRole.Owner
      }).ToList()
    };

    var createdBudget = await _budgetService.CreateBudgetAsync(newBudget, UserId);

    return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.Id }, createdBudget);
  }

  // DELETE: api/Budgets/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteBudget(int id) {
    var success = await _budgetService.DeleteBudgetAsync(id, UserId);

    if (!success)
      return NotFound();

    return NoContent();
  }
}