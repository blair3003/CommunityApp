using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using System.Net.Sockets;

namespace CommunityApp.Services
{
    public class CommunityService(ICommunityRepository repository)
    {
        ICommunityRepository _repository = repository;

        public async Task<List<Community>> GetAllCommunitiesAsync()
        {
            var allCommunities = await _repository.GetAllAsync();
            return allCommunities;
        }
    }
}
