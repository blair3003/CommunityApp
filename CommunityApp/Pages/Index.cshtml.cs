using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommunityApp.Services;
using CommunityApp.Data.Models;

namespace CommunityApp.Pages
{
    public class IndexModel(
        LeaseTenantService leaseTenantService,
        PaymentService paymentService,
        CommunityManagerService communityManagerService,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly LeaseTenantService _leaseTenantService = leaseTenantService;
        private readonly PaymentService _paymentService = paymentService;
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Payment> Payments { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");
                var isManager = await _authorizationService.AuthorizeAsync(User, "ManagerOnly");
                var isTenant = !isAdmin.Succeeded && !isManager.Succeeded;

                if (isTenant)
                {
                    var lease = await _leaseTenantService.GetLeaseByTenantIdAsync(userId!);

                    if (lease != null)
                    {
                        _logger.LogInformation("Redirecting to Lease {LeaseId}", lease.Id);

                        return RedirectToPage("/Leases/Details", new { LeaseId = lease.Id });
                    }
                    return Page();
                }

                if (isAdmin.Succeeded)
                {
                    Payments = await _paymentService.GetAllPaymentsAsync();
                }
                else
                {
                    Payments = await _communityManagerService.GetPaymentsByManagerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                Payments = Payments
                    .OrderByDescending(p => p.PaymentDate)
                    .Take(5)
                    .ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Homepage data.");

                return Page();
            }
        }
    }
}