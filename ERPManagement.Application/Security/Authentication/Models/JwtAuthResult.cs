namespace ERPManagement.Application.Security.Authentication.Models
{
	public class JwtAuthResult
	{
		public string? Message { get; set; }
		public bool IsAuthenticated { get; set; }
		public string? Username { get; set; }
		public string? Email { get; set; }
		public List<string>? Roles { get; set; }
		public string? AccessToken { get; set; }
		public DateTime? ExpiresOn { get; set; }
		//[JsonIgnore]
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }

	}
}
