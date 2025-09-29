namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Models
{
	public class RolesBusinessObjectsVmByRoleQueryResult
	{
		public int Id { get; set; }
		public required string NameAr { get; set; }
		public List<RolesBusinessObjectsVmQueryResult> Items { get; set; }

	}
}
