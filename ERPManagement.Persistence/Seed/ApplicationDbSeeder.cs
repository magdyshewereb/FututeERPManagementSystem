using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using ERPManagement.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationDbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();

        try
        {
            //await RoleSeeder.SeedAsync(roleManager, mediator, authorizationService);
            //await UserSeeder.SeedAsync(userManager, mediator);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeding Error: {ex.Message}");
            throw;
        }

    }
}
