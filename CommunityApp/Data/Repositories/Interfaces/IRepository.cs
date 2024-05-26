namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<T?> DeleteAsync(int id);
    }
}
