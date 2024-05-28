namespace CommunityApp.Data.Repositories.Interfaces
{
    public interface IRepository<T, TKey>
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TKey id);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(TKey id, T entity);
        Task<T?> DeleteAsync(TKey id);
    }
}