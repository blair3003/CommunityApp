using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;

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
    }
}
