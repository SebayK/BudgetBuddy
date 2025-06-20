using BudgetBuddy.DTO;
using BudgetBuddy.Models;
using BudgetBuddy.Models.DTO;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShareBudgetsController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ShareBudgetsService _shareBudgetsService;

    public ShareBudgetsController(ShareBudgetsService shareBudgetsService, IHttpContextAccessor httpContextAccessor)
    {
        _shareBudgetsService = shareBudgetsService;
        _httpContextAccessor = httpContextAccessor;
    }

    private string? UserId => _httpContextAccessor.HttpContext?.Items["UserId"] as string;

    // GET: api/ShareBudgets
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ShareBudgetDto>>> GetAllShareBudgetsAsync()
    {
        var shareBudgets = await _shareBudgetsService.GetAllShareBudgetsAsync(UserId);

        var result = shareBudgets.Select(sb => new ShareBudgetDto
        {
            Id = sb.Id,
            Name = sb.Name,
            OwnerUserId = sb.OwnerUserId,
            CreatedAt = sb.CreatedAt,
            Users = sb.UserShareBudgets.Select(usb => new UserWithRoleDto
            {
                Role = usb.Role,
                User = new UserDto
                {
                    Id = usb.User.Id,
                    Email = usb.User.Email ?? "",
                    FirstName = usb.User.FirstName,
                    LastName = usb.User.LastName,
                    AccountId = usb.User.AccountId,
                    Role = usb.User.Role.ToString()
                }
            }).ToList()
        });

        return Ok(result);
    }

    // GET: api/ShareBudgets/5
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ShareBudgetDto>> GetShareBudget(int id)
    {
        var sb = await _shareBudgetsService.GetShareBudgetByIdAsync(id, UserId);
        if (sb == null)
            return NotFound();

        var dto = new ShareBudgetDto
        {
            Id = sb.Id,
            Name = sb.Name,
            OwnerUserId = sb.OwnerUserId,
            CreatedAt = sb.CreatedAt,
            Users = sb.UserShareBudgets.Select(usb => new UserWithRoleDto
            {
                Role = usb.Role,
                User = new UserDto
                {
                    Id = usb.User.Id,
                    Email = usb.User.Email ?? "",
                    FirstName = usb.User.FirstName,
                    LastName = usb.User.LastName,
                    AccountId = usb.User.AccountId,
                    Role = usb.User.Role.ToString()
                }
            }).ToList()
        };

        return Ok(dto);
    }

    // PUT: api/ShareBudgets/5
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutShareBudget(int id, ShareBudgetCreateDto dto)
    {
        var shareBudget = new ShareBudgets
        {
            Id = id,
            Name = dto.Name
            // użytkowników i właściciela nie edytujemy tutaj – tylko nazwę
        };

        var success = await _shareBudgetsService.UpdateShareBudgetAsync(id, shareBudget, UserId);
        if (!success)
            return NotFound();

        return NoContent();
    }

    // POST: api/ShareBudgets
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ShareBudgetDto>> PostShareBudget(ShareBudgetCreateDto dto)
    {
        var shareBudget = new ShareBudgets
        {
            Name = dto.Name
        };

        var created = await _shareBudgetsService.CreateShareBudgetAsync(shareBudget, UserId, dto.Users);

        var result = new ShareBudgetDto
        {
            Id = created.Id,
            Name = created.Name,
            OwnerUserId = created.OwnerUserId,
            CreatedAt = created.CreatedAt,
            Users = created.UserShareBudgets.Select(usb => new UserWithRoleDto
            {
                Role = usb.Role,
                User = new UserDto
                {
                    Id = usb.User.Id,
                    Email = usb.User.Email ?? "",
                    FirstName = usb.User.FirstName,
                    LastName = usb.User.LastName,
                    AccountId = usb.User.AccountId,
                    Role = usb.User.Role.ToString()
                }
            }).ToList()
        };

        return CreatedAtAction(nameof(GetShareBudget), new { id = result.Id }, result);
    }

    // DELETE: api/ShareBudgets/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteShareBudget(int id)
    {
        var success = await _shareBudgetsService.DeleteShareBudgetAsync(id, UserId);
        if (!success)
            return NotFound();

        return NoContent();
    }
}
