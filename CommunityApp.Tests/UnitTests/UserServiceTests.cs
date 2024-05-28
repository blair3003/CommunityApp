using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Moq;

namespace CommunityApp.Tests.UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockRepository.Object);

            var users = new List<UserDto>
            {
                new UserDto { Id = "1", UserName = "user1" },
                new UserDto { Id = "2", UserName = "user2" },
                new UserDto { Id = "3", UserName = "user3" }
            };

            mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockRepository.Object);

            var userId = "1";
            var expectedUser = new UserDto { Id = userId, UserName = "user1" };

            mockRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockRepository.Object);

            var userId = "1";
            var deletedUser = new UserDto { Id = userId, UserName = "user1" };

            mockRepository
                .Setup(repo => repo.DeleteAsync(userId))
                .ReturnsAsync(deletedUser);

            // Act
            var result = await userService.DeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(deletedUser.Id, result.Id);
            Assert.Equal(deletedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task AddIsManagerClaimAsync_AddsClaim()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockRepository.Object);

            var userId = "1";

            mockRepository
                .Setup(repo => repo.AddIsManagerClaimAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await userService.AddIsManagerClaimAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveIsManagerClaimAsync_RemovesClaim()
        {
            // Arrange
            var mockRepository = new Mock<IUserRepository>();
            var userService = new UserService(mockRepository.Object);

            var userId = "1";

            mockRepository
                .Setup(repo => repo.RemoveIsManagerClaimAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await userService.RemoveIsManagerClaimAsync(userId);

            // Assert
            Assert.True(result);
        }
    }
}
