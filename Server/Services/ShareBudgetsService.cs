using BudgetBuddy.DTO;
using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services;

public class ShareBudgetsService(BudgetContext context)
{
    public async Task<IEnumerable<ShareBudgets>> GetAllShareBudgetsAsync(string userId)
    {
        return await context.ShareBudgets
            .AsNoTracking()
            .Include(sb => sb.UserShareBudgets)
                .ThenInclude(usb => usb.User)
            .Where(sb => sb.UserShareBudgets.Any(usb => usb.UserId == userId))
            .ToListAsync();
    }

    public async Task<ShareBudgets?> GetShareBudgetByIdAsync(int id, string userId)
    {
        return await context.ShareBudgets
            .AsNoTracking()
            .Include(sb => sb.UserShareBudgets)
                .ThenInclude(usb => usb.User)
            .Where(sb => sb.UserShareBudgets.Any(usb => usb.UserId == userId))
            .FirstOrDefaultAsync(sb => sb.Id == id);
    }

    public async Task<bool> UpdateShareBudgetAsync(int id, ShareBudgets shareBudgets, string userId)
    {
        if (id != shareBudgets.Id)
            return false;

        var existing = await context.ShareBudgets
            .Include(sb => sb.UserShareBudgets)
            .FirstOrDefaultAsync(sb => sb.Id == id && sb.UserShareBudgets.Any(usb => usb.UserId == userId));

        if (existing == null)
            return false;

        existing.Name = shareBudgets.Name;
        // użytkowników i OwnerId nie ruszamy tutaj

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ShareBudgets> CreateShareBudgetAsync(
        ShareBudgets shareBudgets,
        string userId,
        List<UserShareBudgetCreateDto>? extraUsers = null)
    {
        if (shareBudgets == null)
            throw new ArgumentNullException(nameof(shareBudgets));

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        var owner = await context.Users.FindAsync(userId);
        if (owner == null)
            throw new InvalidOperationException("Nie znaleziono właściciela budżetu.");

        shareBudgets.OwnerUserId = userId;
        shareBudgets.CreatedAt = DateTime.UtcNow;

        shareBudgets.UserShareBudgets = new List<UserShareBudget>
        {
            new UserShareBudget
            {
                UserId = userId,
                Role = "Owner"
            }
        };

        if (extraUsers != null)
        {
            foreach (var userDto in extraUsers)
            {
                if (userDto.UserId == userId) continue; // pomiń właściciela

                var user = await context.Users.FindAsync(userDto.UserId);
                if (user != null)
                {
                    shareBudgets.UserShareBudgets.Add(new UserShareBudget
                    {
                        UserId = user.Id,
                        Role = userDto.Role
                    });
                }
                // ewentualnie logowanie błędu, jeśli użytkownik nie istnieje
            }
        }

        context.ShareBudgets.Add(shareBudgets);
        await context.SaveChangesAsync();

        return shareBudgets;
    }

    public async Task<bool> DeleteShareBudgetAsync(int id, string userId)
    {
        var shareBudget = await context.ShareBudgets
            .Include(sb => sb.UserShareBudgets)
            .FirstOrDefaultAsync(sb => sb.Id == id && sb.OwnerUserId == userId);

        if (shareBudget == null)
            return false;

        context.ShareBudgets.Remove(shareBudget);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    private async Task<bool> ShareBudgetExistsAsync(int id)
    {
        return await context.ShareBudgets.AnyAsync(sb => sb.Id == id);
    }
}
