using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;
using System.Net.Sockets;

namespace CommunityApp.Tests.UnitTests
{
    public class CommunityServiceTests
    {
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

            var communityService = new CommunityService(mockRepository.Object);

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

            var communityService = new CommunityService(mockRepository.Object);

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

            var communityService = new CommunityService(mockRepository.Object);

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

            var communityService = new CommunityService(mockRepository.Object);

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

            var communityService = new CommunityService(mockRepository.Object);

            var result = await communityService.DeleteCommunityAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Community 1", result.Name);

            mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
