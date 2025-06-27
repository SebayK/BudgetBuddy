using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
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
        var accounts = await _accountService.GetAllAccountsAsync();
        return Ok(accounts);
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
    public async Task<IActionResult> PutAccount(int id, AccountDto dto) {
        // Pobieramy zależne obiekty z bazy danych
        var user = await _accountService.GetUserByIdAsync(dto.UserId);
        var accountType = await _accountService.GetAccountTypeByIdAsync(dto.AccountTypesId);

        if (user == null || accountType == null)
            return BadRequest("Nieprawidłowe dane: brak użytkownika lub typu konta.");

        var updatedAccount = new Account {
            Id = id,
            UserId = dto.UserId,
            AccountNumber = dto.AccountNumber,
            AccountTypesId = dto.AccountTypesId,
            CurrencyId = dto.CurrencyId,
            User = user,
            AccountType = accountType
        };

        var success = await _accountService.UpdateAccountAsync(id, updatedAccount);

        if (!success)
            return NotFound();

        return NoContent();
    }

    // POST: api/Account
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Account>> PostAccount(AccountDto dto) {
        // Pobieramy zależne obiekty z bazy danych
        var user = await _accountService.GetUserByIdAsync(dto.UserId);
        var accountType = await _accountService.GetAccountTypeByIdAsync(dto.AccountTypesId);

        if (user == null || accountType == null)
            return BadRequest("Nieprawidłowe dane: brak użytkownika lub typu konta.");

        var newAccount = new Account {
            UserId = dto.UserId,
            AccountNumber = dto.AccountNumber,
            AccountTypesId = dto.AccountTypesId,
            CurrencyId = dto.CurrencyId,
            User = user,
            AccountType = accountType
        };

        var createdAccount = await _accountService.CreateAccountAsync(newAccount);

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
