using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Data.Seeders
{
    public class CommunityManagersSeed(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            var managerClaim = new System.Security.Claims.Claim("IsManager", "true");
            var adminClaim = new System.Security.Claims.Claim("IsAdmin", "true");

            var usersWithManagerClaimTask = _userManager.GetUsersForClaimAsync(managerClaim);
            var adminsTask = _userManager.GetUsersForClaimAsync(adminClaim);

            await Task.WhenAll(usersWithManagerClaimTask, adminsTask);

            var usersWithManagerClaim = await usersWithManagerClaimTask;
            var admins = await adminsTask;

            var manager = usersWithManagerClaim.Except(admins).FirstOrDefault();

            if (manager != null)
            {
                var communities = _context.Communities.Include(c => c.Managers);

                foreach (var community in communities.Where(c => c.Managers.Count == 0))
                {
                    community.Managers.Add(manager);
                }

                await _context.SaveChangesAsync();
            }
        }

    }
}
