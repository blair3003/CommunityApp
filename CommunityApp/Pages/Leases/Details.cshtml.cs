using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Leases
{
	public class DetailsModel(
		LeaseService leaseService,
		CommunityService communityService,
		IAuthorizationService authorizationService,
		ILogger<DetailsModel> logger
		) : PageModel
	{
		private readonly LeaseService _leaseService = leaseService;
		private readonly CommunityService _communityService = communityService;
		private readonly IAuthorizationService _authorizationService = authorizationService;
		private readonly ILogger<DetailsModel> _logger = logger;

		[BindProperty(SupportsGet = true)]
		public int LeaseId { get; set; }
		public Lease? Lease { get; set; }

		public bool CanMakePayment { get; set; } = false;
		public bool CanManageLease { get; set; } = false;
		public bool HasLinkedTenant { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
		{
			try
			{
				Lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
					?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

				var community = await _communityService.GetCommunityByIdAsync(Lease.Home!.CommunityId)
					?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

				var tenantAuthorization = await _authorizationService.AuthorizeAsync(User, Lease, "LeaseTenant");
                var managerAuthorization = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!tenantAuthorization.Succeeded && !managerAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Not authorized.");
                }

                CanMakePayment = tenantAuthorization.Succeeded;
                CanManageLease = managerAuthorization.Succeeded;
				HasLinkedTenant = Lease.Tenant != null;

                return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving Lease {LeaseId}.", LeaseId);

				return NotFound();
			}
		}
	}
}