using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Payments
{
    [Authorize("AdminOnly")]
    public class EditModel(
        PaymentService paymentService,
        ILogger<EditModel> logger
        ) : PageModel
    {
        private readonly PaymentService _paymentService = paymentService;
        private readonly ILogger<EditModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int PaymentId { get; set; }
        [BindProperty]
        public PaymentEditInput Input { get; set; } = new PaymentEditInput();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(PaymentId)
                    ?? throw new InvalidOperationException("GetPaymentByIdAsync returned null.");

                Input = new PaymentEditInput
                {
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Description = payment.Description
                };

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Payment {PaymentId}.", PaymentId);

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
                var payment = await _paymentService.GetPaymentByIdAsync(PaymentId)
                    ?? throw new InvalidOperationException("GetPaymentByIdAsync returned null.");

                payment.Amount = Input.Amount;
                payment.PaymentDate = Input.PaymentDate;
                payment.Description = Input.Description;

                var updatedPayment = await _paymentService.UpdatePaymentAsync(PaymentId, payment)
                    ?? throw new InvalidOperationException("UpdatePaymentAsync returned null.");                    

                _logger.LogInformation("Updated Payment {PaymentId}.", updatedPayment.Id);

                return RedirectToPage("./Details/", new { PaymentId = updatedPayment.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Payment.");

                ModelState.AddModelError(string.Empty, "Unable to update Payment.");

                return Page();
            }
        }
    }
}