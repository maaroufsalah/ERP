// ================================================================
// DTOs POUR PRODUCT MASTER - Product/ProductMasterDtos.cs
// ================================================================
// DTOs pour la gestion des produits génériques (Catalog Context)
// ================================================================

using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Product
{
    /// <summary>
    /// DTO pour ProductMaster (produit générique)
    /// </summary>
    public class ProductMasterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;

        // Relations
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        // Classification
        public string? Category { get; set; }
        public string? Tags { get; set; }

        // Caractéristiques communes
        public string? CommonSpecifications { get; set; }
        public string? Dimensions { get; set; }
        public int? Weight { get; set; }

        // Statut
        public string Status { get; set; } = string.Empty;
        public DateTime? LaunchDate { get; set; }
        public DateTime? EndOfLifeDate { get; set; }
        public bool IsActive { get; set; }

        // Médias
        public string? PrimaryImageUrl { get; set; }
        public string? ImageUrls { get; set; }
        public string? DocumentUrls { get; set; }

        // Marketing
        public string? MarketingSummary { get; set; }
        public string? KeyFeatures { get; set; }
        public string? TargetAudience { get; set; }

        // SEO
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? SeoKeywords { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Multi-tenant
        public int TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;

        // Propriétés calculées
        public int ActiveVariantsCount { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool HasStockVariants { get; set; }
        public int? AgeInDays { get; set; }
    }

    /// <summary>
    /// DTO pour création de ProductMaster
    /// </summary>
    public class CreateProductMasterDto
    {
        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(2000, ErrorMessage = "La description ne peut pas dépasser 2000 caractères")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de référence est obligatoire")]
        [StringLength(50, ErrorMessage = "La référence ne peut pas dépasser 50 caractères")]
        public string ReferenceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type de produit est obligatoire")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "La marque est obligatoire")]
        public int BrandId { get; set; }

        [StringLength(100)]
        public string? Category { get; set; }

        [StringLength(1000)]
        public string? Tags { get; set; }

        [StringLength(3000)]
        public string? CommonSpecifications { get; set; }

        [StringLength(100)]
        public string? Dimensions { get; set; }

        public int? Weight { get; set; }

        public DateTime? LaunchDate { get; set; }
        public DateTime? EndOfLifeDate { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        public string? PrimaryImageUrl { get; set; }

        [StringLength(2000)]
        public string? ImageUrls { get; set; }

        [StringLength(2000)]
        public string? DocumentUrls { get; set; }

        [StringLength(500)]
        public string? MarketingSummary { get; set; }

        [StringLength(1000)]
        public string? KeyFeatures { get; set; }

        [StringLength(200)]
        public string? TargetAudience { get; set; }

        [StringLength(200)]
        public string? SeoTitle { get; set; }

        [StringLength(500)]
        public string? SeoDescription { get; set; }

        [StringLength(500)]
        public string? SeoKeywords { get; set; }
    }

    /// <summary>
    /// DTO pour modification de ProductMaster
    /// </summary>
    public class UpdateProductMasterDto : CreateProductMasterDto
    {
        [Required(ErrorMessage = "L'ID du produit master est obligatoire")]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Status { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// DTO allégé pour listes de ProductMaster
    /// </summary>
    public class ProductMasterForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LaunchDate { get; set; }
        public string? PrimaryImageUrl { get; set; }
        public int ActiveVariantsCount { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TenantId { get; set; }
    }

    /// <summary>
    /// DTO pour filtrer les ProductMaster
    /// </summary>
    public class ProductMasterFilterDto
    {
        public string? SearchTerm { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
        public string? Status { get; set; }
        public bool? ActiveOnly { get; set; }
        public string? Category { get; set; }
        public DateTime? LaunchDateFrom { get; set; }
        public DateTime? LaunchDateTo { get; set; }
        public bool? HasVariants { get; set; }

        // Pagination
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    /// <summary>
    /// DTO pour opérations en lot sur ProductMaster
    /// </summary>
    public class BulkProductMasterUpdateDto
    {
        public List<int> ProductMasterIds { get; set; } = new List<int>();
        public string? NewStatus { get; set; }
        public string? NewCategory { get; set; }
        public bool? SetActive { get; set; }
        public DateTime? LaunchDate { get; set; }
        public string? Notes { get; set; }
    }
}