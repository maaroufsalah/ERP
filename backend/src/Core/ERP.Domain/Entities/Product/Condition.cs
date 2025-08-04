// ================================================================
// ENTITÉ CONDITION - CONDITIONS/ÉTATS DES PRODUITS
// ================================================================
// Neuf, Excellent, Très Bon, Bon, Correct
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Conditions/États des produits (Neuf, Excellent, Très Bon, Bon, Correct)
    /// Cette entité standardise les états des produits d'occasion/reconditionnés
    /// </summary>
    public class Condition : BaseAuditableEntity
    {
        /// <summary>
        /// Nom de la condition (ex: "Neuf", "Excellent", "Très Bon", "Bon", "Correct")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description détaillée de ce que signifie cette condition
        /// Aide les utilisateurs à comprendre l'état exact du produit
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Pourcentage de qualité approximatif (ex: 100% pour Neuf, 95% pour Excellent)
        /// Utilisé pour des calculs ou affichages graphiques
        /// </summary>
        [Range(0, 100)]
        public int QualityPercentage { get; set; } = 100;

        /// <summary>
        /// Ordre d'affichage dans les listes déroulantes
        /// Généralement du meilleur au moins bon état
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Indique si cette condition est active dans le système
        /// Permet de désactiver une condition sans la supprimer
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS
        // ================================================================

        /// <summary>
        /// Liste des produits dans cette condition
        /// Relation One-to-Many : Une condition peut être attribuée à plusieurs produits
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Représentation textuelle de l'entité
        /// </summary>
        /// <returns>Le nom de la condition</returns>
        public override string ToString() => Name;
    }
}