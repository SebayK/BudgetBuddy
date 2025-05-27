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
            return await context.User.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await context.User.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            context.User.Add(user);
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
            var user = await context.User.FindAsync(id);
            if (user == null)
                return false;

            context.User.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UserExistsAsync(string id)
        {
            return await context.User.AnyAsync(u => u.Id == id);
        }
    }
}
