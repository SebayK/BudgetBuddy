using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BudgetsController : ControllerBase
{
    private readonly BudgetService _budgetService;

    public BudgetsController(BudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Budget>>> GetAllBudgetsAsync()
    {
        var budgets = await _budgetService.GetAllBudgetsAsync();
        return Ok(budgets);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Budget>> GetBudget(int id)
    {
        var budget = await _budgetService.GetBudgetByIdAsync(id);

        if (budget == null)
            return NotFound();

        return Ok(budget);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutBudget(int id, Budget budget)
    {
        var success = await _budgetService.UpdateBudgetAsync(id, budget);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Budget>> PostBudget([FromBody] CreateBudgetDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newBudget = new Budget
        {
            TotalAmount = dto.TotalAmount,
            UserBudgets = dto.Users.Select(u => new UserBudget
            {
                UserId = u.UserId,
                Role = string.IsNullOrWhiteSpace(u.Role) ? "Owner" : u.Role
            }).ToList()
        };

        var createdBudget = await _budgetService.CreateBudgetAsync(newBudget);

        return CreatedAtAction(nameof(GetBudget), new { id = createdBudget.Id }, createdBudget);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBudget(int id)
    {
        var success = await _budgetService.DeleteBudgetAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
