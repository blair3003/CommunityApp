using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommunityApp.Services;

namespace CommunityApp.Pages.Leases
{
    [Authorize("ManagerOnly")]
    public class IndexModel(
        LeaseService leaseService,
        CommunityManagerService communityManagerService,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly LeaseService _leaseService = leaseService;
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Lease> Leases { get; set; } = [];
        public bool CanManageAllLeases { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");
                CanManageAllLeases = isAdmin.Succeeded;

                if (CanManageAllLeases)
                {
                    Leases = await _leaseService.GetAllLeasesAsync();
                }
                else
                {
                    Leases = await _communityManagerService.GetLeasesByManagerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Leases.");

                return RedirectToPage("/Error");
            }
        }
    }
}
