using AutoMapper;
using ERPManagement.Application.Contracts.Persistence;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.UpdateUser;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Transactions;

namespace ERPManagement.Persistence.Repositories
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields

        private readonly UserManager<User> _userManager;
        private readonly IQueryGenericRepository<Role> _roleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfwork _unitOfwork;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        #endregion

        #region Constructor

        public ApplicationUserService(UserManager<User> userManager, IMapper mapper,
                                      IHttpContextAccessor httpContextAccessor,
                                      ApplicationDbContext dbContext,
                                      IQueryGenericRepository<Role> roleRepository,
                                      IAuthenticationService authenticationService,
                                      IUnitOfwork unitOfwork)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _roleRepository = roleRepository;
            _unitOfwork = unitOfwork;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        #endregion

        #region Methods

        public async Task<CreateUserCommand> AddUserAsync(CreateUserCommand userDto, List<int> roleIds)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (await _userManager.FindByEmailAsync(userDto.Email) is not null)
                {
                    userDto.Message = "EmailIsExist";//"Email already exists. Please use a different email address.";
                    return userDto;
                }
                if (await _userManager.FindByNameAsync(userDto.UserName) is not null)
                {
                    userDto.Message = "UserNameIsExist";//"Username already exists. Please choose a different username.";
                    return userDto;
                }


                var user = _mapper.Map<User>(userDto);

                var createResult = await _userManager.CreateAsync(user, userDto.Password);
                if (!createResult.Succeeded)
                {
                    userDto.Message = string.Join(",", createResult.Errors.Select(e => e.Description));
                    return userDto;
                }


                var roles = await _roleRepository.GetListAsync(r => roleIds.Contains(r.Id));
                foreach (var role in roles)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!roleResult.Succeeded)
                    {
                        userDto.Message = "RoleAssignmentFailed";
                        return userDto;
                    }

                }
                scope.Complete();
                await _authenticationService.CreateJwtToken(user);
                userDto.Message = "Success";
                return userDto;
            }
            catch (Exception ex)
            {
                // يمكنك تسجيل الخطأ هنا إن كنت تستخدم ILogger 
                userDto.Message = "Failed";
                return userDto;
            }
        }



        public async Task<string> UpdateUserAsync(UpdateUserCommand userDto, List<int> roleIds)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = _mapper.Map<User>(userDto);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    return string.Join(",", updateResult.Errors.Select(e => e.Description));

                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!removeResult.Succeeded)
                    return "RemoveRolesFailed";

                var roles = await _roleRepository.GetListAsync(r => roleIds.Contains(r.Id));
                foreach (var role in roles)
                {
                    var addResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (!addResult.Succeeded)
                        return "AddRolesFailed";
                }

                scope.Complete();
                return "Success";
            }
            catch (Exception ex)
            {
                return "Failed";
            }
        }


        #endregion
    }
}
