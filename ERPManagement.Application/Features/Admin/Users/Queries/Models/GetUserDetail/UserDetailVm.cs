using ERPManagement.Application.Features.Admin.UserRoles.Query.Models;

namespace ERPManagement.Application.Features.Admin.Users.Queries.Models.GetUserDetail
{
	public class UserDetailVm
	{
		public UserDetailVm()
		{
			UserForms = new HashSet<RolesBusinessObjectsVmQueryResult>();
		}
		public int ID { get; set; }
		public string UserName { get; set; }
		public string? NameAr { get; set; }
		public string? NameEn { get; set; }
		public string Email { get; set; }
		public string? Password { get; set; }
		public List<int> RoleId { get; set; }
		public virtual ICollection<RolesBusinessObjectsVmQueryResult> UserForms { get; set; }


	}
}
