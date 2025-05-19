using BudgetBuddy.Enums;
using Microsoft.AspNetCore.Mvc;
using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;

namespace BudgetBuddy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            var success = await _userService.UpdateUserAsync(id, user);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var success = await _userService.DeleteUserAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
