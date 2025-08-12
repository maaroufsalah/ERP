// ================================================================
// ENTITÉ TENANT - ERP.Domain.Entities.Shared
// ================================================================
// Gestion multi-tenant avec configuration spécifique par client
// ================================================================

using ERP.Domain.Entities.Catalog;
using ERP.Domain.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Domain.Entities.Shared
{
    /// <summary>
    /// Tenant - Organisation cliente utilisant l'ERP
    /// </summary>
    public class Tenant
    {
        public int Id { get; set; }

        // ===== IDENTIFICATION =====
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // Nom de l'entreprise

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // Code unique (ex: COMP001)

        [StringLength(200)]
        public string? LegalName { get; set; } // Raison sociale

        [StringLength(50)]
        public string? TaxId { get; set; } // N° TVA/SIRET

        [StringLength(50)]
        public string? RegistrationNumber { get; set; } // N° registre commerce

        // ===== CONTACT =====
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(20)]
        public string? Fax { get; set; }

        [StringLength(200)]
        public string? Website { get; set; }

        // ===== ADRESSE =====
        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(100)]
        public string? Country { get; set; } = "Maroc";

        // ===== CONFIGURATION BUSINESS =====
        [StringLength(50)]
        public string BusinessType { get; set; } = "Retail"; // Retail, Wholesale, Both

        [StringLength(100)]
        public string? Industry { get; set; } // Electronics, Fashion, etc.

        [StringLength(3)]
        public string Currency { get; set; } = "MAD"; // Devise principale

        [StringLength(50)]
        public string? SecondaryCountries { get; set; } // Pays d'opération

        public bool UseMultiCurrency { get; set; } = false;

        // ===== CONFIGURATION PRODUITS =====
        public bool UseProductVariants { get; set; } = true;
        public bool UseDynamicAttributes { get; set; } = true;
        public bool UseProductFamilies { get; set; } = false;
        public bool AutoGenerateSKU { get; set; } = true;
        public bool UseBarcode { get; set; } = true;

        [StringLength(100)]
        public string? SKUPattern { get; set; } = "{TYPE}{BRAND}{SEQUENCE}"; // Pattern SKU

        // Configuration des attributs par défaut
        [StringLength(500)]
        public string? DefaultAttributes { get; set; } // JSON: ["COLOR", "SIZE", "WEIGHT"]

        // ===== CONFIGURATION PRICING =====
        public bool IncludeTransportInCost { get; set; } = true;
        public bool UseMultiplePriceLists { get; set; } = false;

        [Column(TypeName = "decimal(5,2)")]
        public decimal DefaultMarginPercentage { get; set; } = 30; // Marge par défaut

        [Column(TypeName = "decimal(5,2)")]
        public decimal DefaultVATRate { get; set; } = 20; // TVA par défaut

        [StringLength(50)]
        public string PricingStrategy { get; set; } = "CostPlus"; // CostPlus, MarketBased

        // ===== CONFIGURATION STOCK =====
        public bool UseMultiWarehouse { get; set; } = false;
        public bool UseLocationTracking { get; set; } = true;
        public bool UseLotTracking { get; set; } = false;
        public bool UseSerialNumbers { get; set; } = false;
        public bool UseExpiryDates { get; set; } = false;

        [StringLength(50)]
        public string StockValuationMethod { get; set; } = "FIFO"; // FIFO, LIFO, AVG

        public int DefaultMinStockLevel { get; set; } = 5;
        public int DefaultReorderPoint { get; set; } = 10;

        // ===== CONFIGURATION FOURNISSEURS =====
        public bool RequireSupplierCode { get; set; } = false;
        public bool TrackSupplierPerformance { get; set; } = true;
        public int DefaultLeadTimeDays { get; set; } = 7;

        // ===== ABONNEMENT & LIMITES =====
        [StringLength(50)]
        public string SubscriptionPlan { get; set; } = "Standard"; // Free, Standard, Premium, Enterprise

        public DateTime SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }

        public int MaxUsers { get; set; } = 5;
        public int MaxProducts { get; set; } = 1000;
        public int MaxWarehouses { get; set; } = 1;
        public int MaxMonthlyTransactions { get; set; } = 10000;

        public long MaxStorageGB { get; set; } = 10;
        public long CurrentStorageUsedMB { get; set; } = 0;

        // ===== PERSONNALISATION =====
        [StringLength(500)]
        public string? LogoUrl { get; set; }

        [StringLength(7)]
        public string? PrimaryColor { get; set; } = "#1976d2";

        [StringLength(7)]
        public string? SecondaryColor { get; set; } = "#dc004e";

        [StringLength(50)]
        public string Theme { get; set; } = "Light"; // Light, Dark, Auto

        [StringLength(10)]
        public string DateFormat { get; set; } = "DD/MM/YYYY";

        [StringLength(10)]
        public string TimeZone { get; set; } = "GMT+1";

        [StringLength(5)]
        public string Language { get; set; } = "fr-FR";

        // ===== INTÉGRATIONS =====
        public bool UseEmailIntegration { get; set; } = true;
        public bool UseSMSIntegration { get; set; } = false;
        public bool UseAccountingIntegration { get; set; } = false;
        public bool UseEcommerceIntegration { get; set; } = false;

        [StringLength(2000)]
        public string? IntegrationSettings { get; set; } // JSON configuration

        // ===== SÉCURITÉ =====
        public bool RequireTwoFactor { get; set; } = false;
        public int PasswordExpiryDays { get; set; } = 90;
        public int SessionTimeoutMinutes { get; set; } = 30;

        [StringLength(500)]
        public string? AllowedIPAddresses { get; set; } // Liste IP autorisées

        // ===== MODULES ACTIVÉS =====
        public bool ModuleSales { get; set; } = true;
        public bool ModulePurchases { get; set; } = true;
        public bool ModuleInventory { get; set; } = true;
        public bool ModuleAccounting { get; set; } = false;
        public bool ModuleCRM { get; set; } = false;
        public bool ModuleHR { get; set; } = false;
        public bool ModuleProduction { get; set; } = false;
        public bool ModuleProjects { get; set; } = false;

        // ===== STATUT =====
        [StringLength(50)]
        public string Status { get; set; } = "Active"; // Active, Suspended, Expired, Cancelled

        public bool IsActive { get; set; } = true;
        public bool IsTrial { get; set; } = false;
        public DateTime? TrialEndDate { get; set; }

        // ===== RELATIONS =====
        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<ProductMaster> ProductMasters { get; set; } = new List<ProductMaster>();
        public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

        // ===== AUDIT =====
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // ===== SOFT DELETE =====
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        // ===== MÉTHODES HELPER =====
        public bool IsSubscriptionValid()
        {
            if (Status != "Active" || !IsActive) return false;
            if (IsTrial && TrialEndDate.HasValue) return TrialEndDate.Value > DateTime.UtcNow;
            if (SubscriptionEndDate.HasValue) return SubscriptionEndDate.Value > DateTime.UtcNow;
            return true;
        }

        public bool CanAddMoreUsers(int currentUserCount)
        {
            return currentUserCount < MaxUsers;
        }

        public bool CanAddMoreProducts(int currentProductCount)
        {
            return currentProductCount < MaxProducts;
        }

        public Dictionary<string, object> GetConfiguration()
        {
            return new Dictionary<string, object>
            {
                ["BusinessType"] = BusinessType,
                ["Currency"] = Currency,
                ["UseProductVariants"] = UseProductVariants,
                ["UseDynamicAttributes"] = UseDynamicAttributes,
                ["IncludeTransportInCost"] = IncludeTransportInCost,
                ["DefaultMarginPercentage"] = DefaultMarginPercentage,
                ["StockValuationMethod"] = StockValuationMethod,
                ["DateFormat"] = DateFormat,
                ["Language"] = Language
            };
        }
    }
}