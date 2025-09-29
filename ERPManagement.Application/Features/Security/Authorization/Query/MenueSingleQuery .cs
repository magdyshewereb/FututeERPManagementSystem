using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Security.Authorization.Query
{
	public class MenueSingleQuery : IQuery<BaseResponse<MenueResult>>
	{
		public string UserName { get; set; }
		public string WebUrl { get; set; }
	}
}

