using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Data.Seeders
{
    public class ManagerUsersSeed(UserManager<ApplicationUser> userManager)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task InitializeAsync()
        {
            var managers = new List<ApplicationUser>
            {
                new() {
                    UserName = "managertom@communityapp.com",
                    Email = "managertom@community.com",
                },
                new() {
                    UserName = "managerwilson@communityapp.com",
                    Email = "managerwilson@community.com",
                },
                new() {
                    UserName = "manageralex@communityapp.com",
                    Email = "manageralex@community.com",
                }
            };

            foreach (var manager in managers)
            {
                var user = await _userManager.FindByEmailAsync(manager.Email!);

                if (user == null)
                {
                    var createManagerUser = await _userManager.CreateAsync(manager, "M@nager1");

                    if (createManagerUser.Succeeded)
                    {
                        await _userManager.AddClaimAsync(manager, new System.Security.Claims.Claim("IsManager", "true"));
                    }
                }
                else
                {
                    var claims = await _userManager.GetClaimsAsync(user);

                    if (!claims.Any(c => c.Type == "IsManager"))
                    {
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsManager", "true"));
                    }
                }
            }
        }
    }
}
