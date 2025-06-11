using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoalController : ControllerBase
{
    private readonly GoalService _goalService;

    public GoalController(GoalService goalService)
    {
        _goalService = goalService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Goal>>> GetAllGoalsAsync()
    {
        var goals = await _goalService.GetAllGoalsAsync();
        return Ok(goals);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Goal>> GetGoal(int id)
    {
        var goal = await _goalService.GetGoalByIdAsync(id);

        if (goal == null)
            return NotFound();

        return Ok(goal);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutGoal(int id, Goal goal)
    {
        var success = await _goalService.UpdateGoalAsync(id, goal);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Goal>> PostGoal([FromBody] CreateGoalDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newGoal = new Goal
        {
            Name = dto.Name,
            TargetAmount = dto.TargetAmount,
            TargetDate = dto.TargetDate,
            BudgetId = dto.BudgetId,
            UserId = dto.UserId // lub: User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        };

        var createdGoal = await _goalService.CreateGoalAsync(newGoal);
        return CreatedAtAction(nameof(GetGoal), new { id = createdGoal.Id }, createdGoal);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteGoal(int id)
    {
        var success = await _goalService.DeleteGoalAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
