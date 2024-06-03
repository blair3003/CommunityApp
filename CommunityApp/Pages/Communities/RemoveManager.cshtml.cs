using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class RemoveManagerModel : PageModel
    {
        private readonly CommunityManagerService _communityManagerService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? UserId { get; set; }

        public RemoveManagerModel(CommunityManagerService communityManagerService)
        {
            _communityManagerService = communityManagerService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var managerRemoved = await _communityManagerService.RemoveManagerFromCommunityAsync(UserId!, CommunityId);
                if (!managerRemoved)
                {
                    throw new InvalidOperationException("Manager removal failed.");
                }

                return RedirectToPage("./Details/", new { CommunityId = CommunityId });
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}