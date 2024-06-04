using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class DeleteModel(
        CommunityService communityService,
        ILogger<DeleteModel> logger
        ) : PageModel
    {
        private readonly CommunityService _communityService = communityService;
        private readonly ILogger<DeleteModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Community? Community { get; set; }
        public bool CanDelete { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                CanDelete = Community.Homes.Count == 0;

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Community {CommunityId}.", CommunityId);

                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Community = await _communityService.DeleteCommunityAsync(CommunityId)
                    ?? throw new InvalidOperationException("DeleteCommunityAsync returned null.");

                _logger.LogInformation("Deleted Community {CommunityId}.", CommunityId);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Community.");

                return RedirectToPage("/Error");
            }
        }
    }
}