using ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.UpdateUser;

namespace ERPManagement.Application.Security.Authentication.Services
{
	public interface IApplicationUserService
	{
		public Task<CreateUserCommand> AddUserAsync(CreateUserCommand user, List<int> roles);
		public Task<string> UpdateUserAsync(UpdateUserCommand user, List<int> roles);
	}
}
