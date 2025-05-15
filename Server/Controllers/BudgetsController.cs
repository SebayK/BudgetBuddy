using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.AspNetCore.Authorization;

namespace BudgetBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public BudgetsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Budgets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudget()
        {
            return await _context.Budget.ToListAsync();
        }

        // GET: api/Budgets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Budget>> GetBudget(int id)
        {
            var budget = await _context.Budget.FindAsync(id);

            if (budget == null)
            {
                return NotFound();
            }

            return budget;
        }

        // PUT: api/Budgets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBudget(int id, Budget budget)
        {
            if (id != budget.Id)
            {
                return BadRequest();
            }

            _context.Entry(budget).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Budgets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
            _context.Budget.Add(budget);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBudget", new { id = budget.Id }, budget);
        }

        // DELETE: api/Budgets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _context.Budget.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            _context.Budget.Remove(budget);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BudgetExists(int id)
        {
            return _context.Budget.Any(e => e.Id == id);
        }
    }
}
