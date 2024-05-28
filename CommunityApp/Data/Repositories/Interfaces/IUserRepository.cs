using CommunityApp.Data.Models;

namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserDto, string>
    {
        Task<bool> AddIsManagerClaimAsync(string userId);
        Task<bool> RemoveIsManagerClaimAsync(string userId);
    }
}
