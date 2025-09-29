namespace ERPManagement.Application.Caching
{
	public class CacheConfiguration
	{
		public int AbsoluteExpirationInHours { get; set; }
		public int SlidingExpirationInMinutes { get; set; }
	}
}
