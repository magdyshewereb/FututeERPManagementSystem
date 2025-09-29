using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Responses;
using ERPManagement.Application.Security.Authentication.Models;

namespace ERPManagement.Application.Features.Security.Authentication.Command.Models
{
	public class RefreshTokenCommand : ICommand<BaseResponse<JwtAuthResult>>
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}

}
