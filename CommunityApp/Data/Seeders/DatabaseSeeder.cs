using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CommunityApp.Data.Seeders
{
    public class DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<DatabaseSeeder> _logger = logger;

        public void Seed()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                try
                {
                    AdminUserSeed.Initialize(userManager).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
