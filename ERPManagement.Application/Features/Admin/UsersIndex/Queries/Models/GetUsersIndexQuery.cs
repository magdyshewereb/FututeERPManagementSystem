using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.UsersIndex.Queries.Models
{
	public class GetUsersIndexQuery : IQuery<BaseResponse<UsersIndexVm>>
	{

		public string? PageUrl { get; set; }

	}



}
