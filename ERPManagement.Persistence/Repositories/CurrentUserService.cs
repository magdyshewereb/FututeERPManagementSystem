using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Security.Authentication.Models;
using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;
using System.Linq.Expressions;

namespace ERPManagement.Persistence.Repositories
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IQueryGenericRepository<User> _usersQuery;

        #endregion

        #region Constructor

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            IQueryGenericRepository<User> usersQuery)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _usersQuery = usersQuery;
        }

        #endregion

        #region Public Methods

        public int GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.Claims
                ?.FirstOrDefault(c => c.Type == nameof(UserClaimModel.Id))?.Value;

            if (string.IsNullOrWhiteSpace(claim) || !int.TryParse(claim, out var userId))
                throw new UnauthorizedAccessException("User is not authenticated.");

            return userId;
        }

        public async Task<User> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user ?? throw new UnauthorizedAccessException("User not found.");
        }

        public async Task<User> GetUserAllDataAsync()
        {
            var userId = GetUserId();
            var user = await _usersQuery.GetSingleAsync(
                predicate: u => u.Id == userId,
                includes: new List<Expression<Func<User, object>>>
                {
                    // u => u.Employee
                }
            );

            return user ?? throw new UnauthorizedAccessException("User not found.");
        }


        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        //public async Task<UserForm?> GetCurrentUserFormRolesAsync(string formName)
        //{
        //    var user = await GetUserAsync();
        //    var forms = await _userManager.GetForms(user.Id, formName);
        //    return forms;
        //}

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
        }

        public string GetIpAddress()
        {
            return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
        }

        public string GetDeviceInfo()
        {
            return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString() ?? string.Empty;
        }

        public Task<UserForm?> GetCurrentUserFormRolesAsync(string formname)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
