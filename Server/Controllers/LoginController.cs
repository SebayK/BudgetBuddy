
using BudgetBuddy.Enums;
using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase {
  private readonly IAuthorizationService _authService;
  private readonly ILogger<AuthenticationController> _logger;

  public AuthenticationController(IAuthorizationService authService, ILogger<AuthenticationController> logger) {
    _authService = authService;
    _logger = logger;
  }


  [HttpPost]
  [Route("login")]
  public async Task<IActionResult> Login(LoginModel model) {
    try {
      if (!ModelState.IsValid)
        return BadRequest("Invalid payload");
      var (status, message) = await _authService.Login(model);
      if (status == 0)
        return BadRequest(message);
      return Ok(message);
    }
    catch (Exception ex) {
      _logger.LogError(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }

  [HttpPost]
  [Route("registration")]
  public async Task<IActionResult> Register(RegistrationModel model) {
    try {
      if (!ModelState.IsValid)
        return BadRequest("Invalid payload");
      
      var (status, message) = await _authService.Registration(model, model.Role);
      if (status == 0) {
        return BadRequest(message);
      }

      return CreatedAtAction(nameof(Register), model);
    }
    catch (Exception ex) {
      _logger.LogError(ex.Message);
      return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
    }
  }
}