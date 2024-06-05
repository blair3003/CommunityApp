using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CommunityApp.Pages.Users
{
    [Authorize("AdminOnly")]
    public class DeleteModel(
        UserService userService,
        ILogger<DeleteModel> logger
        ) : PageModel
    {
        private readonly UserService _userService = userService;
        private readonly ILogger<DeleteModel> _logger = logger;

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
                UserDto = await _userService.DeleteUserAsync(UserId!)
                    ?? throw new InvalidOperationException("DeleteUserAsync returned null.");

                _logger.LogInformation("Deleted User {UserId}.", UserId);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Manager {UserId}.", UserId);

                return RedirectToPage("/Error");
            }
        }
    }
}