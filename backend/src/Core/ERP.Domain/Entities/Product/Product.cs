// ================================================================
// ENTIT� PRODUCT - PRODUIT PRINCIPAL
// ================================================================
// Entit� centrale du module produits avec relations vers les r�f�rentiels
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Entit� principale repr�sentant un produit dans l'inventaire
    /// Version avec relations vers les tables de r�f�rence
    /// </summary>
    public class Product : BaseAuditableEntity
    {
        // ================================================================
        // INFORMATIONS DE BASE DU PRODUIT
        // ================================================================

        /// <summary>
        /// Nom commercial du produit tel qu'affich� aux clients
        /// Ex: "Samsung Galaxy S24 Ultra 512GB Noir"
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description d�taill�e du produit
        /// Inclut les caract�ristiques principales et points de vente
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        // ================================================================
        // RELATIONS VERS LES TABLES DE R�F�RENCE (CL�S �TRANG�RES)
        // ================================================================

        /// <summary>
        /// Type de produit (Smartphone, Laptop, Camera, Accessoire)
        /// Cl� �trang�re vers ProductType
        /// </summary>
        [Required]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Marque du produit (Samsung, Apple, Dell, Canon, etc.)
        /// Cl� �trang�re vers Brand
        /// </summary>
        [Required]
        public int BrandId { get; set; }

        /// <summary>
        /// Mod�le sp�cifique du produit (Galaxy S24, iPhone 15, XPS 13, etc.)
        /// Cl� �trang�re vers Model
        /// </summary>
        [Required]
        public int ModelId { get; set; }

        /// <summary>
        /// Couleur du produit (Noir, Blanc, Bleu, etc.)
        /// Cl� �trang�re vers Color
        /// </summary>
        [Required]
        public int ColorId { get; set; }

        /// <summary>
        /// �tat/Condition du produit (Neuf, Excellent, Tr�s Bon, etc.)
        /// Cl� �trang�re vers Condition
        /// </summary>
        [Required]
        public int ConditionId { get; set; }

        // ================================================================
        // PRICING & COSTS - CALCULS FINANCIERS
        // ================================================================

        /// <summary>
        /// Prix d'achat unitaire du produit en euros (�)
        /// Prix pay� au fournisseur italien
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix d'achat doit �tre sup�rieur � 0")]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Frais de transport par unit� en euros (�)
        /// Inclut les co�ts de transport depuis l'Italie
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Les frais de transport ne peuvent pas �tre n�gatifs")]
        public decimal TransportCost { get; set; } = 0;

        /// <summary>
        /// Co�t total par unit� (Prix d'achat + Transport + autres frais)
        /// Calcul� automatiquement : PurchasePrice + TransportCost
        /// </summary>
        public decimal TotalCostPrice { get; set; }

        /// <summary>
        /// Prix de vente unitaire en euros (�)
        /// Prix affich� au client final
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix de vente doit �tre sup�rieur � 0")]
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// Marge b�n�ficiaire unitaire en euros (�)
        /// Calcul� automatiquement : SellingPrice - TotalCostPrice
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// Pourcentage de marge b�n�ficiaire
        /// Calcul� automatiquement : (Margin / TotalCostPrice) * 100
        /// </summary>
        public decimal MarginPercentage { get; set; }

        // ================================================================
        // GESTION DU STOCK
        // ================================================================

        /// <summary>
        /// Quantit� actuellement en stock
        /// Nombre d'unit�s disponibles � la vente
        /// </summary>
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas �tre n�gatif")]
        public int Stock { get; set; }

        /// <summary>
        /// Seuil minimum de stock pour d�clencher une alerte
        /// Utilis� pour la gestion automatique des r�approvisionnements
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Le stock minimum ne peut pas �tre n�gatif")]
        public int MinStockLevel { get; set; } = 5;

        // ================================================================
        // CARACT�RISTIQUES TECHNIQUES SP�CIFIQUES
        // ================================================================

        /// <summary>
        /// Capacit� de stockage du produit
        /// Ex: "128GB", "256GB", "512GB", "1TB"
        /// </summary>
        [MaxLength(50)]
        public string? Storage { get; set; }

        /// <summary>
        /// M�moire RAM (principalement pour laptops/ordinateurs)
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
        /// Taille de l'�cran
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
        /// Utilis�e pour le suivi et les calculs de rentabilit�
        /// </summary>
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date d'arriv�e de la marchandise au Maroc
        /// Peut diff�rer de la date d'achat selon le transport
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// Num�ro de lot d'importation
        /// Permet de regrouper les produits par exp�dition
        /// Ex: "IT2025005"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ImportBatch { get; set; } = string.Empty;

        /// <summary>
        /// Num�ro de facture du fournisseur
        /// R�f�rence pour la comptabilit� et les contr�les
        /// Ex: "INV-2025-008"
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        // ================================================================
        // STATUT ET �TAT DU PRODUIT
        // ================================================================

        /// <summary>
        /// Statut actuel du produit dans le processus de vente
        /// Ex: "Available", "Sold", "Reserved", "Damaged"
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Available";

        // ================================================================
        // INFORMATIONS COMPL�MENTAIRES
        // ================================================================

        /// <summary>
        /// Notes sp�ciales sur le produit
        /// Ex: "Quelques micro-rayures sur l'�cran", "Emballage d'origine inclus"
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
        // M�DIAS ET DOCUMENTATION
        // ================================================================

        /// <summary>
        /// URL de l'image principale du produit
        /// Utilis�e pour l'affichage dans les listes et fiches produit
        /// </summary>
        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// URLs des images suppl�mentaires (format JSON)
        /// Permet de stocker plusieurs photos du produit
        /// Ex: ["url1.jpg", "url2.jpg", "url3.jpg"]
        /// </summary>
        [MaxLength(2000)]
        public string? ImagesUrls { get; set; }

        /// <summary>
        /// URLs des documents associ�s (format JSON)
        /// Factures, certificats, manuels, etc.
        /// Ex: ["facture.pdf", "certificat.pdf"]
        /// </summary>
        [MaxLength(2000)]
        public string? DocumentsUrls { get; set; }

        // ================================================================
        // PROPRI�T�S CALCUL�ES (READ-ONLY)
        // ================================================================

        /// <summary>
        /// Valeur totale du stock pour ce produit
        /// Calcul� : Stock � SellingPrice
        /// </summary>
        public decimal TotalValue => Stock * SellingPrice;

        /// <summary>
        /// Indique si le produit est en stock faible
        /// Bas� sur la comparaison Stock vs MinStockLevel
        /// </summary>
        public bool IsLowStock => Stock <= MinStockLevel;

        /// <summary>
        /// Nombre de jours �coul�s depuis l'arriv�e du produit
        /// Utile pour analyser la rotation des stocks
        /// </summary>
        public int DaysInStock => (DateTime.UtcNow - (ArrivalDate ?? CreatedAt)).Days;

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS ENTITY FRAMEWORK
        // ================================================================

        /// <summary>
        /// Type de produit associ�
        /// Relation Many-to-One : Plusieurs produits peuvent avoir le m�me type
        /// </summary>
        public virtual ProductType ProductType { get; set; } = null!;

        /// <summary>
        /// Marque associ�e
        /// Relation Many-to-One : Plusieurs produits peuvent avoir la m�me marque
        /// </summary>
        public virtual Brand Brand { get; set; } = null!;

        /// <summary>
        /// Mod�le associ�
        /// Relation Many-to-One : Plusieurs produits peuvent avoir le m�me mod�le
        /// </summary>
        public virtual Model Model { get; set; } = null!;

        /// <summary>
        /// Couleur associ�e
        /// Relation Many-to-One : Plusieurs produits peuvent avoir la m�me couleur
        /// </summary>
        public virtual Color Color { get; set; } = null!;

        /// <summary>
        /// Condition/�tat associ�
        /// Relation Many-to-One : Plusieurs produits peuvent avoir la m�me condition
        /// </summary>
        public virtual Condition Condition { get; set; } = null!;

        /// <summary>
        /// Repr�sentation textuelle de l'entit�
        /// </summary>
        /// <returns>Le nom du produit</returns>
        public override string ToString() => Name;
    }
}