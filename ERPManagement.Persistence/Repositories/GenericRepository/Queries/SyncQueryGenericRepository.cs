using ERPManagement.Application.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Linq.Expressions;

namespace Infrastructure.Hr.Persistence.Contexts.GenericRepository.Repository
{
    public sealed partial class QueryGenericRepository<T> : IQueryGenericRepository<T>
    {
        private IQueryable<T> BuildQuery(Expression<Func<T, bool>>? filter = null,
                                         List<string>? stringIncludes = null,
                                         List<Expression<Func<T, object>>>? expressionIncludes = null,
                                         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                         int? skip = null,
                                         int? take = null,
                                         bool asTracking = false,
                                         bool distinct = false)
        {
            var query = _entity.AsQueryable();

            if (!asTracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (stringIncludes != null)
                query = stringIncludes.Aggregate(query, (current, include) => current.Include(include));

            if (expressionIncludes != null)
                query = expressionIncludes.Aggregate(query, (current, include) => current.Include(include));

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (distinct)
                query = query.Distinct();

            return query;
        }

        public long GetCount(Expression<Func<T, bool>>? query = null)
            => query == null ? _entity.LongCount() : _entity.Count(query);

        public List<TResult> GetList<TResult>(Expression<Func<T, bool>>? query,
                                              Func<T, TResult> selector,
                                              Expression<Func<T, object>>? orderBy = null,
                                              OrderType? orderType = null,
                                              List<string>? includes = null,
                                              int? skip = null,
                                              int? take = null,
                                              bool? distinct = null,
                                              bool asTracking = false)
        {
            var orderedQuery = orderBy != null ?
                (orderType == OrderType.DESC ?
                    (Func<IQueryable<T>, IOrderedQueryable<T>>)(q => q.OrderByDescending(orderBy)) :
                    (q => q.OrderBy(orderBy))) : null;

            return BuildQuery(query, includes, null, orderedQuery, skip, take, asTracking, distinct == true)
                   .Select(selector)
                   .ToList();
        }

        public List<T> GetList(Expression<Func<T, bool>>? query = null,
                               Expression<Func<T, object>>? orderBy = null,
                               OrderType? orderType = null,
                               List<string>? includes = null,
                               int? skip = null,
                               int? take = null,
                               bool? distinct = null,
                               bool asTracking = false)
            => BuildQuery(query, includes, null,
                          orderBy != null ? (orderType == OrderType.DESC ?
                              (Func<IQueryable<T>, IOrderedQueryable<T>>)
                              (q => q.OrderByDescending(orderBy)) :
                              (q => q.OrderBy(orderBy))) : null,
                          skip, take, asTracking, distinct == true).ToList();

        public List<T> GetList(Expression<Func<T, bool>>? query = null,
                              Expression<Func<T, object>>? orderBy = null,
                               OrderType? orderType = null,
                               Expression<Func<T, object>>? includes = null,
                               int? skip = null,
                               int? take = null,
                               bool? distinct = null,
                               bool asTracking = false)
        {
            var expressionIncludes = includes != null ? new List<Expression<Func<T, object>>> { includes } : null;

            return BuildQuery(query, null, expressionIncludes,
                              orderBy != null ? (orderType == OrderType.DESC ?
                                  (Func<IQueryable<T>, IOrderedQueryable<T>>)
                                  (q => q.OrderByDescending(orderBy)) :
                                  (q => q.OrderBy(orderBy))) : null,
                              skip, take, asTracking, distinct == true).ToList();
        }

        public List<TResult> GetAll<TResult>(Func<T, TResult> selector,
                                             Expression<Func<T, object>>? orderBy = null,
                                             OrderType? orderType = null,
                                             List<string>? includes = null,
                                             int? skip = null,
                                             int? take = null,
                                             bool? distinct = null,
                                             bool asTracking = false)
            => GetList(null, selector, orderBy, orderType, includes, skip, take, distinct, asTracking);

        public List<T> GetAll(Expression<Func<T, object>>? orderBy = null,
                              OrderType? orderType = null,
                              List<string>? includes = null,
                              int? skip = null,
                              int? take = null,
                              bool? distinct = null,
                              bool asTracking = false)
            => GetList(null, orderBy, orderType, includes, skip, take, distinct, asTracking);

        public TResult? GetSingle<TResult>(Expression<Func<T, bool>>? query,
                                           Func<T, TResult> selector,
                                           List<string>? includes = null,
                                           bool asTracking = false)
        {
            return BuildQuery(query, includes, null, null, null, null, asTracking)
                   .Select(selector)
                   .FirstOrDefault();
        }

        public T? GetSingle(Expression<Func<T, bool>>? query,
                            List<string>? includes = null,
                            bool asTracking = false)
            => BuildQuery(query, includes, null, null, null, null, asTracking).FirstOrDefault();

        public T? GetSingle(Expression<Func<T, bool>> query,
                            Expression<Func<T, object>>? include,
                            bool asTracking = false)
        {
            var expressionIncludes = include != null ? new List<Expression<Func<T, object>>> { include } : null;
            var result = BuildQuery(query, null, expressionIncludes, null, null, null, asTracking).FirstOrDefault();
            return result ?? Activator.CreateInstance<T>();
        }

        public IQueryable<T> GetQueriable() => _entity.AsQueryable();

        public T? Find(long id) => _entity.Find(id);

        public bool Any(Expression<Func<T, bool>> query) => _entity.Any(query);

        public bool IsNameExist(Expression<Func<T, bool>>? predicate)
            => _entity.Any(predicate);

        public Task<IQueryable<T>> GetQueryableAsync() => Task.FromResult(_entity.AsQueryable());

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate,
                                             List<Expression<Func<T, object>>>? includes = null,
                                             bool asTracking = false)
            => await BuildQuery(predicate, null, includes, null, null, null, asTracking).FirstOrDefaultAsync();

        public async Task<TResult?> GetSingleAsync<TResult>(Expression<Func<T, bool>> predicate,
                                                             Expression<Func<T, TResult>> selector,
                                                             List<Expression<Func<T, object>>>? includes = null,
                                                             bool asTracking = false)
            => await BuildQuery(predicate, null, includes, null, null, null, asTracking).Select(selector).FirstOrDefaultAsync();

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null,
                                                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                List<Expression<Func<T, object>>>? includes = null,
                                                int? skip = null,
                                                int? take = null,
                                                bool asTracking = false,
                                                bool distinct = false)
            => await BuildQuery(predicate, null, includes, orderBy, skip, take, asTracking, distinct).ToListAsync();

        public async Task<List<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>>? predicate,
                                                               Expression<Func<T, TResult>> selector,
                                                               Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                               List<Expression<Func<T, object>>>? includes = null,
                                                               int? skip = null,
                                                               int? take = null,
                                                               bool asTracking = false,
                                                               bool distinct = false)
            => await BuildQuery(predicate, null, includes, orderBy, skip, take, asTracking, distinct).Select(selector).ToListAsync();

        public async Task<List<T>> GetListNoCacheAsync(Expression<Func<T, bool>>? predicate = null,
                                                       List<Expression<Func<T, object>>>? includes = null)
            => await BuildQuery(predicate, null, includes, null, null, null, false).ToListAsync();

        public async Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null)
            => predicate == null ? await _entity.LongCountAsync() : await _entity.LongCountAsync(predicate);

        public Task RefreshCacheAsync() => Task.CompletedTask;

        public async Task<TResult?> GetMaxAsync<TResult>(Expression<Func<T, TResult>> selector,
                                                         Expression<Func<T, bool>>? predicate = null)
            => await BuildQuery(predicate).MaxAsync(selector);
    }
}
