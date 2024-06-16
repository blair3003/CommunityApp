namespace CommunityApp.Data.Seeders
{
    public class DatabaseSeeder(
        IServiceProvider serviceProvider,
        ILogger<DatabaseSeeder> logger,
        AdminUserSeed adminUserSeed,
        ManagerUsersSeed managerUsersSeed,
        CommunitiesSeed communitiesSeed,
        CommunityManagersSeed communityManagersSeed,
        HomesSeed homesSeed,
        LeasesSeed leasesSeed,
        PaymentsSeed paymentsSeed)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<DatabaseSeeder> _logger = logger;
        private readonly AdminUserSeed _adminUserSeed = adminUserSeed;
        private readonly ManagerUsersSeed _managerUsersSeed = managerUsersSeed;
        private readonly CommunitiesSeed _communitiesSeed = communitiesSeed;
        private readonly CommunityManagersSeed _communityManagersSeed = communityManagersSeed;
        private readonly HomesSeed _homesSeed = homesSeed;
        private readonly LeasesSeed _leasesSeed = leasesSeed;
        private readonly PaymentsSeed _paymentsSeed = paymentsSeed;

        public void Seed()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    _adminUserSeed.InitializeAsync().GetAwaiter().GetResult();
                    _managerUsersSeed.InitializeAsync().GetAwaiter().GetResult();
                    _communitiesSeed.InitializeAsync().GetAwaiter().GetResult();
                    _communityManagersSeed.InitializeAsync().GetAwaiter().GetResult();
                    _homesSeed.InitializeAsync().GetAwaiter().GetResult();
                    _leasesSeed.InitializeAsync().GetAwaiter().GetResult();
                    _paymentsSeed.InitializeAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
