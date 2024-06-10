using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommunityApp.Services;

namespace CommunityApp.Pages.Payments
{
    [Authorize("ManagerOnly")]
    public class IndexModel(
        PaymentService paymentService,
        CommunityManagerService communityManagerService,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly PaymentService _paymentService = paymentService;
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Payment> Payments { get; set; } = [];
        public bool CanManageAllPayments { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");
                CanManageAllPayments = isAdmin.Succeeded;

                if (CanManageAllPayments)
                {
                    Payments = await _paymentService.GetAllPaymentsAsync();
                }
                else
                {
                    Payments = await _communityManagerService.GetPaymentsByManagerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Payments.");

                return RedirectToPage("/Error");
            }
        }
    }
}
