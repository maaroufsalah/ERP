using ERP.Domain.Entities;
using ERP.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ERP.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ErpDbContext>>();
            var context = services.GetRequiredService<ErpDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                logger.LogInformation("🗄️ Applying migrations...");

                // ✅ IMPORTANT: Créer la base de données si elle n'existe pas
                await context.Database.EnsureCreatedAsync();
                await context.Database.MigrateAsync();

                // ✅ IMPORTANT: Attendre que les migrations soient complètement appliquées
                logger.LogInformation("✅ Migrations applied successfully");

                // ✅ Maintenant on peut procéder au seeding
                // Seed Roles (après que les tables Identity existent)
                await SeedRolesAsync(roleManager, logger);

                // Seed Admin Users (après que les rôles existent)
                await SeedAdminUsersAsync(userManager, logger);

                // Seed Sample Products (après que les utilisateurs existent)
                await SeedProductsAsync(context, logger);

                logger.LogInformation("✅ Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ An error occurred while initializing the database");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            logger.LogInformation("👥 Seeding roles...");

            string[] roles = { "Admin", "Manager", "Employee", "Customer" };

            foreach (var role in roles)
            {
                try
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        logger.LogInformation("Creating role: {Role}", role);
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (result.Succeeded)
                        {
                            logger.LogInformation("✅ Role {Role} created successfully", role);
                        }
                        else
                        {
                            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                            logger.LogError("❌ Failed to create role {Role}: {Errors}", role, errors);
                        }
                    }
                    else
                    {
                        logger.LogInformation("ℹ️ Role {Role} already exists", role);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "❌ Error while seeding role {Role}", role);
                    throw;
                }
            }
        }

        private static async Task SeedAdminUsersAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            logger.LogInformation("👤 Seeding admin users...");

            await AddUserAsync(userManager, logger, "admin", "Admin", "ERP", "admin@erp.com", "admin123!", "Admin");
            await AddUserAsync(userManager, logger, "manager", "Manager", "ERP", "manager@erp.com", "manager123!", "Manager");
        }

        private static async Task SeedProductsAsync(ErpDbContext context, ILogger logger)
        {
            logger.LogInformation("📱 Seeding sample products...");

            if (!await context.Products.AnyAsync())
            {
                var products = new List<Product>
                {
                    // SMARTPHONES
                    new Product
                    {
                        Name = "Samsung Galaxy A55 5G",
                        Description = "Smartphone Samsung Galaxy A55 5G 8/256GB en excellent état",
                        Category = "Smartphones",
                        Brand = "Samsung",
                        Model = "Galaxy A55",
                        Storage = "256GB",
                        Memory = "8GB",
                        Color = "Bleu",
                        ScreenSize = "6.6\"",
                        Condition = "Très bon état",
                        ConditionGrade = "A",
                        PurchasePrice = 200.00m,
                        TransportCost = 15.00m,
                        TotalCostPrice = 215.00m,
                        SellingPrice = 280.00m,
                        Margin = 65.00m,
                        MarginPercentage = 30.23m,
                        Stock = 5,
                        MinStockLevel = 2,
                        SupplierName = "TechItalia SRL",
                        SupplierCity = "Milano",
                        PurchaseDate = DateTime.UtcNow.AddDays(-15),
                        ArrivalDate = DateTime.UtcNow.AddDays(-10),
                        ImportBatch = "IT2025001",
                        InvoiceNumber = "INV-2025-001",
                        Status = "Available",
                        CreatedBy = "System"
                    },

                    new Product
                    {
                        Name = "iPhone 15 128GB",
                        Description = "Apple iPhone 15 128GB Noir en parfait état avec boîte",
                        Category = "Smartphones",
                        Brand = "Apple",
                        Model = "iPhone 15",
                        Storage = "128GB",
                        Color = "Noir",
                        ScreenSize = "6.1\"",
                        Condition = "Neuf",
                        ConditionGrade = "A+",
                        PurchasePrice = 650.00m,
                        TransportCost = 20.00m,
                        TotalCostPrice = 670.00m,
                        SellingPrice = 799.00m,
                        Margin = 129.00m,
                        MarginPercentage = 19.25m,
                        Stock = 3,
                        MinStockLevel = 1,
                        SupplierName = "MobileWorld Italia",
                        SupplierCity = "Roma",
                        PurchaseDate = DateTime.UtcNow.AddDays(-8),
                        ArrivalDate = DateTime.UtcNow.AddDays(-3),
                        ImportBatch = "IT2025002",
                        InvoiceNumber = "INV-2025-002",
                        Status = "Available",
                        WarrantyInfo = "Garantie constructeur 1 an",
                        CreatedBy = "System"
                    },

                    new Product
                    {
                        Name = "Samsung Galaxy A34 5G",
                        Description = "Samsung Galaxy A34 5G 6/128GB Violet occasion",
                        Category = "Smartphones",
                        Brand = "Samsung",
                        Model = "Galaxy A34",
                        Storage = "128GB",
                        Memory = "6GB",
                        Color = "Violet",
                        ScreenSize = "6.6\"",
                        Condition = "Bon état",
                        ConditionGrade = "B+",
                        PurchasePrice = 150.00m,
                        TransportCost = 12.00m,
                        TotalCostPrice = 162.00m,
                        SellingPrice = 220.00m,
                        Margin = 58.00m,
                        MarginPercentage = 35.80m,
                        Stock = 8,
                        MinStockLevel = 3,
                        SupplierName = "RefurbItalia",
                        SupplierCity = "Napoli",
                        PurchaseDate = DateTime.UtcNow.AddDays(-20),
                        ArrivalDate = DateTime.UtcNow.AddDays(-15),
                        ImportBatch = "IT2025001",
                        InvoiceNumber = "INV-2025-003",
                        Status = "Available",
                        Notes = "Quelques micro-rayures sur l'écran",
                        CreatedBy = "System"
                    },

                    // LAPTOPS
                    new Product
                    {
                        Name = "Dell XPS 13 Plus",
                        Description = "Ordinateur portable Dell XPS 13 Plus Intel i7 16GB 512GB",
                        Category = "Laptops",
                        Brand = "Dell",
                        Model = "XPS 13 Plus",
                        Storage = "512GB SSD",
                        Memory = "16GB",
                        Processor = "Intel Core i7-12700H",
                        ScreenSize = "13.4\"",
                        Color = "Platine",
                        Condition = "Très bon état",
                        ConditionGrade = "A",
                        PurchasePrice = 800.00m,
                        TransportCost = 35.00m,
                        TotalCostPrice = 835.00m,
                        SellingPrice = 1150.00m,
                        Margin = 315.00m,
                        MarginPercentage = 37.72m,
                        Stock = 2,
                        MinStockLevel = 1,
                        SupplierName = "ComputerItalia",
                        SupplierCity = "Torino",
                        PurchaseDate = DateTime.UtcNow.AddDays(-12),
                        ArrivalDate = DateTime.UtcNow.AddDays(-7),
                        ImportBatch = "IT2025003",
                        InvoiceNumber = "INV-2025-004",
                        Status = "Available",
                        WarrantyInfo = "6 mois garantie magasin",
                        CreatedBy = "System"
                    },

                    new Product
                    {
                        Name = "MacBook Air M2",
                        Description = "Apple MacBook Air 13\" M2 8GB 256GB Gris Sidéral",
                        Category = "Laptops",
                        Brand = "Apple",
                        Model = "MacBook Air M2",
                        Storage = "256GB SSD",
                        Memory = "8GB",
                        Processor = "Apple M2",
                        ScreenSize = "13.6\"",
                        Color = "Gris Sidéral",
                        Condition = "Neuf",
                        ConditionGrade = "A+",
                        PurchasePrice = 950.00m,
                        TransportCost = 40.00m,
                        TotalCostPrice = 990.00m,
                        SellingPrice = 1299.00m,
                        Margin = 309.00m,
                        MarginPercentage = 31.21m,
                        Stock = 1,
                        MinStockLevel = 1,
                        SupplierName = "AppleStore Milano",
                        SupplierCity = "Milano",
                        PurchaseDate = DateTime.UtcNow.AddDays(-5),
                        ArrivalDate = DateTime.UtcNow.AddDays(-1),
                        ImportBatch = "IT2025004",
                        InvoiceNumber = "INV-2025-005",
                        Status = "Available",
                        WarrantyInfo = "Garantie Apple 1 an",
                        CreatedBy = "System"
                    },

                    // TABLETS
                    new Product
                    {
                        Name = "iPad Air 5th Gen",
                        Description = "Apple iPad Air 5e génération 64GB WiFi Bleu",
                        Category = "Tablets",
                        Brand = "Apple",
                        Model = "iPad Air 5",
                        Storage = "64GB",
                        ScreenSize = "10.9\"",
                        Color = "Bleu",
                        Condition = "Très bon état",
                        ConditionGrade = "A",
                        PurchasePrice = 400.00m,
                        TransportCost = 18.00m,
                        TotalCostPrice = 418.00m,
                        SellingPrice = 549.00m,
                        Margin = 131.00m,
                        MarginPercentage = 31.34m,
                        Stock = 4,
                        MinStockLevel = 2,
                        SupplierName = "TabletItalia",
                        SupplierCity = "Bologna",
                        PurchaseDate = DateTime.UtcNow.AddDays(-18),
                        ArrivalDate = DateTime.UtcNow.AddDays(-13),
                        ImportBatch = "IT2025002",
                        InvoiceNumber = "INV-2025-006",
                        Status = "Available",
                        CreatedBy = "System"
                    },

                    // ACCESSOIRES
                    new Product
                    {
                        Name = "AirPods Pro 2nd Gen",
                        Description = "Apple AirPods Pro 2e génération avec boîtier de charge MagSafe",
                        Category = "Accessoires",
                        Brand = "Apple",
                        Model = "AirPods Pro 2",
                        Color = "Blanc",
                        Condition = "Neuf",
                        ConditionGrade = "A+",
                        PurchasePrice = 180.00m,
                        TransportCost = 8.00m,
                        TotalCostPrice = 188.00m,
                        SellingPrice = 249.00m,
                        Margin = 61.00m,
                        MarginPercentage = 32.45m,
                        Stock = 10,
                        MinStockLevel = 5,
                        SupplierName = "AudioItalia",
                        SupplierCity = "Firenze",
                        PurchaseDate = DateTime.UtcNow.AddDays(-10),
                        ArrivalDate = DateTime.UtcNow.AddDays(-6),
                        ImportBatch = "IT2025003",
                        InvoiceNumber = "INV-2025-007",
                        Status = "Available",
                        WarrantyInfo = "Garantie Apple 1 an",
                        CreatedBy = "System"
                    }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
                logger.LogInformation("✅ Added {Count} sample products to the database", products.Count);
            }
            else
            {
                logger.LogInformation("ℹ️ Products already exist in the database");
            }
        }

        private static async Task AddUserAsync(
            UserManager<ApplicationUser> userManager,
            ILogger logger,
            string username,
            string firstName,
            string lastName,
            string email,
            string password,
            string userRole)
        {
            try
            {
                var user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    logger.LogInformation("Creating user: {Username}", username);

                    user = new ApplicationUser
                    {
                        UserName = username,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        logger.LogError("❌ Error creating user {Username}: {Errors}", username, errors);
                        throw new Exception($"Error creating user {username}: {errors}");
                    }

                    await userManager.AddToRoleAsync(user, userRole);
                    logger.LogInformation("✅ User {Username} created and added to role {Role}", username, userRole);
                }
                else
                {
                    logger.LogInformation("ℹ️ User {Username} already exists", username);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Error while seeding user {Username}", username);
                throw;
            }
        }
    }
}