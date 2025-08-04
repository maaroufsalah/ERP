// ================================================================
// ENTITÉ PRODUCT - PRODUIT PRINCIPAL
// ================================================================
// Entité centrale du module produits avec relations vers les référentiels
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Entité principale représentant un produit dans l'inventaire
    /// Version avec relations vers les tables de référence
    /// </summary>
    public class Product : BaseAuditableEntity
    {
        // ================================================================
        // INFORMATIONS DE BASE DU PRODUIT
        // ================================================================

        /// <summary>
        /// Nom commercial du produit tel qu'affiché aux clients
        /// Ex: "Samsung Galaxy S24 Ultra 512GB Noir"
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description détaillée du produit
        /// Inclut les caractéristiques principales et points de vente
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        // ================================================================
        // RELATIONS VERS LES TABLES DE RÉFÉRENCE (CLÉS ÉTRANGÈRES)
        // ================================================================

        /// <summary>
        /// Type de produit (Smartphone, Laptop, Camera, Accessoire)
        /// Clé étrangère vers ProductType
        /// </summary>
        [Required]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Marque du produit (Samsung, Apple, Dell, Canon, etc.)
        /// Clé étrangère vers Brand
        /// </summary>
        [Required]
        public int BrandId { get; set; }

        /// <summary>
        /// Modèle spécifique du produit (Galaxy S24, iPhone 15, XPS 13, etc.)
        /// Clé étrangère vers Model
        /// </summary>
        [Required]
        public int ModelId { get; set; }

        /// <summary>
        /// Couleur du produit (Noir, Blanc, Bleu, etc.)
        /// Clé étrangère vers Color
        /// </summary>
        [Required]
        public int ColorId { get; set; }

        /// <summary>
        /// État/Condition du produit (Neuf, Excellent, Très Bon, etc.)
        /// Clé étrangère vers Condition
        /// </summary>
        [Required]
        public int ConditionId { get; set; }

        // ================================================================
        // PRICING & COSTS - CALCULS FINANCIERS
        // ================================================================

        /// <summary>
        /// Prix d'achat unitaire du produit en euros (€)
        /// Prix payé au fournisseur italien
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix d'achat doit être supérieur à 0")]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Frais de transport par unité en euros (€)
        /// Inclut les coûts de transport depuis l'Italie
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Les frais de transport ne peuvent pas être négatifs")]
        public decimal TransportCost { get; set; } = 0;

        /// <summary>
        /// Coût total par unité (Prix d'achat + Transport + autres frais)
        /// Calculé automatiquement : PurchasePrice + TransportCost
        /// </summary>
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// Prix de vente unitaire en euros (€)
        /// Prix affiché au client final
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix de vente doit être supérieur à 0")]
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// Marge bénéficiaire unitaire en euros (€)
        /// Calculé automatiquement : SellingPrice - TotalCostPrice
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// Pourcentage de marge bénéficiaire
        /// Calculé automatiquement : (Margin / TotalCostPrice) * 100
        /// </summary>
        public decimal MarginPercentage { get; set; }

        // ================================================================
        // GESTION DU STOCK
        // ================================================================

        /// <summary>
        /// Quantité actuellement en stock
        /// Nombre d'unités disponibles à la vente
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif")]
        public int Stock { get; set; }

        /// <summary>
        /// Seuil minimum de stock pour déclencher une alerte
        /// Utilisé pour la gestion automatique des réapprovisionnements
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Le stock minimum ne peut pas être négatif")]
        public int MinStockLevel { get; set; } = 5;

        // ================================================================
        // CARACTÉRISTIQUES TECHNIQUES SPÉCIFIQUES
        // ================================================================

        /// <summary>
        /// Capacité de stockage du produit
        /// Ex: "128GB", "256GB", "512GB", "1TB"
        /// </summary>
        [MaxLength(50)]
        public string? Storage { get; set; }

        /// <summary>
        /// Mémoire RAM (principalement pour laptops/ordinateurs)
        /// Ex: "8GB", "16GB", "32GB"
        /// </summary>
        [MaxLength(50)]
        public string? Memory { get; set; }

        /// <summary>
        /// Processeur du produit
        /// Ex: "Intel i7", "Apple M2", "Snapdragon 8 Gen 3"
        /// </summary>
        [MaxLength(150)]
        public string? Processor { get; set; }

        /// <summary>
        /// Taille de l'écran
        /// Ex: "6.1\"", "13.3\"", "15.6\""
        /// </summary>
        [MaxLength(20)]
        public string? ScreenSize { get; set; }

        // ================================================================
        // INFORMATIONS FOURNISSEUR ET IMPORT
        // ================================================================

        /// <summary>
        /// Nom du fournisseur en Italie
        /// Ex: "TechItalia Milano", "AppleStore Roma"
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        /// <summary>
        /// Ville du fournisseur en Italie
        /// Ex: "Milano", "Roma", "Napoli"
        /// </summary>
        [MaxLength(100)]
        public string? SupplierCity { get; set; }

        /// <summary>
        /// Date d'achat chez le fournisseur
        /// Utilisée pour le suivi et les calculs de rentabilité
        /// </summary>
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date d'arrivée de la marchandise au Maroc
        /// Peut différer de la date d'achat selon le transport
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// Numéro de lot d'importation
        /// Permet de regrouper les produits par expédition
        /// Ex: "IT2025005"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ImportBatch { get; set; } = string.Empty;

        /// <summary>
        /// Numéro de facture du fournisseur
        /// Référence pour la comptabilité et les contrôles
        /// Ex: "INV-2025-008"
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        // ================================================================
        // STATUT ET ÉTAT DU PRODUIT
        // ================================================================

        /// <summary>
        /// Statut actuel du produit dans le processus de vente
        /// Ex: "Available", "Sold", "Reserved", "Damaged"
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Available";

        // ================================================================
        // INFORMATIONS COMPLÉMENTAIRES
        // ================================================================

        /// <summary>
        /// Notes spéciales sur le produit
        /// Ex: "Quelques micro-rayures sur l'écran", "Emballage d'origine inclus"
        /// </summary>
        [MaxLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Informations sur la garantie
        /// Ex: "Garantie constructeur 2 ans", "6 mois garantie magasin"
        /// </summary>
        [MaxLength(300)]
        public string? WarrantyInfo { get; set; }

        // ================================================================
        // MÉDIAS ET DOCUMENTATION
        // ================================================================

        /// <summary>
        /// URL de l'image principale du produit
        /// Utilisée pour l'affichage dans les listes et fiches produit
        /// </summary>
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// URLs des images supplémentaires (format JSON)
        /// Permet de stocker plusieurs photos du produit
        /// Ex: ["url1.jpg", "url2.jpg", "url3.jpg"]
        /// </summary>
        [MaxLength(2000)]
        public string? ImagesUrls { get; set; }

        /// <summary>
        /// URLs des documents associés (format JSON)
        /// Factures, certificats, manuels, etc.
        /// Ex: ["facture.pdf", "certificat.pdf"]
        /// </summary>
        [MaxLength(2000)]
        public string? DocumentsUrls { get; set; }

        // ================================================================
        // PROPRIÉTÉS CALCULÉES (READ-ONLY)
        // ================================================================

        /// <summary>
        /// Valeur totale du stock pour ce produit
        /// Calculé : Stock × SellingPrice
        /// </summary>
        public decimal TotalValue => Stock * SellingPrice;

        /// <summary>
        /// Indique si le produit est en stock faible
        /// Basé sur la comparaison Stock vs MinStockLevel
        /// </summary>
        public bool IsLowStock => Stock <= MinStockLevel;

        /// <summary>
        /// Nombre de jours écoulés depuis l'arrivée du produit
        /// Utile pour analyser la rotation des stocks
        /// </summary>
        public int DaysInStock => (DateTime.UtcNow - (ArrivalDate ?? CreatedAt)).Days;

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS ENTITY FRAMEWORK
        // ================================================================

        /// <summary>
        /// Type de produit associé
        /// Relation Many-to-One : Plusieurs produits peuvent avoir le même type
        /// </summary>
        public virtual ProductType ProductType { get; set; } = null!;

        /// <summary>
        /// Marque associée
        /// Relation Many-to-One : Plusieurs produits peuvent avoir la même marque
        /// </summary>
        public virtual Brand Brand { get; set; } = null!;

        /// <summary>
        /// Modèle associé
        /// Relation Many-to-One : Plusieurs produits peuvent avoir le même modèle
        /// </summary>
        public virtual Model Model { get; set; } = null!;

        /// <summary>
        /// Couleur associée
        /// Relation Many-to-One : Plusieurs produits peuvent avoir la même couleur
        /// </summary>
        public virtual Color Color { get; set; } = null!;

        /// <summary>
        /// Condition/État associé
        /// Relation Many-to-One : Plusieurs produits peuvent avoir la même condition
        /// </summary>
        public virtual Condition Condition { get; set; } = null!;

        /// <summary>
        /// Représentation textuelle de l'entité
        /// </summary>
        /// <returns>Le nom du produit</returns>
        public override string ToString() => Name;
    }
}