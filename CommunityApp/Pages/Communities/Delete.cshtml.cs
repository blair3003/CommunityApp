using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class DeleteModel : PageModel
    {
        private readonly CommunityService _communityService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Community? Community { get; set; }
        public bool CanDelete { get; set; } = false;

        public DeleteModel(CommunityService communityService)
        {
            _communityService = communityService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

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
                Community = await _communityService.DeleteCommunityAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community delete failed.");

                return RedirectToPage("./Index");
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}