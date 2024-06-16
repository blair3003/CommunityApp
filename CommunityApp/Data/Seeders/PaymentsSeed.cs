using CommunityApp.Data.Models;

namespace CommunityApp.Data.Seeders
{
    public class PaymentsSeed(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            if (!_context.Payments.Any())
            {
                var payments = new List<Payment>();
                var leases = _context.Leases;

                foreach (var lease in leases)
                {
                    var payment = new Payment
                    {
                        LeaseId = lease.Id,
                        Amount = lease.MonthlyPayment,
                        PaymentDate = lease.LeaseStartDate.AddDays(5),
                        Description = "Rent"
                    };

                    payments.Add(payment);
                }

                await _context.Payments.AddRangeAsync(payments);
                await _context.SaveChangesAsync();
            }
        }
    }
}
