using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Services
{
    public class CommunityService(ICommunityRepository repository, UserManager<ApplicationUser> userManager)
    {
        private readonly ICommunityRepository _repository = repository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<List<Community>> GetAllCommunitiesAsync()
        {
            var allCommunities = await _repository.GetAllAsync();
            return allCommunities;
        }

        public async Task<Community?> GetCommunityByIdAsync(int id)
        {
            var community = await _repository.GetByIdAsync(id);
            return community;
        }

        public async Task<Community?> AddCommunityAsync(Community community)
        {
            var newCommunity = await _repository.AddAsync(community);
            return newCommunity;
        }

        public async Task<Community?> UpdateCommunityAsync(int id, Community community)
        {
            var updatedCommunity = await _repository.UpdateAsync(id, community);
            return updatedCommunity;
        }

        public async Task<Community?> DeleteCommunityAsync(int id)
        {
            var deletedCommunity = await _repository.DeleteAsync(id);
            return deletedCommunity;
        }

        public async Task<Community?> AssignManagerToCommunityAsync(string managerId, int communityId)
        {
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var user = await _userManager.FindByIdAsync(managerId) ?? throw new ArgumentException("User not found", nameof(managerId));

                var claims = await _userManager.GetClaimsAsync(user);
                if (!claims.Any(c => c.Type == "IsManager" && c.Value == "true"))
                {
                    throw new InvalidOperationException("User is not a manager");
                }

                var managedCommunity = await _repository.AssignManagerToCommunityAsync(managerId, communityId) ?? throw new InvalidOperationException("Failed to assign manager to the community");
                
                return managedCommunity;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error assigning manager to community.");
                throw;
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }

        public async Task<Community?> RemoveManagerFromCommunityAsync(string managerId, int communityId)
        {
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var managedCommunity = await _repository.RemoveManagerFromCommunityAsync(managerId, communityId) ?? throw new InvalidOperationException("Failed to remove manager from the community");

                return managedCommunity;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error removing manager from community.");
                throw;
            }
#pragma warning restore CS0168 // Variable is declared but never used
        }
    }
}