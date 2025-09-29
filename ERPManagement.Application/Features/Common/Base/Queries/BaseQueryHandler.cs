using AutoMapper;
using ERPManagement.Application.Contracts.Persistence.Connection;
using ERPManagement.Application.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Caching.Memory;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Linq.Expressions;

namespace ERPManagement.Application.Features.Common.Base.Queries
{
	public class BaseQueryHandler<T, TViewModel> : BaseResponseHandler where T : class
	{
		private readonly IQueryGenericRepository<T> _repo;
		private readonly IMapper _mapper;

		public BaseQueryHandler(IQueryGenericRepository<T> modelRepository, IMapper mapper)
		{
			_repo = modelRepository;
			_mapper = mapper;
		}

		public async Task<List<TViewModel>> GetListDataAsync(
			Expression<Func<T, bool>>? predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
			List<Expression<Func<T, object>>>? includes = null,
			int? skip = null,
			int? take = null,
			bool asTracking = false,
			bool distinct = false)
		{
			var result = await _repo.GetListAsync(predicate, orderBy, includes, skip, take, asTracking, distinct);
			return _mapper.Map<List<TViewModel>>(result);
		}

		public async Task<List<TViewModel>> GetListWithSelectorAsync<TResult>(
			Expression<Func<T, bool>>? predicate,
			Expression<Func<T, TResult>> selector,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
			List<Expression<Func<T, object>>>? includes = null,
			int? skip = null,
			int? take = null,
			bool asTracking = false,
			bool distinct = false)
		{
			var result = await _repo.GetListAsync(predicate, selector, orderBy, includes, skip, take, asTracking, distinct);
			return _mapper.Map<List<TViewModel>>(result);
		}

		public async Task<TViewModel?> GetDataDetailsAsync(
			Expression<Func<T, bool>> predicate,
			List<Expression<Func<T, object>>>? includes = null,
			bool asTracking = false)
		{
			var entity = await _repo.GetSingleAsync(predicate, includes, asTracking);
			return entity == null ? default : _mapper.Map<TViewModel>(entity);
		}

		public async Task<TViewModel?> GetDataDetailsWithSelectorAsync<TResult>(
			Expression<Func<T, bool>> predicate,
			Expression<Func<T, TResult>> selector,
			List<Expression<Func<T, object>>>? includes = null,
			bool asTracking = false)
		{
			var result = await _repo.GetSingleAsync(predicate, selector, includes, asTracking);
			return result == null ? default : _mapper.Map<TViewModel>(result);
		}

		public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
		{
			return await _repo.AnyAsync(predicate);
		}

		public async Task<long> CountAsync(Expression<Func<T, bool>>? predicate = null)
		{
			return await _repo.CountAsync(predicate);
		}
	}


	public class CachingBaseQueryHandler<T, TViewModel> : BaseResponseHandler where T : class
	{
		private readonly IQueryGenericRepository<T> _repo;
		private readonly IMapper _mapper;
		private readonly IMemoryCache _memoryCache;

		public CachingBaseQueryHandler(
			IQueryGenericRepository<T> modelRepository,
			IMapper mapper,
			IMemoryCache memoryCache)
		{
			_repo = modelRepository;
			_mapper = mapper;
			_memoryCache = memoryCache;
		}

		public async Task<List<TViewModel>> GetListWithCacheAsync(
			string cacheKey,
			Expression<Func<T, bool>>? predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
			List<Expression<Func<T, object>>>? includes = null,
			int? skip = null,
			int? take = null,
			bool asTracking = false,
			bool distinct = false,
			TimeSpan? expiration = null)
		{
			if (_memoryCache.TryGetValue(cacheKey, out List<TViewModel> cachedList))
			{
				return cachedList;
			}

			var result = await _repo.GetListAsync(predicate, orderBy, includes, skip, take, asTracking, distinct);
			var mapped = _mapper.Map<List<TViewModel>>(result);

			_memoryCache.Set(cacheKey, mapped, expiration ?? TimeSpan.FromMinutes(10));
			return mapped;
		}

		public async Task<TViewModel?> GetDetailsWithCacheAsync(
			string cacheKey,
			Expression<Func<T, bool>> predicate,
			List<Expression<Func<T, object>>>? includes = null,
			bool asTracking = false,
			TimeSpan? expiration = null)
		{
			if (_memoryCache.TryGetValue(cacheKey, out TViewModel cachedData))
			{
				return cachedData;
			}

			var result = await _repo.GetSingleAsync(predicate, includes, asTracking);
			var mapped = _mapper.Map<TViewModel>(result);

			_memoryCache.Set(cacheKey, mapped, expiration ?? TimeSpan.FromMinutes(10));
			return mapped;
		}

		public async Task<List<TViewModel>> GetListAsync(
			Expression<Func<T, bool>>? predicate = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
			List<Expression<Func<T, object>>>? includes = null,
			int? skip = null,
			int? take = null,
			bool asTracking = false,
			bool distinct = false)
		{
			var result = await _repo.GetListAsync(predicate, orderBy, includes, skip, take, asTracking, distinct);
			return _mapper.Map<List<TViewModel>>(result);
		}

		public async Task<TViewModel?> GetDetailsAsync(
			Expression<Func<T, bool>> predicate,
			List<Expression<Func<T, object>>>? includes = null,
			bool asTracking = false)
		{
			var result = await _repo.GetSingleAsync(predicate, includes, asTracking);
			return _mapper.Map<TViewModel>(result);
		}
	}


	public static class BaseQuery
	{
		public static IQueryable<TEntity> Include_Entity<TEntity>(this IQueryable<TEntity> query
		  , IApplicationDbContext context, params string[] includes) where TEntity : class
		{
			// Do a safety check first
			if (includes == null)
				return query;

			List<string> includeList = new List<string>();
			if (includes.Any())
				return includes
					.Where(x => !string.IsNullOrEmpty(x) && !includeList.Contains(x))
					.Aggregate(query, (current, include) => current.Include(include));

			IEnumerable<INavigation> navigationProperties = ((DbContext)context).Model.FindEntityType(typeof(TEntity)).GetNavigations();
			if (navigationProperties == null)
				return query;

			foreach (INavigation navigationProperty in navigationProperties)
			{
				if (!includes.Contains(navigationProperty.Name))
					continue;

				includeList.Add(navigationProperty.Name);
				query = query.Include(navigationProperty.Name);
			}

			return query;

		}
	}

}
