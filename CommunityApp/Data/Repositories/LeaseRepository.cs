using Microsoft.EntityFrameworkCore;
using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;

namespace CommunityApp.Data.Repositories
{
    public class LeaseRepository(ApplicationDbContext context) : ILeaseRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Lease>> GetAllAsync()
        {
            var leases = await _context.Leases
                .Include(l => l.Home)
                    .ThenInclude(h => h.Community)
                .Include(l => l.Tenant)
                .ToListAsync();

            return leases;
        }

        public async Task<Lease?> GetByIdAsync(int id)
        {
            var lease = await _context.Leases
                .Include(l => l.Home)
                .Include(l => l.Tenant)
                .Include(l => l.Payments)
                .FirstOrDefaultAsync(l => l.Id == id);

            return lease;
        }

        public async Task<Lease?> AddAsync(Lease lease)
        {
            await _context.Leases.AddAsync(lease);
            await _context.SaveChangesAsync();
            return lease;
        }

        public async Task<Lease?> UpdateAsync(int id, Lease lease)
        {
            if (id != lease.Id)
            {
                return null;
            }

            var existingLease = await _context.Leases.FindAsync(id);

            if (existingLease == null)
            {
                return null;
            }

            existingLease.HomeId = lease.HomeId;
            existingLease.TenantId = lease.TenantId;
            existingLease.TenantName = lease.TenantName;
            existingLease.TenantEmail = lease.TenantEmail;
            existingLease.TenantPhone = lease.TenantPhone;
            existingLease.MonthlyPayment = lease.MonthlyPayment;
            existingLease.DepositAmount = lease.DepositAmount;
            existingLease.LeaseStartDate = lease.LeaseStartDate;
            existingLease.LeaseEndDate = lease.LeaseEndDate;
            existingLease.Notes = lease.Notes;

            await _context.SaveChangesAsync();
            return existingLease;
        }

        public async Task<Lease?> DeleteAsync(int id)
        {
            var lease = await _context.Leases.FindAsync(id);

            if (lease == null)
            {
                return null;
            }

            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();
            return lease;
        }
    }
}