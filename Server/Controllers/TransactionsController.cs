using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using BudgetBuddy.Models.Dto;
using System.Runtime.InteropServices;

namespace BudgetBuddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public TransactionsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/transactions?userId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions([FromQuery] int userId)
        {
            var transactions = await _context.Transaction
                .Where(t => t.UserId == userId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type,
                    Description = t.Description,
                    Date = t.Date,
                    IsRecurring = t.IsRecurring,
                    RecurrenceInterval = t.RecurrenceInterval,
                    NextOccurrenceDate = t.NextOccurrenceDate,
                    CategoryId = t.CategoryId,
                    BudgetId = t.BudgetId
                })
                .ToListAsync();

            return Ok(transactions);
        }

        // GET: api/transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var t = await _context.Transaction.FindAsync(id);
            if (t == null)
                return NotFound();

            return new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                Date = t.Date,
                IsRecurring = t.IsRecurring,
                RecurrenceInterval = t.RecurrenceInterval,
                NextOccurrenceDate = t.NextOccurrenceDate,
                CategoryId = t.CategoryId,
                BudgetId = t.BudgetId
            };
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<ActionResult<TransactionDto>> PostTransaction([FromBody] CreateTransactionDto dto)
        {
            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Type = dto.Type,
                Description = dto.Description,
                Date = dto.Date,
                IsRecurring = dto.IsRecurring,
                RecurrenceInterval = dto.RecurrenceInterval,
                NextOccurrenceDate = dto.NextOccurrenceDate,
                CategoryId = dto.CategoryId,
                BudgetId = dto.BudgetId,
                UserId = dto.UserId
            };

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            var result = new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Type = transaction.Type,
                Description = transaction.Description,
                Date = transaction.Date,
                IsRecurring = transaction.IsRecurring,
                RecurrenceInterval = transaction.RecurrenceInterval,
                NextOccurrenceDate = transaction.NextOccurrenceDate,
                CategoryId = transaction.CategoryId,
                BudgetId = transaction.BudgetId
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, result);
        }

        // PUT: api/transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] UpdateTransactionDto dto)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
                return NotFound();

            transaction.Amount = dto.Amount;
            transaction.Type = dto.Type;
            transaction.Description = dto.Description;
            transaction.Date = dto.Date;
            transaction.IsRecurring = dto.IsRecurring;
            transaction.RecurrenceInterval = dto.RecurrenceInterval;
            transaction.NextOccurrenceDate = dto.NextOccurrenceDate;
            transaction.CategoryId = dto.CategoryId;
            transaction.BudgetId = dto.BudgetId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
                return NotFound();

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
