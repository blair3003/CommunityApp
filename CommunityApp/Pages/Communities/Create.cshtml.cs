using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class CreateModel : PageModel
    {
        private readonly CommunityService _communityService;

        [BindProperty]
        public CreateCommunityInput Input { get; set; } = new CreateCommunityInput();

        public CreateModel(CommunityService communityService)
        {
            _communityService = communityService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newCommunity = await _communityService.AddCommunityAsync(
                    new Community
                    {
                        Name = Input.Name
                    }
                ) ?? throw new InvalidOperationException("Community creation failed.");

                return RedirectToPage("./Details/", new { CommunityId = newCommunity.Id });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to create community.");
                return Page();
            }            
        }
    }
}
