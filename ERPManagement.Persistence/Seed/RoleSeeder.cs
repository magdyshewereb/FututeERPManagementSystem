using ERPManagement.Application.Features.Admin.UserRoles.Command.Models;
using ERPManagement.Application.Features.Admin.UserRoles.Command.Models.CreateRole;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;


namespace ERPManagement.Persistence.Seed
{

    public static class RoleSeeder
    {

        public static async Task SeedAsync(RoleManager<Role> roleManager, IMediator mediator, IAuthorizationService authorizationService)
        {
            string[] roles = ["Admin", "User", "HR"];
            foreach (var roleName in roles)
            {
                // التأكد من عدم وجود دور Admin مسبقًا
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // الحصول على كل Business Objects
                    var businessObjectList = await authorizationService.GetAllPermmision();

                    // إنشاء قائمة الصلاحيات (BusinessObjectId فقط)
                    var roleBusinessObjects = businessObjectList
                        .Select(obj => new RolesBusinessObjectsVm
                        {
                            BusinessObjectId = obj.Id
                        })
                        .ToList();

                    var command = new CreateRoleCommand
                    {
                        RoleName = roleName,
                        RolesBusinessObjects = roleBusinessObjects
                    };

                    await mediator.Send(command);
                }

            }
        }

    }

}

