using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;

namespace CommunityApp.Services
{
    public class HomeService(IHomeRepository repository)
    {
        private readonly IHomeRepository _repository = repository;

        public async Task<List<Home>> GetAllHomesAsync()
        {
            var allHomes = await _repository.GetAllAsync();
            return allHomes;
        }

        public async Task<Home?> GetHomeByIdAsync(int id)
        {
            var home = await _repository.GetByIdAsync(id);
            return home;
        }

        public async Task<Home?> AddHomeAsync(Home home)
        {
            var newHome = await _repository.AddAsync(home);
            return newHome;
        }

        public async Task<Home?> UpdateHomeAsync(int id, Home home)
        {
            var updatedHome = await _repository.UpdateAsync(id, home);
            return updatedHome;
        }

        public async Task<Home?> DeleteHomeAsync(int id)
        {
            var deletedHome = await _repository.DeleteAsync(id);
            return deletedHome;
        }
    }
}
