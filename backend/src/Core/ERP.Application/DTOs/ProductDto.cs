// ================================================================
// DTOs POUR PRODUITS LEGACY - Product/LegacyProductDtos.cs
// ================================================================
// DTOs pour maintenir la compatibilité avec l'ancien système
// ================================================================

using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Product
{
    /// <summary>
    /// DTO complet du produit legacy avec toutes les relations incluses
    /// Utilisé pour l'affichage dans les listes et détails
    /// </summary>
    public class ProductDto
    {
        // Informations de base
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // IDs des relations (pour les modifications)
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public int ConditionId { get; set; }

        // Informations des relations (pour l'affichage)
        public string ProductTypeName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public string? ColorHexCode { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public int ConditionQualityPercentage { get; set; }

        // Prix et coûts
        public decimal PurchasePrice { get; set; }
        public decimal TransportCost { get; set; }
        public decimal TotalCostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPercentage { get; set; }

        // Stock
        public int Stock { get; set; }
        public int MinStockLevel { get; set; }

        // Caractéristiques techniques
        public string? Storage { get; set; }
        public string? Memory { get; set; }
        public string? Processor { get; set; }
        public string? ScreenSize { get; set; }

        // Informations fournisseur
        public string SupplierName { get; set; } = string.Empty;
        public string? SupplierCity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ImportBatch { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;

        // Statut
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Informations complémentaires
        public string? Notes { get; set; }
        public string? WarrantyInfo { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImagesUrls { get; set; }
        public string? DocumentsUrls { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Multi-tenant
        public int TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;

        // Propriétés calculées
        public decimal TotalValue { get; set; }
        public bool IsLowStock { get; set; }
        public int DaysInStock { get; set; }
    }

    /// <summary>
    /// DTO pour la création d'un nouveau produit legacy
    /// </summary>
    public class CreateProductDto
    {
        // Informations de base - OBLIGATOIRES
        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string Description { get; set; } = string.Empty;

        // Relations - OBLIGATOIRES
        [Required(ErrorMessage = "Le type de produit est obligatoire")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "La marque est obligatoire")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Le modèle est obligatoire")]
        public int ModelId { get; set; }

        [Required(ErrorMessage = "La couleur est obligatoire")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "La condition est obligatoire")]
        public int ConditionId { get; set; }

        // Prix - OBLIGATOIRES
        [Required(ErrorMessage = "Le prix d'achat est obligatoire")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix d'achat doit être supérieur à 0")]
        public decimal PurchasePrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Les frais de transport ne peuvent pas être négatifs")]
        public decimal TransportCost { get; set; } = 0;

        [Required(ErrorMessage = "Le prix de vente est obligatoire")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix de vente doit être supérieur à 0")]
        public decimal SellingPrice { get; set; }

        // Stock - OBLIGATOIRES
        [Required(ErrorMessage = "La quantité en stock est obligatoire")]
        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le stock minimum ne peut pas être négatif")]
        public int MinStockLevel { get; set; } = 1;

        // Caractéristiques techniques - OPTIONNELLES
        [StringLength(50)]
        public string? Storage { get; set; }

        [StringLength(50)]
        public string? Memory { get; set; }

        [StringLength(150)]
        public string? Processor { get; set; }

        [StringLength(20)]
        public string? ScreenSize { get; set; }

        // Fournisseur - OBLIGATOIRES
        [Required(ErrorMessage = "Le nom du fournisseur est obligatoire")]
        [StringLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SupplierCity { get; set; }

        [Required(ErrorMessage = "Le lot d'import est obligatoire")]
        [StringLength(50)]
        public string ImportBatch { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de facture est obligatoire")]
        [StringLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        // Dates - OPTIONNELLES
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }

        // Informations complémentaires - OPTIONNELLES
        [StringLength(500)]
        public string? Notes { get; set; }

        [StringLength(300)]
        public string? WarrantyInfo { get; set; }

        [StringLength(500)]
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        public string? ImageUrl { get; set; }
    }

    /// <summary>
    /// DTO pour la modification d'un produit existant legacy
    /// </summary>
    public class UpdateProductDto : CreateProductDto
    {
        [Required(ErrorMessage = "L'ID du produit est obligatoire")]
        public int Id { get; set; }
    }

    /// <summary>
    /// DTO allégé pour l'affichage dans les listes de produits legacy
    /// </summary>
    public class ProductForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public string? ColorHexCode { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MarginPercentage { get; set; }
        public int Stock { get; set; }
        public bool IsLowStock { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TenantId { get; set; }
    }

    /// <summary>
    /// DTO pour les filtres de recherche de produits legacy
    /// </summary>
    public class ProductFilterDto
    {
        // Filtres textuels
        public string? SearchTerm { get; set; }
        public string? SupplierName { get; set; }
        public string? ImportBatch { get; set; }

        // Filtres par ID (relations)
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public int? ColorId { get; set; }
        public int? ConditionId { get; set; }

        // Filtres par prix
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinMarginPercentage { get; set; }

        // Filtres par stock
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public bool? LowStockOnly { get; set; }

        // Filtres par statut
        public string? Status { get; set; }
        public bool? ActiveOnly { get; set; }

        // Filtres par dates
        public DateTime? PurchaseDateFrom { get; set; }
        public DateTime? PurchaseDateTo { get; set; }
        public DateTime? ArrivalDateFrom { get; set; }
        public DateTime? ArrivalDateTo { get; set; }

        // Pagination et tri
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }

    /// <summary>
    /// DTO pour les opérations de mise à jour en lot (Legacy Products)
    /// </summary>
    public class BulkUpdateDto
    {
        public List<int> ProductIds { get; set; } = new List<int>();
        public string? NewStatus { get; set; }
        public decimal? NewPrice { get; set; }
        public int? StockAdjustment { get; set; }
        public string? Notes { get; set; }
    }
}