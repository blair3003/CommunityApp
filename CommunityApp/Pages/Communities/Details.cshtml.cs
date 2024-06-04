using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("ManagerOnly")]
    public class DetailsModel(
        CommunityService communityService,
        IAuthorizationService authorizationService,
        ILogger<DetailsModel> logger
        ) : PageModel
    {
        private readonly CommunityService _communityService = communityService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<DetailsModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Community? Community { get; set; }
        public bool CanDelete { get; set; } = false;
        public bool CanAddManagers { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, Community, "CommunityManager");

                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Manager not authorized.");
                }

                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");
                CanAddManagers = isAdmin.Succeeded;
                CanDelete = Community.Homes.Count == 0;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Community {CommunityId}.", CommunityId);

                return NotFound();
            }
        }
    }
}
