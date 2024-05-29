using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;
using System.Security.Claims;

namespace CommunityApp.Tests.IntegrationTests
{
    public class UserRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            AddUsers().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.GetAllAsync();

                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Contains(result, u => u.UserName == "TestUser1");
                Assert.Contains(result, u => u.UserName == "TestUser2");
                Assert.Contains(result, u => u.UserName == "TestUser3");
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.GetByIdAsync("1");

                Assert.NotNull(result);
                Assert.Equal("TestUser1", result.UserName);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.GetByIdAsync("999");
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesUser()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.DeleteAsync("1");
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var deletedUser = await _fixture.UserManager.FindByIdAsync("1");
                Assert.Null(deletedUser);
            }
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.DeleteAsync("999");
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddIsManagerClaimAsync_AddsClaimToUser()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.AddIsManagerClaimAsync("1");
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var userWithClaim = await _fixture.UserManager.FindByIdAsync("1");
                var claims = await _fixture.UserManager.GetClaimsAsync(userWithClaim!);
                Assert.Contains(claims, c => c.Type == "IsManager" && c.Value == "true");
            }
        }

        [Fact]
        public async Task AddIsManagerClaimAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.AddIsManagerClaimAsync("999");
                Assert.False(result);
            }
        }

        [Fact]
        public async Task RemoveIsManagerClaimAsync_RemovesClaimFromUser()
        {
            using (var context = _fixture.CreateContext())
            {
                var user = await _fixture.UserManager.FindByIdAsync("1");
                var result = await _fixture.UserManager.AddClaimAsync(user!, new Claim("IsManager", "true"));
                Assert.True(result.Succeeded);
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.RemoveIsManagerClaimAsync("1");
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var userWithClaim = await _fixture.UserManager.FindByIdAsync("1");
                var claims = await _fixture.UserManager.GetClaimsAsync(userWithClaim!);
                Assert.DoesNotContain(claims, c => c.Type == "IsManager" && c.Value == "true");
            }
        }

        [Fact]
        public async Task RemoveIsManagerClaimAsync_ReturnsFalse_WhenUserDoesNotExist()
        {
            using (var context = _fixture.CreateContext())
            {
                var repository = new UserRepository(_fixture.UserManager);
                var result = await repository.RemoveIsManagerClaimAsync("999");
                Assert.False(result);
            }
        }

        public async void Dispose()
        {
            var users = _fixture.UserManager.Users.ToList();
            foreach (var user in users)
            {
                await _fixture.UserManager.DeleteAsync(user);
            }
        }

        private async Task AddUsers()
        {
            var users = new List<ApplicationUser>
            {
                new() { Id = "1", UserName = "TestUser1", Email = "test1@user.com" },
                new() { Id = "2", UserName = "TestUser2", Email = "test2@user.com" },
                new() { Id = "3", UserName = "TestUser3", Email = "test3@user.com" }
            };

            foreach (var user in users)
            {
                await _fixture.UserManager.CreateAsync(user);
            }
        }
    }
}