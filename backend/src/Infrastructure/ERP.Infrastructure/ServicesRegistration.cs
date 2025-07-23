using ERP.Application.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Data;
using ERP.Infrastructure.Entities;
using ERP.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ERP.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuration Serilog
            AddSerilogLogging(services, configuration);

            // Database Configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The ConnectionString property has not been initialized.");
            }

            // Configure Entity Framework Core avec PostgreSQL
            services.AddDbContext<ErpDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                });

                // Enable detailed errors in development
                var serviceProvider = services.BuildServiceProvider();
                var environment = serviceProvider.GetService<IWebHostEnvironment>();
                if (environment?.IsDevelopment() == true)
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // Configure Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password options
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // User options
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                // Sign in options
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Lockout options
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ErpDbContext>()
            .AddDefaultTokenProviders();

            // JWT Configuration
            var jwtSettings = configuration.GetSection("JWTSettings");
            services.Configure<JwtSettings>(jwtSettings);

            // Register repositories and services
            RegisterRepositories(services);
            RegisterServices(services);
        }

        /// <summary>
        /// Configure Serilog logging
        /// </summary>
        private static void AddSerilogLogging(IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            services.AddSerilog();
        }

        /// <summary>
        /// Register all repositories
        /// </summary>
        private static void RegisterRepositories(IServiceCollection services)
        {
            // Generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Specific repositories can be added here
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        /// <summary>
        /// Register all services
        /// </summary>
        private static void RegisterServices(IServiceCollection services)
        {
            // Identity services
            //services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            //services.AddScoped<TokenService>();

            // Other services can be added here
            // services.AddScoped<IEmailService, EmailService>();
        }

        /// <summary>
        /// Extension method to handle database migration and seeding on application startup.
        /// </summary>
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ErpDbContext>>();

            try
            {
                logger.LogInformation("Initializing database...");
                await DbInitializer.InitializeAsync(services);
                logger.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database initialization");

                // In development, we can continue even if there's an error
                var env = services.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
                if (env != null && !env.IsDevelopment())
                {
                    throw; // Rethrow in production
                }
            }
        }
    }

    /// <summary>
    /// JWT Settings configuration class
    /// </summary>
    public class JwtSettings
    {
        public string TokenKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInMinutes { get; set; } = 60;
    }
}