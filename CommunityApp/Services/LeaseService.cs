using CommunityApp.Data.Models;
using CommunityApp.Data.Repositories.Interfaces;

namespace CommunityApp.Services
{
    public class LeaseService(ILeaseRepository repository)
    {
        private readonly ILeaseRepository _repository = repository;

        public async Task<List<Lease>> GetAllLeasesAsync()
        {
            var leases = await _repository.GetAllAsync();            
            return leases;
        }

        public async Task<Lease?> GetLeaseByIdAsync(int id)
        {
            var lease = await _repository.GetByIdAsync(id);
            return lease;
        }

        public async Task<Lease?> AddLeaseAsync(Lease lease)
        {
            var newLease = await _repository.AddAsync(lease);
            return newLease;
        }

        public async Task<Lease?> UpdateLeaseAsync(int id, Lease lease)
        {
            var updatedLease = await _repository.UpdateAsync(id, lease);
            return updatedLease;
        }

        public async Task<Lease?> DeleteLeaseAsync(int id)
        {
            var deletedLease = await _repository.DeleteAsync(id);
            return deletedLease;
        }
    }
}
