using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;

namespace CommunityApp.Tests.UnitTests
{
    public class HomeServiceTests
    {
        [Fact]
        public async Task GetAllHomesAsync_ReturnsAllHomes()
        {
            var mockRepository = new Mock<IHomeRepository>();

            var homes = new List<Home>
            {
                new() { Id = 1, Number = "1" },
                new() { Id = 2, Number = "2" },
                new() { Id = 3, Number = "3" }
            };

            mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(homes);

            var homeService = new HomeService(mockRepository.Object);

            var result = await homeService.GetAllHomesAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Number == "1");
            Assert.Contains(result, c => c.Number == "2");
            Assert.Contains(result, c => c.Number == "3");

            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetHomeByIdAsync_ReturnsHome()
        {
            var mockRepository = new Mock<IHomeRepository>();
            var expectedHome = new Home { Id = 1, Number = "1" };

            mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedHome);

            var homeService = new HomeService(mockRepository.Object);

            var result = await homeService.GetHomeByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("1", result.Number);

            mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task AddHomeAsync_CreatesHome()
        {
            var mockRepository = new Mock<IHomeRepository>();
            var newHome = new Home { Id = 1, Number = "1" };

            mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Home>()))
                .ReturnsAsync((Home h) => h);

            var homeService = new HomeService(mockRepository.Object);

            var result = await homeService.AddHomeAsync(newHome);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("1", result.Number);

            mockRepository.Verify(repo => repo.AddAsync(It.Is<Home>(h => h == newHome)), Times.Once);
        }

        [Fact]
        public async Task UpdateHomeAsync_ModifiesHome()
        {
            var mockRepository = new Mock<IHomeRepository>();
            var updatedHome = new Home { Id = 1, Number = "1" };

            mockRepository
                .Setup(repo => repo.UpdateAsync(1, It.IsAny<Home>()))
                .ReturnsAsync(updatedHome);

            var homeService = new HomeService(mockRepository.Object);

            var result = await homeService.UpdateHomeAsync(1, updatedHome);

            Assert.NotNull(result);
            Assert.Equal("1", result.Number);

            mockRepository.Verify(repo => repo.UpdateAsync(1, updatedHome), Times.Once);
        }

        [Fact]
        public async Task DeleteHomeAsync_RemovesHome()
        {
            var mockRepository = new Mock<IHomeRepository>();
            var deletedHome = new Home { Id = 1, Number = "1" };

            mockRepository
                .Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(deletedHome);

            var homeRepository = new HomeService(mockRepository.Object);

            var result = await homeRepository.DeleteHomeAsync(1);

            Assert.NotNull(result);
            Assert.Equal("1", result.Number);

            mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
