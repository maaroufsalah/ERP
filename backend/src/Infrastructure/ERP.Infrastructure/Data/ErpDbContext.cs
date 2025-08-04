// ================================================================
// DBCONTEXT MODIFIÉ POUR LES NOUVELLES ENTITÉS
// ================================================================
// Mise à jour du contexte avec les nouvelles entités Products
// ================================================================

using ERP.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities.Product;

namespace ERP.Infrastructure.Data
{
    /// <summary>
    /// Contexte de base de données Entity Framework pour l'ERP
    /// Version mise à jour avec les entités de référence
    /// </summary>
    public class ErpDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
        {
        }

        // ================================================================
        // DBSETS - TABLES DU MODULE PRODUCTS
        // ================================================================

        /// <summary>
        /// Table des produits (table principale)
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
            // CONFIGURATION DES ENTITÉS DE RÉFÉRENCE
            // ================================================================

            ConfigureProductType(modelBuilder);
            ConfigureBrand(modelBuilder);
            ConfigureModel(modelBuilder);
            ConfigureColor(modelBuilder);
            ConfigureCondition(modelBuilder);
            ConfigureProduct(modelBuilder);
            ConfigureApplicationUser(modelBuilder);
        }

        // ================================================================
        // CONFIGURATION PRODUCTTYPE
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

        // ================================================================
        // CONFIGURATION BRAND
        // ================================================================
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

        // ================================================================
        // CONFIGURATION MODEL
        // ================================================================
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

        // ================================================================
        // CONFIGURATION COLOR
        // ================================================================
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

        // ================================================================
        // CONFIGURATION CONDITION
        // ================================================================
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
        // CONFIGURATION PRODUCT
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

                // Caractéristiques techniques
                entity.Property(e => e.Storage)
                    .HasMaxLength(50);

                entity.Property(e => e.Memory)
                    .HasMaxLength(50);

                entity.Property(e => e.Processor)
                    .HasMaxLength(150);

                entity.Property(e => e.ScreenSize)
                    .HasMaxLength(20);

                // Fournisseur
                entity.Property(e => e.SupplierName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.SupplierCity)
                    .HasMaxLength(100);

                entity.Property(e => e.ImportBatch)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.InvoiceNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                // Statut
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Available");

                // Informations complémentaires
                entity.Property(e => e.Notes)
                    .HasMaxLength(500);

                entity.Property(e => e.WarrantyInfo)
                    .HasMaxLength(300);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);

                entity.Property(e => e.ImagesUrls)
                    .HasMaxLength(2000);

                entity.Property(e => e.DocumentsUrls)
                    .HasMaxLength(2000);

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