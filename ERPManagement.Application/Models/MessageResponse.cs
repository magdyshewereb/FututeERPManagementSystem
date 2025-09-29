using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;

namespace ERPManagement.Application.Models
{
	public record MessageResponse : IMessage
	{
		public int Id { get; set; }

		public MessageResponse()
		{
			Id = -1;
		}
	}
}
