﻿using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories;
using CommunityApp.Tests.IntegrationTests.Fixtures;

namespace CommunityApp.Tests.IntegrationTests
{
    public class HomeRepositoryTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _fixture;

        public HomeRepositoryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;

            AddCommunities().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllHomes()
        {
            var homes = new List<Home>
            {
                new() { Id = 1, CommunityId = 1, Number = "1" },
                new() { Id = 2, CommunityId = 2, Number = "2" },
                new() { Id = 3, CommunityId = 3, Number = "3" }
            };

            using (var context = _fixture.CreateContext())
            {
                context.Homes.AddRange(homes);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new HomeRepository(context);
                var result = await repository.GetAllAsync();
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.Contains(result, h => h.Number == "1");
                Assert.Contains(result, h => h.Number == "2");
                Assert.Contains(result, h => h.Number == "3");
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsHome()
        {
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };

            using (var context = _fixture.CreateContext())
            {
                context.Homes.Add(home);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new HomeRepository(context);
                var result = await repository.GetByIdAsync(1);
                Assert.NotNull(result);
                Assert.Equal("1", result.Number);
            }
        }

        [Fact]
        public async Task AddAsync_CreatesHome()
        {
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };

            using (var context = _fixture.CreateContext())
            {
                var repository = new HomeRepository(context);
                var newHome = await repository.AddAsync(home);
                Assert.NotNull(newHome);
            }

            using (var context = _fixture.CreateContext())
            {
                var result = await context.Homes.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("1", result.Number);
            }
        }

        [Fact]
        public async Task UpdateAsync_ModifiesHome()
        {
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };

            using (var context = _fixture.CreateContext())
            {
                context.Homes.Add(home);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var homeToUpdate = await context.Homes.FindAsync(1);
                Assert.NotNull(homeToUpdate);
                homeToUpdate.Number = "2";

                var repository = new HomeRepository(context);
                var result = await repository.UpdateAsync(1, homeToUpdate);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var updatedHome = await context.Homes.FindAsync(1);
                Assert.NotNull(updatedHome);
                Assert.Equal("2", updatedHome.Number);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesHome()
        {
            var home = new Home { Id = 1, CommunityId = 1, Number = "1" };

            using (var context = _fixture.CreateContext())
            {
                context.Homes.Add(home);
                await context.SaveChangesAsync();
            }

            using (var context = _fixture.CreateContext())
            {
                var repository = new HomeRepository(context);
                var result = await repository.DeleteAsync(1);
                Assert.NotNull(result);
            }

            using (var context = _fixture.CreateContext())
            {
                var deletedHome = await context.Homes.FindAsync(1);
                Assert.Null(deletedHome);
            }
        }

        public void Dispose()
        {
            using (var context = _fixture.CreateContext())
            {
                context.Homes.RemoveRange(context.Homes);
                context.Communities.RemoveRange(context.Communities);
                context.SaveChanges();

                if (context.Homes.Any() || context.Communities.Any())
                {
                    throw new InvalidOperationException("Failed to clear data.");
                }
            }
        }

        private async Task AddCommunities()
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
        }
    }
}