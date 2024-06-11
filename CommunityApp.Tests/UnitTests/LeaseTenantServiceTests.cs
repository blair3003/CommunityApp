using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;

namespace CommunityApp.Tests.UnitTests
{
    public class LeaseTenantServiceTests : IDisposable
    {
        private readonly Mock<ILeaseTenantRepository> _mockRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly LeaseTenantService _leaseTenantService;

        public LeaseTenantServiceTests()
        {
            _mockRepository = new Mock<ILeaseTenantRepository>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null!, null!, null!, null!, null!, null!, null!, null!);
            _leaseTenantService = new LeaseTenantService(_mockRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task LinkTenantToLeaseAsync_LinksTenantToLease()
        {
            // Arrange            
            var tenantId = "1";
            var leaseId = 1;

            _mockRepository.Setup(repo => repo.LinkTenantToLeaseAsync(tenantId, leaseId)).ReturnsAsync(true);

            // Act
            var result = await _leaseTenantService.LinkTenantToLeaseAsync(tenantId, leaseId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.LinkTenantToLeaseAsync(tenantId, leaseId), Times.Once);
        }

        [Fact]
        public async Task UnlinkTenantFromLeaseAsync_UnlinksTenantFromLease()
        {
            // Arrange            
            var tenantId = "1";
            var leaseId = 1;

            _mockRepository.Setup(repo => repo.UnlinkTenantFromLeaseAsync(tenantId, leaseId)).ReturnsAsync(true);

            // Act
            var result = await _leaseTenantService.UnlinkTenantFromLeaseAsync(tenantId, leaseId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.UnlinkTenantFromLeaseAsync(tenantId, leaseId), Times.Once);
        }

        public void Dispose()
        {
            _mockUserManager.Reset();
            _mockRepository.Reset();
        }
    }
}
