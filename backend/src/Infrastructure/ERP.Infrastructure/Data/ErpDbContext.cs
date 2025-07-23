using ERP.Domain.Entities;
using ERP.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Data
{
    public class ErpDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration pour Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Basic Information
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);

                // Pricing & Costs
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TransportCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalCostPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SellingPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Margin).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MarginPercentage).HasColumnType("decimal(5,2)");

                // Technical Specifications
                entity.Property(e => e.Storage).HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.Memory).HasMaxLength(50);
                entity.Property(e => e.Processor).HasMaxLength(150);
                entity.Property(e => e.ScreenSize).HasMaxLength(20);

                // Condition & Status
                entity.Property(e => e.Condition).HasMaxLength(50);
                entity.Property(e => e.ConditionGrade).HasMaxLength(10);
                entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Available");

                // Import Information
                entity.Property(e => e.SupplierName).HasMaxLength(200);
                entity.Property(e => e.SupplierCity).HasMaxLength(100);
                entity.Property(e => e.ImportBatch).HasMaxLength(50);
                entity.Property(e => e.InvoiceNumber).HasMaxLength(100);

                // Optional fields
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.WarrantyInfo).HasMaxLength(300);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.ImagesUrls).HasMaxLength(2000);
                entity.Property(e => e.DocumentsUrls).HasMaxLength(2000);

                // Audit fields
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                entity.Property(e => e.DeletedBy).HasMaxLength(100);

                // Indexes for better performance
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Brand);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ImportBatch);
                entity.HasIndex(e => e.SupplierName);
                entity.HasIndex(e => e.FlagDelete);
                entity.HasIndex(e => new { e.Brand, e.Model });
                entity.HasIndex(e => new { e.Category, e.Status });
            });

            // Configuration pour ApplicationUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(500);
            });
        }
    }
}