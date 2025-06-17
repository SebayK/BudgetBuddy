using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetBalanceController : ControllerBase
{
    private readonly BudgetContext _context;

    public BudgetBalanceController(BudgetContext context)
    {
        _context = context;
    }

    // GET: api/BudgetBalance/5
    [HttpGet("{budgetId}")]
    public async Task<ActionResult<object>> GetBalance(int budgetId)
    {
        // Pobierz ID zalogowanego użytkownika z JWT
        var userId = User.FindFirst("id")?.Value;
        if (userId == null)
            return Unauthorized("Nie udało się pobrać ID użytkownika.");

        // Pobierz wszystkie dochody użytkownika przypisane do danego budżetu
        var incomes = await _context.Incomes
            .Where(i => i.UserId == userId && i.BudgetId == budgetId)
            .ToListAsync();

        // Pobierz wszystkie wydatki użytkownika przypisane do danego budżetu
        var expenses = await _context.Expenses
            .Where(e => e.UserId == userId && e.BudgetId == budgetId)
            .ToListAsync();

        // Oblicz saldo = suma dochodów - suma wydatków
        var totalIncome = incomes.Sum(i => i.Amount);
        var totalExpense = expenses.Sum(e => e.Amount);
        var balance = totalIncome - totalExpense;

        // Zwróć wynik
        return Ok(new
        {
            budgetId = budgetId,
            balance = balance
        });
    }
}