using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRolesList
{
	// استعلام: الحصول على قائمة الأدوار
	public class GetRolesListQuery : IQuery<BaseResponse<List<GetRolesListResultVm>?>>
	{
	}
}
