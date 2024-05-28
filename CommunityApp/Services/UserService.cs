using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;

namespace CommunityApp.Services
{
    public class UserService(IUserRepository repository)
    {
        private readonly IUserRepository _repository = repository;

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
            var deletedUser = await _repository.DeleteAsync(userId);
            return deletedUser;
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