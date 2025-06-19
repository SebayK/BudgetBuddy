using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public ExpenseController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    // GET: api/Expense
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpensesAsync()
    {
        var expenses = await _expenseService.GetAllExpensesAsync();
        return Ok(expenses);
    }

    // GET: api/Expense/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Expense>> GetExpense(int id)
    {
        var expense = await _expenseService.GetExpenseByIdAsync(id);

        if (expense == null)
            return NotFound();

        return Ok(expense);
    }

    // PUT: api/Expense/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutExpense(int id, Expense expense)
    {
        var success = await _expenseService.UpdateExpenseAsync(id, expense);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // POST: api/Expense
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Expense>> PostExpense([FromBody] CreateExpenseDto expense)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newExpense = new Expense
        {
            Name = expense.Name,
            Amount = expense.Amount,
            UserId = expense.UserId,
            Date = expense.Date,
            CategoryId = expense.CategoryId,
            InvoiceId = expense.InvoiceId
        };

        var createdExpense = await _expenseService.CreateExpenseAsync(newExpense);

        return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);
    }

    // DELETE: api/Expense/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var success = await _expenseService.DeleteExpenseAsync(id);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
