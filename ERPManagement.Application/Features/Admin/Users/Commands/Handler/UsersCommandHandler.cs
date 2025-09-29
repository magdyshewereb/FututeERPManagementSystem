using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Contracts.Persistence;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.DeleteUser;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.UpdateUser;
using ERPManagement.Application.Features.Common.Base.Commands;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Common;
using ERPManagement.Domain.Entities;
using Hr.Application.Interfaces.GenericRepository.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Transactions;

namespace ERPManagement.Application.Features.Admin.Users.Commands.Handler
{
    public class UsersCommandHandler : BaseCommand<User, AuditableEntity>,
        ICommandHandler<CreateUserCommand, IMessage>,
        ICommandHandler<UpdateUserCommand, IMessage>,
        ICommandHandler<DeleteUserCommand, IMessage>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserForm> _formRepository;
        private readonly IQueryGenericRepository<UserForm> _formQuery;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IUnitOfwork _unitOfWork;

        public UsersCommandHandler(
            IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IApplicationUserService applicationUserService,
            IBaseRepository<User> userRepository,
            IBaseRepository<UserForm> formRepository,
            IQueryGenericRepository<UserForm> formQuery,
            IUnitOfwork unitOfWork)
            : base(userRepository, mapper, unitOfWork)
        {
            _mapper = mapper;
            _userManager = userManager;
            _applicationUserService = applicationUserService;
            _userRepository = userRepository;
            _formRepository = formRepository;
            _formQuery = formQuery;
            _unitOfWork = unitOfWork;
        }

        public async Task<IMessage> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var identityUser = _mapper.Map<User>(request);

            var result = await _applicationUserService.AddUserAsync(request, request.RolesId);
            string message = result.Message;
            return message switch
            {
                "EmailIsExist" => BadRequest<string>("Email already exists"),
                "UserNameIsExist" => BadRequest<string>("Username already exists"),
                "ErrorInCreateUser" => BadRequest<string>("An error occurred while creating user"),
                "Failed" => BadRequest<string>("Creation failed"),
                "Success" => Success(identityUser.Id.ToString(), identityUser.Id),
                _ => BadRequest<string>(message),
            };
        }

        public async Task<IMessage> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var existingUser = await _userManager.FindByIdAsync(request.PageId.ToString());
            if (existingUser == null)
                return NotFound<string>();

            _mapper.Map(request, existingUser);

            var duplicateUserName = await _userManager.Users
                .AnyAsync(x => x.UserName == existingUser.UserName && x.Id != existingUser.Id);
            if (duplicateUserName)
                return BadRequest<string>("Username already exists");

            var existingForms = await _formQuery.GetListAsync(i => i.UserId == request.ParentID);
            await _formRepository.DeleteRangeAsync(existingForms);

            var result = await _applicationUserService.UpdateUserAsync(request, request.RolesId);

            switch (result)
            {
                case "EmailIsExist": return BadRequest<string>("Email already exists");
                case "UserNameIsExist": return BadRequest<string>("Username already exists");
                case "ErrorInCreateUser": return BadRequest<string>("An error occurred while updating user");
                case "Failed": return BadRequest<string>("Update failed");
                case "Success":
                    scope.Complete();
                    return Success("Updated successfully");
                default:
                    return BadRequest<string>(result);
            }
        }

        public async Task<IMessage> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _ = await DeleteHandle(request.Id, cancellationToken);
            return Success("Deleted successfully");
        }
    }
}
