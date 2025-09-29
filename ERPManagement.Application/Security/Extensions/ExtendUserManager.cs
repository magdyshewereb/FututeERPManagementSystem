using ERPManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERPManagement.Application.Security.Extensions
{
    public static class ExtendUserManager
    {
        public static async Task<UserForm?> GetForms(this UserManager<User> userManager, int userId, string formName)
        {
            var user = await userManager.Users
                .Include(i => i.UserForms)
                .ThenInclude(i => i.BusinessObjects)
                .FirstOrDefaultAsync(i => i.Id == userId);

            if (user == null || user.UserForms == null)
                return null;

            var forms = user.UserForms.FirstOrDefault(i => i.BusinessObjects?.Name == formName);

            return forms ?? null;
        }

    }
}
