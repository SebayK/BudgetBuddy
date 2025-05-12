using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using BudgetBuddy.Models.Dto;

namespace BudgetBuddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public GoalsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/goals?userId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalDto>>> GetGoals([FromQuery] int userId)
        {
            var goals = await _context.Goal
                .Where(g => g.UserId == userId)
                .Select(g => new GoalDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    TargetAmount = g.TargetAmount,
                    CurrentAmount = g.CurrentAmount,
                    Deadline = g.Deadline
                })
                .ToListAsync();

            return goals;
        }

        // GET: api/goals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GoalDto>> GetGoal(int id)
        {
            var goal = await _context.Goal.FindAsync(id);
            if (goal == null)
                return NotFound();

            var dto = new GoalDto
            {
                Id = goal.Id,
                Name = goal.Name,
                TargetAmount = goal.TargetAmount,
                CurrentAmount = goal.CurrentAmount,
                Deadline = goal.Deadline
            };

            return dto;
        }

        // POST: api/goals
        [HttpPost]
        public async Task<ActionResult<GoalDto>> PostGoal([FromBody] CreateGoalDto dto)
        {
            var goal = new Goal
            {
                Name = dto.Name,
                Description = dto.Description,
                TargetAmount = dto.TargetAmount,
                CurrentAmount = 0,
                Deadline = dto.Deadline,
                BudgetId = dto.BudgetId,
                UserId = dto.UserId
            };

            _context.Goal.Add(goal);
            await _context.SaveChangesAsync();

            var result = new GoalDto
            {
                Id = goal.Id,
                Name = goal.Name,
                TargetAmount = goal.TargetAmount,
                CurrentAmount = goal.CurrentAmount,
                Deadline = goal.Deadline
            };

            return CreatedAtAction(nameof(GetGoal), new { id = goal.Id }, result);
        }

        // PUT: api/goals/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGoal(int id, [FromBody] UpdateGoalDto dto)
        {
            var goal = await _context.Goal.FindAsync(id);
            if (goal == null)
                return NotFound();

            goal.Name = dto.Name;
            goal.Description = dto.Description;
            goal.TargetAmount = dto.TargetAmount;
            goal.CurrentAmount = dto.CurrentAmount;
            goal.Deadline = dto.Deadline;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/goals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(int id)
        {
            var goal = await _context.Goal.FindAsync(id);
            if (goal == null)
                return NotFound();

            _context.Goal.Remove(goal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
