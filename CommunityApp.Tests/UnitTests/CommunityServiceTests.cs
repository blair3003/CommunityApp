using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace CommunityApp.Tests.UnitTests
{
    public class CommunityServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

        public CommunityServiceTests()
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null)!;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public async Task GetAllCommunitiesAsync_ReturnsAllCommunities()
        {
            var mockRepository = new Mock<ICommunityRepository>();

            var communities = new List<Community>
            {
                new() { Id = 1, Name = "Community 1" },
                new() { Id = 2, Name = "Community 2" },
                new() { Id = 3, Name = "Community 3" }
            };

            mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(communities);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.GetAllCommunitiesAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Name == "Community 1");
            Assert.Contains(result, c => c.Name == "Community 2");
            Assert.Contains(result, c => c.Name == "Community 3");

            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCommunityByIdAsync_ReturnsCommunity()
        {
            var mockRepository = new Mock<ICommunityRepository>();

            var expectedCommunity = new Community { Id = 1, Name = "Community 1" };

            mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedCommunity);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.GetCommunityByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Community 1", result.Name);

            mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task AddCommunityAsync_CreatesCommunity()
        {
            var mockRepository = new Mock<ICommunityRepository>();

            var newCommunity = new Community { Id = 1, Name = "Community 1" };

            mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Community>()))
                .ReturnsAsync((Community c) => c);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.AddCommunityAsync(newCommunity);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Community 1", result.Name);

            mockRepository.Verify(repo => repo.AddAsync(It.Is<Community>(c => c == newCommunity)), Times.Once);
        }

        [Fact]
        public async Task UpdateCommunityAsync_ModifiesCommunity()
        {
            var mockRepository = new Mock<ICommunityRepository>();

            var updatedCommunity = new Community { Id = 1, Name = "Community 1" };

            mockRepository
                .Setup(repo => repo.UpdateAsync(1, It.IsAny<Community>()))
                .ReturnsAsync(updatedCommunity);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.UpdateCommunityAsync(1, updatedCommunity);

            Assert.NotNull(result);
            Assert.Equal("Community 1", result.Name);

            mockRepository.Verify(repo => repo.UpdateAsync(1, updatedCommunity), Times.Once);
        }

        [Fact]
        public async Task DeleteCommunityAsync_RemovesCommunity()
        {
            var mockRepository = new Mock<ICommunityRepository>();

            var deletedCommunity = new Community { Id = 1, Name = "Community 1" };

            mockRepository
                .Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(deletedCommunity);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.DeleteCommunityAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Community 1", result.Name);

            mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task AssignManagerToCommunityAsync_AssignsManager()
        {
            var mockRepository = new Mock<ICommunityRepository>();
            var managerId = "manager1";
            var communityId = 1;
            var user = new ApplicationUser { Id = managerId, UserName = "manager" };

            _mockUserManager
                .Setup(um => um.FindByIdAsync(managerId))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(um => um.GetClaimsAsync(user))
                .ReturnsAsync(
                [
                    new Claim("IsManager", "true")
                ]);

            var expectedCommunity = new Community { Id = communityId, Name = "Community 1" };

            mockRepository
                .Setup(repo => repo.AssignManagerToCommunityAsync(managerId, communityId))
                .ReturnsAsync(expectedCommunity);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.AssignManagerToCommunityAsync(managerId, communityId);

            Assert.NotNull(result);
            Assert.Equal(expectedCommunity.Name, result.Name);

            _mockUserManager.Verify(um => um.FindByIdAsync(managerId), Times.Once);
            _mockUserManager.Verify(um => um.GetClaimsAsync(user), Times.Once);
            mockRepository.Verify(repo => repo.AssignManagerToCommunityAsync(managerId, communityId), Times.Once);
        }

        [Fact]
        public async Task RemoveManagerFromCommunityAsync_RemovesManager()
        {
            var mockRepository = new Mock<ICommunityRepository>();
            var managerId = "manager1";
            var communityId = 1;
            var expectedCommunity = new Community { Id = communityId, Name = "Community 1" };

            mockRepository
                .Setup(repo => repo.RemoveManagerFromCommunityAsync(managerId, communityId))
                .ReturnsAsync(expectedCommunity);

            var communityService = new CommunityService(mockRepository.Object, _mockUserManager.Object);

            var result = await communityService.RemoveManagerFromCommunityAsync(managerId, communityId);

            Assert.NotNull(result);
            Assert.Equal(expectedCommunity.Name, result.Name);

            mockRepository.Verify(repo => repo.RemoveManagerFromCommunityAsync(managerId, communityId), Times.Once);
        }
    }
}
