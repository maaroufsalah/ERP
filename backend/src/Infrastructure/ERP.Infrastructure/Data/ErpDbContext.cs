// ================================================================
// DBCONTEXT MODIFIÉ POUR LA NOUVELLE ARCHITECTURE
// ================================================================
// Ajout des nouvelles entités Product Master/Variant + Inventory
// ================================================================

using ERP.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities.Product;
using ERP.Domain.Entities.Catalog;
using ERP.Domain.Entities.Inventory;

namespace ERP.Infrastructure.Data
{
    /// <summary>
    /// Contexte de base de données Entity Framework pour l'ERP
    /// Version mise à jour avec la nouvelle architecture Product Master/Variant + Inventory
    /// </summary>
    public class ErpDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
        {
        }

        // ================================================================
        // DBSETS - CATALOG CONTEXT (Gestion des produits)
        // ================================================================

        /// <summary>
        /// Produits master (références génériques)
        /// </summary>
        public DbSet<ProductMaster> ProductMasters { get; set; }

        /// <summary>
        /// Variantes de produits (produits physiques vendables)
        /// </summary>
        public DbSet<ProductVariant> ProductVariants { get; set; }

        // ================================================================
        // DBSETS - INVENTORY CONTEXT (Gestion des stocks)
        // ================================================================

        /// <summary>
        /// Entrepôts et lieux de stockage
        /// </summary>
        public DbSet<Warehouse> Warehouses { get; set; }

        /// <summary>
        /// Emplacements spécifiques dans les entrepôts
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Stocks par produit variant et emplacement
        /// </summary>
        public DbSet<Stock> Stocks { get; set; }

        /// <summary>
        /// Historique des mouvements de stock
        /// </summary>
        public DbSet<StockMovement> StockMovements { get; set; }

        // ================================================================
        // DBSETS - TABLES DE RÉFÉRENCE (Shared Kernel)
        // ================================================================

        /// <summary>
        /// Table des produits (LEGACY - à migrer vers ProductVariant)
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Table des types de produits (Smartphone, Laptop, Camera, Accessoire)
        /// </summary>
        public DbSet<ProductType> ProductTypes { get; set; }

        /// <summary>
        /// Table des marques (Samsung, Apple, Dell, Canon, etc.)
        /// </summary>
        public DbSet<Brand> Brands { get; set; }

        /// <summary>
        /// Table des modèles (Galaxy S24, iPhone 15, XPS 13, etc.)
        /// </summary>
        public DbSet<Model> Models { get; set; }

        /// <summary>
        /// Table des couleurs (Noir, Blanc, Bleu, etc.)
        /// </summary>
        public DbSet<Color> Colors { get; set; }

        /// <summary>
        /// Table des conditions (Neuf, Excellent, Très Bon, Bon, Correct)
        /// </summary>
        public DbSet<Condition> Conditions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================================================================
            // CONFIGURATION DES NOUVELLES ENTITÉS CATALOG
            // ================================================================
            ConfigureProductMaster(modelBuilder);
            ConfigureProductVariant(modelBuilder);

            // ================================================================
            // CONFIGURATION DES ENTITÉS INVENTORY
            // ================================================================
            ConfigureWarehouse(modelBuilder);
            ConfigureLocation(modelBuilder);
            ConfigureStock(modelBuilder);
            ConfigureStockMovement(modelBuilder);

