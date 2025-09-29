using System.ComponentModel.DataAnnotations;

namespace ERPManagement.Application.Security.Authentication.Models
{
	public class TokenRequestModel
	{
		[EmailAddress]
		public string Email { get; set; }
		public string Password { get; set; }
	}
}