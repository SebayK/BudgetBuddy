using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShareBudgetController : ControllerBase {
  private readonly ShareBudgetService _shareBudgetService;

  public ShareBudgetController(ShareBudgetService shareBudgetService) {
    _shareBudgetService = shareBudgetService;
  }

  // GET: api/ShareBudget
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<ShareBudget>>> GetAllShareBudgetsAsync() {
    var shareBudget = await _shareBudgetService.GetAllShareBudgetsAsync();
    return Ok(shareBudget);
  }

  // GET: api/ShareBudget/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<ShareBudget>> GetShareBudget(int id) {
    var shareBudget = await _shareBudgetService.GetShareBudgetByIdAsync(id);

    if (shareBudget == null)
      return NotFound();

    return Ok(shareBudget);
  }

  // PUT: api/ShareBudget/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutShareBudget(int id, ShareBudget shareBudget) {
    var success = await _shareBudgetService.UpdateShareBudgetAsync(id, shareBudget);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/ShareBudget
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<ShareBudget>> PostShareBudget(ShareBudget shareBudget) {
    var createdShareBudget = await _shareBudgetService.CreateShareBudgetAsync(shareBudget);
    return CreatedAtAction(nameof(GetShareBudget), new { id = createdShareBudget.Id }, createdShareBudget);
  }

  // DELETE: api/ShareBudget/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteShareBudget(int id) {
    var success = await _shareBudgetService.DeleteShareBudgetAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}