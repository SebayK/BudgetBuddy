using BudgetBuddy.Models;
using BudgetBuddy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase {
  private readonly NotificationsService _notificationsService;

  public NotificationsController(NotificationsService notificationsService) {
    _notificationsService = notificationsService;
  }

  // GET: api/Notifications
  [HttpGet]
  [Authorize]
  public async Task<ActionResult<IEnumerable<Notifications>>> GetAllNotificationsAsync() {
    var notifications = await _notificationsService.GetAllNotificationsAsync();
    return Ok(notifications);
  }

  // GET: api/Notifications/5
  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Notifications>> GetNotifications(int id) {
    var notifications = await _notificationsService.GetNotificationByIdAsync(id);

    if (notifications == null)
      return NotFound();

    return Ok(notifications);
  }

  // PUT: api/Notifications/5
  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> PutNotifications(int id, Notifications notifications) {
    var success = await _notificationsService.UpdateNotificationAsync(id, notifications);

    if (!success)
      return NotFound();

    return NoContent();
  }

  // POST: api/Notifications
  [HttpPost]
  [Authorize]
  public async Task<ActionResult<Notifications>> PostNotifications(Notifications notifications) {
    var createdNotifications = await _notificationsService.CreateNotificationAsync(notifications);
    return CreatedAtAction(nameof(GetNotifications), new { id = createdNotifications.Id }, createdNotifications);
  }

  // DELETE: api/Notifications/5
  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> DeleteNotifications(int id) {
    var success = await _notificationsService.DeleteNotificationAsync(id);

    if (!success)
      return NotFound();

    return NoContent();
  }
}