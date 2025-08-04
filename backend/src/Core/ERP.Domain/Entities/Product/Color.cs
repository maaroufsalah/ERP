// ================================================================
// ENTITÉ COLOR - COULEURS DES PRODUITS
// ================================================================
// Noir, Blanc, Bleu Titane, Gris Sidéral, etc.
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Product
{
    /// <summary>
    /// Couleurs disponibles pour les produits
    /// Cette entité standardise les couleurs disponibles
    /// </summary>
    public class Color : BaseAuditableEntity
    {
        /// <summary>
        /// Nom de la couleur (ex: "Noir", "Blanc", "Bleu Titane", "Gris Sidéral")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Code hexadécimal de la couleur (optionnel)
        /// Utilisé pour afficher la couleur dans l'interface (ex: "#000000" pour noir)
        /// </summary>
        [MaxLength(7)]
        public string? HexCode { get; set; }

        /// <summary>
        /// Description de la couleur (optionnel)
        /// Peut inclure des nuances ou descriptions marketing
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Ordre d'affichage dans les listes déroulantes
        /// Permet de mettre les couleurs populaires en premier
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// Indique si cette couleur est active dans le système
        /// Permet de désactiver une couleur sans la supprimer
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ================================================================
        // NAVIGATION PROPERTIES - RELATIONS
        // ================================================================

        /// <summary>
        /// Liste des produits de cette couleur
        /// Relation One-to-Many : Une couleur peut être utilisée par plusieurs produits
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Représentation textuelle de l'entité
        /// </summary>
        /// <returns>Le nom de la couleur</returns>
        public override string ToString() => Name;
    }
}