using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = ERPManagement.Application.Exceptions.ValidationException;

namespace ERPManagement.Application.Behaviors
{
	public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : ICommand<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			if (!_validators.Any())
				return await next();

			var context = new ValidationContext<TRequest>(request);

			var validationResults = _validators
				.Select(validator => validator.Validate(context))
				.SelectMany(result => result.Errors)
				.Where(failure => failure != null)
				.ToList();

			if (validationResults.Count != 0)
			{
				var errorDict = new Dictionary<string, string>();

				foreach (var error in validationResults)
				{
					if (!errorDict.ContainsKey(error.PropertyName))
					{
						errorDict.AddErrorMsg(error.ErrorMessage);
					}
				}

				throw new ValidationException(errorDict);
			}

			return await next();
		}
	}
}
