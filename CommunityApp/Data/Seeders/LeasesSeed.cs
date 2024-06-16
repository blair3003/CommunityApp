using CommunityApp.Data.Models;

namespace CommunityApp.Data.Seeders
{
    public class LeasesSeed(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            if (!_context.Leases.Any())
            {
                var homes = _context.Homes;
                var leases = new List<Lease>();

                var random = new Random();

                var names = new List<string>
                {
                    "John Smith",
                    "Emily Johnson",
                    "Michael Williams",
                    "Jessica Brown",
                    "David Jones",
                    "Sarah Davis",
                    "Robert Miller",
                    "Jennifer Wilson",
                    "William Moore",
                    "Elizabeth Taylor"
                };

                foreach (var home in homes)
                {
                    var lease = new Lease
                    {
                        HomeId = home.Id,
                        TenantName = names[random.Next(names.Count)],
                        TenantEmail = "tenant@communityapp.com",
                        TenantPhone = "123-456-7890",
                        MonthlyPayment = home.BaseRent + random.Next(-100, 101),
                        DepositAmount = home.BaseDeposit,
                        PaymentDueDay = random.Next(1, 28),
                        LeaseStartDate = DateTime.UtcNow.AddMonths(random.Next(-6, 1)),
                        LeaseEndDate = DateTime.UtcNow.AddMonths(random.Next(1, 13)),
                        Notes = "Example lease notes"
                    };

                    leases.Add(lease);
                }

                await _context.Leases.AddRangeAsync(leases);
                await _context.SaveChangesAsync();
            }
        }
    }
}
