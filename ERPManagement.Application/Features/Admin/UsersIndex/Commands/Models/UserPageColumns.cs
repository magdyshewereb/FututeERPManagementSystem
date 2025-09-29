using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Features.Admin.UsersIndex.Commands.Models
{
	public class UserPageColumns : ICommand<IMessage>
	{
		public int PageId { get; set; }
		public string? PageUrl { get; set; }
		public string ColumnsName { get; set; }

	}
}
