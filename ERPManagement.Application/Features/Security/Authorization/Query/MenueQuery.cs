using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Security.Authorization.Query
{
	public class MenueQuery : IQuery<BaseResponse<List<MenueResult>>>
	{
		public string UserName { get; set; }
	}
}
