using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // GET: api/transaction
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    // GET: api/transaction/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        var transaction = await _transactionService.GetTransactionByIdAsync(id);

        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    // POST: api/transaction
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Transaction>> PostTransaction([FromBody] CreateTransactionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Tworzymy obiekt Transaction z DTO
        var newTransaction = new Transaction
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            BudgetId = dto.BudgetId,
            UserId = dto.UserId,
            Type = dto.Type,
            IsRecurring = dto.IsRecurring,
            RecurrenceInterval = dto.RecurrenceInterval,
            NextOccurrenceDate = dto.NextOccurrenceDate,
            CategoryId = dto.CategoryId
        };

        var createdTransaction = await _transactionService.CreateTransactionAsync(newTransaction);
        return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransaction);
    }

    // PUT: api/transaction/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutTransaction(int id, [FromBody] CreateTransactionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedTransaction = new Transaction
        {
            Id = id,
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            BudgetId = dto.BudgetId,
            UserId = dto.UserId,
            Type = dto.Type,
            IsRecurring = dto.IsRecurring,
            RecurrenceInterval = dto.RecurrenceInterval,
            NextOccurrenceDate = dto.NextOccurrenceDate,
            CategoryId = dto.CategoryId
        };

        var success = await _transactionService.UpdateTransactionAsync(id, updatedTransaction);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/transaction/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var success = await _transactionService.DeleteTransactionAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
