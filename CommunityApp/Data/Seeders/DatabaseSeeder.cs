namespace CommunityApp.Data.Seeders
{
    public class DatabaseSeeder(
        IServiceProvider serviceProvider,
        ILogger<DatabaseSeeder> logger,
        AdminUserSeed adminUserSeed,
        CommunitiesSeed communitiesSeed,
        HomesSeed homesSeed)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<DatabaseSeeder> _logger = logger;
        private readonly AdminUserSeed _adminUserSeed = adminUserSeed;
        private readonly CommunitiesSeed _communitiesSeed = communitiesSeed;
        private readonly HomesSeed _homesSeed = homesSeed;

        public void Seed()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    _adminUserSeed.InitializeAsync().GetAwaiter().GetResult();
                    _communitiesSeed.InitializeAsync().GetAwaiter().GetResult();
                    _homesSeed.InitializeAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
