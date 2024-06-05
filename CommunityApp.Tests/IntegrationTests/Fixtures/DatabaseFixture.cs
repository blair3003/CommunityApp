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

        public async Task AddCommunities()
        {
            var communities = new List<Community>
            {
                new() { Id = 1, Name = "Community 1" },
                new() { Id = 2, Name = "Community 2" },
                new() { Id = 3, Name = "Community 3" }
            };

            using (var context = CreateContext())
            {
                context.Communities.AddRange(communities);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddHomes()
        {
            var homes = new List<Home>
            {
                new() { Id = 1, CommunityId = 1, Number = "1" },
                new() { Id = 2, CommunityId = 2, Number = "2" },
                new() { Id = 3, CommunityId = 3, Number = " 3" }
            };

            using (var context = CreateContext())
            {
                context.Homes.AddRange(homes);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddUsers()
        {
            var users = new List<ApplicationUser>
            {
                new() { Id = "1", UserName = "TestUser1", Email = "test1@user.com" },
                new() { Id = "2", UserName = "TestUser2", Email = "test2@user.com" },
                new() { Id = "3", UserName = "TestUser3", Email = "test3@user.com" }
            };

            foreach (var user in users)
            {
                await UserManager.CreateAsync(user);
            }
        }

        public async Task AddLeases()
        {
            var leases = new List<Lease>
            {
                new() { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 2, HomeId = 2, TenantName = "Tenant 2", TenantEmail = "tenant2@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 3, HomeId = 3, TenantName = "Tenant 3", TenantEmail = "tenant3@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) }
            };

            using (var context = CreateContext())
            {
                context.Leases.AddRange(leases);
                await context.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            _applicationDbContext.Database.EnsureDeleted();
            _applicationDbContext.Dispose();
            _sqliteConnection.Close();
        }
    }
}
