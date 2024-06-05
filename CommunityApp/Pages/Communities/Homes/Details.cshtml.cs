using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CommunityApp.Pages.Communities.Homes
{
    [Authorize("ManagerOnly")]
    public class DetailsModel(
        HomeService homeService,
        CommunityService communityService,
        IAuthorizationService authorizationService,
        ILogger<DetailsModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<DetailsModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        public Home? Home { get; set; }

        public async Task<IActionResult> OnGetAsync()
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

                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("GetHomeByIdAsync returned null.");

                if (Home.CommunityId != CommunityId)
                {
                    throw new InvalidOperationException("Home not part of Community");
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