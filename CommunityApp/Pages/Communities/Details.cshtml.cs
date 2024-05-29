using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Sockets;

namespace CommunityApp.Pages.Communities
{
    public class DetailsModel : PageModel
    {
        private readonly CommunityService _communityService;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public Community? Community { get; set; }

        public DetailsModel(CommunityService communityService)
        {
            _communityService = communityService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Community = await _communityService.GetCommunityByIdAsync(Id)
                    ?? throw new InvalidOperationException("Community retrieval failed.");

                return Page();
            }
            catch
            {
                return NotFound();
            }            
        }
    }
}
