using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Tests.IntegrationTests
{
    public class LeaseTenantRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture = fixture;

        [Fact]
        public async Task LinkTenantToLeaseAsync_LinksTenantToLease()
        {
            var tenant = new ApplicationUser { Id = "1", UserName = "Tenant", Email = "tenant@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };
            var home = new Home { Id = 1, CommunityId = 1, Number = "Home 1" };
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant@user.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(tenant);
                context.Communities.Add(community);
                context.Homes.Add(home);
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseTenantRepository(context);
                var result = await repository.LinkTenantToLeaseAsync("1", 1);
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var linkedLease = await context.Leases
                    .Include(l => l.Tenant)
                    .FirstOrDefaultAsync(l => l.Id == 1);
                Assert.NotNull(linkedLease?.Tenant);
                Assert.Equal("Tenant", linkedLease.Tenant.UserName);
            }
        }

        [Fact]
        public async Task UnlinkTenantFromLeaseAsync_UnlinksTenantFromLease()
        {
            var tenant = new ApplicationUser { Id = "1", UserName = "Tenant", Email = "tenant@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };
            var home = new Home { Id = 1, CommunityId = 1, Number = "Home 1" };
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant@user.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(tenant);
                context.Communities.Add(community);
                context.Homes.Add(home);
                lease.Tenant = tenant;
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseTenantRepository(context);
                var result = await repository.UnlinkTenantFromLeaseAsync("1", 1);
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var unlinkedLease = await context.Leases
                    .Include(l => l.Tenant)
                    .FirstOrDefaultAsync(l => l.Id == 1);
                Assert.NotNull(unlinkedLease);
                Assert.Null(unlinkedLease?.Tenant);
            }
        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Users.RemoveRange(context.Users);
                context.Leases.RemoveRange(context.Leases);
                context.Homes.RemoveRange(context.Homes);
                context.Communities.RemoveRange(context.Communities);
                context.SaveChanges();

                if (context.Users.Any() || context.Leases.Any() || context.Homes.Any() || context.Communities.Any())
                {
                    throw new InvalidOperationException("Failed to clear data.");
                }
            }
        }
    }
}
