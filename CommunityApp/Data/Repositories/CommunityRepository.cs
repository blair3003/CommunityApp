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

        public async Task<Community?> GetByIdAsync(int id)
        {
            var community = await _context.Communities
                .Include(c => c.Homes)
                .Include(c => c.Managers)
                .FirstOrDefaultAsync(c => id == c.Id);

            return community;
        }

        public async Task<Community?> AddAsync(Community community)
        {
            await _context.Communities.AddAsync(community);
            await _context.SaveChangesAsync();
            return community;
        }

        public async Task<Community?> UpdateAsync(int id, Community community)
        {
            if (id != community.Id)
            {
                return null;
            }

            var existingCommunity = await _context.Communities.FindAsync(id);

            if (existingCommunity == null)
            {
                return null;
            }

            existingCommunity.Name = community.Name;

            await _context.SaveChangesAsync();
            return existingCommunity;
        }

        public async Task<Community?> DeleteAsync(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if (community == null)
            {
                return null;
            }

            var hasHomes = await _context.Homes.AnyAsync(h => h.CommunityId == id);

            if (hasHomes)
            {
                return null;
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();
            return community;
        }
    }
}