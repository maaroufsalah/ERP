// ================================================================
// DTOs COMPLETS POUR PRODUCT VARIANT - Application/DTOs/Product
// ================================================================
// Support complet des attributs dynamiques et calcul de prix
// ================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Product
{
    /// <summary>
    /// DTO complet pour ProductVariant avec tous les détails
    /// </summary>
    public class ProductVariantDetailDto
    {
        // ===== IDENTIFICATION =====
        public int Id { get; set; }
        public int ProductMasterId { get; set; }
        public string ProductMasterName { get; set; } = string.Empty;
        public string ProductMasterReference { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public string? FullName { get; set; }

        // ===== CLASSIFICATION =====
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public int? ModelId { get; set; }
        public string? ModelName { get; set; }

        // ===== ATTRIBUTS DYNAMIQUES =====
        public List<AttributeValueDto> Attributes { get; set; } = new();

        // Compatibilité ancien système
        public int? ColorId { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
        public string? Size { get; set; }
        public string? Storage { get; set; }
        public string? Memory { get; set; }

        // ===== PRICING DÉTAILLÉ =====
        public decimal PurchasePrice { get; set; }
        public decimal TransportCost { get; set; }
        public decimal AdditionalCosts { get; set; }
        public decimal TotalCostPrice { get; set; } // Calculé
        public decimal SellingPrice { get; set; }
        public decimal? MSRP { get; set; }
        public decimal Margin { get; set; } // Calculé
        public decimal MarginPercentage { get; set; } // Calculé
        public bool UseTransportInCost { get; set; }

        // ===== CONDITION =====
        public int ConditionId { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public int ConditionQualityPercentage { get; set; }
        public string? ConditionNotes { get; set; }
        public int? QualityScore { get; set; }

        // ===== FOURNISSEUR =====
        public string SupplierName { get; set; } = string.Empty;
        public string? SupplierCode { get; set; }
        public string? SupplierReference { get; set; }
        public string? CountryOfOrigin { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? ManufactureDate { get; set; }

        // ===== TRAÇABILITÉ =====
        public string? ImportBatch { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? CustomsDeclaration { get; set; }

        // ===== GARANTIE =====
        public string? WarrantyInfo { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public bool IsWarrantyValid { get; set; }
        public int? WarrantyDaysRemaining { get; set; }

        // ===== CARACTÉRISTIQUES PHYSIQUES =====
        public decimal? Weight { get; set; }
        public string? Dimensions { get; set; }
        public string? PackagingInfo { get; set; }

        // ===== STATUT =====
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime? LaunchDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }

        // ===== MÉDIAS =====
        public string? PrimaryImageUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<string> DocumentUrls { get; set; } = new();

        // ===== STOCK (depuis Inventory Context) =====
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int ReservedStock { get; set; }
        public bool IsInStock { get; set; }
        public bool IsLowStock { get; set; }
        public List<StockLocationDto> StockByLocation { get; set; } = new();

        // ===== MÉTADONNÉES =====
        public string? Slug { get; set; }
        public string? Keywords { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // ===== PROPRIÉTÉS CALCULÉES =====
        public int AgeInDays { get; set; }
        public decimal TotalValue { get; set; } // Stock * SellingPrice
        public decimal TotalCostValue { get; set; } // Stock * TotalCostPrice
        public string DisplayName { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO pour création de ProductVariant avec attributs dynamiques
    /// </summary>
    public class CreateProductVariantDto
    {
        [Required(ErrorMessage = "Le produit master est obligatoire")]
        public int ProductMasterId { get; set; }

        [Required(ErrorMessage = "Le SKU est obligatoire")]
        [StringLength(50)]
        public string SKU { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Barcode { get; set; }

        [Required(ErrorMessage = "Le nom de la variante est obligatoire")]
        [StringLength(200)]
        public string VariantName { get; set; } = string.Empty;

        // ===== ATTRIBUTS DYNAMIQUES =====
        public List<CreateAttributeValueDto> Attributes { get; set; } = new();

        // ===== PRICING =====
        [Required(ErrorMessage = "Le prix d'achat est obligatoire")]
        [Range(0.01, double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TransportCost { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal AdditionalCosts { get; set; } = 0;

        [Required(ErrorMessage = "Le prix de vente est obligatoire")]
        [Range(0.01, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        public decimal? MSRP { get; set; }
        public bool UseTransportInCost { get; set; } = true;

        // ===== CONDITION =====
        [Required(ErrorMessage = "La condition est obligatoire")]
        public int ConditionId { get; set; }

        [StringLength(1000)]
        public string? ConditionNotes { get; set; }

        [Range(0, 100)]
        public int? QualityScore { get; set; }

        // ===== FOURNISSEUR =====
        [Required(ErrorMessage = "Le fournisseur est obligatoire")]
        [StringLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SupplierCode { get; set; }

        [StringLength(100)]
        public string? SupplierReference { get; set; }

        [StringLength(100)]
        public string? CountryOfOrigin { get; set; }

        [StringLength(100)]
        public string? SerialNumber { get; set; }

        public DateTime? ManufactureDate { get; set; }

        // ===== TRAÇABILITÉ =====
        [StringLength(50)]
        public string? ImportBatch { get; set; }

        [StringLength(100)]
        public string? InvoiceNumber { get; set; }

        public DateTime? PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }

        // ===== GARANTIE =====
        [StringLength(300)]
        public string? WarrantyInfo { get; set; }

        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyEndDate { get; set; }

        // ===== CARACTÉRISTIQUES =====
        public decimal? Weight { get; set; }

        [StringLength(100)]
        public string? Dimensions { get; set; }

        [StringLength(200)]
        public string? PackagingInfo { get; set; }

        // ===== MÉDIAS =====
        [Url]
        public string? PrimaryImageUrl { get; set; }

        public List<string>? ImageUrls { get; set; }

        // ===== STOCK INITIAL (optionnel) =====
        public int? InitialStock { get; set; }
        public int? WarehouseId { get; set; }
        public int? LocationId { get; set; }
    }

    /// <summary>
    /// DTO pour valeur d'attribut
    /// </summary>
    public class AttributeValueDto
    {
        public int AttributeDefinitionId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public string AttributeCode { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string? Value { get; set; }
        public string? DisplayValue { get; set; }
        public int? AttributeValueId { get; set; } // Pour les listes
        public string? UnitOfMeasure { get; set; }
    }

    /// <summary>
    /// DTO pour création d'attribut
    /// </summary>
    public class CreateAttributeValueDto
    {
        [Required]
        public int AttributeDefinitionId { get; set; }

        public string? StringValue { get; set; }
        public decimal? NumericValue { get; set; }
        public DateTime? DateValue { get; set; }
        public bool? BooleanValue { get; set; }
        public int? AttributeValueId { get; set; } // Pour les listes
        public List<int>? MultipleValueIds { get; set; } // Pour valeurs multiples
    }

    /// <summary>
    /// DTO pour génération automatique de variantes
    /// </summary>
    public class GenerateVariantsDto
    {
        [Required]
        public int ProductMasterId { get; set; }

        [Required]
        public List<VariantGenerationAttributeDto> Attributes { get; set; } = new();

        // Configuration de génération
        public bool AutoGenerateSKU { get; set; } = true;
        public string? SKUPattern { get; set; } // {MASTER}-{COLOR}-{SIZE}

        public bool AutoGenerateName { get; set; } = true;
        public string? NamePattern { get; set; } // {MASTER} {COLOR} Taille {SIZE}

        // Pricing
        public bool UseBasePricing { get; set; } = true;
        public decimal? BasePurchasePrice { get; set; }
        public decimal? BaseSellingPrice { get; set; }

        // Ajustements de prix par attribut
        public List<PriceAdjustmentRuleDto>? PriceAdjustments { get; set; }

        // Fournisseur par défaut
        [Required]
        public string SupplierName { get; set; } = string.Empty;
        public string? CountryOfOrigin { get; set; }

        // Condition par défaut
        [Required]
        public int ConditionId { get; set; }
    }

    /// <summary>
    /// DTO pour attribut de génération
    /// </summary>
    public class VariantGenerationAttributeDto
    {
        [Required]
        public int AttributeDefinitionId { get; set; }

        [Required]
        public List<int> SelectedValueIds { get; set; } = new();
    }

    /// <summary>
    /// DTO pour règle d'ajustement de prix
    /// </summary>
    public class PriceAdjustmentRuleDto
    {
        public int AttributeDefinitionId { get; set; }
        public int AttributeValueId { get; set; }
        public decimal? PriceAdjustment { get; set; } // Montant fixe
        public decimal? PriceMultiplier { get; set; } // Multiplicateur
    }

    /// <summary>
    /// DTO pour stock par emplacement
    /// </summary>
    public class StockLocationDto
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public int QuantityOnHand { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public decimal AverageCost { get; set; }
        public DateTime? LastMovement { get; set; }
    }

    /// <summary>
    /// DTO pour import en masse avec attributs dynamiques
    /// </summary>
    public class ImportProductVariantDto
    {
        // Identification
        [Required]
        public string ProductMasterReference { get; set; } = string.Empty;

        [Required]
        public string SKU { get; set; } = string.Empty;

        public string? Barcode { get; set; }

        [Required]
        public string VariantName { get; set; } = string.Empty;

        // Attributs sous forme de dictionnaire
        public Dictionary<string, string> Attributes { get; set; } = new();

        // Pricing
        [Required]
        public decimal PurchasePrice { get; set; }

        public decimal TransportCost { get; set; } = 0;
        public decimal AdditionalCosts { get; set; } = 0;

        [Required]
        public decimal SellingPrice { get; set; }

        // Condition
        [Required]
        public string ConditionName { get; set; } = string.Empty;

        // Fournisseur
        [Required]
        public string SupplierName { get; set; } = string.Empty;

        public string? CountryOfOrigin { get; set; }
        public string? ImportBatch { get; set; }
        public string? InvoiceNumber { get; set; }

        // Stock initial
        public int? InitialStock { get; set; }
        public string? WarehouseCode { get; set; }
        public string? LocationCode { get; set; }

        // Validation
        public int RowNumber { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public bool IsValid { get; set; } = true;
    }
}