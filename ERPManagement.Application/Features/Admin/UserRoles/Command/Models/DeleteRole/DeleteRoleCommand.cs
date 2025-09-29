using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Features.Admin.UserRoles.Command.Models.DeleteRole
{
	public class DeleteRoleCommand : ICommand<IMessage>
	{
		public int Id { get; set; }
	}
}
