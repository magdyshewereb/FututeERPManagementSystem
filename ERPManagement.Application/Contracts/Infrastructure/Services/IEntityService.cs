namespace ERPManagement.Application.Contracts.Infrastructure.Services
{
    public interface IEntityService<T>
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> GetAllAsync();
    }
}
