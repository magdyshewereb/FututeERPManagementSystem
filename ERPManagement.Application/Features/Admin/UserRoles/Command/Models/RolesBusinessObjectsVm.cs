using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Features.Admin.UserRoles.Command.Models;

public class RolesBusinessObjectsVm : ICommand<IMessage>
{
	public int Id { get; set; }

	// هذا الحقل غالبًا يتم توليده داخل النظام وليس من المستخدم، لذا يمكن تجاهله أثناء الإرسال من العميل
	public int RoleId { get; set; }

	public int BusinessObjectId { get; set; }
}
