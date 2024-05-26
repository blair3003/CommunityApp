using CommunityApp.Data.Models;
using CommunityApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityApp.Tests.IntegrationTests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManager<ApplicationUser> UserManager => _userManager;

        public DatabaseFixture()
        {
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();

            var serviceProvider = CreateServiceProvider();

            _applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _applicationDbContext.Database.OpenConnection();
            _applicationDbContext.Database.EnsureCreated();

            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        }

        private ServiceProvider CreateServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection
                .AddLogging()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_sqliteConnection))
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            return serviceCollection.BuildServiceProvider();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;
            return new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            _applicationDbContext.Database.EnsureDeleted();
            _applicationDbContext.Dispose();
            _sqliteConnection.Close();
        }
    }
}
