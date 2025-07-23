namespace ERP.Application.DTOs
{
    /// <summary>
    /// DTO pour l'affichage complet d'un produit
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        // Pricing & Costs
        public decimal PurchasePrice { get; set; }
        public decimal TransportCost { get; set; }
        public decimal TotalCostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPercentage { get; set; }

        // Stock & Condition
        public int Stock { get; set; }
        public int MinStockLevel { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string ConditionGrade { get; set; } = string.Empty;

        // Technical Specifications
        public string Storage { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Memory { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string ScreenSize { get; set; } = string.Empty;

        // Import Information
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierCity { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ImportBatch { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;

        // Product Status
        public string Status { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public string? WarrantyInfo { get; set; }

        // Images & Documentation
        public string? ImageUrl { get; set; }
        public string? ImagesUrls { get; set; }
        public string? DocumentsUrls { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Calculated Properties
        public decimal TotalValue { get; set; }
        public bool IsLowStock { get; set; }
        public int DaysInStock { get; set; }
    }

    /// <summary>
    /// DTO pour la liste des produits (version allégée)
    /// </summary>
    public class ProductForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string ConditionGrade { get; set; } = string.Empty;

        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPercentage { get; set; }

        public int Stock { get; set; }
        public string Status { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierCity { get; set; } = string.Empty;
        public DateTime? ArrivalDate { get; set; }

        public string? ImageUrl { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsLowStock { get; set; }
        public int DaysInStock { get; set; }
    }

    /// <summary>
    /// DTO pour la création d'un produit
    /// </summary>
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        // Pricing & Costs
        public decimal PurchasePrice { get; set; }
        public decimal TransportCost { get; set; }
        public decimal SellingPrice { get; set; }

        // Stock & Condition
        public int Stock { get; set; }
        public int MinStockLevel { get; set; } = 5;
        public string Condition { get; set; } = string.Empty;
        public string ConditionGrade { get; set; } = string.Empty;

        // Technical Specifications
        public string Storage { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Memory { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string ScreenSize { get; set; } = string.Empty;

        // Import Information
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierCity { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ImportBatch { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;

        // Product Status
        public string Status { get; set; } = "Available";
        public string? Notes { get; set; }
        public string? WarrantyInfo { get; set; }

        // Images & Documentation
        public string? ImageUrl { get; set; }
        public string? ImagesUrls { get; set; }
        public string? DocumentsUrls { get; set; }
    }

    /// <summary>
    /// DTO pour la mise à jour d'un produit
    /// </summary>
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        // Pricing & Costs
        public decimal PurchasePrice { get; set; }
        public decimal TransportCost { get; set; }
        public decimal SellingPrice { get; set; }

        // Stock & Condition
        public int Stock { get; set; }
        public int MinStockLevel { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string ConditionGrade { get; set; } = string.Empty;

        // Technical Specifications
        public string Storage { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Memory { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public string ScreenSize { get; set; } = string.Empty;

        // Import Information
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierCity { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ImportBatch { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;

        // Product Status
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? WarrantyInfo { get; set; }

        // Images & Documentation
        public string? ImageUrl { get; set; }
        public string? ImagesUrls { get; set; }
        public string? DocumentsUrls { get; set; }
    }

    /// <summary>
    /// DTO pour les statistiques des produits
    /// </summary>
    public class ProductStatsDto
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public decimal TotalStockValue { get; set; }
        public decimal TotalMargin { get; set; }
        public decimal AverageMarginPercentage { get; set; }
        public List<CategoryStatsDto> CategoryStats { get; set; } = new();
        public List<SupplierStatsDto> SupplierStats { get; set; } = new();
    }

    /// <summary>
    /// DTO pour les statistiques par catégorie
    /// </summary>
    public class CategoryStatsDto
    {
        public string Category { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
    }

    /// <summary>
    /// DTO pour les statistiques par fournisseur
    /// </summary>
    public class SupplierStatsDto
    {
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierCity { get; set; } = string.Empty;
        public int ProductCount { get; set; }
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalSellingValue { get; set; }
        public decimal TotalMargin { get; set; }
    }

    /// <summary>
    /// DTO pour les filtres de recherche
    /// </summary>
    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? Condition { get; set; }
        public string? Status { get; set; }
        public string? SupplierName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsLowStock { get; set; }
        public DateTime? PurchaseDateFrom { get; set; }
        public DateTime? PurchaseDateTo { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public string SortDirection { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}