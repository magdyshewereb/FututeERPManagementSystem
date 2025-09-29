namespace ERPManagement.Application.Security.Authentication.Models
{
	public class RefreshToken
	{
		public string UserName { get; set; }
		public string Token { get; set; }
		public DateTime ExpiresOn { get; set; }
	}
}
