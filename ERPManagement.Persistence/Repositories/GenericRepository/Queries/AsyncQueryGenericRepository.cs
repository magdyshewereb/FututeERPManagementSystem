using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Shared.Enums;
using ERPManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Linq.Expressions;

namespace Infrastructure.Hr.Persistence.Contexts.GenericRepository.Repository
{
    public sealed partial class QueryGenericRepository<T> : IQueryGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _entity;
        private readonly Func<CacheTech, ICacheService> _cacheService;
        private static readonly CacheTech cacheTech = CacheTech.Memory;
        private readonly string cacheKey = typeof(T).ToString();

        public QueryGenericRepository(ApplicationDbContext context, Func<CacheTech, ICacheService> cacheService)
        {
            _dbContext = context;
            _entity = _dbContext.Set<T>();
            _cacheService = cacheService;
        }

        public async Task<long> GetCountAsync(Expression<Func<T, bool>> query = null)
            => await _entity.CountAsync(query);

        private IQueryable<T> ApplyQueryOptions(IQueryable<T> queryable,
            Expression<Func<T, bool>>? filter,
            Func<T, object>? orderBy,
            OrderType? orderType,
            List<string>? includes,
            int? skip,
            int? take,
            bool? distinct,
            bool asTracking)
        {
            if (!asTracking)
                queryable = queryable.AsNoTrackingWithIdentityResolution();

            //if (includes != null)
            //    queryable = queryable.Include_Entity(_dbContext, includes.ToArray());

            if (filter != null)
                queryable = queryable.Where(filter);

            if (orderBy != null)
            {
                queryable = orderType == OrderType.DESC
                    ? queryable.OrderByDescending(orderBy).AsQueryable()
                    : queryable.OrderBy(orderBy).AsQueryable();
            }

            if (skip.HasValue)
                queryable = queryable.Skip(skip.Value);

            if (take.HasValue)
                queryable = queryable.Take(take.Value);

            if (distinct == true)
                queryable = queryable.Distinct();

            return queryable;
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? query = null,
                                                Func<T, object>? orderBy = null,
                                                OrderType? orderType = null,
                                                List<string>? includes = null,
                                                int? skip = 0,
                                                int? take = null,
                                                bool? distinct = null,
                                                bool asTracking = false)
        {
            var queryable = ApplyQueryOptions(_entity.AsQueryable(), query, orderBy, orderType, includes, skip, take, distinct, asTracking);
            return await queryable.ToListAsync();
        }

        public async Task<List<TResult>> GetListAsync<TResult>(Expression<Func<T, bool>>? query = null,
                                                               Func<T, TResult> selector = null,
                                                               Func<T, object> orderBy = null,
                                                               OrderType? orderType = null,
                                                               List<string> includes = null,
                                                               int? skip = null,
                                                               int? take = null,
                                                               bool? distinct = null,
                                                               bool asTracking = false)
        {
            var queryable = ApplyQueryOptions(_entity.AsQueryable(), query, orderBy, orderType, includes, skip, take, distinct, asTracking);
            return await Task.FromResult(queryable.Select(selector).ToList());
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>>? query,
                                            List<string>? includes = null,
                                            bool asTracking = false)
        {
            var queryable = _entity.AsQueryable();

            if (!asTracking)
                queryable = queryable.AsNoTrackingWithIdentityResolution();

            if (includes != null)
                includes.ForEach(include => queryable = queryable.Include(include));

            if (query != null)
                queryable = queryable.Where(query);

            var result = await queryable.FirstOrDefaultAsync();
            return result ?? Activator.CreateInstance<T>();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _entity.AnyAsync(predicate);

        public async Task RefreshCache()
        {
            _cacheService(cacheTech).Remove(cacheKey);
            var cachedList = await _entity.ToListAsync();
            _cacheService(cacheTech).Set(cacheKey, cachedList);
        }

        public async Task<T> FindAsync(long id) => await _entity.FindAsync(id);
        public async Task<T> FindAsync(int id) => await _entity.FindAsync(id);

        public async Task<IQueryable<T>> GetQueriableAsync() => _entity.AsQueryable();

        public async Task<TResult> GetMaxIdAsync<TResult>(Expression<Func<T, TResult>> selector,
                                                          Expression<Func<T, bool>> query = null)
        {
            var queryable = _entity.AsQueryable();
            if (query != null)
                queryable = queryable.Where(query);

            return await queryable.MaxAsync(selector);
        }

        // Additional helper methods such as SelectorFunc<T> and language/culture-sensitive selector handling can be included here with similar cleanup if needed.
    }
}
