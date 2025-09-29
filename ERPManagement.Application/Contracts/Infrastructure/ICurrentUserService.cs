using ERPManagement.Domain.Entities;

namespace ERPManagement.Application.Contracts.Infrastructure
{
    public interface ICurrentUserService
    {
        public Task<User> GetUserAsync();
        public Task<List<string>> GetCurrentUserRolesAsync();
        Task<UserForm?> GetCurrentUserFormRolesAsync(string formname);
        Task<User> GetUserAllDataAsync();
        public int GetUserId();
        string GetIpAddress();      // جديد
        string GetDeviceInfo();     // جديد
        string GetUserName(); // جديد
    }
}