            // ================================================================
            // CONFIGURATION DES ENTITÉS DE RÉFÉRENCE (EXISTANTES)
            // ================================================================
            ConfigureProductType(modelBuilder);
            ConfigureBrand(modelBuilder);
            ConfigureModel(modelBuilder);
            ConfigureColor(modelBuilder);
            ConfigureCondition(modelBuilder);
            ConfigureProduct(modelBuilder); // LEGACY
            ConfigureApplicationUser(modelBuilder);
        }

        // ================================================================
        // CONFIGURATION PRODUCTMASTER
        // ================================================================
        private void ConfigureProductMaster(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductMaster>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("Nom commercial générique du produit");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasComment("Description générale du produit");

                entity.Property(e => e.ReferenceNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Numéro de référence interne unique");

                entity.Property(e => e.ProductTypeId)
                    .IsRequired()
                    .HasComment("Type de produit");

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasComment("Marque du produit");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Draft");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Relations
                entity.HasOne(pm => pm.ProductType)
                    .WithMany()
                    .HasForeignKey(pm => pm.ProductTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductMaster_ProductType");

                entity.HasOne(pm => pm.Brand)
                    .WithMany()
                    .HasForeignKey(pm => pm.BrandId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductMaster_Brand");

                // Index
                entity.HasIndex(e => e.ReferenceNumber)
                    .IsUnique()
                    .HasDatabaseName("IX_ProductMaster_ReferenceNumber_Unique");

                entity.HasIndex(e => new { e.ProductTypeId, e.BrandId })
                    .HasDatabaseName("IX_ProductMaster_Type_Brand");

                entity.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_ProductMaster_Status");

                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_ProductMaster_IsActive");
            });
        }

        // ================================================================
        // CONFIGURATION PRODUCTVARIANT
        // ================================================================
        private void ConfigureProductVariant(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProductMasterId)
                    .IsRequired()
                    .HasComment("Référence vers le produit master");

                entity.Property(e => e.SKU)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("SKU unique de la variante");

                entity.Property(e => e.VariantName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("Nom spécifique de la variante");

                entity.Property(e => e.PurchasePrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasComment("Prix d'achat unitaire");

                entity.Property(e => e.AdditionalCosts)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0)
                    .HasComment("Frais additionnels");

                entity.Property(e => e.TotalCostPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("Coût total unitaire");

                entity.Property(e => e.SellingPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasComment("Prix de vente unitaire");

                entity.Property(e => e.Margin)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("Marge bénéficiaire");

                entity.Property(e => e.MarginPercentage)
                    .HasColumnType("decimal(5,2)")
                    .HasComment("Pourcentage de marge");

                entity.Property(e => e.ConditionId)
                    .IsRequired()
                    .HasComment("Condition du produit");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Available");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Relations
                entity.HasOne(pv => pv.ProductMaster)
                    .WithMany(pm => pm.Variants)
                    .HasForeignKey(pv => pv.ProductMasterId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProductVariant_ProductMaster");

                entity.HasOne(pv => pv.Color)
                    .WithMany()
                    .HasForeignKey(pv => pv.ColorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductVariant_Color");

                entity.HasOne(pv => pv.Condition)
                    .WithMany()
                    .HasForeignKey(pv => pv.ConditionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ProductVariant_Condition");

                // Index
                entity.HasIndex(e => e.SKU)
                    .IsUnique()
                    .HasDatabaseName("IX_ProductVariant_SKU_Unique");

                entity.HasIndex(e => e.Barcode)
                    .HasDatabaseName("IX_ProductVariant_Barcode");

                entity.HasIndex(e => e.ProductMasterId)
                    .HasDatabaseName("IX_ProductVariant_ProductMasterId");

                entity.HasIndex(e => new { e.Status, e.IsActive })
                    .HasDatabaseName("IX_ProductVariant_Status_Active");

                entity.HasIndex(e => e.SupplierName)
                    .HasDatabaseName("IX_ProductVariant_SupplierName");

                entity.HasIndex(e => e.ImportBatch)
                    .HasDatabaseName("IX_ProductVariant_ImportBatch");
            });
        }

        // ================================================================
        // CONFIGURATION WAREHOUSE
        // ================================================================
        private void ConfigureWarehouse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("Nom de l'entrepôt");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Code court de l'entrepôt");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Warehouse");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValue("Maroc");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.Priority)
                    .HasDefaultValue(100);

                // Index
                entity.HasIndex(e => e.Code)
                    .IsUnique()
                    .HasDatabaseName("IX_Warehouse_Code_Unique");

                entity.HasIndex(e => e.Type)
                    .HasDatabaseName("IX_Warehouse_Type");

                entity.HasIndex(e => new { e.Status, e.IsActive })
                    .HasDatabaseName("IX_Warehouse_Status_Active");

                entity.HasIndex(e => e.City)
                    .HasDatabaseName("IX_Warehouse_City");
            });
        }

        // ================================================================
        // CONFIGURATION LOCATION
        // ================================================================
        private void ConfigureLocation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.WarehouseId)
                    .IsRequired()
                    .HasComment("Entrepôt auquel appartient cet emplacement");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Code unique de l'emplacement");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Shelf");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Available");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.Priority)
                    .HasDefaultValue(100);

                // Relations
                entity.HasOne(l => l.Warehouse)
                    .WithMany(w => w.Locations)
                    .HasForeignKey(l => l.WarehouseId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Location_Warehouse");

                entity.HasOne(l => l.ParentLocation)
                    .WithMany(l => l.ChildLocations)
                    .HasForeignKey(l => l.ParentLocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Location_ParentLocation");

                // Index
                entity.HasIndex(e => new { e.WarehouseId, e.Code })
                    .IsUnique()
                    .HasDatabaseName("IX_Location_Warehouse_Code_Unique");

                entity.HasIndex(e => e.Type)
                    .HasDatabaseName("IX_Location_Type");

                entity.HasIndex(e => new { e.Status, e.IsActive })
                    .HasDatabaseName("IX_Location_Status_Active");

                entity.HasIndex(e => e.Zone)
                    .HasDatabaseName("IX_Location_Zone");

                entity.HasIndex(e => e.Barcode)
                    .HasDatabaseName("IX_Location_Barcode");
            });
        }

        // ================================================================
        // CONFIGURATION STOCK
        // ================================================================
        private void ConfigureStock(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProductVariantSKU)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("SKU du produit variant");

                entity.Property(e => e.WarehouseId)
                    .IsRequired()
                    .HasComment("Entrepôt où se trouve ce stock");

                entity.Property(e => e.LocationId)
                    .IsRequired()
                    .HasComment("Emplacement concerné");

                entity.Property(e => e.MovementNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Numéro de mouvement unique");

                entity.Property(e => e.MovementType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Type de mouvement");

                entity.Property(e => e.Direction)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasComment("Direction du mouvement");

                entity.Property(e => e.Quantity)
                    .IsRequired()
                    .HasComment("Quantité du mouvement");

                entity.Property(e => e.UnitCost)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(e => e.TotalCost)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.EffectiveDate)
                    .IsRequired()
                    .HasComment("Date effective du mouvement");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Completed");

                entity.Property(e => e.IsConfirmed)
                    .HasDefaultValue(true);

                entity.Property(e => e.Method)
                    .HasMaxLength(50)
                    .HasDefaultValue("Manual");

                entity.Property(e => e.Source)
                    .HasMaxLength(50)
                    .HasDefaultValue("ERP");

                // Relations
                entity.HasOne(sm => sm.Warehouse)
                    .WithMany(w => w.StockMovements)
                    .HasForeignKey(sm => sm.WarehouseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockMovement_Warehouse");

                entity.HasOne(sm => sm.Location)
                    .WithMany(l => l.StockMovements)
                    .HasForeignKey(sm => sm.LocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockMovement_Location");

                entity.HasOne(sm => sm.SourceWarehouse)
                    .WithMany()
                    .HasForeignKey(sm => sm.SourceWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockMovement_SourceWarehouse");

                entity.HasOne(sm => sm.SourceLocation)
                    .WithMany()
                    .HasForeignKey(sm => sm.SourceLocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockMovement_SourceLocation");

                entity.HasOne(sm => sm.DestinationWarehouse)
                    .WithMany()
                    .HasForeignKey(sm => sm.DestinationWarehouseId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockMovement_DestinationWarehouse");

                entity.HasOne(sm => sm.DestinationLocation)
                    .WithMany()
                    .HasForeignKey(sm => sm.DestinationLocationId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_StockMovement_DestinationLocation");

                // Index
                entity.HasIndex(e => e.MovementNumber)
                    .IsUnique()
                    .HasDatabaseName("IX_StockMovement_Number_Unique");

                entity.HasIndex(e => e.ProductVariantSKU)
                    .HasDatabaseName("IX_StockMovement_ProductVariantSKU");

                entity.HasIndex(e => new { e.MovementType, e.Direction })
                    .HasDatabaseName("IX_StockMovement_Type_Direction");

                entity.HasIndex(e => e.EffectiveDate)
                    .HasDatabaseName("IX_StockMovement_EffectiveDate");

                entity.HasIndex(e => new { e.Status, e.IsConfirmed })
                    .HasDatabaseName("IX_StockMovement_Status_Confirmed");

                entity.HasIndex(e => e.ExternalReference)
                    .HasDatabaseName("IX_StockMovement_ExternalReference");

                entity.HasIndex(e => e.LotNumber)
                    .HasDatabaseName("IX_StockMovement_LotNumber");

                entity.HasIndex(e => new { e.WarehouseId, e.EffectiveDate })
                    .HasDatabaseName("IX_StockMovement_Warehouse_Date");
            });
        }

        // ================================================================
        // CONFIGURATION DES ENTITÉS DE RÉFÉRENCE (EXISTANTES)
        // ================================================================

        private void ConfigureProductType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Nom du type de produit");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasComment("Description du type");

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0)
                    .HasComment("Ordre d'affichage");

                entity.Property(e => e.IconUrl)
                    .HasMaxLength(500)
                    .HasComment("URL de l'icône");

                entity.Property(e => e.CategoryColor)
                    .HasMaxLength(7)
                    .HasComment("Couleur du type (hex)");

                // Index pour l'unicité du nom
                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_ProductType_Name_Unique");

                entity.HasIndex(e => e.SortOrder)
                    .HasDatabaseName("IX_ProductType_SortOrder");
            });
        }

        private void ConfigureBrand(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("Nom de la marque");

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.Website)
                    .HasMaxLength(200);

                entity.Property(e => e.ProductTypeId)
                    .IsRequired()
                    .HasComment("Type de produit de cette marque");

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Relation avec ProductType
                entity.HasOne(b => b.ProductType)
                    .WithMany(pt => pt.Brands)
                    .HasForeignKey(b => b.ProductTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Brand_ProductType");

                // Index composé pour l'unicité nom + type
                entity.HasIndex(e => new { e.Name, e.ProductTypeId })
                    .IsUnique()
                    .HasDatabaseName("IX_Brand_Name_ProductType_Unique");

                entity.HasIndex(e => e.ProductTypeId)
                    .HasDatabaseName("IX_Brand_ProductTypeId");

                entity.HasIndex(e => e.SortOrder)
                    .HasDatabaseName("IX_Brand_SortOrder");
            });
        }

        private void ConfigureModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasComment("Nom du modèle");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.ProductTypeId)
                    .IsRequired();

                entity.Property(e => e.BrandId)
                    .IsRequired();

                entity.Property(e => e.ModelReference)
                    .HasMaxLength(50);

                entity.Property(e => e.ReleaseYear);

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Relations
                entity.HasOne(m => m.ProductType)
                    .WithMany(pt => pt.Models)
                    .HasForeignKey(m => m.ProductTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Model_ProductType");

                entity.HasOne(m => m.Brand)
                    .WithMany(b => b.Models)
                    .HasForeignKey(m => m.BrandId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Model_Brand");

                // Index pour l'unicité nom + marque + type
                entity.HasIndex(e => new { e.Name, e.BrandId, e.ProductTypeId })
                    .IsUnique()
                    .HasDatabaseName("IX_Model_Name_Brand_Type_Unique");

                entity.HasIndex(e => new { e.ProductTypeId, e.BrandId })
                    .HasDatabaseName("IX_Model_Type_Brand");

                entity.HasIndex(e => e.ReleaseYear)
                    .HasDatabaseName("IX_Model_ReleaseYear");
            });
        }

        private void ConfigureColor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Nom de la couleur");

                entity.Property(e => e.HexCode)
                    .HasMaxLength(7)
                    .HasComment("Code hexadécimal de la couleur");

                entity.Property(e => e.Description)
                    .HasMaxLength(200);

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Index pour l'unicité du nom
                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Color_Name_Unique");

                entity.HasIndex(e => e.SortOrder)
                    .HasDatabaseName("IX_Color_SortOrder");
            });
        }

        private void ConfigureCondition(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Condition>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasComment("Nom de la condition");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasComment("Description détaillée de la condition");

                entity.Property(e => e.QualityPercentage)
                    .HasDefaultValue(100)
                    .HasComment("Pourcentage de qualité (0-100)");

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Index pour l'unicité du nom
                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Condition_Name_Unique");

                entity.HasIndex(e => e.SortOrder)
                    .HasDatabaseName("IX_Condition_SortOrder");

                entity.HasIndex(e => e.QualityPercentage)
                    .HasDatabaseName("IX_Condition_QualityPercentage");
            });
        }

        // ================================================================
        // CONFIGURATION PRODUCT (LEGACY - À MIGRER)
        // ================================================================
        private void ConfigureProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Propriétés de base
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("Nom commercial du produit");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasComment("Description détaillée du produit");

                // Clés étrangères
                entity.Property(e => e.ProductTypeId)
                    .IsRequired()
                    .HasComment("Type de produit");

                entity.Property(e => e.BrandId)
                    .IsRequired()
                    .HasComment("Marque du produit");

                entity.Property(e => e.ModelId)
                    .IsRequired()
                    .HasComment("Modèle du produit");

                entity.Property(e => e.ColorId)
                    .IsRequired()
                    .HasComment("Couleur du produit");

                entity.Property(e => e.ConditionId)
                    .IsRequired()
                    .HasComment("Condition du produit");

                // Prix et coûts
                entity.Property(e => e.PurchasePrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasComment("Prix d'achat unitaire");

                entity.Property(e => e.TransportCost)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0)
                    .HasComment("Frais de transport");

                entity.Property(e => e.TotalCostPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("Coût total unitaire");

                entity.Property(e => e.SellingPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasComment("Prix de vente unitaire");

                entity.Property(e => e.Margin)
                    .HasColumnType("decimal(18,2)")
                    .HasComment("Marge bénéficiaire");

                entity.Property(e => e.MarginPercentage)
                    .HasColumnType("decimal(5,2)")
                    .HasComment("Pourcentage de marge");

                // Stock
                entity.Property(e => e.Stock)
                    .IsRequired()
                    .HasDefaultValue(0)
                    .HasComment("Quantité en stock");

                entity.Property(e => e.MinStockLevel)
                    .HasDefaultValue(5)
                    .HasComment("Seuil minimum de stock");

                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ImportBatch)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Available");

                // Relations (clés étrangères)
                entity.HasOne(p => p.ProductType)
                    .WithMany(pt => pt.Products)
                    .HasForeignKey(p => p.ProductTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_ProductType");

                entity.HasOne(p => p.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(p => p.BrandId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(p => p.Model)
                    .WithMany(m => m.Products)
                    .HasForeignKey(p => p.ModelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Model");

                entity.HasOne(p => p.Color)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.ColorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Color");

                entity.HasOne(p => p.Condition)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.ConditionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Condition");

                // Index pour les performances
                entity.HasIndex(e => new { e.ProductTypeId, e.BrandId })
                    .HasDatabaseName("IX_Product_Type_Brand");

                entity.HasIndex(e => new { e.BrandId, e.ModelId })
                    .HasDatabaseName("IX_Product_Brand_Model");

                entity.HasIndex(e => new { e.Status, e.IsDeleted })
                    .HasDatabaseName("IX_Product_Status_Deleted");

                entity.HasIndex(e => new { e.Stock, e.MinStockLevel })
                    .HasDatabaseName("IX_Product_Stock_MinLevel");

                entity.HasIndex(e => e.ImportBatch)
                    .HasDatabaseName("IX_Product_ImportBatch");

                entity.HasIndex(e => e.SupplierName)
                    .HasDatabaseName("IX_Product_SupplierName");

                entity.HasIndex(e => e.IsDeleted)
                    .HasDatabaseName("IX_Product_IsDeleted");
            });
        }

        // ================================================================
        // CONFIGURATION APPLICATIONUSER (EXISTANTE)
        // ================================================================
        private void ConfigureApplicationUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
            });
        }
    }
}
                    .IsRequired()
                    .HasComment("Emplacement spécifique");

