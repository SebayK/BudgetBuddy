using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GoalControllers : ControllerBase {
  private readonly BudgetContext _context;

  public GoalControllers(BudgetContext context) {
    _context = context;
  }

  // GET: api/GoalControllers
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Goal>>> GetGoal() {
    return await _context.Goal.ToListAsync();
  }

  // GET: api/GoalControllers/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Goal>> GetGoal(int id) {
    var goal = await _context.Goal.FindAsync(id);

    if (goal == null) return NotFound();

    return goal;
  }

  // PUT: api/GoalControllers/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutGoal(int id, Goal goal) {
    if (id != goal.Id) return BadRequest();

    _context.Entry(goal).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) {
      if (!GoalExists(id)) return NotFound();

      throw;
    }

    return NoContent();
  }

  // POST: api/GoalControllers
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Goal>> PostGoal(Goal goal) {
    _context.Goal.Add(goal);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetGoal", new { id = goal.Id }, goal);
  }

  // DELETE: api/GoalControllers/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteGoal(int id) {
    var goal = await _context.Goal.FindAsync(id);
    if (goal == null) return NotFound();

    _context.Goal.Remove(goal);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool GoalExists(int id) {
    return _context.Goal.Any(e => e.Id == id);
  }
}