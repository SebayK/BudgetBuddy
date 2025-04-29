using BudgetBuddy.Models;
using BudgetBuddy.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly BudgetContext _context;

    public ExpensesController(BudgetContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Expense>> GetExpenses()
    {
        return Ok(_context.Expenses.ToList());
    }

    [HttpPost]
    public ActionResult<Expense> AddExpense(Expense expense)
    {
        _context.Expenses.Add(expense);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetExpenses), new { id = expense.Id }, expense);
    }
}
