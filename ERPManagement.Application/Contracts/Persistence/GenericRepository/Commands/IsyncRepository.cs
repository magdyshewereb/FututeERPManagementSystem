using Microsoft.EntityFrameworkCore.Storage;

namespace Hr.Application.Interfaces.GenericRepository.Command
{
    public partial interface IBaseRepository<T> where T : class
    {
        #region Transactions
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        #endregion

        #region Create
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        #endregion

        #region Update
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        #endregion

        #region Delete
        void Delete(T entity);
        void Delete(long id);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> entities);
        #endregion

        #region Utilities
        void ClearChangeTracker();
        #endregion
    }
}
