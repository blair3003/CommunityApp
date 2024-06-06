using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Homes
{
    [Authorize("ManagerOnly")]
    public class DetailsModel(
        HomeService homeService,
        IAuthorizationService authorizationService,
        ILogger<DetailsModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<DetailsModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        public Home? Home { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("GetHomeByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, Home.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Home {HomeId}.", HomeId);

                return NotFound();
            }
        }
    }
}