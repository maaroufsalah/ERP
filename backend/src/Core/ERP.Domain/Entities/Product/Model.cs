// ================================================================
// ENTITÉ MODEL - MODÈLES DE PRODUITS
// ================================================================
// Galaxy S24, iPhone 15, XPS 13, etc.
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Modèles de produits (Galaxy S24, iPhone 15, XPS 13, etc.)
    /// Cette entité stocke les modèles spécifiques par marque et type
    /// </summary>
    public class Model : BaseAuditableEntity
    {
        /// <summary>
        /// Nom du modèle (ex: "Galaxy S24 Ultra", "iPhone 15 Pro", "XPS 13")
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description détaillée du modèle
        /// Peut inclure les spécifications principales
        /// </summary>
        [MaxLength(1000)]
        public string? Description { get; set; }

        /// <summary>
        /// Type de produit de ce modèle
        /// Clé étrangère vers ProductType
        /// </summary>
        [Required]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Marque de ce modèle
        /// Clé étrangère vers Brand
        /// </summary>
        [Required]
        public int BrandId { get; set; }

        /// <summary>
        /// Référence/code interne du modèle (optionnel)
        /// Peut être utilisé pour des références techniques
        /// </summary>
        [MaxLength(50)]
        public string? ModelReference { get; set; }

        /// <summary>
        /// Année de sortie du modèle (optionnel)
        /// Utile pour les smartphones/laptops qui évoluent annuellement
        /// </summary>
        public int? ReleaseYear { get; set; }

        /// <summary>
        /// Ordre d'affichage dans les listes déroulantes
        /// Permet de mettre les modèles récents en premier
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Indique si ce modèle est actif dans le système
        /// Permet de désactiver les anciens modèles sans les supprimer
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS
        // ================================================================

        /// <summary>
        /// Type de produit de ce modèle
        /// Relation Many-to-One : Plusieurs modèles peuvent appartenir à un type
        /// </summary>
        public virtual ProductType ProductType { get; set; } = null!;

        /// <summary>
        /// Marque de ce modèle
        /// Relation Many-to-One : Plusieurs modèles peuvent appartenir à une marque
        /// </summary>
        public virtual Brand Brand { get; set; } = null!;

        /// <summary>
        /// Liste des produits de ce modèle
        /// Relation One-to-Many : Un modèle peut avoir plusieurs produits (différentes couleurs, stockages, etc.)
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Représentation textuelle de l'entité
        /// </summary>
        /// <returns>Le nom du modèle</returns>
        public override string ToString() => Name;
    }
}