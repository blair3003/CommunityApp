using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Users
{
    [Authorize("AdminOnly")]
    public class IndexModel(
        UserService userService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly UserService _userService = userService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<UserDto> Users { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Users = await _userService.GetAllUsersAsync();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Users.");

                return NotFound();
            }
        }
    }
}