using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class EditModel : PageModel
    {
        private readonly CommunityService _communityService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty]
        public UpdateCommunityInput Input { get; set; } = new UpdateCommunityInput();

        public EditModel(CommunityService communityService)
        {
            _communityService = communityService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

                Input = new UpdateCommunityInput { Name = community.Name };

                return Page();
            }
            catch
            {
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
                    ?? throw new InvalidOperationException("Community retrieval failed.");

                community.Name = Input.Name;

                var updatedCommunity = await _communityService.UpdateCommunityAsync(CommunityId, community)
                    ?? throw new InvalidOperationException("Community update failed.");

                return RedirectToPage("./Details/", new { CommunityId = updatedCommunity.Id });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to update community.");
                return Page();
            }
        }
    }
}