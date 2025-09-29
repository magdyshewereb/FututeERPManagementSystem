using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.UpdateRole;
using ERPManagement.Domain.Entities;

namespace ERPManagement.Application.Security.Authentication.Services
{
    public interface IAuthorizationService
    {

        public Task<List<Role>> GetRolesList();
        public Task<Role> GetRoleData(int? id, string? rolename, string? rolesid);
        public Task<string> AddRoleAsync(string roleName, List<RoleBusinessObject> rolesBusinessObjects);
        public Task<string> UpdateRoleAsync(UpdateRoleCommand roleName, List<RoleBusinessObject> rolesBusinessObjects);
        Task<List<BusinessObject>> GetPermmisionByid(int id);
        Task<string> DeletePermissionAsync(int id);
        Task<List<BusinessObject>> GetAllPermmision();
    }
}
