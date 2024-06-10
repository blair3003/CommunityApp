using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("ManagerOnly")]
    public class IndexModel(
        CommunityService communityService,
        CommunityManagerService communityManagerService,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly CommunityService _communityService = communityService;
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Community> Communities { get; set; } = [];
        public bool CanManageAllCommunities { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");
                CanManageAllCommunities = isAdmin.Succeeded;

                if (CanManageAllCommunities)
                {
                    Communities = await _communityService.GetAllCommunitiesAsync();
                }
                else
                {
                    Communities = await _communityManagerService.GetCommunitiesByManagerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Communities.");

                return RedirectToPage("/Error");
            }
        }
    }
}
