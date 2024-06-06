using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Homes
{
    [Authorize("ManagerOnly")]
    public class DeleteModel(
        HomeService homeService,
        CommunityService communityService,
        IAuthorizationService authorizationService,
        ILogger<DeleteModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<DeleteModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        public Home? Home { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("Home retrieval failed.");

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("Home retrieval failed.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, Home.Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                var deletedHome = await _homeService.DeleteHomeAsync(HomeId)
                    ?? throw new InvalidOperationException("DeleteHomeAsync returned null.");

                _logger.LogInformation("Deleted Home {HomeId}.", HomeId);

                return RedirectToPage("/Communities/Details/", new { deletedHome.CommunityId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Home.");

                return RedirectToPage("/Error");
            }
        }
    }
}