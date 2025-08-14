using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task<string?> GetCurrentUserIdAsync()
        {
            ApplicationUser? user = await GetCurrentUserAsync();
            return user?.Id;
        }

        public async Task DeleteUser(string userId)
        {
            ApplicationUser? user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return _userManager.GetUserAsync(user!);
        }

        public async Task CreateUserAsync(ApplicationUser user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
