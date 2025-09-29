using Microsoft.EntityFrameworkCore.Storage;

namespace Hr.Application.Interfaces.GenericRepository.Command
{
    public partial interface IBaseRepository<T> where T : class
    {
        #region Transactions
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        #endregion

        #region Create
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        #endregion

        #region Update
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        #endregion

        #region Delete
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(long id);
        Task DeleteAsync(int id);
        #endregion

        #region Read
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        #endregion
    }
}
