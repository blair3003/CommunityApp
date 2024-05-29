using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;

namespace CommunityApp.Tests.UnitTests
{
    public class HomeServiceTests : IDisposable
    {
        private readonly Mock<IHomeRepository> _mockRepository;
        private readonly HomeService _homeService;

        public HomeServiceTests()
        {
            _mockRepository = new Mock<IHomeRepository>();
            _homeService = new HomeService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllHomesAsync_ReturnsAllHomes()
        {
            // Arrange
            var homes = new List<Home>
            {
                new() { Id = 1, Number = "1" },
                new() { Id = 2, Number = "2" },
                new() { Id = 3, Number = "3" }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(homes);

            // Act
            var result = await _homeService.GetAllHomesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.Number == "1");
            Assert.Contains(result, c => c.Number == "2");
            Assert.Contains(result, c => c.Number == "3");
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetHomeByIdAsync_ReturnsHome()
        {
            // Arrange
            var expectedHome = new Home { Id = 1, Number = "1" };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(expectedHome);

            // Act
            var result = await _homeService.GetHomeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Number);
            _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetHomeByIdAsync_ReturnsNull_WhenHomeDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Home?)null);

            // Act
            var result = await _homeService.GetHomeByIdAsync(999);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task AddHomeAsync_CreatesHome()
        {
            // Arrange
            var newHome = new Home { Id = 1, Number = "1" };

            _mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Home>()))
                .ReturnsAsync((Home h) => h);

            // Act
            var result = await _homeService.AddHomeAsync(newHome);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("1", result.Number);
            _mockRepository.Verify(repo => repo.AddAsync(It.Is<Home>(h => h == newHome)), Times.Once);
        }

        [Fact]
        public async Task UpdateHomeAsync_ModifiesHome()
        {
            // Arrange
            var updatedHome = new Home { Id = 1, Number = "1" };

            _mockRepository
                .Setup(repo => repo.UpdateAsync(1, It.IsAny<Home>()))
                .ReturnsAsync(updatedHome);

            // Act
            var result = await _homeService.UpdateHomeAsync(1, updatedHome);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Number);
            _mockRepository.Verify(repo => repo.UpdateAsync(1, updatedHome), Times.Once);
        }

        [Fact]
        public async Task UpdateHomeAsync_ReturnsNull_WhenIdMismatch()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Home>()))
                .ReturnsAsync((Home?)null);

            // Act
            var result = await _homeService.UpdateHomeAsync(999, new Home { Id = 1, Number = "2" });

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Home>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHomeAsync_ReturnsNull_WhenHomeDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Home>()))
                .ReturnsAsync((Home?)null);

            // Act
            var result = await _homeService.UpdateHomeAsync(999, new Home { Id = 999, Number = "2" });

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.UpdateAsync(999, It.IsAny<Home>()), Times.Once);
        }

        [Fact]
        public async Task DeleteHomeAsync_RemovesHome()
        {
            // Arrange
            var deletedHome = new Home { Id = 1, Number = "1" };

            _mockRepository
                .Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(deletedHome);

            // Act
            var result = await _homeService.DeleteHomeAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Number);
            _mockRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteHomeAsync_ReturnsNull_WhenHomeDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((Home?)null);

            // Act
            var result = await _homeService.DeleteHomeAsync(999);

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