using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.Users.Queries.Models
{
	public class RequestsNotifecationsQuery : IQuery<BaseResponse<List<RequestsNotifecationsList>>>
	{
		public int? UserId { get; set; }
	}
}
