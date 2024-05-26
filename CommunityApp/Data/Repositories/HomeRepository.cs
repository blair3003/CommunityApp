using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Data.Repositories
{
    public class HomeRepository(ApplicationDbContext context) : IHomeRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Home>> GetAllAsync()
        {
            var allHomes = await _context.Homes.ToListAsync();
            return allHomes;
        }

        public async Task<Home?> GetByIdAsync(int id)
        {
            var home = await _context.Homes.FindAsync(id);
            return home;
        }

        public Task<Home?> AddAsync(Home entity)
        {
            throw new NotImplementedException();
        }

        public Task<Home?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Home?> UpdateAsync(int id, Home entity)
        {
            throw new NotImplementedException();
        }
    }
}
