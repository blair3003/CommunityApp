using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Tests.IntegrationTests
{
    public class CommunityManagerRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public CommunityManagerRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetCommunitiesByManagerIdAsync_ReturnsCommunitiesForManager()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.GetCommunitiesByManagerIdAsync("1");
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Contains(result, c => c.Name == "Community 1");
            }
        }

        [Fact]
        public async Task AddManagerToCommunityAsync_AddsManagerToCommunity()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.AddManagerToCommunityAsync("1", 1);
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var managedCommunity = await context.Communities
                    .Include(c => c.Managers)
                    .FirstOrDefaultAsync(c => c.Id == 1);
                Assert.NotNull(managedCommunity);
                Assert.Single(managedCommunity.Managers);
            }
        }

        [Fact]
        public async Task RemoveManagerFromCommunityAsync_RemovesManagerFromCommunity()
        {
            var manager = new ApplicationUser { Id = "1", UserName = "TestUser", Email = "test@user.com" };
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Users.Add(manager);
                community.Managers.Add(manager);
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityManagerRepository(context);
                var result = await repository.RemoveManagerFromCommunityAsync("1", 1);
                Assert.True(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var managedCommunity = await context.Communities
                    .Include(c => c.Managers)
                    .FirstOrDefaultAsync(c => c.Id == 1);
                Assert.NotNull(managedCommunity);
                Assert.Empty(managedCommunity.Managers);
            }

        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Communities.RemoveRange(context.Communities);
                context.Users.RemoveRange(context.Users);
                context.SaveChanges();

                if (context.Communities.Any() || context.Users.Any())
                {
                    throw new InvalidOperationException("Failed to clear data.");
                }
            }
        }
    }
}