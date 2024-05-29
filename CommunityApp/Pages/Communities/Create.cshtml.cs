using CommunityApp.Data.Models;
using CommunityApp.Pages.Communities.InputModels;
using CommunityApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CommunityApp.Pages.Communities
{
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

                return RedirectToPage("./Details/", new { newCommunity.Id });
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to create community.");
                return Page();
            }            
        }
    }
}
