using MediatR;

namespace ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries
{
	public interface IQuerytHandler<in TQuery, TResponse>
		: IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>

	{
	}
}