using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities
{
    [Authorize("ManagerOnly")]
    public class DetailsModel : PageModel
    {
        private readonly CommunityService _communityService;
        private readonly CommunityManagerService _communityManagerService;
        private readonly IAuthorizationService _authorizationService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Community? Community { get; set; }
        public bool CanDelete { get; set; } = false;

        public DetailsModel(CommunityService communityService, CommunityManagerService communityManagerService, IAuthorizationService authorizationService)
        {
            _communityService = communityService;
            _communityManagerService = communityManagerService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

                var authorizationResult = await _authorizationService.AuthorizeAsync(User, Community, "CommunityManager");
                if (!authorizationResult.Succeeded)
                {
                    throw new InvalidOperationException("Access denied.");
                }

                CanDelete = Community.Homes.Count == 0;

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var managerAdded = await _communityManagerService.AddManagerToCommunityAsync("30193cf0-4ef3-4a76-8b91-9599638783cb", CommunityId);

                return RedirectToPage("./Details/", new { CommunityId = CommunityId });
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}
