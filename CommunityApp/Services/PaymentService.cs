using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;

namespace CommunityApp.Services
{
    public class PaymentService(IPaymentRepository repository)
    {
        private readonly IPaymentRepository _repository = repository;

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var payments = await _repository.GetAllAsync();
            return payments;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            var payment = await _repository.GetByIdAsync(id);
            return payment;
        }

        public async Task<Payment?> AddPaymentAsync(Payment payment)
        {
            var newPayment = await _repository.AddAsync(payment);
            return newPayment;
        }

        public async Task<Payment?> UpdatePaymentAsync(int id, Payment payment)
        {
            var updatedPayment = await _repository.UpdateAsync(id, payment);
            return updatedPayment;
        }

        public async Task<Payment?> DeletePaymentAsync(int id)
        {
            var deletedPayment = await _repository.DeleteAsync(id);
            return deletedPayment;
        }
    }
}
