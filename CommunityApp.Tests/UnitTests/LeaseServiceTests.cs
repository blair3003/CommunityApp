using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;

namespace CommunityApp.Tests.UnitTests
{
    public class LeaseServiceTests : IDisposable
    {
        private readonly Mock<ILeaseRepository> _mockRepository;
        private readonly LeaseService _leaseService;

        public LeaseServiceTests()
        {
            _mockRepository = new Mock<ILeaseRepository>();
            _leaseService = new LeaseService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllLeasesAsync_ReturnsAllLeases()
        {
            // Arrange
            var leases = new List<Lease>
            {
                new() { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 2, HomeId = 2, TenantName = "Tenant 2", TenantEmail = "tenant2@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 3, HomeId = 3, TenantName = "Tenant 3", TenantEmail = "tenant3@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(leases);

            // Act
            var result = await _leaseService.GetAllLeasesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, l => l.TenantName == "Tenant 1");
            Assert.Contains(result, l => l.TenantName == "Tenant 2");
            Assert.Contains(result, l => l.TenantName == "Tenant 3");

            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetLeaseByIdAsync_ReturnsLease()
        {
            // Arrange
            var expectedLease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedLease);

            // Act
            var result = await _leaseService.GetLeaseByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tenant 1", result.TenantName);

            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetLeaseByIdAsync_ReturnsNull_WhenLeaseDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Lease?)null);

            // Act
            var result = await _leaseService.GetLeaseByIdAsync(999);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task AddLeaseAsync_CreatesLease()
        {
            // Arrange
            var newLease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Lease>()))
                .ReturnsAsync((Lease l) => l);

            // Act
            var result = await _leaseService.AddLeaseAsync(newLease);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Tenant 1", result.TenantName);

            _mockRepository.Verify(repo => repo.AddAsync(It.Is<Lease>(h => h == newLease)), Times.Once);
        }

        [Fact]
        public async Task UpdateLeaseAsync_ModifiesLease()
        {
            // Arrange
            var updatedLease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(1, It.IsAny<Lease>()))
                .ReturnsAsync(updatedLease);

            // Act
            var result = await _leaseService.UpdateLeaseAsync(1, updatedLease);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tenant 1", result.TenantName);

            _mockRepository.Verify(repo => repo.UpdateAsync(1, updatedLease), Times.Once);
        }

        [Fact]
        public async Task UpdateLeaseAsync_ReturnsNull_WhenIdMismatch()
        {
            // Arrange
            var lease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Lease>()))
                .ReturnsAsync((Lease?)null);

            // Act
            var result = await _leaseService.UpdateLeaseAsync(999, lease);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Lease>()), Times.Once);
        }

        [Fact]
        public async Task UpdateLeaseAsync_ReturnsNull_WhenLeaseDoesNotExist()
        {
            // Arrange
            var lease = new Lease { Id = 999, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Lease>()))
                .ReturnsAsync((Lease?)null);

            // Act
            var result = await _leaseService.UpdateLeaseAsync(999, lease);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Lease>()), Times.Once);
        }

        [Fact]
        public async Task DeleteLeaseAsync_RemovesLease()
        {
            // Arrange
            var deletedLease = new Lease { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) };

            _mockRepository
                .Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(deletedLease);

            // Act
            var result = await _leaseService.DeleteLeaseAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tenant 1", result.TenantName);

            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteLeaseAsync_ReturnsNull_WhenLeaseDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((Lease?)null);

            // Act
            var result = await _leaseService.DeleteLeaseAsync(999);

            // Assert
            Assert.Null(result);

            _mockRepository.Verify(repo => repo.DeleteAsync(999), Times.Once);
        }

        public void Dispose()
        {
            _mockRepository.Reset();
        }
    }
}
