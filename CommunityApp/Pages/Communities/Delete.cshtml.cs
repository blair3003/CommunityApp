using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    public class DeleteModel : PageModel
    {
        private readonly CommunityService _communityService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }

        public DeleteModel(CommunityService communityService)
        {
            _communityService = communityService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var community = await _communityService.GetCommunityByIdAsync(CommunityId)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

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
                var deletedCommunity = await _communityService.DeleteCommunityAsync(CommunityId)
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