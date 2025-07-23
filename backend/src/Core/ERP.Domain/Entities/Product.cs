namespace ERP.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Smartphones, Laptops, Tablets, etc.
        public string Brand { get; set; } = string.Empty; // Samsung, Apple, Dell, HP, etc.
        public string Model { get; set; } = string.Empty; // Galaxy S23, iPhone 15, XPS 13, etc.

        // Pricing & Costs
        public decimal PurchasePrice { get; set; } // Prix d'achat en Italie (€)
        public decimal TransportCost { get; set; } // Frais de transport par unité
        public decimal TotalCostPrice { get; set; } // Prix d'achat + transport + autres frais
        public decimal SellingPrice { get; set; } // Prix de vente final
        public decimal Margin { get; set; } // Marge bénéficiaire
        public decimal MarginPercentage { get; set; } // Pourcentage de marge

        // Stock & Condition
        public int Stock { get; set; }
        public int MinStockLevel { get; set; } = 5; // Seuil d'alerte stock
        public string Condition { get; set; } = string.Empty; // Neuf, Très bon état, Bon état, Occasion
        public string ConditionGrade { get; set; } = string.Empty; // A+, A, B+, B, C

        // Technical Specifications
        public string Storage { get; set; } = string.Empty; // 128GB, 256GB, 512GB, 1TB
        public string Color { get; set; } = string.Empty; // Noir, Blanc, Bleu, etc.
        public string Memory { get; set; } = string.Empty; // RAM 8GB, 16GB, 32GB (pour ordinateurs)
        public string Processor { get; set; } = string.Empty; // Intel i7, Apple M2, Snapdragon, etc.
        public string ScreenSize { get; set; } = string.Empty; // 6.1", 13.3", 15.6"

        // Import Information
        public string SupplierName { get; set; } = string.Empty; // Nom du fournisseur en Italie
        public string SupplierCity { get; set; } = string.Empty; // Ville du fournisseur
        public DateTime PurchaseDate { get; set; } // Date d'achat
        public DateTime? ArrivalDate { get; set; } // Date d'arrivée
        public string ImportBatch { get; set; } = string.Empty; // Numéro de lot d'importation
        public string InvoiceNumber { get; set; } = string.Empty; // Numéro de facture

        // Product Status
        public string Status { get; set; } = "Available"; // Available, Sold, Reserved, Damaged
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; } // Notes spéciales
        public string? WarrantyInfo { get; set; } // Information garantie

        // Images & Documentation
        public string? ImageUrl { get; set; } // Photo principale
        public string? ImagesUrls { get; set; } // Photos multiples (JSON)
        public string? DocumentsUrls { get; set; } // Factures, certificats, etc.

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public bool FlagDelete { get; set; } = false;

        // Calculated Properties
        public decimal TotalValue => Stock * SellingPrice;
        public bool IsLowStock => Stock <= MinStockLevel;
        public int DaysInStock => (DateTime.UtcNow - (ArrivalDate ?? CreatedAt)).Days;
    }
}