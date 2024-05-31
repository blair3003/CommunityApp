using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities.Homes
{
    [Authorize("ManagerOnly")]
    public class DetailsModel : PageModel
    {
        private readonly HomeService _homeService;
        private readonly CommunityService _communityService;
        private readonly IAuthorizationService _authorizationService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        public Home? Home { get; set; }

        public DetailsModel(HomeService homeService, CommunityService communityService, IAuthorizationService authorizationService)
        {
            _homeService = homeService;
            _communityService = communityService;
            _authorizationService = authorizationService;
        }

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
                    ?? throw new InvalidOperationException("Home retrieval failed");

                if (Home.CommunityId != CommunityId)
                {
                    throw new InvalidOperationException("Home not part of community");
                }

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}