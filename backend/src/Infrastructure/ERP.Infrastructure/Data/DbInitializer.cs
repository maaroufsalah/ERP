using ERP.Domain.Entities;
using ERP.Domain.Entities.Product;
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

                // ✅ NOUVEAU: Seed des données de référence AVANT les produits
                await SeedReferenceDataAsync(context, logger);

                // Seed Sample Products (après que les références existent)
                await SeedProductsAsync(context, logger);

                logger.LogInformation("✅ Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ An error occurred while initializing the database");
                throw;
            }
        }

        // ================================================================
        // NOUVEAU: SEEDING DES DONNÉES DE RÉFÉRENCE
        // ================================================================

        private static async Task SeedReferenceDataAsync(ErpDbContext context, ILogger logger)
        {
            logger.LogInformation("🎯 Seeding reference data...");

            // Seeding dans l'ordre des dépendances
            await SeedProductTypesAsync(context, logger);
            await SeedColorsAsync(context, logger);
            await SeedConditionsAsync(context, logger);
            await SeedBrandsAsync(context, logger);
            await SeedModelsAsync(context, logger);

            // Sauvegarder tous les changements
            await context.SaveChangesAsync();
            logger.LogInformation("✅ Reference data seeding completed");
        }

        private static async Task SeedProductTypesAsync(ErpDbContext context, ILogger logger)
        {
            if (await context.ProductTypes.AnyAsync())
            {
                logger.LogInformation("ℹ️ ProductTypes already exist, skipping...");
                return;
            }

            logger.LogInformation("📱 Seeding ProductTypes...");

            var productTypes = new[]
            {
                new ProductType
                {
                    Name = "Smartphone",
                    Description = "Téléphones mobiles intelligents (Android, iOS)",
                    SortOrder = 1,
                    IconUrl = "📱",
                    CategoryColor = "#007AFF",
                    CreatedBy = "System"
                },
                new ProductType
                {
                    Name = "Laptop",
                    Description = "Ordinateurs portables (Windows, macOS, Linux)",
                    SortOrder = 2,
                    IconUrl = "💻",
                    CategoryColor = "#34C759",
                    CreatedBy = "System"
                },
                new ProductType
                {
                    Name = "Tablet",
                    Description = "Tablettes tactiles (iPad, Android, Windows)",
                    SortOrder = 3,
                    IconUrl = "📔",
                    CategoryColor = "#FF9500",
                    CreatedBy = "System"
                },
                new ProductType
                {
                    Name = "Camera",
                    Description = "Appareils photo et caméras (DSLR, mirrorless, action cam)",
                    SortOrder = 4,
                    IconUrl = "📸",
                    CategoryColor = "#FF3B30",
                    CreatedBy = "System"
                },
                new ProductType
                {
                    Name = "Accessoire",
                    Description = "Accessoires électroniques (coques, chargeurs, écouteurs, etc.)",
                    SortOrder = 5,
                    IconUrl = "🔌",
                    CategoryColor = "#8E8E93",
                    CreatedBy = "System"
                }
            };

            await context.ProductTypes.AddRangeAsync(productTypes);
            logger.LogInformation("✅ Added {Count} ProductTypes", productTypes.Length);
        }

        private static async Task SeedColorsAsync(ErpDbContext context, ILogger logger)
        {
            if (await context.Colors.AnyAsync())
            {
                logger.LogInformation("ℹ️ Colors already exist, skipping...");
                return;
            }

            logger.LogInformation("🎨 Seeding Colors...");

            var colors = new[]
            {
                new Color { Name = "Noir", HexCode = "#000000", SortOrder = 1, CreatedBy = "System" },
                new Color { Name = "Blanc", HexCode = "#FFFFFF", SortOrder = 2, CreatedBy = "System" },
                new Color { Name = "Gris", HexCode = "#808080", SortOrder = 3, CreatedBy = "System" },
                new Color { Name = "Gris Sidéral", HexCode = "#666666", SortOrder = 4, CreatedBy = "System" },
                new Color { Name = "Bleu", HexCode = "#0066CC", SortOrder = 5, CreatedBy = "System" },
                new Color { Name = "Bleu Titane", HexCode = "#4A90E2", SortOrder = 6, CreatedBy = "System" },
                new Color { Name = "Rouge", HexCode = "#FF0000", SortOrder = 7, CreatedBy = "System" },
                new Color { Name = "Rose", HexCode = "#FFB6C1", SortOrder = 8, CreatedBy = "System" },
                new Color { Name = "Violet", HexCode = "#8A2BE2", SortOrder = 9, CreatedBy = "System" },
                new Color { Name = "Vert", HexCode = "#00FF00", SortOrder = 10, CreatedBy = "System" },
                new Color { Name = "Jaune", HexCode = "#FFFF00", SortOrder = 11, CreatedBy = "System" },
                new Color { Name = "Or", HexCode = "#FFD700", SortOrder = 12, CreatedBy = "System" },
                new Color { Name = "Argent", HexCode = "#C0C0C0", SortOrder = 13, CreatedBy = "System" },
                new Color { Name = "Platine", HexCode = "#E5E4E2", SortOrder = 14, CreatedBy = "System" }
            };

            await context.Colors.AddRangeAsync(colors);
            logger.LogInformation("✅ Added {Count} Colors", colors.Length);
        }

        private static async Task SeedConditionsAsync(ErpDbContext context, ILogger logger)
        {
            if (await context.Conditions.AnyAsync())
            {
                logger.LogInformation("ℹ️ Conditions already exist, skipping...");
                return;
            }

            logger.LogInformation("⭐ Seeding Conditions...");

            var conditions = new[]
            {
                new Condition
                {
                    Name = "Neuf",
                    Description = "Produit jamais utilisé, emballage d'origine intact, garantie constructeur complète",
                    QualityPercentage = 100,
                    SortOrder = 1,
                    CreatedBy = "System"
                },
                new Condition
                {
                    Name = "Excellent",
                    Description = "Produit en parfait état de fonctionnement, aucune rayure visible, très peu utilisé",
                    QualityPercentage = 95,
                    SortOrder = 2,
                    CreatedBy = "System"
                },
                new Condition
                {
                    Name = "Très Bon",
                    Description = "Produit en très bon état, quelques micro-rayures invisibles à l'usage normal",
                    QualityPercentage = 90,
                    SortOrder = 3,
                    CreatedBy = "System"
                },
                new Condition
                {
                    Name = "Bon",
                    Description = "Produit en bon état avec quelques signes d'usage visibles mais n'affectant pas le fonctionnement",
                    QualityPercentage = 80,
                    SortOrder = 4,
                    CreatedBy = "System"
                },
                new Condition
                {
                    Name = "Correct",
                    Description = "Produit fonctionnel avec des signes d'usage importants, rayures ou marques visibles",
                    QualityPercentage = 70,
                    SortOrder = 5,
                    CreatedBy = "System"
                }
            };

            await context.Conditions.AddRangeAsync(conditions);
            logger.LogInformation("✅ Added {Count} Conditions", conditions.Length);
        }

        private static async Task SeedBrandsAsync(ErpDbContext context, ILogger logger)
        {
            if (await context.Brands.AnyAsync())
            {
                logger.LogInformation("ℹ️ Brands already exist, skipping...");
                return;
            }

            logger.LogInformation("🏷️ Seeding Brands...");

            // Récupérer les IDs des types
            var smartphoneType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Smartphone");
            var laptopType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Laptop");
            var tabletType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Tablet");
            var cameraType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Camera");
            var accessoireType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Accessoire");

            var brands = new[]
            {
                // Smartphones
                new Brand { Name = "Samsung", ProductTypeId = smartphoneType.Id, SortOrder = 1, CreatedBy = "System" },
                new Brand { Name = "Apple", ProductTypeId = smartphoneType.Id, SortOrder = 2, CreatedBy = "System" },
                new Brand { Name = "Xiaomi", ProductTypeId = smartphoneType.Id, SortOrder = 3, CreatedBy = "System" },
                new Brand { Name = "OnePlus", ProductTypeId = smartphoneType.Id, SortOrder = 4, CreatedBy = "System" },
                new Brand { Name = "Google", ProductTypeId = smartphoneType.Id, SortOrder = 5, CreatedBy = "System" },

                // Laptops
                new Brand { Name = "Apple", ProductTypeId = laptopType.Id, SortOrder = 1, CreatedBy = "System" },
                new Brand { Name = "Dell", ProductTypeId = laptopType.Id, SortOrder = 2, CreatedBy = "System" },
                new Brand { Name = "HP", ProductTypeId = laptopType.Id, SortOrder = 3, CreatedBy = "System" },
                new Brand { Name = "Lenovo", ProductTypeId = laptopType.Id, SortOrder = 4, CreatedBy = "System" },
                new Brand { Name = "ASUS", ProductTypeId = laptopType.Id, SortOrder = 5, CreatedBy = "System" },

                // Tablets
                new Brand { Name = "Apple", ProductTypeId = tabletType.Id, SortOrder = 1, CreatedBy = "System" },
                new Brand { Name = "Samsung", ProductTypeId = tabletType.Id, SortOrder = 2, CreatedBy = "System" },
                new Brand { Name = "Microsoft", ProductTypeId = tabletType.Id, SortOrder = 3, CreatedBy = "System" },

                // Cameras
                new Brand { Name = "Canon", ProductTypeId = cameraType.Id, SortOrder = 1, CreatedBy = "System" },
                new Brand { Name = "Nikon", ProductTypeId = cameraType.Id, SortOrder = 2, CreatedBy = "System" },
                new Brand { Name = "Sony", ProductTypeId = cameraType.Id, SortOrder = 3, CreatedBy = "System" },
                new Brand { Name = "GoPro", ProductTypeId = cameraType.Id, SortOrder = 4, CreatedBy = "System" },

                // Accessoires
                new Brand { Name = "Anker", ProductTypeId = accessoireType.Id, SortOrder = 1, CreatedBy = "System" },
                new Brand { Name = "Belkin", ProductTypeId = accessoireType.Id, SortOrder = 2, CreatedBy = "System" },
                new Brand { Name = "Logitech", ProductTypeId = accessoireType.Id, SortOrder = 3, CreatedBy = "System" }
            };

            await context.Brands.AddRangeAsync(brands);
            logger.LogInformation("✅ Added {Count} Brands", brands.Length);
        }

        private static async Task SeedModelsAsync(ErpDbContext context, ILogger logger)
        {
            if (await context.Models.AnyAsync())
            {
                logger.LogInformation("ℹ️ Models already exist, skipping...");
                return;
            }

            logger.LogInformation("📋 Seeding Models...");

            // Récupérer les types
            var smartphoneType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Smartphone");
            var laptopType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Laptop");
            var tabletType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Tablet");

            // Récupérer les marques
            var samsungSmartphone = await context.Brands.FirstAsync(b => b.Name == "Samsung" && b.ProductTypeId == smartphoneType.Id);
            var appleSmartphone = await context.Brands.FirstAsync(b => b.Name == "Apple" && b.ProductTypeId == smartphoneType.Id);
            var appleLaptop = await context.Brands.FirstAsync(b => b.Name == "Apple" && b.ProductTypeId == laptopType.Id);
            var dellLaptop = await context.Brands.FirstAsync(b => b.Name == "Dell" && b.ProductTypeId == laptopType.Id);
            var appleTablet = await context.Brands.FirstAsync(b => b.Name == "Apple" && b.ProductTypeId == tabletType.Id);

            var models = new[]
            {
                // Samsung Smartphones
                new Model { Name = "Galaxy S24 Ultra", ProductTypeId = smartphoneType.Id, BrandId = samsungSmartphone.Id, ReleaseYear = 2024, SortOrder = 1, CreatedBy = "System" },
                new Model { Name = "Galaxy S24", ProductTypeId = smartphoneType.Id, BrandId = samsungSmartphone.Id, ReleaseYear = 2024, SortOrder = 2, CreatedBy = "System" },
                new Model { Name = "Galaxy A55", ProductTypeId = smartphoneType.Id, BrandId = samsungSmartphone.Id, ReleaseYear = 2024, SortOrder = 3, CreatedBy = "System" },
                new Model { Name = "Galaxy A34", ProductTypeId = smartphoneType.Id, BrandId = samsungSmartphone.Id, ReleaseYear = 2023, SortOrder = 4, CreatedBy = "System" },

                // Apple Smartphones
                new Model { Name = "iPhone 15 Pro Max", ProductTypeId = smartphoneType.Id, BrandId = appleSmartphone.Id, ReleaseYear = 2023, SortOrder = 1, CreatedBy = "System" },
                new Model { Name = "iPhone 15 Pro", ProductTypeId = smartphoneType.Id, BrandId = appleSmartphone.Id, ReleaseYear = 2023, SortOrder = 2, CreatedBy = "System" },
                new Model { Name = "iPhone 15", ProductTypeId = smartphoneType.Id, BrandId = appleSmartphone.Id, ReleaseYear = 2023, SortOrder = 3, CreatedBy = "System" },
                new Model { Name = "iPhone 14", ProductTypeId = smartphoneType.Id, BrandId = appleSmartphone.Id, ReleaseYear = 2022, SortOrder = 4, CreatedBy = "System" },

                // Apple Laptops
                new Model { Name = "MacBook Air M2", ProductTypeId = laptopType.Id, BrandId = appleLaptop.Id, ReleaseYear = 2022, SortOrder = 1, CreatedBy = "System" },
                new Model { Name = "MacBook Pro 14\"", ProductTypeId = laptopType.Id, BrandId = appleLaptop.Id, ReleaseYear = 2023, SortOrder = 2, CreatedBy = "System" },

                // Dell Laptops
                new Model { Name = "XPS 13 Plus", ProductTypeId = laptopType.Id, BrandId = dellLaptop.Id, ReleaseYear = 2022, SortOrder = 1, CreatedBy = "System" },
                new Model { Name = "Inspiron 15 3000", ProductTypeId = laptopType.Id, BrandId = dellLaptop.Id, ReleaseYear = 2023, SortOrder = 2, CreatedBy = "System" },

                // Apple Tablets
                new Model { Name = "iPad Air 5", ProductTypeId = tabletType.Id, BrandId = appleTablet.Id, ReleaseYear = 2022, SortOrder = 1, CreatedBy = "System" },
                new Model { Name = "iPad Pro 12.9\"", ProductTypeId = tabletType.Id, BrandId = appleTablet.Id, ReleaseYear = 2022, SortOrder = 2, CreatedBy = "System" }
            };

            await context.Models.AddRangeAsync(models);
            logger.LogInformation("✅ Added {Count} Models", models.Length);
        }

        // ================================================================
        // MÉTHODES EXISTANTES (INCHANGÉES)
        // ================================================================

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

        // ================================================================
        // PRODUITS MODIFIÉS POUR UTILISER LES RÉFÉRENCES
        // ================================================================

        private static async Task SeedProductsAsync(ErpDbContext context, ILogger logger)
        {
            logger.LogInformation("📱 Seeding sample products...");

            if (!await context.Products.AnyAsync())
            {
                // Récupérer les IDs des références pour créer les produits
                var smartphoneType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Smartphone");
                var laptopType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Laptop");
                var tabletType = await context.ProductTypes.FirstAsync(pt => pt.Name == "Tablet");

                var samsungSmartphone = await context.Brands.FirstAsync(b => b.Name == "Samsung" && b.ProductTypeId == smartphoneType.Id);
                var appleSmartphone = await context.Brands.FirstAsync(b => b.Name == "Apple" && b.ProductTypeId == smartphoneType.Id);
                var appleLaptop = await context.Brands.FirstAsync(b => b.Name == "Apple" && b.ProductTypeId == laptopType.Id);
                var dellLaptop = await context.Brands.FirstAsync(b => b.Name == "Dell" && b.ProductTypeId == laptopType.Id);
                var appleTablet = await context.Brands.FirstAsync(b => b.Name == "Apple" && b.ProductTypeId == tabletType.Id);

                var galaxyA55 = await context.Models.FirstAsync(m => m.Name == "Galaxy A55");
                var iphone15 = await context.Models.FirstAsync(m => m.Name == "iPhone 15");
                var galaxyA34 = await context.Models.FirstAsync(m => m.Name == "Galaxy A34");
                var xps13 = await context.Models.FirstAsync(m => m.Name == "XPS 13 Plus");
                var macbookAir = await context.Models.FirstAsync(m => m.Name == "MacBook Air M2");
                var ipadAir = await context.Models.FirstAsync(m => m.Name == "iPad Air 5");

                var colorBleu = await context.Colors.FirstAsync(c => c.Name == "Bleu");
                var colorNoir = await context.Colors.FirstAsync(c => c.Name == "Noir");
                var colorViolet = await context.Colors.FirstAsync(c => c.Name == "Violet");
                var colorPlatine = await context.Colors.FirstAsync(c => c.Name == "Platine");
                var colorGrisSideral = await context.Colors.FirstAsync(c => c.Name == "Gris Sidéral");

                var conditionTresBon = await context.Conditions.FirstAsync(c => c.Name == "Très Bon");
                var conditionNeuf = await context.Conditions.FirstAsync(c => c.Name == "Neuf");
                var conditionBon = await context.Conditions.FirstAsync(c => c.Name == "Bon");

                var products = new List<Product>
                {
                    // SMARTPHONES avec nouvelles relations
                    new Product
                    {
                        Name = "Samsung Galaxy A55 5G",
                        Description = "Smartphone Samsung Galaxy A55 5G 8/256GB en excellent état",
                        ProductTypeId = smartphoneType.Id,
                        BrandId = samsungSmartphone.Id,
                        ModelId = galaxyA55.Id,
                        ColorId = colorBleu.Id,
                        ConditionId = conditionTresBon.Id,
                        Storage = "256GB",
                        Memory = "8GB",
                        ScreenSize = "6.6\"",
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
                        ProductTypeId = smartphoneType.Id,
                        BrandId = appleSmartphone.Id,
                        ModelId = iphone15.Id,
                        ColorId = colorNoir.Id,
                        ConditionId = conditionNeuf.Id,
                        Storage = "128GB",
                        ScreenSize = "6.1\"",
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
                        ProductTypeId = smartphoneType.Id,
                        BrandId = samsungSmartphone.Id,
                        ModelId = galaxyA34.Id,
                        ColorId = colorViolet.Id,
                        ConditionId = conditionBon.Id,
                        Storage = "128GB",
                        Memory = "6GB",
                        ScreenSize = "6.6\"",
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

                    // LAPTOPS avec nouvelles relations
                    new Product
                    {
                        Name = "Dell XPS 13 Plus",
                        Description = "Ordinateur portable Dell XPS 13 Plus Intel i7 16GB 512GB",
                        ProductTypeId = laptopType.Id,
                        BrandId = dellLaptop.Id,
                        ModelId = xps13.Id,
                        ColorId = colorPlatine.Id,
                        ConditionId = conditionTresBon.Id,
                        Storage = "512GB SSD",
                        Memory = "16GB",
                        Processor = "Intel Core i7-12700H",
                        ScreenSize = "13.4\"",
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
                        ProductTypeId = laptopType.Id,
                        BrandId = appleLaptop.Id,
                        ModelId = macbookAir.Id,
                        ColorId = colorGrisSideral.Id,
                        ConditionId = conditionNeuf.Id,
                        Storage = "256GB SSD",
                        Memory = "8GB",
                        Processor = "Apple M2",
                        ScreenSize = "13.6\"",
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

                    // TABLETS avec nouvelles relations
                    new Product
                    {
                        Name = "iPad Air 5th Gen",
                        Description = "Apple iPad Air 5e génération 64GB WiFi Bleu",
                        ProductTypeId = tabletType.Id,
                        BrandId = appleTablet.Id,
                        ModelId = ipadAir.Id,
                        ColorId = colorBleu.Id,
                        ConditionId = conditionTresBon.Id,
                        Storage = "64GB",
                        ScreenSize = "10.9\"",
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