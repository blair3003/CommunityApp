using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using CommunityApp.Services;

namespace CommunityApp.Pages.Homes
{
    [Authorize("ManagerOnly")]
    public class IndexModel(
        HomeService homeService,
        CommunityManagerService communityManagerService,
        IAuthorizationService authorizationService,
        ILogger<IndexModel> logger
        ) : PageModel
    {
        private readonly HomeService _homeService = homeService;
        private readonly CommunityManagerService _communityManagerService = communityManagerService;
        private readonly IAuthorizationService _authorizationService = authorizationService;
        private readonly ILogger<IndexModel> _logger = logger;

        public List<Home> Homes { get; set; } = [];
        public bool CanManageAllHomes { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var isAdmin = await _authorizationService.AuthorizeAsync(User, "AdminOnly");
                CanManageAllHomes = isAdmin.Succeeded;

                if (CanManageAllHomes)
                {
                    Homes = await _homeService.GetAllHomesAsync();
                }
                else
                {
                    Homes = await _communityManagerService.GetHomesByManagerIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Homes.");

                return RedirectToPage("/Error");
            }
        }
    }
}
