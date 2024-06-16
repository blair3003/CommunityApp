using CommunityApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CommunityApp.Data.Seeders
{
    public class DatabaseSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly AdminUserSeed _adminUserSeed;

        public DatabaseSeeder(IServiceProvider serviceProvider, ILogger<DatabaseSeeder> logger, AdminUserSeed adminUserSeed)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _adminUserSeed = adminUserSeed;
        }

        public void Seed()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    _adminUserSeed.InitializeAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
