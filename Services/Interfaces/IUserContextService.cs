using ExpenseTracker.Models;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IUserContextService
    {
        public Task<string?> GetCurrentUserIdAsync();
        public Task DeleteUser(string userId);
        public Task<ApplicationUser?> GetCurrentUserAsync();
        public Task CreateUserAsync(ApplicationUser user);

    }
}
