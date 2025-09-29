using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRolesList;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRoleDetail
{
	// استعلام: الحصول على بيانات دور معين
	public class GetRoleDetailQuery : IQuery<BaseResponse<GetRolesListResultVm>?>
	{
		public int RoleId { get; set; }
		// لدعم الاستعلامات التي تستخدم أكثر من Role دفعة واحدة
		public string? RolesId { get; set; }
		public string? RoleName { get; set; }
	}
}
