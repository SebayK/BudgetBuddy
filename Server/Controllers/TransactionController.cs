using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase {
  private readonly BudgetContext _context;

  public TransactionController(BudgetContext context) {
    _context = context;
  }

  // GET: api/Tramsaction
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction() {
    return await _context.Transaction.ToListAsync();
  }

  // GET: api/Tramsaction/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Transaction>> GetTransaction(int id) {
    var transaction = await _context.Transaction.FindAsync(id);

    if (transaction == null) return NotFound();

    return transaction;
  }

  // PUT: api/Tramsaction/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutTransaction(int id, Transaction transaction) {
    if (id != transaction.Id) return BadRequest();

    _context.Entry(transaction).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) {
      if (!TransactionExists(id)) return NotFound();

      throw;
    }

    return NoContent();
  }

  // POST: api/Tramsaction
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction) {
    _context.Transaction.Add(transaction);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
  }

  // DELETE: api/Tramsaction/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteTransaction(int id) {
    var transaction = await _context.Transaction.FindAsync(id);
    if (transaction == null) return NotFound();

    _context.Transaction.Remove(transaction);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool TransactionExists(int id) {
    return _context.Transaction.Any(e => e.Id == id);
  }
}