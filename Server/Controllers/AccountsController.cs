using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase {
  private readonly AccountService _accountService;

  public AccountController(AccountService accountService) {
    _accountService = accountService;
  }

  // GET: api/Account
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Account>>> GetAllAccountsAsync() {
    var account = await _accountService.GetAllAccountsAsync();
    return Ok(account);
  }

  // GET: api/Account/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Account>> GetAccount(int id) {
    var account = await _accountService.GetAccountByIdAsync(id);

    if (account == null)
      return NotFound();

    return Ok(account);
  }

  // PUT: api/Account/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutAccount(int id, Account account) {
    var success = await _accountService.UpdateAccountAsync(id, account);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Account
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Account>> PostAccount(Account account) {
    var createdAccount = await _accountService.CreateAccountAsync(account);
    return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.Id }, createdAccount);
  }

  // DELETE: api/Account/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteAccount(int id) {
    var success = await _accountService.DeleteAccountAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}