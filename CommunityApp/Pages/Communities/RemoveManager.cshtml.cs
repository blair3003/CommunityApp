using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class RemoveManagerModel(
        CommunityManagerService communityManagerService,
        ILogger<RemoveManagerModel> logger
        ) : PageModel
    {
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly ILogger<RemoveManagerModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? UserId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var managerRemoved = await _communityManagerService.RemoveManagerFromCommunityAsync(UserId!, CommunityId);

                if (!managerRemoved)
                {
                    throw new InvalidOperationException("RemoveManagerFromCommunityAsync returned false.");
                }

                _logger.LogInformation("Removed Manager {UserId} from Community {CommunityId}.", UserId, CommunityId);

                return RedirectToPage("./Details/", new { CommunityId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing Manager {UserId} from Community {CommunityId}.", UserId, CommunityId);

                return RedirectToPage("/Error");
            }
        }
    }
}