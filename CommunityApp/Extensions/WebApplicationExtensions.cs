using CommunityApp.Data.Seeders;

namespace CommunityApp.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void UseDatabaseSeeder(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeder = services.GetRequiredService<DatabaseSeeder>();

                try
                {
                    seeder.Seed();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<DatabaseSeeder>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
