using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using CommunityApp.Data.Models;

namespace CommunityApp.Data.Seeders
{
    public class AdminUserSeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdminUserSeed(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@forthdev.com",
                Email = "admin@forthdev.com",
            };

            var userPassword = _configuration["AdminPassword"]
                ?? throw new Exception("Admin password is not set in the configuration.");

            var user = await _userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createAdminUser = await _userManager.CreateAsync(adminUser, userPassword);

                if (createAdminUser.Succeeded)
                {
                    await _userManager.AddClaimAsync(adminUser, new System.Security.Claims.Claim("IsAdmin", "true"));
                    await _userManager.AddClaimAsync(adminUser, new System.Security.Claims.Claim("IsManager", "true"));
                }
            }
            else
            {
                var claims = await _userManager.GetClaimsAsync(user);

                if (!claims.Any(c => c.Type == "IsAdmin"))
                {
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsAdmin", "true"));
                }

                if (!claims.Any(c => c.Type == "IsManager"))
                {
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("IsManager", "true"));
                }
            }
        }
    }
}
