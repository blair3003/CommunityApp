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
                var communities = new List<Community>();
                var communityNames = new string[]
                {
                    "Harmony Heights",
                    "Serenity Springs",
                    "Tranquil Grove",
                    "Whispering Pines",
                    "Radiant Meadows",
                    "Emerald Vista",
                    "Sunset Ridge",
                    "Azure Haven",
                    "Enchanted Woods",
                    "Crystal Waters"
                };

                foreach (string communityName in communityNames)
                {
                    var community = new Community
                    {
                        Name = communityName,
                        // Optionally, you can seed other properties here
                    };

                    communities.Add(community);
                }

                await _context.Communities.AddRangeAsync(communities);
                await _context.SaveChangesAsync();
            }
        }
    }
}
