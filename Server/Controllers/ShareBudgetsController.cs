using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShareBudgetsController : ControllerBase {
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ShareBudgetsService _shareBudgetsService;

  public ShareBudgetsController(ShareBudgetsService shareBudgetsService, IHttpContextAccessor httpContextAccessor) {
    _shareBudgetsService = shareBudgetsService;
    _httpContextAccessor = httpContextAccessor;
  }

  private string? UserId => _httpContextAccessor.HttpContext?.Items["UserId"] as string;

  // GET: api/ShareBudget
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<ShareBudgets>>> GetAllShareBudgetsAsync() {
    var shareBudgets = await _shareBudgetsService.GetAllShareBudgetsAsync(UserId);
    return Ok(shareBudgets);
  }

  // GET: api/ShareBudget/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<ShareBudgets>> GetShareBudget(int id) {
    var shareBudget = await _shareBudgetsService.GetShareBudgetByIdAsync(id, UserId);

    if (shareBudget == null)
      return NotFound();

    return Ok(shareBudget);
  }

  // PUT: api/ShareBudget/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutShareBudget(int id, ShareBudgets shareBudgets) {
    var success = await _shareBudgetsService.UpdateShareBudgetAsync(id, shareBudgets, UserId);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/ShareBudget
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<ShareBudgets>> PostShareBudget(ShareBudgets shareBudgets) {
    var createdShareBudget = await _shareBudgetsService.CreateShareBudgetAsync(shareBudgets, UserId);
    return CreatedAtAction(nameof(GetShareBudget), new { id = createdShareBudget.Id }, createdShareBudget);
  }

  // DELETE: api/ShareBudget/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteShareBudget(int id) {
    var success = await _shareBudgetsService.DeleteShareBudgetAsync(id, UserId);

    if (!success)
      return NotFound();

    return NoContent();
  }
}