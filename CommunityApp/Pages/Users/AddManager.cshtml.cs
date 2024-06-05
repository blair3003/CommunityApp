using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CommunityApp.Pages.Users
{
    [Authorize("AdminOnly")]
    public class AddManagerModel(
        UserService userService,
        ILogger<AddManagerModel> logger
        ) : PageModel
    {
        private readonly UserService _userService = userService;
        private readonly ILogger<AddManagerModel> _logger = logger;

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
                var succeeded = await _userService.AddIsManagerClaimAsync(UserId!);
                if (!succeeded)
                {
                    throw new InvalidOperationException("AddIsManagerClaimAsync returned false.");
                }

                _logger.LogInformation("Added Manager {UserId}.", UserId);

                return RedirectToPage("./Details/", new { UserId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Manager {UserId}.", UserId);

                return RedirectToPage("/Error");
            }
        }
    }
}