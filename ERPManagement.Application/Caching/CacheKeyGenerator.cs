using System.Text;

namespace ERPManagement.Application.Caching
{
	public static class CacheKeyGenerator
	{
		public static string GenerateCacheKey<TRequest>(TRequest request)
		{
			var keyBuilder = new StringBuilder();
			keyBuilder.Append(typeof(TRequest).Name);

			var properties = typeof(TRequest).GetProperties();

			foreach (var property in properties)
			{
				var value = property.GetValue(request) ?? "null";
				keyBuilder.Append($"|{property.Name}:{value}");
			}

			return keyBuilder.ToString();
		}
	}
}
