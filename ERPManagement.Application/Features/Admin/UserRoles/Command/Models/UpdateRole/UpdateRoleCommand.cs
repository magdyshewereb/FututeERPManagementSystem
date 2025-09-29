using ERPManagement.Application.Features.Admin.UserRoles.Command.Models;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.UserRoles.Command.Models.UpdateRole
{
	public class UpdateRoleCommand : BaseViewModel
	{
		public int Id { get; set; }

		public required string RoleName { get; set; }

		// منع NullReferenceException عند الوصول للقائمة
		public List<RolesBusinessObjectsVm> RolesBusinessObjects { get; set; } = new();
	}
}
