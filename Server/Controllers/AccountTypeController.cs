using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountTypeController : ControllerBase {
  private readonly AccountTypeService _accountTypeService;

  public AccountTypeController(AccountTypeService accountTypeService) {
    _accountTypeService = accountTypeService;
  }

  // GET: api/AccountType
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<AccountType>>> GetAllAccountTypesAsync() {
    var accountType = await _accountTypeService.GetAllAccountTypesAsync();
    return Ok(accountType);
  }

  // GET: api/AccountType/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<AccountType>> GetAccountType(int id) {
    var accountType = await _accountTypeService.GetAccountTypeByIdAsync(id);

    if (accountType == null)
      return NotFound();

    return Ok(accountType);
  }

  // PUT: api/AccountType/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutAccountType(int id, AccountType accountType) {
    var success = await _accountTypeService.UpdateAccountTypeAsync(id, accountType);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/AccountType
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<AccountType>> PostAccountType(AccountType accountType) {
    var createdAccountType = await _accountTypeService.CreateAccountTypeAsync(accountType);
    return CreatedAtAction(nameof(GetAccountType), new { id = createdAccountType.Id }, createdAccountType);
  }

  // DELETE: api/AccountType/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteAccountTypeAsync(int id) {
    var success = await _accountTypeService.DeleteAccountTypeAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}