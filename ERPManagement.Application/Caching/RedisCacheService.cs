using ERPManagement.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ERPManagement.Application.Caching
{
	public class RedisCacheService : ICacheService
	{
		private readonly IDistributedCache _distributedCache;
		private readonly ILogger<RedisCacheService> _logger;

		public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
		{
			_distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		// Synchronous methods
		public bool TryGet<T>(string cacheKey, out T value)
		{
			var cachedData = _distributedCache.GetString(cacheKey);
			if (string.IsNullOrEmpty(cachedData))
			{
				value = default;
				return false;
			}

			value = JsonConvert.DeserializeObject<T>(cachedData);
			_logger.LogInformation($"[RedisCache] Synchronously fetched key: {cacheKey}");
			return true;
		}

		public T Set<T>(string cacheKey, T value)
		{
			var serializedData = JsonConvert.SerializeObject(value);
			_distributedCache.SetString(cacheKey, serializedData);
			_logger.LogInformation($"[RedisCache] Synchronously set key: {cacheKey}");
			return value;
		}

		public void Remove(string cacheKey)
		{
			_distributedCache.Remove(cacheKey);
			_logger.LogInformation($"[RedisCache] Synchronously removed key: {cacheKey}");
		}

		// Asynchronous methods
		public async Task<(bool found, T value)> TryGetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
		{
			var cachedData = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
			if (string.IsNullOrEmpty(cachedData))
			{
				return (false, default);
			}

			var value = JsonConvert.DeserializeObject<T>(cachedData);
			_logger.LogInformation($"[RedisCache] Asynchronously fetched key: {cacheKey}");
			return (true, value);
		}

		public async Task<T> SetAsync<T>(string cacheKey, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
		{
			var serializedData = JsonConvert.SerializeObject(value);
			var options = new DistributedCacheEntryOptions
			{
				SlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(30)
			};

			await _distributedCache.SetStringAsync(cacheKey, serializedData, options, cancellationToken);
			_logger.LogInformation($"[RedisCache] Asynchronously set key: {cacheKey}");
			return value;
		}

		public async Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
		{
			await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
			_logger.LogInformation($"[RedisCache] Asynchronously removed key: {cacheKey}");
		}
	}
}
