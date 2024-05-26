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
            Assert.True(false);
        }

        [Fact]
        public async Task AddAsync_CreatesCommunity()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task UpdateAsync_ModifiesCommunity()
        {
            Assert.True(false);
        }

        [Fact]
        public async Task DeleteAsync_RemovesCommunity()
        {
            Assert.True(false);
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
