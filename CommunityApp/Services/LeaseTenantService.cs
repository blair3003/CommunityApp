using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Services
{
    public class LeaseTenantService(ILeaseTenantRepository repository, UserManager<ApplicationUser> userManager)
    {
        private readonly ILeaseTenantRepository _repository = repository;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<bool> LinkTenantToLeaseAsync(string tenantId, int leaseId)
        {
            var tenantLinked = await _repository.LinkTenantToLeaseAsync(tenantId, leaseId);
            return tenantLinked;
        }

        public async Task<bool> UnlinkTenantFromLeaseAsync(string tenantId, int leaseId)
        {
            var tenantUnlinked = await _repository.UnlinkTenantFromLeaseAsync(tenantId, leaseId);
            return tenantUnlinked;
        }
    }
}
