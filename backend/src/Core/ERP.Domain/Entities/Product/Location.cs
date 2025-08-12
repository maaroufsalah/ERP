// ================================================================
// ENTITÉ LOCATION - EMPLACEMENTS DE STOCKAGE (INVENTORY CONTEXT)
// ================================================================
// Emplacements spécifiques dans un entrepôt : A1-B2-C3
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Inventory
{
    /// <summary>
    /// Emplacement spécifique de stockage dans un entrepôt
    /// Exemple: "A1-B2-C3" (Allée A1, Rangée B2, Niveau C3)
    /// </summary>
    public class Location : BaseAuditableEntity
    {
        // ================================================================
        // RÉFÉRENCE À L'ENTREPÔT
        // ================================================================

        /// <summary>
        /// Entrepôt auquel appartient cet emplacement
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }

        // ================================================================
        // IDENTIFICATION DE L'EMPLACEMENT
        // ================================================================

        /// <summary>
        /// Code unique de l'emplacement dans l'entrepôt
        /// Ex: "A1-B2-C3", "ZONE-A-001", "PICKING-01"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Nom descriptif de l'emplacement
        /// Ex: "Allée A - Rangée 1 - Niveau 3"
        /// </summary>
        [MaxLength(200)]
        public string? Name { get; set; }

        /// <summary>
        /// Description de l'emplacement
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        // ================================================================
        // STRUCTURE HIÉRARCHIQUE
        // ================================================================

        /// <summary>
        /// Zone principale (ex: "Zone A", "Zone Picking", "Zone Réception")
        /// </summary>
        [MaxLength(50)]
        public string? Zone { get; set; }

        /// <summary>
        /// Allée ou section
        /// Ex: "A1", "A2", "B1"
        /// </summary>
        [MaxLength(20)]
        public string? Aisle { get; set; }

        /// <summary>
        /// Rangée dans l'allée
        /// Ex: "R01", "R02", "R15"
        /// </summary>
        [MaxLength(20)]
        public string? Row { get; set; }

        /// <summary>
        /// Niveau/Étage (hauteur)
        /// Ex: "N1", "N2", "N3" (Sol, 1er niveau, 2ème niveau)
        /// </summary>
        [MaxLength(20)]
        public string? Level { get; set; }

        /// <summary>
        /// Position dans la rangée
        /// Ex: "P01", "P02", "P10"
        /// </summary>
        [MaxLength(20)]
        public string? Position { get; set; }

        /// <summary>
        /// Emplacement parent (pour hiérarchie complexe)
        /// </summary>
        public int? ParentLocationId { get; set; }

        // ================================================================
        // CARACTÉRISTIQUES PHYSIQUES
        // ================================================================

        /// <summary>
        /// Type d'emplacement
        /// Ex: "Shelf", "Floor", "Rack", "Bin", "Pallet", "Picking", "Receiving"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = "Shelf";

        /// <summary>
        /// Longueur en centimètres
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Largeur en centimètres
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Hauteur en centimètres
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Volume total en mètres cubes
        /// Calculé automatiquement : (Length × Width × Height) / 1,000,000
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Capacité de poids maximale en kilogrammes
        /// </summary>
        public decimal? MaxWeight { get; set; }

        /// <summary>
        /// Capacité maximale en nombre d'articles
        /// </summary>
        public int? MaxItems { get; set; }

        // ================================================================
        // CARACTÉRISTIQUES ENVIRONNEMENTALES
        // ================================================================

        /// <summary>
        /// Température requise/recommandée
        /// Ex: "Ambiante", "Réfrigéré", "Congelé"
        /// </summary>
        [MaxLength(50)]
        public string? TemperatureRequirement { get; set; }

        /// <summary>
        /// Plage de température acceptable
        /// Ex: "18-25°C", "-18°C", "2-8°C"
        /// </summary>
        [MaxLength(50)]
        public string? TemperatureRange { get; set; }

        /// <summary>
        /// Exigences d'humidité
        /// Ex: "Standard", "Faible humidité", "Contrôlée"
        /// </summary>
        [MaxLength(50)]
        public string? HumidityRequirement { get; set; }

        /// <summary>
        /// Exigences de luminosité
        /// Ex: "Standard", "Sombre", "Protégé UV"
        /// </summary>
        [MaxLength(50)]
        public string? LightRequirement { get; set; }

        // ================================================================
        // ACCESSIBILITÉ ET ÉQUIPEMENT
        // ================================================================

        /// <summary>
        /// Méthode d'accès requis
        /// Ex: "Manual", "Forklift", "Ladder", "Crane", "Conveyor"
        /// </summary>
        [MaxLength(50)]
        public string? AccessMethod { get; set; }

        /// <summary>
        /// Hauteur d'accès (en centimètres depuis le sol)
        /// </summary>
        public decimal? AccessHeight { get; set; }

        /// <summary>
        /// Équipement nécessaire pour accéder
        /// Ex: "Chariot élévateur", "Échelle", "Grue"
        /// </summary>
        [MaxLength(200)]
        public string? RequiredEquipment { get; set; }

        /// <summary>
        /// Facilité d'accès (1=Très difficile, 5=Très facile)
        /// </summary>
        [Range(1, 5)]
        public int? AccessibilityScore { get; set; }

        /// <summary>
        /// Indique si l'emplacement est accessible aux personnes à mobilité réduite
        /// </summary>
        public bool IsAccessible { get; set; } = true;

        // ================================================================
        // RESTRICTIONS ET RÈGLES
        // ================================================================

        /// <summary>
        /// Types de produits autorisés (format JSON)
        /// Ex: ["electronics", "small_items"] ou null = tous autorisés
        /// </summary>
        [MaxLength(500)]
        public string? AllowedProductTypes { get; set; }

        /// <summary>
        /// Types de produits interdits (format JSON)
        /// Ex: ["hazardous", "liquid", "fragile"]
        /// </summary>
        [MaxLength(500)]
        public string? RestrictedProductTypes { get; set; }

        /// <summary>
        /// Matières dangereuses autorisées
        /// </summary>
        public bool AllowsHazardousMaterials { get; set; } = false;

        /// <summary>
        /// Produits en vrac autorisés
        /// </summary>
        public bool AllowsBulkItems { get; set; } = true;

        /// <summary>
        /// Picking direct autorisé (accès client/picking rapide)
        /// </summary>
        public bool AllowsDirectPicking { get; set; } = true;

        /// <summary>
        /// Classe de sécurité requise
        /// Ex: "Standard", "High", "Maximum", "Restricted"
        /// </summary>
        [MaxLength(50)]
        public string? SecurityClass { get; set; }

        // ================================================================
        // STATUT ET DISPONIBILITÉ
        // ================================================================

        /// <summary>
        /// Statut de l'emplacement
        /// Available, Occupied, Reserved, Maintenance, Damaged, Blocked
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Available";

        /// <summary>
        /// Indique si l'emplacement est actif
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indique si l'emplacement est occupé
        /// </summary>
        public bool IsOccupied { get; set; } = false;

        /// <summary>
        /// Indique si l'emplacement est réservé
        /// </summary>
        public bool IsReserved { get; set; } = false;

        /// <summary>
        /// Date de dernière occupation
        /// </summary>
        public DateTime? LastOccupiedDate { get; set; }

        /// <summary>
        /// Date de dernière libération
        /// </summary>
        public DateTime? LastReleasedDate { get; set; }

        /// <summary>
        /// Utilisateur ayant réservé l'emplacement
        /// </summary>
        [MaxLength(100)]
        public string? ReservedBy { get; set; }

        /// <summary>
        /// Date d'expiration de la réservation
        /// </summary>
        public DateTime? ReservationExpiry { get; set; }

        // ================================================================
        // MAINTENANCE ET CONTRÔLE QUALITÉ
        // ================================================================

        /// <summary>
        /// Date de dernière inspection
        /// </summary>
        public DateTime? LastInspectionDate { get; set; }

        /// <summary>
        /// Date de prochaine inspection programmée
        /// </summary>
        public DateTime? NextInspectionDate { get; set; }

        /// <summary>
        /// Résultat de la dernière inspection
        /// Ex: "Conforme", "Non-conforme", "Réparation nécessaire"
        /// </summary>
        [MaxLength(100)]
        public string? InspectionResult { get; set; }

        /// <summary>
        /// Date de dernière maintenance
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// Notes de maintenance
        /// </summary>
        [MaxLength(1000)]
        public string? MaintenanceNotes { get; set; }

        /// <summary>
        /// Score de condition (1=Très mauvais, 5=Excellent)
        /// </summary>
        [Range(1, 5)]
        public int? ConditionScore { get; set; }

        // ================================================================
        // CONFIGURATION ET PARAMÈTRES
        // ================================================================

        /// <summary>
        /// Priorité de sélection (pour allocation automatique)
        /// Plus la valeur est élevée, plus l'emplacement est prioritaire
        /// </summary>
        public int Priority { get; set; } = 100;

        /// <summary>
        /// Coût d'utilisation par jour
        /// </summary>
        public decimal? DailyCost { get; set; }

        /// <summary>
        /// Configuration spécifique (format JSON)
        /// Paramètres personnalisés pour cet emplacement
        /// </summary>
        [MaxLength(1000)]
        public string? Configuration { get; set; }

        /// <summary>
        /// Code-barres de l'emplacement
        /// Pour scan et identification rapide
        /// </summary>
        [MaxLength(50)]
        public string? Barcode { get; set; }

        /// <summary>
        /// QR Code de l'emplacement
        /// </summary>
        [MaxLength(100)]
        public string? QRCode { get; set; }

        // ================================================================
        // INFORMATIONS COMPLÉMENTAIRES
        // ================================================================

        /// <summary>
        /// Instructions spéciales pour cet emplacement
        /// </summary>
        [MaxLength(1000)]
        public string? SpecialInstructions { get; set; }

        /// <summary>
        /// Notes diverses
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

        /// <summary>
        /// Informations de sécurité spécifiques
        /// </summary>
        [MaxLength(500)]
        public string? SafetyInfo { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES
        // ================================================================

        /// <summary>
        /// Entrepôt auquel appartient cet emplacement
        /// </summary>
        public virtual Warehouse Warehouse { get; set; } = null!;

        /// <summary>
        /// Emplacement parent (pour hiérarchie)
        /// </summary>
        public virtual Location? ParentLocation { get; set; }

        /// <summary>
        /// Emplacements enfants
        /// </summary>
        public virtual ICollection<Location> ChildLocations { get; set; } = new List<Location>();

        /// <summary>
        /// Stocks présents dans cet emplacement
        /// </summary>
        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        /// <summary>
        /// Mouvements de stock pour cet emplacement
        /// </summary>
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Code complet avec hiérarchie
        /// Ex: "ZONA-A1-R02-N3-P05"
        /// </summary>
        public string FullCode => $"{Zone}-{Aisle}-{Row}-{Level}-{Position}".Replace("--", "-").Trim('-');

        /// <summary>
        /// Volume occupé actuellement
        /// </summary>
        public decimal CurrentVolume => Stocks?.Sum(s => s.VolumeOccupied) ?? 0;

        /// <summary>
        /// Poids actuel
        /// </summary>
        public decimal CurrentWeight => Stocks?.Sum(s => s.WeightOccupied) ?? 0;

        /// <summary>
        /// Nombre d'articles actuellement stockés
        /// </summary>
        public int CurrentItemCount => Stocks?.Sum(s => s.QuantityOnHand) ?? 0;

        /// <summary>
        /// Pourcentage d'occupation par volume
        /// </summary>
        public decimal? VolumeOccupancyPercentage => Volume > 0
            ? Math.Round(CurrentVolume / Volume.Value * 100, 2)
            : null;

        /// <summary>
        /// Pourcentage d'occupation par poids
        /// </summary>
        public decimal? WeightOccupancyPercentage => MaxWeight > 0
            ? Math.Round(CurrentWeight / MaxWeight.Value * 100, 2)
            : null;

        /// <summary>
        /// Pourcentage d'occupation par nombre d'articles
        /// </summary>
        public decimal? ItemOccupancyPercentage => MaxItems > 0
            ? Math.Round((decimal)CurrentItemCount / MaxItems.Value * 100, 2)
            : null;

        /// <summary>
        /// Indique si la réservation a expiré
        /// </summary>
        public bool IsReservationExpired => IsReserved && ReservationExpiry.HasValue && ReservationExpiry < DateTime.UtcNow;

        /// <summary>
        /// Temps depuis la dernière occupation (en jours)
        /// </summary>
        public int? DaysSinceLastOccupied => LastOccupiedDate.HasValue
            ? (DateTime.UtcNow - LastOccupiedDate.Value).Days
            : null;

        /// <summary>
        /// Indique si une inspection est due
        /// </summary>
        public bool IsInspectionDue => NextInspectionDate.HasValue && NextInspectionDate <= DateTime.UtcNow;

        /// <summary>
        /// Représentation textuelle de l'emplacement
        /// </summary>
        public override string ToString() => $"{Code} ({Warehouse?.Code})";
    }
}