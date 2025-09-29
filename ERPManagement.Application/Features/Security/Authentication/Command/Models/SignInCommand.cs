using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Responses;
using ERPManagement.Application.Security.Authentication.Models;

namespace ERPManagement.Application.Features.Security.Authentication.Command.Models
{
	public class SignInCommand : ICommand<BaseResponse<JwtAuthResult>>
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string? Email { get; set; }
		public string? ReturnUrl { get; set; }
	}
}
