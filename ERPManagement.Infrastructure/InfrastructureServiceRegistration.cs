using ERPManagement.Application.Configuration;
using ERPManagement.Application.Contracts.Infrastructure;
using ERPManagement.Application.Contracts.Infrastructure.Services;
using ERPManagement.Application.Contracts.Persistence.Connection;
using ERPManagement.Infrastructure.DataAccess;
using ERPManagement.Infrastructure.FileExport;
using ERPManagement.Infrastructure.Mail;
using ERPManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERPManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICsvExporter, CsvExporter>();
            //services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddTransient<IApplicationReadDbConnection, ApplicationReadDbConnection>();
            services.AddScoped<INotificationService, SweetAlertNotificationService>();
            services.AddScoped(typeof(IValidationService<>), typeof(FluentValidationService<>));
            //services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));


            return services;
        }
    }
}
