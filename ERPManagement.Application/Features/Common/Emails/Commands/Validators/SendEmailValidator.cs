using ERPManagement.Application.Features.Common.Emails.Commands.Models;
using FluentValidation;

namespace ERPManagement.Application.Features.Common.Emails.Commands.Validators
{
	public class SendEmailValidator : AbstractValidator<SendEmailCommand>
	{
		#region Fields

		#endregion
		#region Constructors
		public SendEmailValidator()
		{

			ApplyValidationsRules();
		}
		#endregion
		#region Actions
		public void ApplyValidationsRules()
		{
			RuleFor(x => x.MailTo)
				 .NotEmpty().WithMessage("")
				 .NotNull().WithMessage("");

			RuleFor(x => x.Body)
				 .NotEmpty().WithMessage("")
				 .NotNull().WithMessage("");
		}
		#endregion
	}
}
