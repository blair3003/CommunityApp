using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;

namespace CommunityApp.Tests.IntegrationTests
{
    public class PaymentRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public PaymentRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.AddCommunities().GetAwaiter().GetResult();
            _fixture.AddHomes().GetAwaiter().GetResult();
            _fixture.AddLeases().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPayments()
        {
            var payments = new List<Payment>
            {
                new() { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) },
                new() { Id = 2, LeaseId = 2, Amount = 150.00m, PaymentDate = new DateTime(2023, 1, 1) },
                new() { Id = 3, LeaseId = 3, Amount = 200.00m, PaymentDate = new DateTime(2023, 1, 1) }
            };

            using (var context = _fixture.CreateContext())
            {
                context.Payments.AddRange(payments);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.GetAllAsync();
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Contains(result, p => p.Amount == 100.00m);
                Assert.Contains(result, p => p.Amount == 150.00m);
                Assert.Contains(result, p => p.Amount == 200.00m);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPayment()
        {
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.GetByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal(100.00m, result.Amount);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenPaymentDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.GetByIdAsync(999);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddAsync_CreatesPayment()
        {
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var newPayment = await repository.AddAsync(payment);
                Assert.NotNull(newPayment);
            }

            using (var context = _fixture.CreateContext())
            {
                var result = await context.Payments.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal(100.00m, result.Amount);
            }
        }

        [Fact]
        public async Task UpdateAsync_ModifiesPayment()
        {
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var paymentToUpdate = await context.Payments.FindAsync(1);
                Assert.NotNull(paymentToUpdate);

                paymentToUpdate.Amount = 150.00m;

                var repository = new PaymentRepository(context);
                var result = await repository.UpdateAsync(1, paymentToUpdate);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var updatedPayment = await context.Payments.FindAsync(1);
                Assert.NotNull(updatedPayment);
                Assert.Equal(150.00m, updatedPayment.Amount);
            }
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenIdMismatch()
        {
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };
            var paymentUpdate = new Payment { Id = 2, LeaseId = 2, Amount = 150.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.UpdateAsync(1, paymentUpdate);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenPaymentDoesNotExist()
        {
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.UpdateAsync(999, payment);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesPayment()
        {
            var payment = new Payment { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) };

            using (var context = _fixture.CreateContext())
            {
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.DeleteAsync(1);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var deletedPayment = await context.Payments.FindAsync(1);
                Assert.Null(deletedPayment);
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenPaymentDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new PaymentRepository(context);
                var result = await repository.DeleteAsync(999);
                Assert.Null(result);
            }
        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Payments.RemoveRange(context.Payments);
                context.Leases.RemoveRange(context.Leases);
                context.Homes.RemoveRange(context.Homes);
                context.Communities.RemoveRange(context.Communities);
                context.SaveChanges();

                if (context.Payments.Any() || context.Leases.Any() || context.Homes.Any() || context.Communities.Any())
                {
                    throw new InvalidOperationException("Failed to clear data.");
                }
            }
        }
    }
}