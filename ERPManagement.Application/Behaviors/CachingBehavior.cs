using ERPManagement.Application.Caching;
using ERPManagement.Application.Configuration;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace ERPManagement.Application.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;

        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableMediatrQuery cacheableQuery)
            {
                TResponse response;
                string cacheKey = cacheableQuery.CacheKey;
                if (string.IsNullOrWhiteSpace(cacheKey))
                    cacheKey = CacheKeyGenerator.GenerateCacheKey(request);

                if (cacheableQuery.BypassCache)
                    return await next().ConfigureAwait(false);

                async Task<TResponse> GetResponseAndAddToCache()
                {
                    response = await next().ConfigureAwait(false);

                    var slidingExpiration = cacheableQuery.SlidingExpiration ?? TimeSpan.FromHours(_settings.SlidingExpiration);
                    var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };

                    var serializedData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

                    await _cache.SetAsync(cacheKey, serializedData, options, cancellationToken);
                    return response;
                }

                try
                {
                    var cachedResponse = await _cache.GetAsync(cacheKey, cancellationToken);
                    if (cachedResponse != null)
                    {
                        response = JsonConvert.DeserializeObject<TResponse>(Encoding.UTF8.GetString(cachedResponse));
                        if (response != null)
                        {
                            _logger.LogInformation($"Fetched from Cache -> '{cacheKey}'.");
                            return response;
                        }
                        else
                        {
                            _logger.LogWarning($"Deserialization returned null for CacheKey -> '{cacheKey}'. Fetching fresh data...");
                        }
                    }

                    response = await GetResponseAndAddToCache();
                    _logger.LogInformation($"Added to Cache -> '{cacheKey}'.");

                    return response;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during cache processing. Proceeding without cache.");
                    return await next().ConfigureAwait(false);
                }
            }
            else
            {
                return await next().ConfigureAwait(false);
            }
        }
    }
}
