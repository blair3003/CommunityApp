using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities
{
    public class DetailsModel : PageModel
    {
        private readonly CommunityService _communityService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Community? Community { get; set; }
        public bool CanDelete { get; set; } = false;

        public DetailsModel(CommunityService communityService)
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
    }
}
