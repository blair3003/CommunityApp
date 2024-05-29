using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities
{
    public class IndexModel : PageModel
    {
        private readonly CommunityService _communityService;
        public List<Community> Communities { get; set; } = [];

        public IndexModel(CommunityService communityService)
        {
            _communityService = communityService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Communities = await _communityService.GetAllCommunitiesAsync();

            return Page();
        }
    }
}
