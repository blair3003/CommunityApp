using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;

namespace CommunityApp.Tests.IntegrationTests
{
    public class LeaseRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public LeaseRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.AddCommunities().GetAwaiter().GetResult();
            _fixture.AddHomes().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllLeases()
        {
            var leases = new List<Lease>
            {
                new() { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 2, HomeId = 2, TenantName = "Tenant 2", TenantEmail = "tenant2@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 3, HomeId = 3, TenantName = "Tenant 3", TenantEmail = "tenant3@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) }
            };

            using (var context = _fixture.CreateContext())
            {
                context.Leases.AddRange(leases);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.GetAllAsync();
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Contains(result, l => l.TenantName == "Tenant 1");
                Assert.Contains(result, l => l.TenantName == "Tenant 2");
                Assert.Contains(result, l => l.TenantName == "Tenant 3");
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsLease()
        {
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.GetByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal("Tenant 1", result.TenantName);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenLeaseDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.GetByIdAsync(999);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddAsync_CreatesLease()
        {
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var newLease = await repository.AddAsync(lease);
                Assert.NotNull(newLease);
            }

            using (var context = _fixture.CreateContext())
            {
                var result = await context.Leases.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("Tenant 1", result.TenantName);
            }
        }

        [Fact]
        public async Task UpdateAsync_ModifiesLease()
        {
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var leaseToUpdate = await context.Leases.FindAsync(1);
                Assert.NotNull(leaseToUpdate);
                leaseToUpdate.TenantName = "Tenant 2";

                var repository = new LeaseRepository(context);
                var result = await repository.UpdateAsync(1, leaseToUpdate);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var updatedLease = await context.Leases.FindAsync(1);
                Assert.NotNull(updatedLease);
                Assert.Equal("Tenant 2", updatedLease.TenantName);
            }
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenIdMismatch()
        {
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };
            var leaseUpdate = new Lease { Id = 2, HomeId = 1, TenantName = "Tenant 2", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.UpdateAsync(1, leaseUpdate);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenLeaseDoesNotExist()
        {
            var lease = new Lease { Id = 999, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.UpdateAsync(999, lease);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesLease()
        {
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            using (var context = _fixture.CreateContext())
            {
                context.Leases.Add(lease);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.DeleteAsync(1);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var deletedLease = await context.Leases.FindAsync(1);
                Assert.Null(deletedLease);
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenLeaseDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new LeaseRepository(context);
                var result = await repository.DeleteAsync(999);
                Assert.Null(result);
            }
        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Leases.RemoveRange(context.Leases);
                context.Homes.RemoveRange(context.Homes);
                context.Communities.RemoveRange(context.Communities);
                context.SaveChanges();

                if (context.Leases.Any() || context.Homes.Any() || context.Communities.Any())
                {
                    throw new InvalidOperationException("Failed to clear data.");
                }
            }
        }
    }
}