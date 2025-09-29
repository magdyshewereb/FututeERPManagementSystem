using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Security.Authentication.Queris.Models
{
	public class ForgetPasswordQuery : IQuery<BaseResponse<string>>
	{
		public string EmailAddress { get; set; }
	}
}
