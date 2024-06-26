﻿using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using CommunityApp.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace CommunityApp.Tests.UnitTests
{
    public class UserServiceTests : IDisposable
    {
        private readonly Mock<IUserRepository> _mockRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockRepository = new Mock<IUserRepository>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null!, null!, null!, null!, null!, null!, null!, null!);
            _userService = new UserService(_mockRepository.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new() { Id = "1", UserName = "user1" },
                new() { Id = "2", UserName = "user2" },
                new() { Id = "3", UserName = "user3" }
            };

            _mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count);
            _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var userId = "1";
            var expectedUser = new UserDto { Id = userId, UserName = "user1" };

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.UserName, result.UserName);
            _mockRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "999";

            _mockRepository
                .Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((UserDto?)null);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }


        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            // Arrange
            var userId = "1";
            var user = new ApplicationUser { Id = userId, UserName = "user1" };
            var deletedUser = new UserDto { Id = userId, UserName = "user1" };

            _mockRepository
                .Setup(repo => repo.DeleteAsync(userId))
                .ReturnsAsync(deletedUser);

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetClaimsAsync(user)).ReturnsAsync([]);

            // Act
            var result = await _userService.DeleteUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(deletedUser.Id, result.Id);
            Assert.Equal(deletedUser.UserName, result.UserName);
            _mockRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_ThrowsException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync((UserDto?)null);

            _mockUserManager.Setup(um => um.FindByIdAsync("999")).ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.DeleteUserAsync("999"));
            _mockRepository.Verify(repo => repo.DeleteAsync("999"), Times.Never);
        }

        [Fact]
        public async Task AddIsManagerClaimAsync_AddsClaim()
        {
            // Arrange
            var userId = "1";

            _mockRepository
                .Setup(repo => repo.AddIsManagerClaimAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.AddIsManagerClaimAsync(userId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.AddIsManagerClaimAsync(userId), Times.Once);
        }

        [Fact]
        public async Task AddIsManagerClaimAsync_ReturnsFalse_WhenAddClaimFails()
        {
            // Arrange
            var userId = "1";

            _mockRepository
                .Setup(repo => repo.AddIsManagerClaimAsync(userId))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.AddIsManagerClaimAsync(userId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.AddIsManagerClaimAsync(userId), Times.Once);
        }


        [Fact]
        public async Task RemoveIsManagerClaimAsync_RemovesClaim()
        {
            // Arrange
            var userId = "1";

            _mockRepository
                .Setup(repo => repo.RemoveIsManagerClaimAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RemoveIsManagerClaimAsync(userId);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(repo => repo.RemoveIsManagerClaimAsync(userId), Times.Once);
        }

        [Fact]
        public async Task RemoveIsManagerClaimAsync_ReturnsFalse_WhenRemoveClaimFails()
        {
            // Arrange
            var userId = "1";

            _mockRepository
                .Setup(repo => repo.RemoveIsManagerClaimAsync(userId))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.RemoveIsManagerClaimAsync(userId);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(repo => repo.RemoveIsManagerClaimAsync(userId), Times.Once);
        }

        public void Dispose()
        {
            _mockRepository.Reset();
        }
    }
}