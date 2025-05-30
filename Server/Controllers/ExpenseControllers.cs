using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase {
  private readonly ExpenseService _expenseService;

  public ExpenseController(ExpenseService expenseService) {
    _expenseService = expenseService;
  }

  // GET: api/Expense
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpensesAsync() {
    var expense = await _expenseService.GetAllExpensesAsync();
    return Ok(expense);
  }

  // GET: api/Expense/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Expense>> GetExpense(int id) {
    var expense = await _expenseService.GetExpenseByIdAsync(id);

    if (expense == null)
      return NotFound();

    return Ok(expense);
  }

  // PUT: api/Expense/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutExpense(int id, Expense expense) {
    var success = await _expenseService.UpdateExpenseAsync(id, expense);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Expense
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Expense>> PostExpense(Expense expense) {
    var createdExpense = await _expenseService.CreateExpenseAsync(expense);
    return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);
  }

  // DELETE: api/Expense/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteExpense(int id) {
    var success = await _expenseService.DeleteExpenseAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}