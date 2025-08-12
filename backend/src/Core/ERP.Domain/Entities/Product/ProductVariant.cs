// ================================================================
// ENTITÉ PRODUCTVARIANT - PRODUIT DIMENSIONNEL (CATALOG CONTEXT)
// ================================================================
// Produit physique vendable : iPhone 15 Pro 512GB Noir
// Contient les informations spécifiques à chaque variante
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Product;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Catalog
{
    /// <summary>
    /// Variante spécifique d'un produit master
    /// Exemple: "iPhone 15 Pro - 512GB - Noir" 
    /// Représente un produit physique vendable avec ses attributs spécifiques
    /// </summary>
    public class ProductVariant : BaseAuditableEntity
    {
        // ================================================================
        // RÉFÉRENCE AU PRODUIT MASTER
        // ================================================================

        /// <summary>
        /// Référence vers le produit master parent
        /// </summary>
        [Required]
        public int ProductMasterId { get; set; }

        // ================================================================
        // IDENTIFICATION UNIQUE
        // ================================================================

        /// <summary>
        /// SKU (Stock Keeping Unit) - Identifiant unique interne
        /// Ex: "IPH15P-512-BLK-001", "SGS24U-256-BTI-002"
        /// Utilisé pour la gestion interne et traçabilité
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Code-barres UPC/EAN pour scan point de vente
        /// Ex: "194253001737" (UPC officiel du fabricant)
        /// </summary>
        [MaxLength(20)]
        public string? Barcode { get; set; }

        /// <summary>
        /// Nom spécifique de la variante
        /// Ex: "512GB - Noir", "256GB - Bleu Titane"
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string VariantName { get; set; } = string.Empty;

        /// <summary>
        /// Nom complet pour affichage
        /// Combinaison du ProductMaster + VariantName
        /// Calculé automatiquement ou stocké pour performance
        /// </summary>
        [MaxLength(400)]
        public string? FullName { get; set; }

        // ================================================================
        // ATTRIBUTS DIMENSIONNELS
        // ================================================================

        /// <summary>
        /// Couleur de cette variante
        /// Clé étrangère vers Color
        /// </summary>
        public int? ColorId { get; set; }

        /// <summary>
        /// Taille (pour vêtements, accessoires)
        /// Ex: "S", "M", "L", "XL", "42mm", "44mm"
        /// </summary>
        [MaxLength(50)]
        public string? Size { get; set; }

        /// <summary>
        /// Capacité de stockage
        /// Ex: "128GB", "256GB", "512GB", "1TB"
        /// </summary>
        [MaxLength(50)]
        public string? Storage { get; set; }

        /// <summary>
        /// Mémoire RAM (pour laptops/ordinateurs)
        /// Ex: "8GB", "16GB", "32GB"
        /// </summary>
        [MaxLength(50)]
        public string? Memory { get; set; }

        /// <summary>
        /// Configuration matérielle spécifique
        /// Ex: "M3 Pro", "Intel i7", "Snapdragon 8 Gen 3"
        /// </summary>
        [MaxLength(150)]
        public string? Configuration { get; set; }

        /// <summary>
        /// Attributs personnalisés (format JSON)
        /// Pour des attributs spécifiques par type de produit
        /// Ex: {"screen_size": "6.1\"", "battery": "3274mAh", "camera": "48MP"}
        /// </summary>
        [MaxLength(2000)]
        public string? CustomAttributes { get; set; }

        // ================================================================
        // PRICING ET COÛTS
        // ================================================================

        /// <summary>
        /// Prix d'achat unitaire (coût fournisseur)
        /// </summary>
        [Required]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Frais additionnels (transport, douane, etc.)
        /// </summary>
        public decimal AdditionalCosts { get; set; } = 0;

        /// <summary>
        /// Coût total unitaire (PurchasePrice + AdditionalCosts)
        /// Calculé automatiquement
        /// </summary>
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// Prix de vente unitaire public
        /// </summary>
        [Required]
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// Prix de vente recommandé par le fabricant (MSRP)
        /// </summary>
        public decimal? MSRP { get; set; }

        /// <summary>
        /// Marge bénéficiaire unitaire
        /// Calculé automatiquement : SellingPrice - TotalCostPrice
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// Pourcentage de marge
        /// Calculé automatiquement : (Margin / TotalCostPrice) * 100
        /// </summary>
        public decimal MarginPercentage { get; set; }

        // ================================================================
        // INFORMATIONS PHYSIQUES
        // ================================================================

        /// <summary>
        /// Poids spécifique de cette variante (en grammes)
        /// Peut différer selon la configuration
        /// </summary>
        public int? Weight { get; set; }

        /// <summary>
        /// Dimensions spécifiques à cette variante
        /// Peut différer selon la taille/configuration
        /// </summary>
        [MaxLength(100)]
        public string? Dimensions { get; set; }

        /// <summary>
        /// Informations d'emballage
        /// Ex: "Boîte d'origine incluse", "Emballage bulk"
        /// </summary>
        [MaxLength(200)]
        public string? PackagingInfo { get; set; }

        // ================================================================
        // INFORMATIONS FOURNISSEUR ET TRAÇABILITÉ
        // ================================================================

        /// <summary>
        /// Nom du fournisseur principal
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        /// <summary>
        /// Référence produit chez le fournisseur
        /// </summary>
        [MaxLength(100)]
        public string? SupplierReference { get; set; }

        /// <summary>
        /// Pays d'origine du produit
        /// Ex: "Italie", "Chine", "États-Unis"
        /// </summary>
        [MaxLength(100)]
        public string? CountryOfOrigin { get; set; }

        /// <summary>
        /// Numéro de série ou IMEI pour traçabilité individuelle
        /// Important pour les produits électroniques
        /// </summary>
        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Date de fabrication
        /// </summary>
        public DateTime? ManufactureDate { get; set; }

        /// <summary>
        /// Lot d'importation/réception
        /// Ex: "IT2025005", "CN2025012"
        /// </summary>
        [MaxLength(50)]
        public string? ImportBatch { get; set; }

        /// <summary>
        /// Numéro de facture fournisseur
        /// </summary>
        [MaxLength(100)]
        public string? InvoiceNumber { get; set; }

        // ================================================================
        // CONDITION ET QUALITÉ
        // ================================================================

        /// <summary>
        /// Condition/État du produit
        /// Clé étrangère vers Condition
        /// </summary>
        [Required]
        public int ConditionId { get; set; }

        /// <summary>
        /// Notes spécifiques sur l'état de cette unité
        /// Ex: "Légères rayures sur l'écran", "Parfait état"
        /// </summary>
        [MaxLength(1000)]
        public string? ConditionNotes { get; set; }

        /// <summary>
        /// Score de qualité spécifique (0-100)
        /// Peut différer du score standard de la condition
        /// </summary>
        [Range(0, 100)]
        public int? QualityScore { get; set; }

        // ================================================================
        // GARANTIE ET SERVICE
        // ================================================================

        /// <summary>
        /// Informations de garantie
        /// Ex: "Garantie constructeur 2 ans", "Garantie magasin 6 mois"
        /// </summary>
        [MaxLength(300)]
        public string? WarrantyInfo { get; set; }

        /// <summary>
        /// Date de début de garantie
        /// </summary>
        public DateTime? WarrantyStartDate { get; set; }

        /// <summary>
        /// Date de fin de garantie
        /// </summary>
        public DateTime? WarrantyEndDate { get; set; }

        /// <summary>
        /// Informations de service après-vente
        /// </summary>
        [MaxLength(500)]
        public string? ServiceInfo { get; set; }

        // ================================================================
        // STATUT ET DISPONIBILITÉ
        // ================================================================

        /// <summary>
        /// Statut de la variante
        /// Available, Reserved, Sold, Damaged, Returned
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Available";

        /// <summary>
        /// Indique si cette variante est active/vendable
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indique si cette variante est featured/mise en avant
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Date de mise en vente
        /// </summary>
        public DateTime? LaunchDate { get; set; }

        /// <summary>
        /// Date de retrait de la vente
        /// </summary>
        public DateTime? DiscontinuedDate { get; set; }

        // ================================================================
        // MÉDIAS SPÉCIFIQUES À LA VARIANTE
        // ================================================================

        /// <summary>
        /// URL de l'image principale de cette variante
        /// Montre la couleur/configuration spécifique
        /// </summary>
        [MaxLength(500)]
        public string? PrimaryImageUrl { get; set; }

        /// <summary>
        /// URLs des images supplémentaires (format JSON)
        /// Photos spécifiques à cette variante
        /// </summary>
        [MaxLength(2000)]
        public string? ImageUrls { get; set; }

        // ================================================================
        // MÉTADONNÉES E-COMMERCE
        // ================================================================

        /// <summary>
        /// URL SEO-friendly pour cette variante
        /// Ex: "iphone-15-pro-512gb-noir"
        /// </summary>
        [MaxLength(200)]
        public string? Slug { get; set; }

        /// <summary>
        /// Mots-clés spécifiques à cette variante
        /// </summary>
        [MaxLength(500)]
        public string? Keywords { get; set; }

        /// <summary>
        /// Informations de tracking e-commerce
        /// Ex: pixels Facebook, Google Analytics events
        /// </summary>
        [MaxLength(1000)]
        public string? TrackingInfo { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES
        // ================================================================

        /// <summary>
        /// Produit master parent
        /// </summary>
        public virtual ProductMaster ProductMaster { get; set; } = null!;

        /// <summary>
        /// Couleur de cette variante
        /// </summary>
        public virtual Color? Color { get; set; }

        /// <summary>
        /// Condition/État de cette variante
        /// </summary>
        public virtual Condition Condition { get; set; } = null!;

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Nom complet calculé (ProductMaster + VariantName)
        /// </summary>
        public string DisplayName => $"{ProductMaster?.Name} - {VariantName}";

        /// <summary>
        /// Indique si la garantie est encore valide
        /// </summary>
        public bool IsWarrantyValid => WarrantyEndDate.HasValue && WarrantyEndDate > DateTime.UtcNow;

        /// <summary>
        /// Nombre de jours restants de garantie
        /// </summary>
        public int? WarrantyDaysRemaining => WarrantyEndDate.HasValue
            ? Math.Max(0, (WarrantyEndDate.Value - DateTime.UtcNow).Days)
            : null;

        /// <summary>
        /// Age de la variante en jours depuis la création
        /// </summary>
        public int AgeInDays => (DateTime.UtcNow - CreatedAt).Days;

        /// <summary>
        /// Indique si le produit est considéré comme "nouveau" (moins de 30 jours)
        /// </summary>
        public bool IsNew => AgeInDays <= 30;

        /// <summary>
        /// Représentation textuelle de la variante
        /// </summary>
        public override string ToString() => $"{SKU} - {VariantName}";
    }
}