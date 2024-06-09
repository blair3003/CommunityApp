using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Payments
{
    public class CreateModel(
        PaymentService paymentService,
        LeaseService leaseService,
        IAuthorizationService authorizationService,
        ILogger<CreateModel> logger
        ) : PageModel
    {
        private readonly PaymentService _paymentService = paymentService;
        private readonly LeaseService _leaseService = leaseService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<CreateModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int LeaseId { get; set; }
        [BindProperty]
        public PaymentCreateInput Input { get; set; } = new PaymentCreateInput();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                Input.Amount = lease.MonthlyPayment;

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

            if (LeaseId <= 0)
            {
                ModelState.AddModelError(string.Empty, "LeaseId is required.");

                return Page();
            }

            try
            {
                var lease = await _leaseService.GetLeaseByIdAsync(LeaseId)
                    ?? throw new InvalidOperationException("GetLeaseByIdAsync returned null.");

                var tenantAuthorization = await _authorizationService.AuthorizeAsync(User, lease, "LeaseTenant");

                if (!tenantAuthorization.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                var newPayment = await _paymentService.AddPaymentAsync(
                    new Payment
                    {
                        LeaseId = lease.Id,
                        Amount = Input.Amount,
                        PaymentDate = Input.PaymentDate,
                        Description = Input.Description
                    })
                    ?? throw new InvalidOperationException("AddPaymentAsync returned null.");

                _logger.LogInformation("Created new Payment {PaymentId}.", newPayment.Id);

                return RedirectToPage("./Details/", new { PaymentId = newPayment.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Payment.");

                ModelState.AddModelError(string.Empty, "Unable to create Payment.");

                return Page();
            }
        }
    }
}