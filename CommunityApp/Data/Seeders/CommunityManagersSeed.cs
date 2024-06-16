using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Data.Seeders
{
    public class CommunityManagersSeed(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            var usersWithManagerClaim = await _userManager
                .GetUsersForClaimAsync(new System.Security.Claims.Claim("IsManager", "true"));

            var admins = await _userManager
                .GetUsersForClaimAsync(new System.Security.Claims.Claim("IsAdmin", "true"));

            var managers = usersWithManagerClaim.Except(admins).ToList();

            var communities = _context.Communities;

            if (managers.Count > 0)
            {
                int managersIndex = 0;

                foreach (var community in communities)
                {
                    var manager = managers[managersIndex];
                    community.Managers.Add(manager);
                    managersIndex = (managersIndex + 1) % managers.Count;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
