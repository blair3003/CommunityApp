using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Services
{
    public class CommunityManagerService(ICommunityManagerRepository repository, UserManager<ApplicationUser> userManager)
    {
        private readonly ICommunityManagerRepository _repository = repository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<List<Community>> GetCommunitiesByManagerIdAsync(string managerId)
        {
            var managerCommunities = await _repository.GetCommunitiesByManagerIdAsync(managerId);
            return managerCommunities;
        }

        public async Task<bool> AddManagerToCommunityAsync(string managerId, int communityId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(managerId) ?? throw new ArgumentException("User not found", nameof(managerId));

                var claims = await _userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == "IsManager" && c.Value == "true"))
                {
                    throw new InvalidOperationException("User is not a manager");
                }

                var managerAdded = await _repository.AddManagerToCommunityAsync(managerId, communityId);
                if (!managerAdded)
                {
                    throw new InvalidOperationException("Failed to add manager to the community");
                }

                return managerAdded;
            }
            catch
            {
                // _logger.LogError(ex, "Error assigning manager to community.");
                throw;
            }
        }

        public async Task<bool> RemoveManagerFromCommunityAsync(string managerId, int communityId)
        {
            try
            {
                var managerRemoved = await _repository.RemoveManagerFromCommunityAsync(managerId, communityId);
                if (!managerRemoved)
                {
                    throw new InvalidOperationException("Failed to remove manager from the community");
                }

                return managerRemoved;
            }
            catch
            {
                // _logger.LogError(ex, "Error removing manager from community.");
                throw;
            }
        }
    }
}