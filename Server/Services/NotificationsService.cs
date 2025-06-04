using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class NotificationsService {
  private readonly BudgetContext _context;

  public NotificationsService(BudgetContext context) {
    _context = context;
  }

  public async Task<IEnumerable<Notifications>> GetAllNotificationsAsync() {
    return await _context.Notifications.AsNoTracking().ToListAsync();
  }

  public async Task<Notifications?> GetNotificationByIdAsync(int id) {
    return await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
  }

  public async Task<bool> UpdateNotificationAsync(int id, Notifications notification) {
    if (id != notification.Id)
      return false;

    _context.Entry(notification).State = EntityState.Modified;

    try {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (DbUpdateConcurrencyException) {
      if (!await NotificationExistsAsync(id))
        return false;

      throw;
    }
  }

  public async Task<Notifications> CreateNotificationAsync(Notifications notification) {
    _context.Notifications.Add(notification);
    await _context.SaveChangesAsync();
    return notification;
  }

  public async Task<bool> DeleteNotificationAsync(int id) {
    var notification = await _context.Notifications.FindAsync(id);
    if (notification == null)
      return false;

    _context.Notifications.Remove(notification);
    await _context.SaveChangesAsync();
    return true;
  }

  private async Task<bool> NotificationExistsAsync(int id) {
    return await _context.Notifications.AnyAsync(n => n.Id == id);
  }
}