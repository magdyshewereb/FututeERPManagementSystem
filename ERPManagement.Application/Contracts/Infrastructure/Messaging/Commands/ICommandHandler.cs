using MediatR;

namespace ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands
{
	public interface ICommandHandler<in TCommand, TResponse>
		: IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>

	{
	}
}