using ERPManagement.Application.Features.Admin.Users.Commands.Models.UpdateUser;
using FluentValidation;

namespace ERPManagement.Application.Features.Admin.Users.Commands.Validation
{
	public class UsersUpdateValidation : AbstractValidator<UpdateUserCommand>
	{
		public UsersUpdateValidation()
		{
			RuleFor(o => o.UserName).NotEmpty();

		}
	}
}
