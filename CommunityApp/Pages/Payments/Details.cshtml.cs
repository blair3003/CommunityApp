using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Payments
{
	public class DetailsModel(
		PaymentService paymentService,
        CommunityService communityService,
        IAuthorizationService authorizationService, 
		ILogger<DetailsModel> logger
		) : PageModel
	{
		private readonly PaymentService _paymentService = paymentService;
		private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<DetailsModel> _logger = logger;

		[BindProperty(SupportsGet = true)]
		public int PaymentId { get; set; }
		public Payment? Payment { get; set; }
		public bool CanManagePayment { get; set; }


        public async Task<IActionResult> OnGetAsync()
		{
			try
			{
                Payment = await _paymentService.GetPaymentByIdAsync(PaymentId)
					?? throw new InvalidOperationException("GetPaymentByIdAsync returned null.");

				var community = await _communityService.GetCommunityByIdAsync(Payment.Lease!.Home!.CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var tenantAuthorization = await _authorizationService.AuthorizeAsync(User, Payment.Lease, "LeaseTenant");

                var managerAuthorization = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!tenantAuthorization.Succeeded && !managerAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Not authorized.");
                }

                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");

				CanManagePayment = isAdmin.Succeeded;

                return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving Payment {PaymentId}.", PaymentId);

				return NotFound();
			}
		}
	}
}