using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommunityApp.Services;
using CommunityApp.Data.Models;

namespace CommunityApp.Pages
{
    public class IndexModel(
        HomeService homeService,
        LeaseService leaseService,
        LeaseTenantService leaseTenantService,
        PaymentService paymentService,
        CommunityManagerService communityManagerService,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly LeaseService _leaseService = leaseService;
        private readonly LeaseTenantService _leaseTenantService = leaseTenantService;
        private readonly PaymentService _paymentService = paymentService;
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Payment> Payments { get; set; } = [];
        public List<Lease> Overdues { get; set; } = [];
        public List<Lease> Leases { get; set; } = [];
        public List<Home> Homes { get; set; } = [];

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
                    Leases = await _leaseService.GetAllLeasesAsync();
                    Overdues = await _leaseService.GetAllLeasesAsync();
                    Homes = await _homeService.GetAllHomesAsync();
                }
                else
                {
                    Payments = await _communityManagerService.GetPaymentsByManagerIdAsync(userId!);
                    Leases = await _communityManagerService.GetLeasesByManagerIdAsync(userId!);
                    Overdues = await _communityManagerService.GetLeasesByManagerIdAsync(userId!);
                    Homes = await _communityManagerService.GetHomesByManagerIdAsync(userId!);
                }

                Payments = Payments
                    .OrderByDescending(p => p.PaymentDate)
                    .Take(5)
                    .ToList();

                Overdues = Overdues
                    .Where(lease => !lease.Payments.Any(payment => payment.PaymentDate >= DateTime.Now.AddDays(-31)))
                    .Take(5)
                    .ToList();

                Leases = Leases
                    .OrderByDescending(l => l.LeaseStartDate)
                    .Take(5)
                    .ToList();

                Homes = Homes
                    .Where(h => !h.Leases.Any(l => l.IsActive))
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