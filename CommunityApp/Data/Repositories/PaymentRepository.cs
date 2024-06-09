using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityApp.Data.Repositories
{
    public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<List<Payment>> GetAllAsync()
        {
            var payments = await _context.Payments
                .Include(p => p.Lease)
                .ToListAsync();

            return payments;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Lease)
                    .ThenInclude(l => l!.Tenant)
                .Include(p => p.Lease)
                    .ThenInclude(l => l!.Home)
                .FirstOrDefaultAsync(p => p.Id == id);

            return payment;
        }

        public async Task<Payment?> AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> UpdateAsync(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return null;
            }

            var existingPayment = await _context.Payments.FindAsync(id);

            if (existingPayment == null)
            {
                return null;
            }

            existingPayment.LeaseId = payment.LeaseId;
            existingPayment.Amount = payment.Amount;
            existingPayment.PaymentDate = payment.PaymentDate;
            existingPayment.Description = payment.Description;

            await _context.SaveChangesAsync();
            return existingPayment;
        }

        public async Task<Payment?> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return null;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}