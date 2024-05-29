using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;

namespace CommunityApp.Services
{
    public class CommunityService(ICommunityRepository repository)
    {
        private readonly ICommunityRepository _repository = repository;

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
    }
}