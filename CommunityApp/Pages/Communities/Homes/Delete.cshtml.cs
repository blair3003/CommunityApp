using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities.Homes
{
    public class DeleteModel : PageModel
    {
        private readonly HomeService _homeService;
        private readonly CommunityService _communityService;

        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        public Home? Home { get; set; }

        public DeleteModel(CommunityService communityService, HomeService homeService)
        {
            _communityService = communityService;
            _homeService = homeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("Home retrieval failed.");

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
                Home = await _homeService.DeleteHomeAsync(HomeId)
                    ?? throw new InvalidOperationException("Home delete failed.");

                return RedirectToPage("../Details/", new { CommunityId });
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}