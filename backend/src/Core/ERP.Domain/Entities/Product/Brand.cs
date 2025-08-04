// ================================================================
// ENTITÉ BRAND - MARQUES DE PRODUITS
// ================================================================
// Samsung, Apple, Dell, Canon, etc.
// ================================================================

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Marques de produits (Samsung, Apple, Dell, Canon, etc.)
    /// Cette entité stocke les différentes marques par type de produit
    /// </summary>
    public class Brand : BaseAuditableEntity
    {
        /// <summary>
        /// Nom de la marque (ex: "Samsung", "Apple", "Dell", "Canon")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description de la marque (optionnel)
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Lien vers le logo de la marque (optionnel)
        /// Peut être utilisé pour afficher le logo dans l'interface
        /// </summary>
        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Site web officiel de la marque (optionnel)
        /// </summary>
        [MaxLength(200)]
        public string? Website { get; set; }

        /// <summary>
        /// Type de produit auquel appartient cette marque
        /// Clé étrangère vers ProductType
        /// </summary>
        [Required]
        public int ProductTypeId { get; set; }

        /// <summary>
        /// Ordre d'affichage dans les listes déroulantes
        /// Permet de mettre les marques populaires en premier
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Indique si cette marque est active dans le système
        /// Permet de désactiver une marque sans la supprimer
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS
        // ================================================================

        /// <summary>
        /// Type de produit auquel appartient cette marque
        /// Relation Many-to-One : Plusieurs marques peuvent appartenir à un type
        /// </summary>
        public virtual ProductType ProductType { get; set; } = null!;

        /// <summary>
        /// Liste des modèles de cette marque
        /// Relation One-to-Many : Une marque peut avoir plusieurs modèles
        /// </summary>
        public virtual ICollection<Model> Models { get; set; } = new List<Model>();

        /// <summary>
        /// Liste des produits de cette marque
        /// Relation One-to-Many : Une marque peut avoir plusieurs produits
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Représentation textuelle de l'entité
        /// </summary>
        /// <returns>Le nom de la marque</returns>
        public override string ToString() => Name;
    }
}