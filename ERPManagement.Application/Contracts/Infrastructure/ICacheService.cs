namespace ERPManagement.Application.Contracts.Infrastructure
{
	public interface ICacheService
	{
		bool TryGet<T>(string cacheKey, out T value);
		T Set<T>(string cacheKey, T value);
		void Remove(string cacheKey);
		// Asynchronous methods
		Task<(bool found, T value)> TryGetAsync<T>(string cacheKey, CancellationToken cancellationToken = default);
		Task<T> SetAsync<T>(string cacheKey, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);
		Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default);
	}
}
