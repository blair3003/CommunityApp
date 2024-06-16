using CommunityApp.Data.Models;

namespace CommunityApp.Data.Seeders
{
    public class CommunitiesSeed(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            if (!_context.Communities.Any())
            {
                var communities = new List<Community>()
                {
                    new() { Name = "Harmony Heights" },
                    new() { Name = "Serenity Springs" },
                    new() { Name = "Tranquil Grove" },
                    new() { Name = "Whispering Pines" },
                    new() { Name = "Radiant Meadows" }
                };

                await _context.Communities.AddRangeAsync(communities);
                await _context.SaveChangesAsync();
            }
        }
    }
}
