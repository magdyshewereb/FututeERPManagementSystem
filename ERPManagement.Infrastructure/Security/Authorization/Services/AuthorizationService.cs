using ERPManagement.Application.Contracts.Persistence;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.UpdateRole;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using Hr.Application.Interfaces.GenericRepository.Command;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Linq.Expressions;
using System.Transactions;

namespace ERPManagement.Infrastructure.Security.Authorization.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IBaseRepository<RoleBusinessObject> _repository;
        private readonly IQueryGenericRepository<BusinessObject> _repoquery;
        private readonly IQueryGenericRepository<RoleBusinessObject> _repobusinessquery;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfwork _unitOfwork;
        private readonly ILogger<AuthorizationService> _logger;

        public AuthorizationService(
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            IBaseRepository<RoleBusinessObject> repository,
            IQueryGenericRepository<BusinessObject> repoquery,
            IQueryGenericRepository<RoleBusinessObject> repobusinessquery,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfwork unitOfwork,
            ILogger<AuthorizationService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _repository = repository;
            _repoquery = repoquery;
            _repobusinessquery = repobusinessquery;
            _httpContextAccessor = httpContextAccessor;
            _unitOfwork = unitOfwork;
            _logger = logger;
        }

        public async Task<string> AddRoleAsync(string roleName, List<RoleBusinessObject> rolesBusinessObjects)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var identityRole = new Role { Name = roleName };
                var result = await _roleManager.CreateAsync(identityRole);

                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                foreach (var item in rolesBusinessObjects)
                    item.RoleId = identityRole.Id;

                _repository.AddRange(rolesBusinessObjects);
                await _unitOfwork.SaveAsync();

                scope.Complete();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return "Failed to create role or assign permissions.";
            }
        }

        public async Task<string> UpdateRoleAsync(UpdateRoleCommand roles, List<RoleBusinessObject> rolesBusinessObjects)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var identityRole = await _roleManager.FindByIdAsync(roles.Id.ToString());
                if (identityRole == null) return "Role not found";

                identityRole.Name = roles.RoleName;
                var result = await _roleManager.UpdateAsync(identityRole);

                if (!result.Succeeded)
                    return string.Join(", ", result.Errors.Select(e => e.Description));

                foreach (var item in rolesBusinessObjects)
                    item.RoleId = identityRole.Id;

                await _repository.AddRangeAsync(rolesBusinessObjects.Where(i => i.Id <= 0));
                await _unitOfwork.SaveAsync();

                scope.Complete();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role");
                return "Failed to update role or assign permissions.";
            }
        }

        public async Task<string> DeletePermissionAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                await _unitOfwork.SaveAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete permission for ID {PermissionId}", id);
                return "Delete permission failed.";
            }
        }

        public async Task<List<Role>> GetRolesList()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleData(int? id, string? roleName = null, string rolesId = null)
        {
            Role? result = null;

            if (string.IsNullOrEmpty(rolesId))
            {
                result = string.IsNullOrEmpty(roleName)
                    ? await _roleManager.Roles
                        .Include(r => r.RolesBusinessObjects)
                        .ThenInclude(rb => rb.BusinessObject)
                        .FirstOrDefaultAsync(r => r.Id == id)
                    : await _roleManager.Roles
                        .Include(r => r.RolesBusinessObjects)
                        .ThenInclude(rb => rb.BusinessObject)
                        .FirstOrDefaultAsync(r => r.Name == roleName);
            }
            else
            {
                var rolesList = rolesId.Split(',').Select(s => s.Trim()).ToList();
                result = await _roleManager.Roles
                    .Include(r => r.RolesBusinessObjects)
                    .ThenInclude(rb => rb.BusinessObject)
                    .FirstOrDefaultAsync(r => rolesList.Contains(r.Id.ToString()));
            }

            return result ?? new Role { RolesBusinessObjects = new List<RoleBusinessObject>() };
        }

        public async Task<List<BusinessObject>> GetPermmisionByid(int roleId)
        {
            var currentPermissions = await _repobusinessquery.GetListAsync(
                predicate: i => i.RoleId == roleId,
                selector: i => i.BusinessObject,
                includes: new List<Expression<Func<RoleBusinessObject, object>>> { i => i.BusinessObject });

            var allPermissions = await _repoquery.GetListAsync(
                predicate: i => i.BusinessObjectID == null,
                includes: new List<Expression<Func<BusinessObject, object>>>
                { i => i.ChildObject });

            return allPermissions
                .Where(p => !currentPermissions.Contains(p) &&
                            !p.ChildObject.Intersect(currentPermissions).Any())
                .ToList();
        }

        public async Task<List<BusinessObject>> GetAllPermmision()
        {
            return await _repoquery.GetListAsync(
                predicate: i => i.BusinessObjectID != null);
        }
    }
}
