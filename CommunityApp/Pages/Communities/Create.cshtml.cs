using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class CreateModel(
        CommunityService communityService,
        ILogger<CreateModel> logger
        ) : PageModel
    {
        private readonly CommunityService _communityService = communityService;
        private readonly ILogger<CreateModel> _logger = logger;

        [BindProperty]
        public CommunityCreateInput Input { get; set; } = new CommunityCreateInput();

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
                ) ?? throw new InvalidOperationException("AddCommunityAsync returned null.");

                _logger.LogInformation("Created new Community {CommunityId}.", newCommunity.Id);

                return RedirectToPage("./Details/", new { CommunityId = newCommunity.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Community.");

                return RedirectToPage("/Error");
            }            
        }
    }
}
