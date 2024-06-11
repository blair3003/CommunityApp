using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Services;
using CommunityApp.Data.Models;

namespace CommunityApp.Pages.Leases
{
    [Authorize("ManagerOnly")]
    public class UnlinkTenantModel(
        LeaseService leaseService,
        LeaseTenantService leaseTenantService,
        IAuthorizationService authorizationService,
        ILogger<UnlinkTenantModel> logger) : PageModel
    {
        private readonly LeaseService _leaseService = leaseService;
        private readonly LeaseTenantService _leaseTenantService = leaseTenantService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<UnlinkTenantModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int LeaseId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? UserId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, lease.Home!.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                var tenantUnlinked = await _leaseTenantService.UnlinkTenantFromLeaseAsync(UserId!, LeaseId);

                if (!tenantUnlinked)
                {
                    throw new InvalidOperationException("UnlinkTenantFromLeaseAsync returned false.");
                }

                _logger.LogInformation("Unlinked Tenant {UserId} from Lease {LeaseId}.", UserId, LeaseId);

                return RedirectToPage("./Details/", new { LeaseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unlinking Tenant {UserId} from Lease {LeaseId}.", UserId, LeaseId);

                return RedirectToPage("/Error");
            }
        }
    }
}