using System.ComponentModel.DataAnnotations;

namespace ERP.Domain.Entities.Shared
{
    /// <summary>
    /// Classe de base abstraite pour toutes les entités avec audit trail
    /// Contient les propriétés communes de traçabilité et de suppression logique
    /// </summary>
    public abstract class BaseAuditableEntity
    {
        /// <summary>
        /// Identifiant unique de l'entité
        /// </summary>
        public int Id { get; set; }

        // ================================================================
        // AUDIT TRAIL - TRAÇABILITÉ COMPLÈTE
        // ================================================================

        /// <summary>
        /// Date de création de l'enregistrement (UTC)
        /// Définie automatiquement lors de la création
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Utilisateur ayant créé l'enregistrement
        /// Référence l'utilisateur connecté au moment de la création
        /// </summary>
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Date de dernière modification (UTC)
        /// Mise à jour automatiquement à chaque modification
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Utilisateur ayant effectué la dernière modification
        /// Référence l'utilisateur connecté au moment de la modification
        /// </summary>
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// Date de suppression logique (UTC)
        /// Utilisée pour la suppression soft delete
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Utilisateur ayant effectué la suppression logique
        /// Référence l'utilisateur qui a supprimé l'enregistrement
        /// </summary>
        [MaxLength(100)]
        public string? DeletedBy { get; set; }

        /// <summary>
        /// Flag de suppression logique
        /// true = supprimé logiquement, false = actif
        /// Permet de "supprimer" sans perdre les données
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Indique si l'enregistrement est actif (non supprimé)
        /// Propriété calculée inverse de IsDeleted
        /// </summary>
        public bool IsActive => !IsDeleted;

        /// <summary>
        /// Indique si l'enregistrement a été modifié depuis sa création
        /// </summary>
        public bool HasBeenModified => UpdatedAt.HasValue;

        /// <summary>
        /// Nombre de jours depuis la création
        /// Utile pour les analyses et rapports
        /// </summary>
        public int DaysCreated => (DateTime.UtcNow - CreatedAt).Days;

        /// <summary>
        /// Nombre de jours depuis la dernière modification
        /// Retourne null si jamais modifié
        /// </summary>
        public int? DaysLastModified => UpdatedAt.HasValue
            ? (DateTime.UtcNow - UpdatedAt.Value).Days
            : null;
    }

}
