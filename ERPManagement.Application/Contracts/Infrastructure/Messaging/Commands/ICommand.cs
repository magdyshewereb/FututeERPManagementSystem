using MediatR;

namespace ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands
{
	public interface ICommand<out TResponse> : IRequest<TResponse>
	{
	}
}