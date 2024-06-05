using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CommunityApp.Pages.Communities.Homes
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
        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Home? Home { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");
                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("Home retrieval failed.");

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
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, community, "CommunityManager");
                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                Home = await _homeService.DeleteHomeAsync(HomeId)
                    ?? throw new InvalidOperationException("DeleteHomeAsync returned null.");

                _logger.LogInformation("Deleted Home {HomeId}.", HomeId);

                return RedirectToPage("../Details/", new { CommunityId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Home.");

                return RedirectToPage("/Error");
            }
        }
    }
}