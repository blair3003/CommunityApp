using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Leases
{
    [Authorize("ManagerOnly")]
    public class EditModel(
        LeaseService leaseService,
        CommunityService communityService,
        IAuthorizationService authorizationService,
        ILogger<EditModel> logger
        ) : PageModel
    {
        private readonly LeaseService _leaseService = leaseService;
        private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<EditModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int LeaseId { get; set; }
        [BindProperty]
        public LeaseEditInput Input { get; set; } = new LeaseEditInput();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var community = await _communityService.GetCommunityByIdAsync(lease.Home!.CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var managerAuthorization = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!managerAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Not authorized.");
                }

                Input = new LeaseEditInput
                {
                    TenantName = lease.TenantName,
                    TenantEmail = lease.TenantEmail,
                    TenantPhone = lease.TenantPhone,
                    MonthlyPayment = lease.MonthlyPayment,
                    DepositAmount = lease.DepositAmount,
                    PaymentDueDay = lease.PaymentDueDay,
                    LeaseStartDate = lease.LeaseStartDate,
                    LeaseEndDate = lease.LeaseEndDate,
                    Notes = lease.Notes
                };

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var community = await _communityService.GetCommunityByIdAsync(lease!.Home!.CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var managerAuthorization = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");

                if (!managerAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Not authorized.");
                }

                lease.TenantName = Input.TenantName!;
                lease.TenantEmail = Input.TenantEmail!;
                lease.TenantPhone = Input.TenantPhone!;
                lease.MonthlyPayment = Input.MonthlyPayment;
                lease.DepositAmount = Input.DepositAmount;
                lease.PaymentDueDay = Input.PaymentDueDay;
                lease.LeaseStartDate = Input.LeaseStartDate;
                lease.LeaseEndDate = Input.LeaseEndDate;
                lease.Notes = Input.Notes;

                var updatedLease = await _leaseService.UpdateLeaseAsync(LeaseId, lease)
                    ?? throw new InvalidOperationException("UpdateLeaseAsync returned null.");                    

                _logger.LogInformation("Updated Lease {LeaseId}.", updatedLease.Id);

                return RedirectToPage("./Details/", new { LeaseId = updatedLease.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Lease.");

                ModelState.AddModelError(string.Empty, "Unable to update Lease.");

                return Page();
            }
        }
    }
}