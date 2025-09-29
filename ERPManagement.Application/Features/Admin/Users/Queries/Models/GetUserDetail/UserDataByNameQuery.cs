using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.Users.Queries.Models.GetUserDetail
{
	public class UserDataByNameQuery : IQuery<BaseResponse<UserDetailVm>>
	{
		public string? UserName { get; set; } = null!;

	}
}
