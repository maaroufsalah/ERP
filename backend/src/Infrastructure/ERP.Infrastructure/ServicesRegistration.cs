using ERP.Application.Interfaces;
using ERP.Application.Interfaces.IServices;
using ERP.Application.Services;
using ERP.Domain.Entities;
using ERP.Domain.Entities.Product;
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
        /// Register all repositories - VERSION MISE À JOUR
        /// </summary>
        private static void RegisterRepositories(IServiceCollection services)
        {
            // ================================================================
            // REPOSITORY PRINCIPAL GÉNÉRIQUE ÉTENDU
            // ================================================================
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // ================================================================
            // REPOSITORIES SPÉCIFIQUES POUR LES NOUVELLES ENTITÉS
            // ================================================================

            // Repository pour Product avec toutes les relations
            services.AddScoped<IRepository<Product>, Repository<Product>>();

            // Repositories pour les nouvelles tables de référence
            services.AddScoped<IRepository<ProductType>, Repository<ProductType>>();
            services.AddScoped<IRepository<Brand>, Repository<Brand>>();
            services.AddScoped<IRepository<Model>, Repository<Model>>();
            services.AddScoped<IRepository<Color>, Repository<Color>>();
            services.AddScoped<IRepository<Condition>, Repository<Condition>>();

            // ================================================================
            // Note: Le repository générique étendu gère déjà toutes les 
            // fonctionnalités avancées (includes, filtres, etc.)
            // ================================================================
        }

        /// <summary>
        /// Register all services - VERSION MISE À JOUR
        /// </summary>
        private static void RegisterServices(IServiceCollection services)
        {
            // ================================================================
            // SERVICES PRINCIPAUX - NOUVEAUX
            // ================================================================

            // Service principal pour les produits avec les nouvelles relations
            services.AddScoped<IProductService, ProductService>();

            // ================================================================
            // SERVICES POUR LES TABLES DE RÉFÉRENCE - NOUVEAUX
            // ================================================================

            // Note: Ces services seront créés progressivement selon vos besoins
            // Pour l'instant, le ProductService gère déjà les dropdowns via les repositories

            // services.AddScoped<IProductTypeService, ProductTypeService>();
            // services.AddScoped<IBrandService, BrandService>();
            // services.AddScoped<IModelService, ModelService>();
            // services.AddScoped<IColorService, ColorService>();
            // services.AddScoped<IConditionService, ConditionService>();

            // ================================================================
            // SERVICES D'IDENTITÉ ET AUTHENTIFICATION (EXISTANTS)
            // ================================================================

            // Ces services seront activés quand vous les créerez
            // services.AddScoped<IAccountService, AccountService>();
            // services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            // services.AddScoped<TokenService>();

            // ================================================================
            // AUTRES SERVICES MÉTIER (À AJOUTER SELON VOS BESOINS)
            // ================================================================

            // services.AddScoped<IEmailService, EmailService>();
            // services.AddScoped<IFileUploadService, FileUploadService>();
            // services.AddScoped<INotificationService, NotificationService>();

            // ================================================================
            // SERVICES UTILITAIRES POUR LES NOUVELLES FONCTIONNALITÉS
            // ================================================================

            // Service de validation des relations (simple implémentation)
            services.AddScoped<IRelationValidationService, RelationValidationService>();
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
                logger.LogInformation("🚀 Initializing database...");
                await DbInitializer.InitializeAsync(services);
                logger.LogInformation("✅ Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ An error occurred during database initialization");

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

    // ================================================================
    // INTERFACE ET IMPLÉMENTATION SIMPLE POUR LA VALIDATION DES RELATIONS
    // ================================================================

    /// <summary>
    /// Interface pour la validation des relations entre entités
    /// </summary>
    public interface IRelationValidationService
    {
        Task<bool> ValidateProductRelationsAsync(int productTypeId, int brandId, int modelId, int colorId, int conditionId);
        Task<bool> ValidateBrandForProductTypeAsync(int brandId, int productTypeId);
        Task<bool> ValidateModelForBrandAsync(int modelId, int brandId);
    }

    /// <summary>
    /// Implémentation simple du service de validation des relations
    /// </summary>
    public class RelationValidationService : IRelationValidationService
    {
        private readonly IRepository<ProductType> _productTypeRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Model> _modelRepository;
        private readonly IRepository<Color> _colorRepository;
        private readonly IRepository<Condition> _conditionRepository;
        private readonly ILogger<RelationValidationService> _logger;

        public RelationValidationService(
            IRepository<ProductType> productTypeRepository,
            IRepository<Brand> brandRepository,
            IRepository<Model> modelRepository,
            IRepository<Color> colorRepository,
            IRepository<Condition> conditionRepository,
            ILogger<RelationValidationService> logger)
        {
            _productTypeRepository = productTypeRepository;
            _brandRepository = brandRepository;
            _modelRepository = modelRepository;
            _colorRepository = colorRepository;
            _conditionRepository = conditionRepository;
            _logger = logger;
        }

        public async Task<bool> ValidateProductRelationsAsync(int productTypeId, int brandId, int modelId, int colorId, int conditionId)
        {
            try
            {
                _logger.LogDebug("Validation des relations pour le produit: ProductType={ProductTypeId}, Brand={BrandId}, Model={ModelId}, Color={ColorId}, Condition={ConditionId}",
                    productTypeId, brandId, modelId, colorId, conditionId);

                // Vérifier que tous les éléments existent
                var productType = await _productTypeRepository.GetByIdAsync(productTypeId);
                var brand = await _brandRepository.GetByIdAsync(brandId);
                var model = await _modelRepository.GetByIdAsync(modelId);
                var color = await _colorRepository.GetByIdAsync(colorId);
                var condition = await _conditionRepository.GetByIdAsync(conditionId);

                if (productType == null || brand == null || model == null || color == null || condition == null)
                {
                    _logger.LogWarning("Une ou plusieurs entités de référence n'existent pas");
                    return false;
                }

                // Vérifier la cohérence des relations
                // Pour l'instant, on fait une validation simple
                // À étendre selon vos règles métier spécifiques

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation des relations");
                return false;
            }
        }

        public async Task<bool> ValidateBrandForProductTypeAsync(int brandId, int productTypeId)
        {
            try
            {
                var brand = await _brandRepository.GetByIdAsync(brandId);
                if (brand == null) return false;

                // Vérifier si la marque appartient au bon type de produit
                // Cette logique sera ajoutée quand vous aurez les propriétés de relation dans Brand
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation de la marque pour le type de produit");
                return false;
            }
        }

        public async Task<bool> ValidateModelForBrandAsync(int modelId, int brandId)
        {
            try
            {
                var model = await _modelRepository.GetByIdAsync(modelId);
                if (model == null) return false;

                // Vérifier si le modèle appartient à la bonne marque
                // Cette logique sera ajoutée quand vous aurez les propriétés de relation dans Model
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation du modèle pour la marque");
                return false;
            }
        }
    }
}