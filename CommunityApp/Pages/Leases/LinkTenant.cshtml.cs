using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Leases
{
    [Authorize("ManagerOnly")]
    public class LinkTenantModel(
        LeaseService leaseService,
        LeaseTenantService leaseTenantService,
        UserService userService,
        IAuthorizationService authorizationService,
        ILogger<LinkTenantModel> logger
        ) : PageModel
    {
        private readonly LeaseService _leaseService = leaseService;
        private readonly LeaseTenantService _leaseTenantService = leaseTenantService;
        private readonly UserService _userService = userService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<LinkTenantModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int LeaseId { get; set; }
        [BindProperty]
        public string? UserId { get; set; }
        public List<UserDto> Tenants { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                Tenants = users.Where(u => !u.IsManager && !u.IsAdmin).ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Tenants.");

                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, lease.Home!.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                var tenantLinked = await _leaseTenantService.LinkTenantToLeaseAsync(UserId!, LeaseId);

                if (!tenantLinked)
                {
                    throw new InvalidOperationException("LinkTenantToLeaseAsync returned false.");
                }

                _logger.LogInformation("Linked Tenant {UserId} to Lease {LeaseId}.", UserId, LeaseId);

                return RedirectToPage("./Details/", new { LeaseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error linking Tenant {UserId} to Lease {LeaseId}.", UserId, LeaseId);

                return RedirectToPage("/Error");
            }
        }
    }
}