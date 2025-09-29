using AutoMapper;
using ERPManagement.Application.Contracts.Infrastructure.Messaging.Queries;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRoleDetail;
using ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRolesList;
using ERPManagement.Application.Responses;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ERPManagement.Application.Features.Admin.UserRoles.Query.Handler
{
    public class GetRoleQueryHandler : BaseResponseHandler,
        IQuerytHandler<GetRolesListQuery, BaseResponse<List<GetRolesListResultVm>>>,
        IQuerytHandler<GetRoleDetailQuery, BaseResponse<GetRolesListResultVm>>,
        IQuerytHandler<GetPermissionsByRoleQuery, BaseResponse<List<RolesBusinessObjectsVmByRoleQueryResult>>>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetRoleQueryHandler(
            IAuthorizationService authorizationService,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<List<GetRolesListResultVm>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesList();
            var result = _mapper.Map<List<GetRolesListResultVm>>(roles);
            return Success(result);
        }

        public async Task<BaseResponse<GetRolesListResultVm>> Handle(GetRoleDetailQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleData(request.RoleId, request.RoleName, request.RolesId);
            var result = _mapper.Map<GetRolesListResultVm>(role);
            return Success(result);
        }

        public async Task<BaseResponse<List<RolesBusinessObjectsVmByRoleQueryResult>>> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _authorizationService.GetPermmisionByid(request.Id);
            var result = _mapper.Map<List<RolesBusinessObjectsVmByRoleQueryResult>>(permissions);
            return Success(result);
        }
    }
}
