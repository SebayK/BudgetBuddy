using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase {
  private readonly TransactionService _transactionService;

  public TransactionController(TransactionService transactionService) {
    _transactionService = transactionService;
  }

  // GET: api/transaction
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactionsAsync() {
    var transaction = await _transactionService.GetAllTransactionsAsync();
    return Ok(transaction);
  }

  // GET: api/transaction/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Transaction>> GetTransaction(int id) {
    var transaction = await _transactionService.GetTransactionByIdAsync(id);

    if (transaction == null)
      return NotFound();

    return Ok(transaction);
  }

  // PUT: api/transaction/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutTransaction(int id, Transaction transaction) {
    var success = await _transactionService.UpdateTransactionAsync(id, transaction);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/transaction
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction) {
    var createdTransaction = await _transactionService.CreateTransactionAsync(transaction);
    return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransaction);
  }

  // DELETE: api/transaction/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteTransaction(int id) {
    var success = await _transactionService.DeleteTransactionAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}