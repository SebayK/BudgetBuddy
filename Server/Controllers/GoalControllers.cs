using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoalController : ControllerBase {
  private readonly GoalService _goalService;

  public GoalController(GoalService goalService) {
    _goalService = goalService;
  }

  // GET: api/Goal
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Goal>>> GetAllGoalsAsync() {
    var goal = await _goalService.GetAllGoalsAsync();
    return Ok(goal);
  }

  // GET: api/Goal/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Goal>> GetGoal(int id) {
    var goal = await _goalService.GetGoalByIdAsync(id);

    if (goal == null)
      return NotFound();

    return Ok(goal);
  }

  // PUT: api/Goal/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutGoal(int id, Goal goal) {
    var success = await _goalService.UpdateGoalAsync(id, goal);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Goal
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Goal>> PostGoal(Goal goal) {
    var createdGoal = await _goalService.CreateGoalAsync(goal);
    return CreatedAtAction(nameof(GetGoal), new { id = createdGoal.Id }, createdGoal);
  }

  // DELETE: api/Goal/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteGoal(int id) {
    var success = await _goalService.DeleteGoalAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}