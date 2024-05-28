using CommunityApp.Data.Models;

namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface ICommunityRepository : IRepository<Community, int>
    {
        Task<Community?> AssignManagerToCommunityAsync(string managerId, int communityId);
        Task<Community?> RemoveManagerFromCommunityAsync(string managerId, int communityId);
    }
}
