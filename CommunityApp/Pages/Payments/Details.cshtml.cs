using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Payments
{
	[Authorize("AdminOnly")]
	public class DetailsModel(PaymentService paymentService, ILogger<DetailsModel> logger) : PageModel
	{
		private readonly PaymentService _paymentService = paymentService;
		private readonly ILogger<DetailsModel> _logger = logger;

		[BindProperty(SupportsGet = true)]
		public int PaymentId { get; set; }
		public Payment? Payment { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			try
			{
				Payment = await _paymentService.GetPaymentByIdAsync(PaymentId)
					?? throw new InvalidOperationException("GetPaymentByIdAsync returned null.");

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