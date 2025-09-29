using ERPManagement.Application.Configuration;
using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Contracts.Persistence;
using ERPManagement.Application.Contracts.Persistence.Connection;
using ERPManagement.Application.Security.Authentication.Services;
using ERPManagement.Domain.Entities;
using ERPManagement.Infrastructure.Mail;
using ERPManagement.Infrastructure.Security.Authentication.Services;
using ERPManagement.Infrastructure.Security.Authorization.Services;
using ERPManagement.Persistence.Repositories;
using Hr.Application.Interfaces.GenericRepository.Command;
using Infrastructure.Hr.Persistence.Contexts.GenericRepository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RakezIntelliERP.Account.Application.Contracts.Persistence.GenericRepository.Queries;


namespace ERPManagement.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Configuration
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ERPConnectionString"));
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // Repository
            services.AddTransient<IUnitOfwork, UnitOfWork>();
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient(typeof(IQueryGenericRepository<>), typeof(QueryGenericRepository<>));

            // Email Configuration

            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.AddTransient<IEmailService, EmailService>();

            // Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            // Identity Configuration 
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            return services;
        }
    }
}
