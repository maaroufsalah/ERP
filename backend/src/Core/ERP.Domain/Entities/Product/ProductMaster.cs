// ================================================================
// ENTITÉ PRODUCTMASTER - PRODUIT GÉNÉRIQUE (CATALOG CONTEXT)
// ================================================================
// Référence globale du produit : iPhone 15 Pro, Galaxy S24 Ultra
// Contient les informations communes à toutes les variantes
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Product;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Catalog
{
    /// <summary>
    /// Produit générique (référence globale)
    /// Exemple: "iPhone 15 Pro" sans spécifier capacité/couleur
    /// Contient les informations communes à toutes les variantes
    /// </summary>
    public class ProductMaster : BaseAuditableEntity
    {
        // ================================================================
        // INFORMATIONS GÉNÉRIQUES DU PRODUIT
        // ================================================================

        /// <summary>
        /// Nom commercial générique du produit
        /// Ex: "iPhone 15 Pro", "Samsung Galaxy S24 Ultra", "MacBook Pro M3"
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description générale du produit
        /// Contient les caractéristiques communes à toutes les variantes
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Numéro de référence interne unique
        /// Ex: "PM-IPHONE15PRO-001", "PM-GALAXYS24U-002"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ReferenceNumber { get; set; } = string.Empty;

        // ================================================================
        // CLASSIFICATION ET CATÉGORISATION
        // ================================================================

        /// <summary>
        /// Type de produit (Smartphone, Laptop, Tablet, etc.)
        /// Clé étrangère vers ProductType
        /// </summary>
        [Required]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Marque du produit (Apple, Samsung, Dell, etc.)
        /// Clé étrangère vers Brand
        /// </summary>
        [Required]
        public int BrandId { get; set; }

        /// <summary>
        /// Catégorie principale pour regroupement
        /// Ex: "Premium", "Mid-range", "Entry-level"
        /// </summary>
        [MaxLength(100)]
        public string? Category { get; set; }

        /// <summary>
        /// Tags pour recherche et filtrage
        /// Format JSON: ["smartphone", "5G", "premium", "gaming"]
        /// </summary>
        [MaxLength(1000)]
        public string? Tags { get; set; }

        // ================================================================
        // CARACTÉRISTIQUES TECHNIQUES COMMUNES
        // ================================================================

        /// <summary>
        /// Informations techniques communes (format JSON)
        /// Ex: {"display": "6.1 inch", "os": "iOS 17", "connectivity": "5G"}
        /// </summary>
        [MaxLength(3000)]
        public string? CommonSpecifications { get; set; }

        /// <summary>
        /// Dimensions physiques standard
        /// Ex: "146.6 x 70.6 x 7.8 mm"
        /// </summary>
        [MaxLength(100)]
        public string? Dimensions { get; set; }

        /// <summary>
        /// Poids approximatif (en grammes)
        /// </summary>
        public int? Weight { get; set; }

        // ================================================================
        // STATUT ET CYCLE DE VIE
        // ================================================================

        /// <summary>
        /// Statut du product master
        /// Active, Discontinued, Planned, Draft
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Draft";

        /// <summary>
        /// Date de lancement du produit
        /// </summary>
        public DateTime? LaunchDate { get; set; }

        /// <summary>
        /// Date de fin de vie prévue/effective
        /// </summary>
        public DateTime? EndOfLifeDate { get; set; }

        /// <summary>
        /// Indique si ce produit est encore commercialisé
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ================================================================
        // MÉDIAS ET DOCUMENTATION
        // ================================================================

        /// <summary>
        /// URL de l'image principale du produit
        /// </summary>
        [MaxLength(500)]
        public string? PrimaryImageUrl { get; set; }

        /// <summary>
        /// URLs des images supplémentaires (format JSON)
        /// Ex: ["image1.jpg", "image2.jpg", "image3.jpg"]
        /// </summary>
        [MaxLength(2000)]
        public string? ImageUrls { get; set; }

        /// <summary>
        /// URLs des documents techniques (format JSON)
        /// Ex: ["datasheet.pdf", "manual.pdf", "warranty.pdf"]
        /// </summary>
        [MaxLength(2000)]
        public string? DocumentUrls { get; set; }

        // ================================================================
        // INFORMATIONS MARKETING
        // ================================================================

        /// <summary>
        /// Résumé marketing court
        /// Utilisé pour les descriptions courtes sur le site
        /// </summary>
        [MaxLength(500)]
        public string? MarketingSummary { get; set; }

        /// <summary>
        /// Points forts du produit (format JSON)
        /// Ex: ["Écran Super Retina XDR", "Puce A17 Pro", "Appareil photo 48MP"]
        /// </summary>
        [MaxLength(1000)]
        public string? KeyFeatures { get; set; }

        /// <summary>
        /// Public cible
        /// Ex: "Professionnels", "Gamers", "Créateurs de contenu"
        /// </summary>
        [MaxLength(200)]
        public string? TargetAudience { get; set; }

        // ================================================================
        // MÉTADONNÉES ET SEO
        // ================================================================

        /// <summary>
        /// Titre SEO pour le site web
        /// </summary>
        [MaxLength(200)]
        public string? SeoTitle { get; set; }

        /// <summary>
        /// Description SEO
        /// </summary>
        [MaxLength(500)]
        public string? SeoDescription { get; set; }

        /// <summary>
        /// Mots-clés SEO (séparés par des virgules)
        /// </summary>
        [MaxLength(500)]
        public string? SeoKeywords { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES
        // ================================================================

        /// <summary>
        /// Type de produit associé
        /// </summary>
        public virtual ProductType ProductType { get; set; } = null!;

        /// <summary>
        /// Marque associée
        /// </summary>
        public virtual Brand Brand { get; set; } = null!;

        /// <summary>
        /// Collection des variantes de ce produit master
        /// Un product master peut avoir plusieurs variantes
        /// </summary>
        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Nombre total de variantes actives
        /// </summary>
        public int ActiveVariantsCount => Variants?.Count(v => v.IsActive) ?? 0;

        /// <summary>
        /// Indique si le produit a des variantes en stock
        /// </summary>
        public bool HasStockVariants => Variants?.Any(v => v.IsActive) ?? false;

        /// <summary>
        /// Prix minimum parmi toutes les variantes
        /// </summary>
        public decimal? MinPrice => Variants?.Where(v => v.IsActive).Min(v => v.SellingPrice);

        /// <summary>
        /// Prix maximum parmi toutes les variantes
        /// </summary>
        public decimal? MaxPrice => Variants?.Where(v => v.IsActive).Max(v => v.SellingPrice);

        /// <summary>
        /// Âge du produit en jours depuis le lancement
        /// </summary>
        public int? AgeInDays => LaunchDate.HasValue
            ? (DateTime.UtcNow - LaunchDate.Value).Days
            : null;

        /// <summary>
        /// Représentation textuelle du ProductMaster
        /// </summary>
        public override string ToString() => $"{Brand?.Name} {Name}";
    }
}