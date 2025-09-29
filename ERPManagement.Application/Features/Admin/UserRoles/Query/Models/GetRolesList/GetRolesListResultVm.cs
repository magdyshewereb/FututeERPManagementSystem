using ERPManagement.Application.Features.Admin.UserRoles.Query.Models;

namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRolesList
{
	public class GetRolesListResultVm
	{
		public int Id { get; set; }
		public string RoleName { get; set; }
		public List<RolesBusinessObjectsVmQueryResult> RolesBusinessObjects { get; set; }
	}
}
