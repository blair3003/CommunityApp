using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Services
{
    public class UserService(IUserRepository repository, UserManager<ApplicationUser> userManager)
    {
        private readonly IUserRepository _repository = repository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var allUsers = await _repository.GetAllAsync();
            return allUsers;
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            return user;
        }

        public async Task<UserDto?> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId) ?? throw new ArgumentException("User not found", nameof(userId));

                var claims = await _userManager.GetClaimsAsync(user);
                if (claims.Any(c => c.Type == "IsAdmin" && c.Value == "true"))
                {
                    throw new InvalidOperationException("User is an admin");
                }

                var deletedUser = await _repository.DeleteAsync(userId);
                return deletedUser;
            }
            catch
            {
                // _logger.LogError(ex, "Error assigning manager to community.");
                throw;
            }
        }

        public async Task<bool> AddIsManagerClaimAsync(string userId)
        {
            var succeeded = await _repository.AddIsManagerClaimAsync(userId);
            return succeeded;
        }

        public async Task<bool> RemoveIsManagerClaimAsync(string userId)
        {
            var succeeded = await _repository.RemoveIsManagerClaimAsync(userId);
            return succeeded;
        }
    }
}