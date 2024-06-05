using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CommunityApp.Pages.Users
{
    [Authorize("AdminOnly")]
    public class RemoveManagerModel(
        UserService userService,
        ILogger<RemoveManagerModel> logger
        ) : PageModel
    {
        private readonly UserService _userService = userService;
        private readonly ILogger<RemoveManagerModel> _logger = logger;

        [BindProperty(SupportsGet = true)]
        public string? UserId { get; set; }
        public UserDto? UserDto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                UserDto = await _userService.GetUserByIdAsync(UserId!)
                    ?? throw new InvalidOperationException("GetUserByIdAsync returned null.");

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving User {UserId}.", UserId);

                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var succeeded = await _userService.RemoveIsManagerClaimAsync(UserId!);
                if (!succeeded)
                {
                    throw new InvalidOperationException("RemoveIsManagerClaimAsync returned false.");
                }

                _logger.LogInformation("Removed Manager {UserId}.", UserId);

                return RedirectToPage("./Details/", new { UserId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing Manager {UserId}.", UserId);

                return RedirectToPage("/Error");
            }
        }
    }
}