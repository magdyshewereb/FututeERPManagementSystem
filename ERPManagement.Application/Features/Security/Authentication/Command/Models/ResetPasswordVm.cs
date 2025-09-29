using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Responses;
using System.ComponentModel.DataAnnotations;

namespace ERPManagement.Application.Features.Security.Authentication.Command.Models
{
	public class ResetPasswordVm : ICommand<BaseResponse<string>>
	{
		public string? EmailAddress { get; set; }
		public string? Token { get; set; }


		[DataType(DataType.Password)]
		public string? Password { get; set; }
		[DataType(DataType.Password)]
		//[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string? ConfirmPassword { get; set; }
	}
}
