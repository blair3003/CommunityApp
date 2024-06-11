using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Data.Repositories
{
    public class CommunityManagerRepository(ApplicationDbContext context) : ICommunityManagerRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Community>> GetCommunitiesByManagerIdAsync(string managerId)
        {
            var managerCommunities = await _context.Communities
                .Where(
                    c => c.Managers.Any(
                        m => m.Id == managerId))
                .ToListAsync();

            return managerCommunities;
        }

        public async Task<List<Home>> GetHomesByManagerIdAsync(string managerId)
        {
            var managerHomes = await _context.Homes
                .Include(h => h.Community)
                    .ThenInclude(c => c!.Managers)
                .Where(
                    h => h.Community!.Managers.Any(
                        m => m.Id == managerId))
                .ToListAsync();

            return managerHomes;
        }

        public async Task<List<Lease>> GetLeasesByManagerIdAsync(string managerId)
        {
            var managerLeases = await _context.Leases
                .Include(l => l.Home)
                    .ThenInclude(h => h!.Community)
                        .ThenInclude(c => c!.Managers)
                .Where(
                    l => l.Home!.Community!.Managers.Any(
                        m => m.Id == managerId))
                .ToListAsync();

            return managerLeases;
        }

        public async Task<List<Payment>> GetPaymentsByManagerIdAsync(string managerId)
        {
            var managerPayments = await _context.Payments
                .Include(p => p.Lease)
                    .ThenInclude(l => l!.Home)
                        .ThenInclude(l => l!.Community)
                            .ThenInclude(c => c!.Managers)
                .Where(
                    p => p.Lease!.Home!.Community!.Managers.Any(
                        m => m.Id == managerId))
                .ToListAsync();

            return managerPayments;
        }

        public async Task<bool> AddManagerToCommunityAsync(string managerId, int communityId)
        {
            var manager = await _context.Users.FindAsync(managerId);
            var community = await _context.Communities
                .Include(c => c.Managers)
                .FirstOrDefaultAsync(c => c.Id == communityId);

            if (manager == null || community == null)
            {
                return false;
            }

            if (!community.Managers.Contains(manager))
            {
                community.Managers.Add((ApplicationUser)manager);
                await _context.SaveChangesAsync();
            }

            return community.Managers.Contains(manager);
        }

        public async Task<bool> RemoveManagerFromCommunityAsync(string managerId, int communityId)
        {
            var manager = await _context.Users.FindAsync(managerId);
            var community = await _context.Communities
                .Include(c => c.Managers)
                .FirstOrDefaultAsync(c => c.Id == communityId);

            if (manager == null || community == null)
            {
                return false;
            }

            if (community.Managers.Contains(manager))
            {
                community.Managers.Remove((ApplicationUser)manager);
                await _context.SaveChangesAsync();
            }

            return !community.Managers.Contains(manager);
        }
    }
}