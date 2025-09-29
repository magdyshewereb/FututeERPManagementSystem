using ERPManagement.Application;
using ERPManagement.Application.Configuration;
using ERPManagement.Infrastructure;
using ERPManagement.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NToastNotify;
using System.Text;

namespace ERPManagement.UI
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            // Bind JwtSettings from appsettings.json
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection(nameof(JwtSettings))
            );

            // Also register JwtSettings as singleton for direct injection if needed
            var jwtSettings = new JwtSettings();
            var emailSettings = new EmailSettings();
            builder.Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
            builder.Configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
            builder.Services.AddSingleton(jwtSettings);
            builder.Services.AddSingleton(emailSettings);


            // JWT Authentication Configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuers = new[] { jwtSettings.Issuer },
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidAudience = jwtSettings.Audience,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime
                };
            });
            // Swagger Configuration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ERP Account API",
                    Description = "API documentation for ERP Account",
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            // Custom Services
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);


            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
                options.AddPolicy("AllowUI", policy =>
                    policy.WithOrigins([
                        builder.Configuration["UIUrl"] ?? "https://localhost:7223"
                    ])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true)
                )
            );
            builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        ProgressBar = true,
        PositionClass = ToastPositions.TopRight,
        PreventDuplicates = true,
        CloseButton = true
    });

            builder.Services.AddEndpointsApiExplorer();

            return builder.Build();
        }

        public static async Task<WebApplication> ConfigurePipeline(this WebApplication app)
        {
            app.UseCors("AllowUI");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            //app.UseCustomExceptionHandler();
            app.UseHttpsRedirection();
            app.UseNToastNotify();
            app.UseStaticFiles();

            // Authentication & Authorization Middlewares
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
