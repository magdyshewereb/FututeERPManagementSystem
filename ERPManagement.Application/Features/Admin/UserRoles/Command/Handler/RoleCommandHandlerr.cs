using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Commands;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.CreateRole;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.DeleteRole;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.UpdateRole;
using ERPManagement.Application.Responses;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Application.Shared.Services;
using ERPManagement.Domain.Entities;

namespace ERPManagement.Application.Features.Admin.UserRoles.Command.Handler
{
    public class RoleCommandHandlerr : BaseResponseHandler,
        ICommandHandler<CreateRoleCommand, IMessage>,
        ICommandHandler<UpdateRoleCommand, IMessage>,
        ICommandHandler<DeletePermissionCommand, IMessage>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IMessageLocalizationService _messageLocalization;

        public RoleCommandHandlerr(IAuthorizationService authorizationService, IMapper mapper, IMessageLocalizationService messageLocalization)
        {
            _authorizationService = authorizationService;
            _messageLocalization = messageLocalization;
            _mapper = mapper;
        }

        public async Task<IMessage> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roleBusinessObjects = _mapper.Map<List<RoleBusinessObject>>(request.RolesBusinessObjects);
                var result = await _authorizationService.AddRoleAsync(request.RoleName, roleBusinessObjects);

                return result == "Success"
                    ? Success("")
                    : BadRequest<string>(result);
            }
            catch (Exception)
            {
                return BadRequest<string>("Failed to create role.");
            }
        }

        public async Task<IMessage> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roleBusinessObjects = _mapper.Map<List<RoleBusinessObject>>(request.RolesBusinessObjects);
                var result = await _authorizationService.UpdateRoleAsync(request, roleBusinessObjects);

                return result == "Success"
                    ? Success("")
                    : BadRequest<string>(result);
            }
            catch (Exception)
            {
                return BadRequest<string>("Failed to update role.");
            }
        }

        public async Task<IMessage> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.DeletePermissionAsync(request.Id);

            return result == "Success"
                ? Success("")
                : BadRequest<string>(result);
        }


    }
}

