using ERPManagement.Application.Features.Admin.UserRoles.Query.Models.GetRoleDetail;
using ERPManagement.Application.Features.Admin.Users.Commands.Models;
using ERPManagement.Application.Features.Admin.Users.Commands.Models.CreateUser;
using ERPManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ERPManagement.Persistence.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager, IMediator mediator)
        {
            var adminUser = await userManager.FindByNameAsync("Admin");
            if (adminUser == null)
            {
                var roleResult = await mediator.Send(new GetRoleDetailQuery { RoleName = "Admin" });
                if (roleResult?.Data == null)
                    throw new InvalidOperationException("Admin role not found during seeding.");

                var permissions = roleResult.Data.RolesBusinessObjects
                    .Select(obj => new BusinessObjectsVmQueryResult
                    {
                        AddData = true,
                        UpdateData = true,
                        DeleteData = true,
                        BusinessObjectId = obj.BusinessObjectId,
                        NameAr = obj.NameAr,
                        NameEn = obj.NameEn
                    })
                    .ToList();

                var createUserCommand = new CreateUserCommand
                {
                    UserName = "Admin",
                    IsAdmin = true,
                    Email = "admin@rakez.local",
                    NameAr = "مدير النظام",
                    NameEn = "System Admin",
                    Password = "Admin@123",
                    UserForms = permissions,
                    RolesId = new List<int> { roleResult.Data.Id }
                };

                await mediator.Send(createUserCommand);
            }

            var newUser = await userManager.FindByNameAsync("User");
            if (newUser == null)
            {
                var roleResult = await mediator.Send(new GetRoleDetailQuery { RoleName = "User" });
                if (roleResult?.Data == null)
                    throw new InvalidOperationException("User role not found during seeding.");

                var permissions = roleResult.Data.RolesBusinessObjects
                    .Select(obj => new BusinessObjectsVmQueryResult
                    {
                        AddData = true,
                        UpdateData = true,
                        DeleteData = true,
                        BusinessObjectId = obj.BusinessObjectId,
                        NameAr = obj.NameAr,
                        NameEn = obj.NameEn
                    })
                    .ToList();

                var createUserCommand = new CreateUserCommand
                {
                    UserName = "User",
                    Email = "user@rakez.local",
                    NameAr = "مستخدم",
                    NameEn = "System User",
                    Password = "User@123",
                    UserForms = permissions,
                    RolesId = new List<int> { roleResult.Data.Id }
                };

                await mediator.Send(createUserCommand);
            }
        }

    }


}
