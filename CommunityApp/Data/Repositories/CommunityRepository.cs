using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Data.Repositories
{
    public class CommunityRepository(ApplicationDbContext context) : ICommunityRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Community>> GetAllAsync()
        {
            var allCommunities = await _context.Communities
                .Include(c => c.Managers)
                .ToListAsync();

            return allCommunities;
        }

        public Task<Community?> AddAsync(Community entity)
        {
            throw new NotImplementedException();
        }

        public Task<Community?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Community?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Community?> UpdateAsync(Community entity)
        {
            throw new NotImplementedException();
        }
    }
}
