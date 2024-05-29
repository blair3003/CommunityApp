using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace CommunityApp.Tests.UnitTests
{
    public class CommunityServiceTests : IDisposable
    {
        private readonly Mock<ICommunityRepository> _mockRepository;
        private readonly CommunityService _communityService;

        public CommunityServiceTests()
        {
            _mockRepository = new Mock<ICommunityRepository>();
            _communityService = new CommunityService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllCommunitiesAsync_ReturnsAllCommunities()
        {
            // Arrange
            var communities = new List<Community>
            {
                new() { Id = 1, Name = "Community 1" },
                new() { Id = 2, Name = "Community 2" },
                new() { Id = 3, Name = "Community 3" }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(communities);

            // Act
            var result = await _communityService.GetAllCommunitiesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Name == "Community 1");
            Assert.Contains(result, c => c.Name == "Community 2");
            Assert.Contains(result, c => c.Name == "Community 3");
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCommunityByIdAsync_ReturnsCommunity()
        {
            // Arrange
            var community = new Community { Id = 1, Name = "Community" };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(community);

            // Act
            var result = await _communityService.GetCommunityByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(community.Name, result.Name);
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetCommunityByIdAsync_ReturnsNull_WhenCommunityDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Community?)null);

            // Act
            var result = await _communityService.GetCommunityByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task AddCommunityAsync_CreatesCommunity()
        {
            // Arrange
            var newCommunity = new Community { Id = 1, Name = "New Community" };

            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Community>()))
                .ReturnsAsync((Community c) => c);

            // Act
            var result = await _communityService.AddCommunityAsync(newCommunity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(newCommunity.Name, result.Name);
            _mockRepository.Verify(repo => repo.AddAsync(It.Is<Community>(c => c == newCommunity)), Times.Once);
        }

        [Fact]
        public async Task UpdateCommunityAsync_ModifiesCommunity()
        {
            // Arrange
            var updatedCommunity = new Community { Id = 1, Name = "Updated Community" };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(1, It.IsAny<Community>()))
                .ReturnsAsync(updatedCommunity);

            // Act
            var result = await _communityService.UpdateCommunityAsync(1, updatedCommunity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedCommunity.Name, result.Name);
            _mockRepository.Verify(repo => repo.UpdateAsync(1, updatedCommunity), Times.Once);
        }

        [Fact]
        public async Task UpdateCommunityAsync_ReturnsNull_WhenIdMismatch()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Community>()))
                .ReturnsAsync((Community?)null);

            // Act
            var result = await _communityService.UpdateCommunityAsync(999, new Community { Id = 1, Name = "Updated Community" });

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Community>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCommunityAsync_ReturnsNull_WhenCommunityDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Community>()))
                .ReturnsAsync((Community?)null);

            // Act
            var result = await _communityService.UpdateCommunityAsync(999, new Community { Id = 999, Name = "Updated Community" });

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Community>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCommunityAsync_RemovesCommunity()
        {
            // Arrange
            var deletedCommunity = new Community { Id = 1, Name = "Deleted Community" };

            _mockRepository
                .Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(deletedCommunity);

            // Act
            var result = await _communityService.DeleteCommunityAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(deletedCommunity.Name, result.Name);
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCommunityAsync_ReturnsNull_WhenCommunityDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((Community?)null);

            // Act
            var result = await _communityService.DeleteCommunityAsync(999);

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