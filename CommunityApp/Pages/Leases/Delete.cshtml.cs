using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Leases
{
    [Authorize("ManagerOnly")]
    public class DeleteModel(
        LeaseService leaseService,
        CommunityService communityService,
        IAuthorizationService authorizationService,
        ILogger<DeleteModel> logger
        ) : PageModel
    {
        private readonly LeaseService _leaseService = leaseService;
        private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<DeleteModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int LeaseId { get; set; }
        public Lease? Lease { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var community = await _communityService.GetCommunityByIdAsync(Lease.Home!.CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var managerAuthorization = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!managerAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Not authorized.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Lease {LeaseId}.", LeaseId);

                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var community = await _communityService.GetCommunityByIdAsync(Lease.Home!.CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var managerAuthorization = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!managerAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Not authorized.");
                }

                var deletedLease = await _leaseService.DeleteLeaseAsync(LeaseId)
                    ?? throw new InvalidOperationException("DeleteLeaseAsync returned null.");

                _logger.LogInformation("Deleted Lease {LeaseId}.", deletedLease.Id);
                _logger.LogInformation("Redirecting to {HomeId}.", deletedLease.HomeId);

                return RedirectToPage("/Homes/Details", new { deletedLease.HomeId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Lease.");

                return RedirectToPage("/Error");
            }
        }
    }
}