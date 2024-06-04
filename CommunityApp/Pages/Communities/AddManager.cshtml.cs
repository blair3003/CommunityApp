using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class AddManagerModel(
        CommunityManagerService communityManagerService,
        UserService userService,
        ILogger<AddManagerModel> logger
        ) : PageModel
    {
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly UserService _userService = userService;
        private readonly ILogger<AddManagerModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty] 
        public string? UserId { get; set; }
        public List<UserDto> Managers { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                Managers = users.Where(u => u.IsManager && !u.IsAdmin).ToList();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Managers.");

                return RedirectToPage("/Error");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var managerAdded = await _communityManagerService.AddManagerToCommunityAsync(UserId!, CommunityId);

                if (!managerAdded)
                {
                    throw new InvalidOperationException("AddManagerToCommunityAsync returned false.");
                }

                _logger.LogInformation("Added Manager {UserId} to Community {CommunityId}.", UserId, CommunityId);

                return RedirectToPage("./Details/", new { CommunityId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Manager {UserId} to Community {CommunityId}.", UserId, CommunityId);

                return RedirectToPage("/Error");
            }
        }
    }
}