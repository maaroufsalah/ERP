// ================================================================
// DTOs MODIFIÉS POUR SUPPORTER LES NOUVELLES RELATIONS
// ================================================================
// Ces DTOs incluent les nouvelles propriétés avec IDs et relations
// pour le système de dropdowns en cascade.
// ================================================================

using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs
{
    // ================================================================
    // DTOs POUR LES TABLES DE RÉFÉRENCE
    // ================================================================

    /// <summary>
    /// DTO pour les types de produits (utilisé dans les dropdowns)
    /// </summary>
    public class ProductTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO pour les marques (utilisé dans les dropdowns)
    /// </summary>
    public class BrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty; // Pour affichage
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO pour les modèles (utilisé dans les dropdowns)
    /// </summary>
    public class ModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string? ModelReference { get; set; }
        public int? ReleaseYear { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO pour les couleurs (utilisé dans les dropdowns)
    /// </summary>
    public class ColorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? HexCode { get; set; }
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO pour les conditions (utilisé dans les dropdowns)
    /// </summary>
    public class ConditionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int QualityPercentage { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    // ================================================================
    // DTO PRODUIT COMPLET AVEC RELATIONS
    // ================================================================

    /// <summary>
    /// DTO complet du produit avec toutes les relations incluses
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

        // Propriétés calculées
        public decimal TotalValue { get; set; }
        public bool IsLowStock { get; set; }
        public int DaysInStock { get; set; }
    }

    // ================================================================
    // DTO POUR LA CRÉATION DE PRODUIT
    // ================================================================

    /// <summary>
    /// DTO pour la création d'un nouveau produit
    /// Utilise les IDs des tables de référence au lieu des chaînes
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

        // Relations - OBLIGATOIRES (IDs au lieu de chaînes)
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
        [StringLength(50, ErrorMessage = "Le stockage ne peut pas dépasser 50 caractères")]
        public string? Storage { get; set; }

        [StringLength(50, ErrorMessage = "La mémoire ne peut pas dépasser 50 caractères")]
        public string? Memory { get; set; }

        [StringLength(150, ErrorMessage = "Le processeur ne peut pas dépasser 150 caractères")]
        public string? Processor { get; set; }

        [StringLength(20, ErrorMessage = "La taille d'écran ne peut pas dépasser 20 caractères")]
        public string? ScreenSize { get; set; }

        // Fournisseur - OBLIGATOIRES
        [Required(ErrorMessage = "Le nom du fournisseur est obligatoire")]
        [StringLength(200, ErrorMessage = "Le nom du fournisseur ne peut pas dépasser 200 caractères")]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "La ville du fournisseur ne peut pas dépasser 100 caractères")]
        public string? SupplierCity { get; set; }

        [Required(ErrorMessage = "Le lot d'import est obligatoire")]
        [StringLength(50, ErrorMessage = "Le lot d'import ne peut pas dépasser 50 caractères")]
        public string ImportBatch { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de facture est obligatoire")]
        [StringLength(100, ErrorMessage = "Le numéro de facture ne peut pas dépasser 100 caractères")]
        public string InvoiceNumber { get; set; } = string.Empty;

        // Dates - OPTIONNELLES (par défaut = maintenant)
        public DateTime? PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }

        // Informations complémentaires - OPTIONNELLES
        [StringLength(500, ErrorMessage = "Les notes ne peuvent pas dépasser 500 caractères")]
        public string? Notes { get; set; }

        [StringLength(300, ErrorMessage = "Les informations de garantie ne peuvent pas dépasser 300 caractères")]
        public string? WarrantyInfo { get; set; }

        [StringLength(500, ErrorMessage = "L'URL de l'image ne peut pas dépasser 500 caractères")]
        [Url(ErrorMessage = "L'URL de l'image n'est pas valide")]
        public string? ImageUrl { get; set; }
    }

    // ================================================================
    // DTO POUR LA MODIFICATION DE PRODUIT
    // ================================================================

    /// <summary>
    /// DTO pour la modification d'un produit existant
    /// Hérite du CreateProductDto et ajoute l'ID
    /// </summary>
    public class UpdateProductDto : CreateProductDto
    {
        [Required(ErrorMessage = "L'ID du produit est obligatoire")]
        public int Id { get; set; }
    }

    // ================================================================
    // DTO ALLÉGÉ POUR LES LISTES
    // ================================================================

    /// <summary>
    /// DTO allégé pour l'affichage dans les listes de produits
    /// Contient uniquement les informations essentielles pour les performances
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
    }

    // ================================================================
    // DTOs POUR LES DROPDOWNS EN CASCADE
    // ================================================================

    /// <summary>
    /// DTO spécifique pour les réponses des dropdowns en cascade
    /// Optimisé pour les appels AJAX rapides
    /// </summary>
    public class DropdownOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO pour les réponses des dropdowns de marques (avec type parent)
    /// </summary>
    public class BrandDropdownDto : DropdownOptionDto
    {
        public int ProductTypeId { get; set; }
    }

    /// <summary>
    /// DTO pour les réponses des dropdowns de modèles (avec type et marque parents)
    /// </summary>
    public class ModelDropdownDto : DropdownOptionDto
    {
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public int? ReleaseYear { get; set; }
    }

    /// <summary>
    /// DTO pour les réponses des dropdowns de couleurs (avec code hex)
    /// </summary>
    public class ColorDropdownDto : DropdownOptionDto
    {
        public string? HexCode { get; set; }
    }

    /// <summary>
    /// DTO pour les réponses des dropdowns de conditions (avec pourcentage qualité)
    /// </summary>
    public class ConditionDropdownDto : DropdownOptionDto
    {
        public int QualityPercentage { get; set; }
    }

    // ================================================================
    // DTOs POUR LES STATISTIQUES
    // ================================================================

    /// <summary>
    /// DTO pour les statistiques globales des produits
    /// </summary>
    public class ProductStatsDto
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public decimal TotalStockValue { get; set; }
        public decimal AverageMarginPercentage { get; set; }
        public int TotalProductTypes { get; set; }
        public int TotalBrands { get; set; }
        public int TotalModels { get; set; }
    }

    /// <summary>
    /// DTO pour les statistiques par type de produit
    /// </summary>
    public class ProductTypeStatsDto
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public int LowStockCount { get; set; }
    }

    /// <summary>
    /// DTO pour les statistiques par marque
    /// </summary>
    public class BrandStatsDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ProductTypeName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageMarginPercentage { get; set; }
    }

    // ================================================================
    // DTOs POUR LES FILTRES AVANCÉS
    // ================================================================

    /// <summary>
    /// DTO pour les filtres de recherche de produits
    /// Permet des recherches complexes avec plusieurs critères
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
    /// DTO pour les résultats paginés
    /// </summary>
    /// <typeparam name="T">Type des éléments de la liste</typeparam>
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    // ================================================================
    // DTOs POUR LES OPÉRATIONS EN LOT
    // ================================================================

    /// <summary>
    /// DTO pour les opérations de mise à jour en lot
    /// </summary>
    public class BulkUpdateDto
    {
        public List<int> ProductIds { get; set; } = new List<int>();
        public string? NewStatus { get; set; }
        public decimal? NewPrice { get; set; }
        public int? StockAdjustment { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO pour les résultats d'opérations en lot
    /// </summary>
    public class BulkOperationResultDto
    {
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<int> ProcessedIds { get; set; } = new List<int>();
    }
}