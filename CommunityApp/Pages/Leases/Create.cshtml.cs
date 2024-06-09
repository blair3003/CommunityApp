using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Humanizer.Localisation;

namespace CommunityApp.Pages.Leases
{
    [Authorize("ManagerOnly")]
    public class CreateModel(
        LeaseService leaseService,
        HomeService homeService,
        IAuthorizationService authorizationService,
        ILogger<CreateModel> logger
        ) : PageModel
    {
        private readonly LeaseService _leaseService = leaseService;
        private readonly HomeService _homeService = homeService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<CreateModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        [BindProperty]
        public LeaseCreateInput Input { get; set; } = new LeaseCreateInput();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("GetHomeByIdAsync returned null.");

                Input.MonthlyPayment = home.BaseRent;
                Input.DepositAmount = home.BaseDeposit;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Home {HomeId}.", HomeId);

                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (HomeId <= 0)
            {
                ModelState.AddModelError(string.Empty, "HomeId is required.");

                return Page();
            }

            try
            {
                var home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("GetHomeByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, home.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                var newLease = await _leaseService.AddLeaseAsync(
                    new Lease
                    {
                        HomeId = home.Id,
                        TenantName = Input.TenantName!,
                        TenantEmail = Input.TenantEmail!,
                        TenantPhone = Input.TenantPhone!,
                        MonthlyPayment = Input.MonthlyPayment,
                        DepositAmount = Input.DepositAmount,
                        PaymentDueDay = Input.PaymentDueDay,
                        LeaseStartDate = Input.LeaseStartDate,
                        LeaseEndDate = Input.LeaseEndDate,
                        Notes = Input.Notes
                    })
                    ?? throw new InvalidOperationException("AddLeaseAsync returned null.");

                _logger.LogInformation("Created new Lease {LeaseId}.", newLease.Id);

                return RedirectToPage("./Details/", new { LeaseId = newLease.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Lease.");

                ModelState.AddModelError(string.Empty, "Unable to create Lease.");

                return Page();
            }
        }
    }
}