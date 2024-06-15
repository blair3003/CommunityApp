using Microsoft.AspNetCore.Identity;
using CommunityApp.Data.Models;

namespace CommunityApp.Data.Seeders
{
    public static class AdminUserSeed
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@forthdev.com",
                Email = "admin@forthdev.com",
            };

            var userPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
                ?? throw new Exception("Admin password is not set in the environment variables.");            

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, userPassword);

                if (createAdminUser.Succeeded)
                {
                    await userManager.AddClaimAsync(adminUser, new System.Security.Claims.Claim("IsAdmin", "true"));
                    await userManager.AddClaimAsync(adminUser, new System.Security.Claims.Claim("IsManager", "true"));
                }
            }
            else
            {
                var claims = await userManager.GetClaimsAsync(user);

                if (!claims.Any(c => c.Type == "IsAdmin"))
                {
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsAdmin", "true"));
                }

                if (!claims.Any(c => c.Type == "IsManager"))
                {
                    await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsManager", "true"));
                }
            }
        }
    }
}
