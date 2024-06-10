using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using System.Security.Claims;

namespace CommunityApp.Tests.UnitTests
{
    public class CommunityManagerServiceTests : IDisposable
    {
        private readonly Mock<ICommunityManagerRepository> _mockRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly CommunityManagerService _communityManagerService;

        public CommunityManagerServiceTests()
        {
            _mockRepository = new Mock<ICommunityManagerRepository>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null!, null!, null!, null!, null!, null!, null!, null!);
            _communityManagerService = new CommunityManagerService(_mockRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task GetCommunitiesByManagerIdAsync_ReturnsCommunitiesForManager()
        {
            // Arrange
            var communities = new List<Community>
            {
                new() { Id = 1, Name = "Community 1" },
                new() { Id = 2, Name = "Community 2" }
            };

            _mockRepository
                .Setup(repo => repo.GetCommunitiesByManagerIdAsync("1"))
                .ReturnsAsync(communities);

            // Act
            var result = await _communityManagerService.GetCommunitiesByManagerIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Community 1");
            Assert.Contains(result, c => c.Name == "Community 2");
            _mockRepository.Verify(repo => repo.GetCommunitiesByManagerIdAsync("1"), Times.Once);
        }

        [Fact]
        public async Task GetHomesByManagerIdAsync_ReturnsHomesForManager()
        {
            // Arrange
            var homes = new List<Home>
            {
                new() { Id = 1, CommunityId = 1, Number = "1" },
                new() { Id = 2, CommunityId = 1, Number = "2" }
            };

            _mockRepository
                .Setup(repo => repo.GetHomesByManagerIdAsync("1"))
                .ReturnsAsync(homes);

            // Act
            var result = await _communityManagerService.GetHomesByManagerIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Number == "1");
            Assert.Contains(result, c => c.Number == "2");
            _mockRepository.Verify(repo => repo.GetHomesByManagerIdAsync("1"), Times.Once);
        }

        [Fact]
        public async Task GetLeasesByManagerIdAsync_ReturnsLeasesForManager()
        {
            // Arrange
            var leases = new List<Lease>
            {
                new() { Id = 1, HomeId = 1, TenantName = "Tenant 1", TenantEmail = "tenant1@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 2, HomeId = 2, TenantName = "Tenant 2", TenantEmail = "tenant2@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) },
                new() { Id = 3, HomeId = 3, TenantName = "Tenant 3", TenantEmail = "tenant3@forthdev.com", TenantPhone = "01234567890", MonthlyPayment = 100.00m, PaymentDueDay = 1, LeaseStartDate = new DateTime(2024, 1, 1), LeaseEndDate = new DateTime(2024, 12, 31) }
            };

            _mockRepository
                .Setup(repo => repo.GetLeasesByManagerIdAsync("1"))
                .ReturnsAsync(leases);

            // Act
            var result = await _communityManagerService.GetLeasesByManagerIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, l => l.TenantName == "Tenant 1");
            Assert.Contains(result, l => l.TenantName == "Tenant 2");
            Assert.Contains(result, l => l.TenantName == "Tenant 3");
            _mockRepository.Verify(repo => repo.GetLeasesByManagerIdAsync("1"), Times.Once);
        }

        [Fact]
        public async Task GetPaymentsByManagerIdAsync_ReturnsPaymentsForManager()
        {
            // Arrange
            var payments = new List<Payment>
            {
                new() { Id = 1, LeaseId = 1, Amount = 100.00m, PaymentDate = new DateTime(2023, 1, 1) },
                new() { Id = 2, LeaseId = 2, Amount = 150.00m, PaymentDate = new DateTime(2023, 1, 1) },
                new() { Id = 3, LeaseId = 3, Amount = 200.00m, PaymentDate = new DateTime(2023, 1, 1) }
            };

            _mockRepository
                .Setup(repo => repo.GetPaymentsByManagerIdAsync("1"))
                .ReturnsAsync(payments);

            // Act
            var result = await _communityManagerService.GetPaymentsByManagerIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, p => p.Amount == 100.00m);
            Assert.Contains(result, p => p.Amount == 150.00m);
            Assert.Contains(result, p => p.Amount == 200.00m);
            _mockRepository.Verify(repo => repo.GetPaymentsByManagerIdAsync("1"), Times.Once);
        }

        [Fact]
        public async Task AddManagerToCommunityAsync_AddsManager()
        {
            // Arrange            
            var managerId = "1";
            var communityId = 1;
            var user = new ApplicationUser { Id = managerId, UserName = "Manager 1" };

            _mockUserManager.Setup(um => um.FindByIdAsync(managerId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetClaimsAsync(user)).ReturnsAsync([new Claim("IsManager", "true")]);
            _mockRepository.Setup(repo => repo.AddManagerToCommunityAsync(managerId, communityId)).ReturnsAsync(true);

            // Act
            var result = await _communityManagerService.AddManagerToCommunityAsync(managerId, communityId);

            // Assert
            Assert.True(result);
            _mockUserManager.Verify(um => um.FindByIdAsync(managerId), Times.Once);
            _mockUserManager.Verify(um => um.GetClaimsAsync(user), Times.Once);
            _mockRepository.Verify(repo => repo.AddManagerToCommunityAsync(managerId, communityId), Times.Once);
        }

        [Fact]
        public async Task AddManagerToCommunityAsync_UserNotFound_ThrowsArgumentException()
        {
            // Arrange
            var managerId = "1";
            var communityId = 1;

            _mockUserManager.Setup(um => um.FindByIdAsync(managerId)).ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _communityManagerService.AddManagerToCommunityAsync(managerId, communityId));
            _mockUserManager.Verify(um => um.FindByIdAsync(managerId), Times.Once);
            _mockUserManager.Verify(um => um.GetClaimsAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _mockRepository.Verify(repo => repo.AddManagerToCommunityAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddManagerToCommunityAsync_UserNotManager_ThrowsInvalidOperationException()
        {
            // Arrange
            var managerId = "1";
            var communityId = 1;
            var user = new ApplicationUser { Id = managerId, UserName = "Manager 1" };

            _mockUserManager.Setup(um => um.FindByIdAsync(managerId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetClaimsAsync(user)).ReturnsAsync([]);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _communityManagerService.AddManagerToCommunityAsync(managerId, communityId));
            _mockUserManager.Verify(um => um.FindByIdAsync(managerId), Times.Once);
            _mockUserManager.Verify(um => um.GetClaimsAsync(user), Times.Once);
            _mockRepository.Verify(repo => repo.AddManagerToCommunityAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RemoveManagerFromCommunityAsync_RemovesManager()
        {
            // Arrange
            var managerId = "1";
            var communityId = 1;

            _mockRepository.Setup(repo => repo.RemoveManagerFromCommunityAsync(managerId, communityId)).ReturnsAsync(true);

            // Act
            var result = await _communityManagerService.RemoveManagerFromCommunityAsync(managerId, communityId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.RemoveManagerFromCommunityAsync(managerId, communityId), Times.Once);
        }

        [Fact]
        public async Task RemoveManagerFromCommunityAsync_Failure_ThrowsInvalidOperationException()
        {
            // Arrange
            var managerId = "1";
            var communityId = 1;

            _mockRepository.Setup(repo => repo.RemoveManagerFromCommunityAsync(managerId, communityId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _communityManagerService.RemoveManagerFromCommunityAsync(managerId, communityId));
            _mockRepository.Verify(repo => repo.RemoveManagerFromCommunityAsync(managerId, communityId), Times.Once);
        }

        public void Dispose()
        {
            _mockUserManager.Reset();
            _mockRepository.Reset();
        }
    }
}