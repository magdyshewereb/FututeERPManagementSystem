using System.Linq.Expressions;

namespace RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries
{
    public partial interface IQueryGenericRepository<T>
    {
        Task<T?> FindAsync(long id);
        Task<T?> FindAsync(int id);
        Task<IQueryable<T>> GetQueryableAsync();

        Task<T?> GetSingleAsync(
            Expression<Func<T, bool>> predicate,
            List<Expression<Func<T, object>>>? includes = null,
            bool asTracking = false);

        Task<TResult?> GetSingleAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            List<Expression<Func<T, object>>>? includes = null,
            bool asTracking = false);

        Task<List<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<Expression<Func<T, object>>>? includes = null,
            int? skip = null,
            int? take = null,
            bool asTracking = false,
            bool distinct = false);

        Task<List<TResult>> GetListAsync<TResult>(
            Expression<Func<T, bool>>? predicate,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            List<Expression<Func<T, object>>>? includes = null,
            int? skip = null,
            int? take = null,
            bool asTracking = false,
            bool distinct = false);

        Task<List<T>> GetListNoCacheAsync(
            Expression<Func<T, bool>>? predicate = null,
            List<Expression<Func<T, object>>>? includes = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null);

        Task RefreshCacheAsync();

        Task<TResult?> GetMaxAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            Expression<Func<T, bool>>? predicate = null);
    }
}
