using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomeController : ControllerBase {
  private readonly BudgetContext _context;

  public IncomeController(BudgetContext context) {
    _context = context;
  }

  // GET: api/Income
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Incomes>>> GetIncomes() {
    return await _context.Incomes.ToListAsync();
  }

  // GET: api/Income/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Incomes>> GetIncomes(int id) {
    var incomes = await _context.Incomes.FindAsync(id);

    if (incomes == null) return NotFound();

    return incomes;
  }

  // PUT: api/Income/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutIncomes(int id, Incomes incomes) {
    if (id != incomes.Id) return BadRequest();

    _context.Entry(incomes).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) {
      if (!IncomesExists(id)) return NotFound();

      throw;
    }

    return NoContent();
  }

  // POST: api/Income
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Incomes>> PostIncomes(Incomes incomes) {
    _context.Incomes.Add(incomes);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetIncomes", new { id = incomes.Id }, incomes);
  }

  // DELETE: api/Income/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteIncomes(int id) {
    var incomes = await _context.Incomes.FindAsync(id);
    if (incomes == null) return NotFound();

    _context.Incomes.Remove(incomes);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool IncomesExists(int id) {
    return _context.Incomes.Any(e => e.Id == id);
  }
}