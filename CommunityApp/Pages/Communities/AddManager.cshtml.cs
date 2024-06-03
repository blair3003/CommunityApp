using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using CommunityApp.Data.Models;
using CommunityApp.Services;

namespace CommunityApp.Pages.Communities
{
    [Authorize("AdminOnly")]
    public class AddManagerModel : PageModel
    {
        private readonly CommunityManagerService _communityManagerService;
        private readonly UserService _userService;

        [BindProperty(SupportsGet = true)]
        public int CommunityId { get; set; }
        [BindProperty] 
        public string? UserId { get; set; }
        public List<UserDto> Managers { get; set; } = [];

        public AddManagerModel(CommunityManagerService communityManagerService, UserService userService)
        {
            _communityManagerService = communityManagerService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                Managers = users.Where(u => u.IsManager && !u.IsAdmin).ToList();

                return Page();
            }
            catch
            {
                return NotFound();
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
                    throw new InvalidOperationException("Manager add failed.");
                }

                return RedirectToPage("./Details/", new { CommunityId = CommunityId });
            }
            catch (Exception ex)
            {
                //return RedirectToPage("/Error");
                return BadRequest(UserId ?? "null");
            }
        }
    }
}