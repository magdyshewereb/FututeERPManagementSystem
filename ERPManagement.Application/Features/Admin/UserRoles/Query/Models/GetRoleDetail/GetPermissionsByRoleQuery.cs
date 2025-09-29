using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRoleDetail
{
	// استعلام: الصلاحيات المرتبطة بدور معين
	public class GetPermissionsByRoleQuery : IQuery<BaseResponse<List<RolesBusinessObjectsVmByRoleQueryResult>>?>
	{
		public int Id { get; set; }
	}
}
