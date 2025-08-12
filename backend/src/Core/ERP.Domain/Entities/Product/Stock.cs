// ================================================================
// ENTITÉ STOCK - GESTION DES STOCKS (INVENTORY CONTEXT)
// ================================================================
// Stock disponible par produit variant, entrepôt et emplacement
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Inventory
{
    /// <summary>
    /// Stock d'un produit variant dans un emplacement spécifique
    /// Représente la quantité physique disponible d'un produit à un endroit donné
    /// </summary>
    public class Stock : BaseAuditableEntity
    {
        // ================================================================
        // RÉFÉRENCES VERS LES AUTRES CONTEXTS
        // ================================================================

        /// <summary>
        /// SKU du produit variant (référence vers Catalog Context)
        /// Ex: "IPH15P-512-BLK-001"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ProductVariantSKU { get; set; } = string.Empty;

        /// <summary>
        /// Entrepôt où se trouve ce stock
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }

        /// <summary>
        /// Emplacement spécifique dans l'entrepôt
        /// </summary>
        [Required]
        public int LocationId { get; set; }

        // ================================================================
        // QUANTITÉS DE STOCK
        // ================================================================

        /// <summary>
        /// Quantité physiquement présente (confirmée par inventaire)
        /// </summary>
        [Required]
        public int QuantityOnHand { get; set; } = 0;

        /// <summary>
        /// Quantité réservée (allouée mais pas encore expédiée)
        /// Ex: produits dans des commandes confirmées
        /// </summary>
        public int QuantityReserved { get; set; } = 0;

        /// <summary>
        /// Quantité allouée (en cours de picking/préparation)
        /// </summary>
        public int QuantityAllocated { get; set; } = 0;

        /// <summary>
        /// Quantité en transit vers cet emplacement
        /// Ex: transferts inter-entrepôts en cours
        /// </summary>
        public int QuantityInTransit { get; set; } = 0;

        /// <summary>
        /// Quantité endommagée/non vendable
        /// </summary>
        public int QuantityDamaged { get; set; } = 0;

        /// <summary>
        /// Quantité en quarantaine (contrôle qualité)
        /// </summary>
        public int QuantityQuarantine { get; set; } = 0;

        /// <summary>
        /// Quantité disponible à la vente
        /// Calculé : QuantityOnHand - QuantityReserved - QuantityAllocated - QuantityDamaged - QuantityQuarantine
        /// </summary>
        public int QuantityAvailable => QuantityOnHand - QuantityReserved - QuantityAllocated - QuantityDamaged - QuantityQuarantine;

        /// <summary>
        /// Quantité totale théorique
        /// QuantityOnHand + QuantityInTransit
        /// </summary>
        public int QuantityTotal => QuantityOnHand + QuantityInTransit;

        // ================================================================
        // SEUILS ET ALERTES
        // ================================================================

        /// <summary>
        /// Seuil de stock minimum pour déclencher une alerte
        /// </summary>
        public int ReorderLevel { get; set; } = 5;

        /// <summary>
        /// Seuil critique en dessous duquel une action urgente est requise
        /// </summary>
        public int CriticalLevel { get; set; } = 1;

        /// <summary>
        /// Niveau de stock optimal/cible
        /// </summary>
        public int OptimalLevel { get; set; } = 50;

        /// <summary>
        /// Niveau de stock maximum recommandé
        /// </summary>
        public int MaxLevel { get; set; } = 100;

        /// <summary>
        /// Quantité de réapprovisionnement recommandée
        /// </summary>
        public int ReorderQuantity { get; set; } = 20;

        // ================================================================
        // COÛTS ET VALORISATION
        // ================================================================

        /// <summary>
        /// Coût unitaire moyen pondéré (CUMP)
        /// Mis à jour à chaque entrée de stock
        /// </summary>
        public decimal AverageCost { get; set; } = 0;

        /// <summary>
        /// Coût du dernier achat unitaire
        /// </summary>
        public decimal LastCost { get; set; } = 0;

        /// <summary>
        /// Coût standard/planifié
        /// </summary>
        public decimal? StandardCost { get; set; }

        /// <summary>
        /// Valeur totale du stock au coût moyen
        /// Calculé : QuantityOnHand × AverageCost
        /// </summary>
        public decimal TotalValue => QuantityOnHand * AverageCost;

        /// <summary>
        /// Valeur au coût de remplacement (dernier coût)
        /// </summary>
        public decimal ReplacementValue => QuantityOnHand * LastCost;

        // ================================================================
        // CARACTÉRISTIQUES PHYSIQUES DU STOCK
        // ================================================================

        /// <summary>
        /// Poids unitaire du produit (en grammes)
        /// </summary>
        public decimal? UnitWeight { get; set; }

        /// <summary>
        /// Volume unitaire du produit (en cm³)
        /// </summary>
        public decimal? UnitVolume { get; set; }

        /// <summary>
        /// Poids total occupé dans l'emplacement
        /// Calculé : QuantityOnHand × UnitWeight
        /// </summary>
        public decimal WeightOccupied => QuantityOnHand * (UnitWeight ?? 0);

        /// <summary>
        /// Volume total occupé dans l'emplacement
        /// Calculé : QuantityOnHand × UnitVolume
        /// </summary>
        public decimal VolumeOccupied => QuantityOnHand * (UnitVolume ?? 0);

        // ================================================================
        // DATES ET TRAÇABILITÉ
        // ================================================================

        /// <summary>
        /// Date de la dernière entrée en stock
        /// </summary>
        public DateTime? LastStockInDate { get; set; }

        /// <summary>
        /// Date de la dernière sortie de stock
        /// </summary>
        public DateTime? LastStockOutDate { get; set; }

        /// <summary>
        /// Date du dernier inventaire physique
        /// </summary>
        public DateTime? LastInventoryDate { get; set; }

        /// <summary>
        /// Date du prochain inventaire programmé
        /// </summary>
        public DateTime? NextInventoryDate { get; set; }

        /// <summary>
        /// Date de première entrée dans cet emplacement
        /// </summary>
        public DateTime? FirstStockDate { get; set; }

        /// <summary>
        /// Date d'expiration du produit (si applicable)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Date de fabrication du lot
        /// </summary>
        public DateTime? ManufactureDate { get; set; }

        // ================================================================
        // LOTS ET SÉRIES
        // ================================================================

        /// <summary>
        /// Numéro de lot du produit
        /// </summary>
        [MaxLength(50)]
        public string? LotNumber { get; set; }

        /// <summary>
        /// Numéro de série (pour articles uniques)
        /// </summary>
        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Numéro de lot d'importation
        /// </summary>
        [MaxLength(50)]
        public string? ImportBatch { get; set; }

        /// <summary>
        /// Référence fournisseur pour ce stock
        /// </summary>
        [MaxLength(200)]
        public string? SupplierReference { get; set; }

        /// <summary>
        /// Numéro de facture d'achat
        /// </summary>
        [MaxLength(100)]
        public string? PurchaseInvoiceNumber { get; set; }

        // ================================================================
        // STATUT ET CONDITION
        // ================================================================

        /// <summary>
        /// Statut du stock
        /// Available, Reserved, Quarantine, Damaged, Blocked, InTransit
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Available";

        /// <summary>
        /// Condition physique du stock
        /// New, Excellent, Good, Fair, Poor, Damaged
        /// </summary>
        [MaxLength(50)]
        public string? Condition { get; set; }

        /// <summary>
        /// Score de qualité (0-100)
        /// </summary>
        [Range(0, 100)]
        public int? QualityScore { get; set; }

        /// <summary>
        /// Indique si le stock est actif/disponible
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indique si le stock est bloqué
        /// </summary>
        public bool IsBlocked { get; set; } = false;

        /// <summary>
        /// Raison du blocage
        /// </summary>
        [MaxLength(500)]
        public string? BlockReason { get; set; }

        /// <summary>
        /// Utilisateur ayant bloqué le stock
        /// </summary>
        [MaxLength(100)]
        public string? BlockedBy { get; set; }

        /// <summary>
        /// Date de blocage
        /// </summary>
        public DateTime? BlockedDate { get; set; }

        // ================================================================
        // CONTRÔLE ET CONFORMITÉ
        // ================================================================

        /// <summary>
        /// Indique si une inspection est requise
        /// </summary>
        public bool RequiresInspection { get; set; } = false;

        /// <summary>
        /// Date de la dernière inspection
        /// </summary>
        public DateTime? LastInspectionDate { get; set; }

        /// <summary>
        /// Résultat de la dernière inspection
        /// </summary>
        [MaxLength(100)]
        public string? InspectionResult { get; set; }

        /// <summary>
        /// Inspecteur responsable
        /// </summary>
        [MaxLength(100)]
        public string? InspectedBy { get; set; }

        /// <summary>
        /// Certifications du stock (format JSON)
        /// </summary>
        [MaxLength(500)]
        public string? Certifications { get; set; }

        /// <summary>
        /// Conformité réglementaire
        /// </summary>
        public bool IsCompliant { get; set; } = true;

        /// <summary>
        /// Commentaires de conformité
        /// </summary>
        [MaxLength(1000)]
        public string? ComplianceNotes { get; set; }

        // ================================================================
        // TEMPÉRATURE ET ENVIRONNEMENT
        // ================================================================

        /// <summary>
        /// Température de stockage requise
        /// </summary>
        [MaxLength(50)]
        public string? StorageTemperature { get; set; }

        /// <summary>
        /// Humidité de stockage recommandée
        /// </summary>
        [MaxLength(50)]
        public string? StorageHumidity { get; set; }

        /// <summary>
        /// Exigences environnementales spéciales
        /// </summary>
        [MaxLength(500)]
        public string? EnvironmentalRequirements { get; set; }

        /// <summary>
        /// Indique si le produit nécessite un contrôle de température
        /// </summary>
        public bool RequiresTemperatureControl { get; set; } = false;

        /// <summary>
        /// Indique si le produit est sensible à l'humidité
        /// </summary>
        public bool IsMoistureSensitive { get; set; } = false;

        // ================================================================
        // ROTATION ET PERFORMANCE
        // ================================================================

        /// <summary>
        /// Méthode de rotation du stock
        /// FIFO (First In, First Out), LIFO (Last In, First Out), FEFO (First Expired, First Out)
        /// </summary>
        [MaxLength(20)]
        public string RotationMethod { get; set; } = "FIFO";

        /// <summary>
        /// Classe ABC (A=High value/volume, B=Medium, C=Low)
        /// </summary>
        [MaxLength(5)]
        public string? ABCClass { get; set; }

        /// <summary>
        /// Vélocité de rotation (mouvements par mois)
        /// </summary>
        public decimal? Velocity { get; set; }

        /// <summary>
        /// Jours de couverture de stock
        /// </summary>
        public int? DaysOfCoverage { get; set; }

        /// <summary>
        /// Taux de rotation annuel
        /// </summary>
        public decimal? TurnoverRate { get; set; }

        // ================================================================
        // RÉSERVATIONS ET ALLOCATIONS
        // ================================================================

        /// <summary>
        /// Réservations détaillées (format JSON)
        /// Ex: [{"orderId": "SO001", "quantity": 5, "reservedDate": "2025-01-15"}]
        /// </summary>
        [MaxLength(2000)]
        public string? ReservationDetails { get; set; }

        /// <summary>
        /// Allocations détaillées (format JSON)
        /// </summary>
        [MaxLength(2000)]
        public string? AllocationDetails { get; set; }

        /// <summary>
        /// Priorité pour allocation automatique
        /// </summary>
        public int AllocationPriority { get; set; } = 100;

        // ================================================================
        // INFORMATIONS COMPLÉMENTAIRES
        // ================================================================

        /// <summary>
        /// Notes sur ce stock spécifique
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

        /// <summary>
        /// Instructions de manipulation
        /// </summary>
        [MaxLength(1000)]
        public string? HandlingInstructions { get; set; }

        /// <summary>
        /// Informations de sécurité
        /// </summary>
        [MaxLength(500)]
        public string? SafetyInfo { get; set; }

        /// <summary>
        /// Configuration spécifique (format JSON)
        /// </summary>
        [MaxLength(1000)]
        public string? Configuration { get; set; }

        /// <summary>
        /// Métadonnées supplémentaires (format JSON)
        /// </summary>
        [MaxLength(2000)]
        public string? Metadata { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES
        // ================================================================

        /// <summary>
        /// Entrepôt où se trouve ce stock
        /// </summary>
        public virtual Warehouse Warehouse { get; set; } = null!;

        /// <summary>
        /// Emplacement spécifique dans l'entrepôt
        /// </summary>
        public virtual Location Location { get; set; } = null!;

        /// <summary>
        /// Mouvements de stock associés
        /// </summary>
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Identifiant unique du stock (pour affichage)
        /// </summary>
        public string StockId => $"{ProductVariantSKU}-{Warehouse?.Code}-{Location?.Code}";

        /// <summary>
        /// Indique si le stock est en rupture
        /// </summary>
        public bool IsOutOfStock => QuantityAvailable <= 0;

        /// <summary>
        /// Indique si le stock est en dessous du seuil de réapprovisionnement
        /// </summary>
        public bool IsLowStock => QuantityAvailable <= ReorderLevel;

        /// <summary>
        /// Indique si le stock est en situation critique
        /// </summary>
        public bool IsCriticalStock => QuantityAvailable <= CriticalLevel;

        /// <summary>
        /// Indique si le stock est en surstock
        /// </summary>
        public bool IsOverStock => QuantityOnHand > MaxLevel;

        /// <summary>
        /// Indique si le produit est proche de l'expiration
        /// </summary>
        public bool IsNearExpiry => ExpiryDate.HasValue && ExpiryDate.Value.AddDays(-30) <= DateTime.UtcNow;

        /// <summary>
        /// Indique si le produit est expiré
        /// </summary>
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value <= DateTime.UtcNow;

        /// <summary>
        /// Jours restants avant expiration
        /// </summary>
        public int? DaysToExpiry => ExpiryDate.HasValue
            ? Math.Max(0, (ExpiryDate.Value - DateTime.UtcNow).Days)
            : null;

        /// <summary>
        /// Âge du stock en jours
        /// </summary>
        public int? AgeInDays => FirstStockDate.HasValue
            ? (DateTime.UtcNow - FirstStockDate.Value).Days
            : null;

        /// <summary>
        /// Jours depuis le dernier mouvement
        /// </summary>
        public int? DaysSinceLastMovement
        {
            get
            {
                var lastMovement = new[] { LastStockInDate, LastStockOutDate }
                    .Where(d => d.HasValue)
                    .DefaultIfEmpty()
                    .Max();

                return lastMovement.HasValue
                    ? (DateTime.UtcNow - lastMovement.Value).Days
                    : null;
            }
        }

        /// <summary>
        /// Pourcentage d'utilisation de l'emplacement (par quantité)
        /// </summary>
        public decimal? LocationUtilizationPercentage => Location?.MaxItems > 0
            ? Math.Round((decimal)QuantityOnHand / Location.MaxItems.Value * 100, 2)
            : null;

        /// <summary>
        /// Valeur par mètre carré
        /// </summary>
        public decimal? ValuePerSquareMeter => Location?.Volume > 0
            ? TotalValue / (Location.Volume.Value / 10000) // Conversion cm³ en m²
            : null;

        /// <summary>
        /// Indique si un inventaire est requis
        /// </summary>
        public bool RequiresInventory => NextInventoryDate.HasValue && NextInventoryDate <= DateTime.UtcNow;

        /// <summary>
        /// Score de performance du stock (basé sur rotation, âge, etc.)
        /// </summary>
        public decimal PerformanceScore
        {
            get
            {
                decimal score = 100;

                // Pénalité pour stock lent
                if (DaysSinceLastMovement > 90) score -= 20;
                else if (DaysSinceLastMovement > 30) score -= 10;

                // Pénalité pour surstock
                if (IsOverStock) score -= 15;

                // Pénalité pour stock critique
                if (IsCriticalStock) score -= 25;

                // Pénalité pour expiration proche
                if (IsNearExpiry) score -= 20;
                if (IsExpired) score -= 50;

                // Bonus pour stock optimal
                if (QuantityOnHand >= ReorderLevel && QuantityOnHand <= OptimalLevel) score += 10;

                return Math.Max(0, score);
            }
        }

        /// <summary>
        /// Représentation textuelle du stock
        /// </summary>
        public override string ToString() => $"{ProductVariantSKU} - {QuantityOnHand} units @ {Location?.Code}";
    }
}