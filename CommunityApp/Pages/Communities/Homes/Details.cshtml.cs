using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities.Homes
{
    public class DetailsModel : PageModel
    {
        private readonly HomeService _homeService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int HomeId { get; set; }
        public Home? Home { get; set; }

        public DetailsModel(HomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Home = await _homeService.GetHomeByIdAsync(HomeId)
                    ?? throw new InvalidOperationException("Home retrieval failed");

                if (Home.CommunityId != CommunityId)
                {
                    throw new InvalidOperationException("Home not part of community");
                }

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}