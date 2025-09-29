using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models;

namespace ERPManagement.Application.Features.Admin.UserRoles.Command.Models.CreateRole
{
	public class CreateRoleCommand : ICommand<IMessage>
	{
		public required string RoleName { get; set; }

		// ضمان عدم كونها null عند التهيئة
		public List<RolesBusinessObjectsVm> RolesBusinessObjects { get; set; } = new();
	}
}
