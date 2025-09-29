using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Features.Admin.Users.Commands.Models.DeleteUser
{
	public class DeleteUserCommand : ICommand<IMessage>
	{
		public int Id { get; set; }

		//public static implicit operator Type(DeleteUser v)
		//{
		//    throw new NotImplementedException();
		//}
	}
}