entity.Property(e => e.QuantityOnHand)
    .IsRequired()
    .HasDefaultValue(0)
    .HasComment("Quantité physiquement présente");

entity.Property(e => e.AverageCost)
    .HasColumnType("decimal(18,2)")
    .HasDefaultValue(0)
    .HasComment("Coût unitaire moyen pondéré");

entity.Property(e => e.LastCost)
    .HasColumnType("decimal(18,2)")
    .HasDefaultValue(0)
    .HasComment("Coût du dernier achat");

entity.Property(e => e.ReorderLevel)
    .HasDefaultValue(5);

entity.Property(e => e.Status)
    .HasMaxLength(50)
    .HasDefaultValue("Available");

entity.Property(e => e.IsActive)
    .HasDefaultValue(true);

// Relations
entity.HasOne(s => s.Warehouse)
    .WithMany(w => w.Stocks)
    .HasForeignKey(s => s.WarehouseId)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_Stock_Warehouse");

entity.HasOne(s => s.Location)
    .WithMany(l => l.Stocks)
    .HasForeignKey(s => s.LocationId)
    .OnDelete(DeleteBehavior.Restrict)
    .HasConstraintName("FK_Stock_Location");

// Index
entity.HasIndex(e => new { e.ProductVariantSKU, e.WarehouseId, e.LocationId })
    .IsUnique()
    .HasDatabaseName("IX_Stock_SKU_Warehouse_Location_Unique");

entity.HasIndex(e => e.ProductVariantSKU)
    .HasDatabaseName("IX_Stock_ProductVariantSKU");

entity.HasIndex(e => new { e.QuantityOnHand, e.ReorderLevel })
    .HasDatabaseName("IX_Stock_Quantity_Reorder");

entity.HasIndex(e => new { e.Status, e.IsActive })
    .HasDatabaseName("IX_Stock_Status_Active");

entity.HasIndex(e => e.LotNumber)
    .HasDatabaseName("IX_Stock_LotNumber");

entity.HasIndex(e => e.ExpiryDate)
    .HasDatabaseName("IX_Stock_ExpiryDate");
            });
        }

        // ================================================================
        // CONFIGURATION STOCKMOVEMENT
        // ================================================================
        private void ConfigureStockMovement(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<StockMovement>(entity =>
    {
    entity.HasKey(e => e.Id);

    entity.Property(e => e.ProductVariantSKU)
        .IsRequired()
        .HasMaxLength(50)
        .HasComment("SKU du produit variant");

    entity.Property(e => e.WarehouseId)
        .IsRequired()
        .HasComment("Entrepôt concerné");

    entity.Property(e => e.LocationId)