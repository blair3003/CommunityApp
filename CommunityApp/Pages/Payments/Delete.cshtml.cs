using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Payments
{
    [Authorize("AdminOnly")]
    public class DeleteModel(
        PaymentService paymentService,
        ILogger<DeleteModel> logger
        ) : PageModel
    {
        private readonly PaymentService _paymentService = paymentService;
        private readonly ILogger<DeleteModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Payment = await _paymentService.GetPaymentByIdAsync(PaymentId)
                    ?? throw new InvalidOperationException("GetPaymentByIdAsync returned null.");

                var deletedPayment = await _paymentService.DeletePaymentAsync(PaymentId)
                    ?? throw new InvalidOperationException("DeletePaymentAsync returned null.");

                _logger.LogInformation("Deleted Payment {PaymentId}.", deletedPayment.Id);

                return RedirectToPage("/Leases/Details", new { deletedPayment.LeaseId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Payment.");

                return RedirectToPage("/Error");
            }
        }
    }
}