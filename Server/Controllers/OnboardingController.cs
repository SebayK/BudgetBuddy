using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;

namespace BudgetBuddy.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class OnboardingController : ControllerBase {
    private readonly BudgetContext _context;

    public OnboardingController(BudgetContext context) {
      _context = context;
    }

    [HttpGet("Check")]
    public async Task<IActionResult> CheckIfUserHasData() {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (string.IsNullOrEmpty(userId))
        return Unauthorized();

      // ✅ Czy użytkownik ma jakiekolwiek konto?
      var hasAccounts = await _context.Accounts.AnyAsync(a => a.UserId == userId);

      // ✅ Czy użytkownik ma przypisany jakikolwiek budżet (nawet współdzielony)?
      var hasBudgets = await _context.UserBudgets.AnyAsync(ub => ub.UserId == userId);

      // ✅ Pobranie imienia użytkownika (nazwa logowania)
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

      return Ok(new {
        hasAccounts,
        hasBudget = hasBudgets,
        userName = user?.UserName ?? "Użytkowniku"
      });
    }
  }
}