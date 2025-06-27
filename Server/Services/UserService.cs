using BudgetBuddy.Infrastructure;
using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Services
{
    public class UserService
    {
        private readonly BudgetContext context;

        public UserService(BudgetContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(string id, User user)
        {
            if (id != user.Id)
                return false;

            context.Entry(user).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                    return false;

                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            return await context.Users.AnyAsync(u => u.Id == id);
        }
    }
}
