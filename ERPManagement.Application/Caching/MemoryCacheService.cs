using ERPManagement.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ERPManagement.Application.Caching
{
	public class MemoryCacheService : ICacheService
	{
		private readonly IMemoryCache _memoryCache;
		private readonly CacheConfiguration _cacheConfig;
		private readonly ILogger<MemoryCacheService> _logger;
		private MemoryCacheEntryOptions _cacheOptions;

		public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfig, ILogger<MemoryCacheService> logger)
		{
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_cacheConfig = cacheConfig?.Value ?? throw new ArgumentNullException(nameof(cacheConfig));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_cacheOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours > 0 ? _cacheConfig.AbsoluteExpirationInHours : 1),
				Priority = CacheItemPriority.High,
				SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes > 0 ? _cacheConfig.SlidingExpirationInMinutes : 30)
			};
		}

		public bool TryGet<T>(string cacheKey, out T value)
		{
			_logger.LogInformation($"Trying to get cache key: {cacheKey}");

			if (_memoryCache.TryGetValue(cacheKey, out value))
			{
				_logger.LogInformation($"Cache hit for key: {cacheKey}");
				return true;
			}
			else
			{
				_logger.LogWarning($"Cache miss for key: {cacheKey}");
				return false;
			}
		}

		public T Set<T>(string cacheKey, T value)
		{
			_logger.LogInformation($"Setting cache for key: {cacheKey}");
			return _memoryCache.Set(cacheKey, value, _cacheOptions);
		}

		public void Remove(string cacheKey)
		{
			_logger.LogInformation($"Removing cache for key: {cacheKey}");
			_memoryCache.Remove(cacheKey);
		}

		public bool TryRemove(string cacheKey)
		{
			if (_memoryCache.TryGetValue(cacheKey, out _))
			{
				_logger.LogInformation($"Cache key {cacheKey} found and will be removed.");
				_memoryCache.Remove(cacheKey);
				return true;
			}
			else
			{
				_logger.LogWarning($"Cache key {cacheKey} not found for removal.");
				return false;
			}
		}

		// Asynchronous methods
		public Task<(bool found, T value)> TryGetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
		{
			_logger.LogInformation($"[MemoryCache] Try get async key: {cacheKey}");
			var found = _memoryCache.TryGetValue(cacheKey, out T value);
			return Task.FromResult((found, value));
		}

		public Task<T> SetAsync<T>(string cacheKey, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
		{
			_logger.LogInformation($"[MemoryCache] Set async key: {cacheKey}");

			var options = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_cacheConfig.AbsoluteExpirationInHours > 0 ? _cacheConfig.AbsoluteExpirationInHours : 1),
				Priority = CacheItemPriority.High,
				SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes > 0 ? _cacheConfig.SlidingExpirationInMinutes : 30)
			};

			return Task.FromResult(_memoryCache.Set(cacheKey, value, options));
		}

		public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
		{
			_logger.LogInformation($"[MemoryCache] Remove async key: {cacheKey}");
			_memoryCache.Remove(cacheKey);
			return Task.CompletedTask;
		}


	}
}
