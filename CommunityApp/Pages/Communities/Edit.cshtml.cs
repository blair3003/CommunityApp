using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class EditModel(
        CommunityService communityService,
        ILogger<EditModel> logger
        ) : PageModel
    {
        private readonly CommunityService _communityService = communityService;
        private readonly ILogger<EditModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty]
        public UpdateCommunityInput Input { get; set; } = new UpdateCommunityInput();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

                Input = new UpdateCommunityInput { Name = community.Name };

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("GetCommunityByIdAsync returned null.");

                community.Name = Input.Name;

                var updatedCommunity = await _communityService.UpdateCommunityAsync(CommunityId, community)
                    ?? throw new InvalidOperationException("UpdateCommunityAsync returned null.");

                _logger.LogInformation("Updated Community {CommunityId}.", CommunityId);

                return RedirectToPage("./Details/", new { CommunityId = updatedCommunity.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Community {CommunityId}.", CommunityId);

                ModelState.AddModelError(string.Empty, "Unable to update community.");
                
                return Page();
            }
        }
    }
}