using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;

namespace CommunityApp.Tests.IntegrationTests
{
    public class CommunityRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture = fixture;

        [Fact]
        public async Task GetAllAsync_ReturnsAllCommunities()
        {
            var communities = new List<Community>
            {
                new() { Id = 1, Name = "Community 1" },
                new() { Id = 2, Name = "Community 2" },
                new() { Id = 3, Name = "Community 3" }
            };

            using (var context = _fixture.CreateContext())
            {
                context.Communities.AddRange(communities);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityRepository(context);
                var result = await repository.GetAllAsync();
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Contains(result, c => c.Name == "Community 1");
                Assert.Contains(result, c => c.Name == "Community 2");
                Assert.Contains(result, c => c.Name == "Community 3");
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCommunity()
        {
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityRepository(context);
                var result = await repository.GetByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal("Community 1", result.Name);
            }
        }

        [Fact]
        public async Task AddAsync_CreatesCommunity()
        {
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityRepository(context);
                var newCommunity = await repository.AddAsync(community);
                Assert.NotNull(newCommunity);
            }

            using (var context = _fixture.CreateContext())
            {
                var result = await context.Communities.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("Community 1", result.Name);
            }
        }

        [Fact]
        public async Task UpdateAsync_ModifiesCommunity()
        {
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var communityToUpdate = await context.Communities.FindAsync(1);
                Assert.NotNull(communityToUpdate);
                communityToUpdate.Name = "Updated Community";

                var repository = new CommunityRepository(context);
                var result = await repository.UpdateAsync(1, communityToUpdate);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var updatedCommunity = await context.Communities.FindAsync(1);
                Assert.NotNull(updatedCommunity);
                Assert.Equal("Updated Community", updatedCommunity.Name);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesCommunity()
        {
            var community = new Community { Id = 1, Name = "Community 1" };

            using (var context = _fixture.CreateContext())
            {
                context.Communities.Add(community);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new CommunityRepository(context);
                var result = await repository.DeleteAsync(1);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var deletedCommunity = await context.Communities.FindAsync(1);
                Assert.Null(deletedCommunity);
            }
        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Communities.RemoveRange(context.Communities);
                context.SaveChanges();

                if (context.Communities.Any())
                {
                    throw new InvalidOperationException("Failed to clear communities.");
                }
            }
        }
    }
}
