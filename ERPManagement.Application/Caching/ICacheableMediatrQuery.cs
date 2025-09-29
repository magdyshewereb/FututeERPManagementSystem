﻿namespace ERPManagement.Application.Caching
{
	public interface ICacheableMediatrQuery
	{
		bool BypassCache { get; }
		string CacheKey { get; }
		TimeSpan? SlidingExpiration { get; }
	}
}