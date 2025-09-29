using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models;

namespace ERPManagement.Application.Features.Admin.Users.Commands.Models
{
	public class BusinessObjectsVmQueryResult : ICommand<IMessage>
	{
		public int BusinessObjectId { get; set; }
		public string NameAr { get; set; }
		public string NameEn { get; set; }
		public int UserId { get; set; }
		public bool? AddData { get; set; }
		public bool? UpdateData { get; set; }
		public bool? DeleteData { get; set; }
		public RolesBusinessObjectsVmQueryResult? businessObjectsVmQueryResult { get; set; }


	}
}
