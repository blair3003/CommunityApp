using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Users
{
    [Authorize("AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly UserService _userService;

        public List<UserDto> Users { get; set; } = [];

        public IndexModel(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Users = await _userService.GetAllUsersAsync();

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}