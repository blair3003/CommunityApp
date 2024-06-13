using CommunityApp.Data.Models;

namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface ILeaseTenantRepository
    {
        Task<Lease?> GetLeaseByTenantIdAsync(string tenantId);
        Task<bool> LinkTenantToLeaseAsync(string tenantId, int leaseId);
        Task<bool> UnlinkTenantFromLeaseAsync(string tenantId, int leaseId);
    }
}
