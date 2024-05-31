using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace CommunityApp.Pages.Communities
{
    [Authorize("ManagerOnly")]
    public class IndexModel : PageModel
    {
        private readonly CommunityService _communityService;
        private readonly CommunityManagerService _communityManagerService;
        private readonly IAuthorizationService _authorizationService;
        public List<Community> Communities { get; set; } = [];

        public IndexModel(
            CommunityService communityService,
            CommunityManagerService communityManagerService,
            IAuthorizationService authorizationService)
        {
            _communityService = communityService;
            _communityManagerService = communityManagerService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");

                if (isAdmin.Succeeded)
                {
                    Communities = await _communityService.GetAllCommunitiesAsync();
                }
                else
                {
                    Communities = await _communityManagerService.GetCommunitiesByManagerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                return Page();
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}
