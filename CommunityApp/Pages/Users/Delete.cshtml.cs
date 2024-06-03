using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CommunityApp.Data.Models;
using CommunityApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CommunityApp.Pages.Users
{
    [Authorize("AdminOnly")]
    public class DeleteModel : PageModel
    {
        private readonly UserService _userService;

        [BindProperty(SupportsGet = true)]
        public string? UserId { get; set; }
        public UserDto? UserDto { get; set; }

        public DeleteModel(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                UserDto = await _userService.GetUserByIdAsync(UserId!)
                    ?? throw new InvalidOperationException("User retrieval failed.");

                return Page();
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                UserDto = await _userService.DeleteUserAsync(UserId!)
                    ?? throw new InvalidOperationException("User delete failed.");

                return RedirectToPage("./Index");
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}