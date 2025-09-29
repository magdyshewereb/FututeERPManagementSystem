using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.Users.Commands.Models;
using ERPManagement.Application.Responses;

namespace ERPManagement.Application.Features.Admin.Users.Queries.Models.GetUserDetail
{
	public class GetUserDetailQuery : IQuery<BaseResponse<UserDetailVm>>
	{
		public GetUserDetailQuery()
		{
			UserForms = new HashSet<BusinessObjectsVmQueryResult>();
		}
		public int? UserId { get; set; }
		public string UserName { get; set; } = null!;

		public string? NameAr { get; set; }
		public string? NameEn { get; set; }
		public string? Email { get; set; }
		public string roleName { get; set; }
		public virtual ICollection<BusinessObjectsVmQueryResult> UserForms { get; set; }

	}
}
