using CommunityApp.Data.Models;

namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface ICommunityManagerRepository
    {
        Task<List<Community>> GetCommunitiesByManagerIdAsync(string managerId);
        Task<bool> AddManagerToCommunityAsync(string managerId, int communityId);
        Task<bool> RemoveManagerFromCommunityAsync(string managerId, int communityId);
    }
}