namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface ILeaseTenantRepository
    {
        Task<bool> LinkTenantToLeaseAsync(string tenantId, int leaseId);
        Task<bool> UnlinkTenantFromLeaseAsync(string tenantId, int leaseId);
    }
}
