// ================================================================
// ENTITÉ PRODUCTTYPE - TYPES DE PRODUITS
// ================================================================
// Smartphone, Laptop, Tablet, Camera, Accessoire
// ================================================================

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Types de produits (Smartphone, Laptop, Camera, Accessoire)
    /// Cette entité définit les grandes catégories de produits vendus
    /// </summary>
    public class ProductType : BaseAuditableEntity
    {
        /// <summary>
        /// Nom du type de produit (ex: "Smartphone", "Laptop", "Camera", "Accessoire")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description détaillée du type de produit
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Ordre d'affichage dans les listes déroulantes
        /// Permet de contrôler l'ordre d'apparition (ex: 1=Smartphone, 2=Laptop, etc.)
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// URL de l'icône pour l'affichage dans l'interface
        /// </summary>
        [MaxLength(500)]
        public string? IconUrl { get; set; }

        /// <summary>
        /// Couleur associée au type pour l'affichage (code hex)
        /// </summary>
        [MaxLength(7)]
        public string? CategoryColor { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS
        // ================================================================

        /// <summary>
        /// Liste des marques disponibles pour ce type de produit
        /// Relation One-to-Many : Un type peut avoir plusieurs marques
        /// </summary>
        public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();

        /// <summary>
        /// Liste des modèles disponibles pour ce type de produit
        /// Relation One-to-Many : Un type peut avoir plusieurs modèles
        /// </summary>
        public virtual ICollection<Model> Models { get; set; } = new List<Model>();

        /// <summary>
        /// Liste des produits de ce type
        /// Relation One-to-Many : Un type peut avoir plusieurs produits
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Représentation textuelle de l'entité
        /// </summary>
        /// <returns>Le nom du type de produit</returns>
        public override string ToString() => Name;
    }
}