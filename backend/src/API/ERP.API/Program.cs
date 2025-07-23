using ERP.Application;
using ERP.Infrastructure;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog;
using ERP.Infrastructure.Data;

// Configuration Serilog avant le démarrage
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .Build())
    .CreateLogger();

try
{
    Log.Information("🚀 Starting ERP API application");

    var builder = WebApplication.CreateBuilder(args);

    // Configuration Serilog
    builder.Host.UseSerilog();

    // ✅ Add and configure CORS pour tous les environnements
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policyBuilder =>
        {
            if (builder.Environment.IsDevelopment())
            {
                // Configuration de développement - Plus permissive
                policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
            else
            {
                // Configuration de production - Plus sécurisée
                var corsSettings = builder.Configuration.GetSection("CorsSettings");
                var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };

                policyBuilder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }
        });
    });

    // ✅ Add Application & Infrastructure Layers
    builder.Services.AddApplicationLayer();
    builder.Services.AddInfrastructureLayer(builder.Configuration);

    // ✅ Add Controller Services
    builder.Services.AddControllers();

    // ✅ Configure Authentication & Authorization
    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();

    // ✅ Configure Swagger avec support pour tous les environnements
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "ERP API",
            Version = "v1.0.0",
            Description = "API complète pour la gestion ERP - Produits, Commandes & Clients",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "ERP Team",
                Email = "support@erp.com"
            }
        });

        // Configuration Bearer JWT
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. 
                            Enter 'Bearer' [space] and then your token in the text input below.
                            Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // ✅ Commentaires XML
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
            Log.Information("✅ XML comments loaded from: {XmlPath}", xmlPath);
        }
        else
        {
            Log.Warning("⚠️ XML file not found: {XmlPath}", xmlPath);
        }

        // Organisation des endpoints par contrôleur
        c.TagActionsBy(api =>
        {
            if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                return new[] { controllerActionDescriptor.ControllerName };
            }
            return new[] { "Default" };
        });

        c.OrderActionsBy(apiDesc =>
        {
            if (apiDesc.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                return $"{controllerActionDescriptor.ControllerName}_{apiDesc.HttpMethod}";
            }
            return $"Default_{apiDesc.HttpMethod}";
        });
    });

    var app = builder.Build();

    // ✅ CORS - Doit être le premier middleware
    app.UseCors("AllowFrontend");

    // ✅ Initialize database with migrations and seed data
    try
    {
        await app.InitializeDatabaseAsync();
        Log.Information("✅ Database initialized successfully");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "❌ Error occurred while initializing database");
        throw;
    }

    // ✅ Configure Swagger pour tous les environnements (dev ET production)
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1");
        c.RoutePrefix = "swagger";

        // ✅ Configuration UI améliorée
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        c.DefaultModelsExpandDepth(2);
        c.DefaultModelExpandDepth(2);
        c.DisplayRequestDuration();
        c.EnableTryItOutByDefault();
        c.EnableFilter();
        c.ShowExtensions();
        c.EnableValidator();

        // ✅ Thème et style
        c.DocumentTitle = "ERP API - Documentation Interactive";
        c.HeadContent = @"
            <style>
                .swagger-ui .topbar { display: none; }
                .swagger-ui .info { margin: 20px 0; }
                .swagger-ui .info .title { color: #2196F3; }
            </style>";

        // ✅ Configuration pour production
        if (!app.Environment.IsDevelopment())
        {
            c.DefaultModelsExpandDepth(-1); // Cache les modèles en production
        }
    });

    // ✅ Page d'accueil avec informations sur l'API
    app.MapGet("/", () =>
    {
        var environment = app.Environment.EnvironmentName;
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

        return Results.Json(new
        {
            message = "🚀 ERP API est opérationnelle !",
            version = version,
            environment = environment,
            documentation = "/swagger",
            endpoints = new
            {
                auth = "/api/auth",
                products = "/api/products"
            },
            timestamp = DateTime.UtcNow,
            status = "healthy"
        });
    }).ExcludeFromDescription().WithTags("Info");

    // ✅ Endpoint de santé simple (sans Health Checks)
    app.MapGet("/health", () =>
    {
        return Results.Json(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            environment = app.Environment.EnvironmentName,
            uptime = Environment.TickCount64
        });
    }).ExcludeFromDescription().WithTags("Info");

    // ✅ Redirection automatique vers Swagger si on accède à /api
    app.MapGet("/api", () => Results.Redirect("/swagger")).ExcludeFromDescription();

    // ✅ Middleware pipeline dans le bon ordre
    app.UseHttpsRedirection();

    // ✅ Security Headers pour la production
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Frame-Options", "DENY");
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
            await next();
        });
    }

    // ✅ Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // ✅ Request logging middleware
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.GetLevel = (httpContext, elapsed, ex) =>
        {
            if (ex != null) return Serilog.Events.LogEventLevel.Error;
            if (httpContext.Response.StatusCode > 499) return Serilog.Events.LogEventLevel.Error;
            if (httpContext.Response.StatusCode > 399) return Serilog.Events.LogEventLevel.Warning;
            return Serilog.Events.LogEventLevel.Information;
        };
    });

    // ✅ Map controllers
    app.MapControllers();

    // ✅ Logging de démarrage
    Log.Information("🚀 ERP API started successfully");
    Log.Information("📍 Environment: {Environment}", app.Environment.EnvironmentName);
    Log.Information("📚 Swagger available at: /swagger");
    Log.Information("🏥 Health check available at: /health");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "❌ Application terminated unexpectedly");
    throw;
}
finally
{
    Log.Information("🛑 ERP API application stopped");
    await Log.CloseAndFlushAsync();
}