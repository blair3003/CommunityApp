using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Tests.IntegrationTests
{
    public class CommunityManagerRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public CommunityManagerRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCommunitiesByManagerIdAsync_ReturnsCommunitiesForManager()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.GetCommunitiesByManagerIdAsync("1");
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Contains(result, c => c.Name == "Community 1");
            }
        }

        [Fact]
        public async Task GetHomesByManagerIdAsync_ReturnsHomesForManager()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                context.Homes.Add(home);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.GetHomesByManagerIdAsync("1");
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Contains(result, c => c.Number == "1");
            }
        }

        [Fact]
        public async Task GetLeasesByManagerIdAsync_ReturnsLeasesForManager()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                context.Homes.Add(home);
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.GetLeasesByManagerIdAsync("1");
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Contains(result, l => l.TenantName == "Tenant 1");
            }
        }

        [Fact]
        public async Task GetPaymentsByManagerIdAsync_ReturnsPaymentsForManager()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                context.Homes.Add(home);
                context.Leases.Add(lease);
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.GetPaymentsByManagerIdAsync("1");
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Contains(result, p => p.Amount == 100.00m);
            }
        }

        [Fact]
        public async Task AddManagerToCommunityAsync_AddsManagerToCommunity()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.AddManagerToCommunityAsync("1", 1);
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var managedCommunity = await context.Communities
                    .Include(c => c.Managers)
                    .FirstOrDefaultAsync(c => c.Id == 1);
                Assert.NotNull(managedCommunity);
                Assert.Single(managedCommunity.Managers);
            }
        }

        [Fact]
        public async Task RemoveManagerFromCommunityAsync_RemovesManagerFromCommunity()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.RemoveManagerFromCommunityAsync("1", 1);
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var managedCommunity = await context.Communities
                    .Include(c => c.Managers)
                    .FirstOrDefaultAsync(c => c.Id == 1);
                Assert.NotNull(managedCommunity);
                Assert.Empty(managedCommunity.Managers);
            }

        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Communities.RemoveRange(context.Communities);
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();

                if (context.Communities.Any() || context.Users.Any())
                {
                    throw new InvalidOperationException("Failed to clear data.");
                }
            }
        }
    }
}