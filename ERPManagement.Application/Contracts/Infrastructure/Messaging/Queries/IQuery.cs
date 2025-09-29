using MediatR;

namespace ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries
{
	public interface IQuery<out TResponse> : IRequest<TResponse>
	{
		//public int skip { get; set; }
		//public int totalCount { get; set; }
		//public int summary { get; set; }
	}
}