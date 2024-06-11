using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Data.Repositories
{
    public class LeaseTenantRepository(ApplicationDbContext context) : ILeaseTenantRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> LinkTenantToLeaseAsync(string tenantId, int leaseId)
        {
            var tenant = await _context.Users.FindAsync(tenantId);
            var lease = await _context.Leases
                .Include(l => l.Tenant)
                .FirstOrDefaultAsync(l => l.Id == leaseId);

            if (tenant == null || lease == null || lease.Tenant != null)
            {
                return false;
            }

            lease.Tenant = (ApplicationUser)tenant;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnlinkTenantFromLeaseAsync(string tenantId, int leaseId)
        {
            var lease = await _context.Leases
                .Include(l => l.Tenant)
                .FirstOrDefaultAsync(l => l.Id == leaseId);

            if (lease == null || lease.Tenant == null || lease.TenantId != tenantId)
            {
                return false;
            }

            lease.Tenant = null;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
