// ================================================================
// ENTITÉ STOCKMOVEMENT - MOUVEMENTS DE STOCK (INVENTORY CONTEXT)
// ================================================================
// Historique complet de tous les mouvements de stock
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Inventory
{
    /// <summary>
    /// Mouvement de stock - Historique de toutes les entrées/sorties
    /// Chaque changement de stock génère un mouvement pour traçabilité complète
    /// </summary>
    public class StockMovement : BaseAuditableEntity
    {
        // ================================================================
        // RÉFÉRENCES ET IDENTIFICATION
        // ================================================================

        /// <summary>
        /// SKU du produit variant concerné
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ProductVariantSKU { get; set; } = string.Empty;

        /// <summary>
        /// Entrepôt concerné par le mouvement
        /// </summary>
        [Required]
        public int WarehouseId { get; set; }

        /// <summary>
        /// Emplacement concerné par le mouvement
        /// </summary>
        [Required]
        public int LocationId { get; set; }

        /// <summary>
        /// Numéro de mouvement unique
        /// Ex: "MV-2025-000001", "MV-IN-2025-001", "MV-OUT-2025-001"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MovementNumber { get; set; } = string.Empty;

        /// <summary>
        /// Référence externe du document source
        /// Ex: "PO-2025-001" (Purchase Order), "SO-2025-001" (Sales Order)
        /// </summary>
        [MaxLength(100)]
        public string? ExternalReference { get; set; }

        // ================================================================
        // TYPE ET DIRECTION DU MOUVEMENT
        // ================================================================

        /// <summary>
        /// Type de mouvement
        /// Purchase, Sale, Transfer, Adjustment, Return, Damage, Loss, Found
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MovementType { get; set; } = string.Empty;

        /// <summary>
        /// Direction du mouvement
        /// In (entrée), Out (sortie), Transfer (transfert)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Direction { get; set; } = string.Empty;

        /// <summary>
        /// Sous-type de mouvement pour plus de précision
        /// Ex: "Receiving", "Shipping", "Internal_Transfer", "Cycle_Count"
        /// </summary>
        [MaxLength(50)]
        public string? SubType { get; set; }

        /// <summary>
        /// Catégorie du mouvement
        /// Operational, Administrative, Correction, Emergency
        /// </summary>
        [MaxLength(50)]
        public string Category { get; set; } = "Operational";

        // ================================================================
        // QUANTITÉS ET IMPACT
        // ================================================================

        /// <summary>
        /// Quantité du mouvement (toujours positive)
        /// La direction (In/Out) détermine l'impact sur le stock
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Quantité signée selon la direction
        /// Positif pour les entrées, négatif pour les sorties
        /// </summary>
        public int SignedQuantity => Direction == "In" ? Quantity : -Quantity;

        /// <summary>
        /// Stock avant le mouvement
        /// </summary>
        public int StockBefore { get; set; }

        /// <summary>
        /// Stock après le mouvement
        /// Calculé : StockBefore + SignedQuantity
        /// </summary>
        public int StockAfter { get; set; }

        /// <summary>
        /// Différence réelle constatée (pour les ajustements)
        /// </summary>
        public int? ActualDifference { get; set; }

        // ================================================================
        // COÛTS ET VALORISATION
        // ================================================================

        /// <summary>
        /// Coût unitaire du mouvement
        /// </summary>
        public decimal UnitCost { get; set; } = 0;

        /// <summary>
        /// Coût total du mouvement
        /// Calculé : Quantity × UnitCost
        /// </summary>
        public decimal TotalCost { get; set; } = 0;

        /// <summary>
        /// Prix de vente unitaire (pour les sorties de vente)
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Prix total de vente
        /// </summary>
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// Coût moyen avant le mouvement
        /// </summary>
        public decimal AverageCostBefore { get; set; } = 0;

        /// <summary>
        /// Coût moyen après le mouvement
        /// </summary>
        public decimal AverageCostAfter { get; set; } = 0;

        /// <summary>
        /// Impact sur la valorisation du stock
        /// </summary>
        public decimal ValuationImpact { get; set; } = 0;

        // ================================================================
        // INFORMATIONS DE TRANSFERT (si applicable)
        // ================================================================

        /// <summary>
        /// Entrepôt source (pour les transferts)
        /// </summary>
        public int? SourceWarehouseId { get; set; }

        /// <summary>
        /// Emplacement source (pour les transferts)
        /// </summary>
        public int? SourceLocationId { get; set; }

        /// <summary>
        /// Entrepôt destination (pour les transferts)
        /// </summary>
        public int? DestinationWarehouseId { get; set; }

        /// <summary>
        /// Emplacement destination (pour les transferts)
        /// </summary>
        public int? DestinationLocationId { get; set; }

        // ================================================================
        // DATES ET TEMPORALITÉ
        // ================================================================

        /// <summary>
        /// Date effective du mouvement
        /// Date réelle où le mouvement a eu lieu physiquement
        /// </summary>
        [Required]
        public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date de transaction/saisie
        /// Date où le mouvement a été enregistré dans le système
        /// </summary>
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date de planification (pour les mouvements programmés)
        /// </summary>
        public DateTime? PlannedDate { get; set; }

        /// <summary>
        /// Date d'expiration du produit concerné
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Date de fabrication
        /// </summary>
        public DateTime? ManufactureDate { get; set; }

        // ================================================================
        // TRAÇABILITÉ ET LOTS
        // ================================================================

        /// <summary>
        /// Numéro de lot
        /// </summary>
        [MaxLength(50)]
        public string? LotNumber { get; set; }

        /// <summary>
        /// Numéro de série
        /// </summary>
        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Lot d'importation
        /// </summary>
        [MaxLength(50)]
        public string? ImportBatch { get; set; }

        /// <summary>
        /// Numéro de facture associée
        /// </summary>
        [MaxLength(100)]
        public string? InvoiceNumber { get; set; }

        /// <summary>
        /// Bon de livraison/réception
        /// </summary>
        [MaxLength(100)]
        public string? DeliveryNote { get; set; }

        /// <summary>
        /// Numéro de tracking (transporteur)
        /// </summary>
        [MaxLength(100)]
        public string? TrackingNumber { get; set; }

        // ================================================================
        // PARTENAIRES ET ACTEURS
        // ================================================================

        /// <summary>
        /// Nom du fournisseur (pour les achats)
        /// </summary>
        [MaxLength(200)]
        public string? SupplierName { get; set; }

        /// <summary>
        /// Nom du client (pour les ventes)
        /// </summary>
        [MaxLength(200)]
        public string? CustomerName { get; set; }

        /// <summary>
        /// Transporteur/livreur
        /// </summary>
        [MaxLength(200)]
        public string? Carrier { get; set; }

        /// <summary>
        /// Responsable du mouvement
        /// Personne qui a effectué/autorisé le mouvement
        /// </summary>
        [MaxLength(100)]
        public string? ResponsiblePerson { get; set; }

        /// <summary>
        /// Opérateur qui a effectué le mouvement physique
        /// </summary>
        [MaxLength(100)]
        public string? Operator { get; set; }

        /// <summary>
        /// Superviseur qui a validé le mouvement
        /// </summary>
        [MaxLength(100)]
        public string? Supervisor { get; set; }

        // ================================================================
        // STATUT ET VALIDATION
        // ================================================================

        /// <summary>
        /// Statut du mouvement
        /// Planned, In_Progress, Completed, Cancelled, Error
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Completed";

        /// <summary>
        /// Indique si le mouvement a été validé/confirmé
        /// </summary>
        public bool IsConfirmed { get; set; } = true;

        /// <summary>
        /// Date de confirmation
        /// </summary>
        public DateTime? ConfirmedDate { get; set; }

        /// <summary>
        /// Utilisateur qui a confirmé
        /// </summary>
        [MaxLength(100)]
        public string? ConfirmedBy { get; set; }

        /// <summary>
        /// Indique si le mouvement a été annulé
        /// </summary>
        public bool IsCancelled { get; set; } = false;

        /// <summary>
        /// Date d'annulation
        /// </summary>
        public DateTime? CancelledDate { get; set; }

        /// <summary>
        /// Utilisateur qui a annulé
        /// </summary>
        [MaxLength(100)]
        public string? CancelledBy { get; set; }

        /// <summary>
        /// Raison de l'annulation
        /// </summary>
        [MaxLength(500)]
        public string? CancellationReason { get; set; }

        // ================================================================
        // QUALITÉ ET CONTRÔLE
        // ================================================================

        /// <summary>
        /// Condition du produit lors du mouvement
        /// </summary>
        [MaxLength(50)]
        public string? Condition { get; set; }

        /// <summary>
        /// Score de qualité constaté
        /// </summary>
        [Range(0, 100)]
        public int? QualityScore { get; set; }

        /// <summary>
        /// Contrôle qualité effectué
        /// </summary>
        public bool QualityControlDone { get; set; } = false;

        /// <summary>
        /// Résultat du contrôle qualité
        /// </summary>
        [MaxLength(100)]
        public string? QualityControlResult { get; set; }

        /// <summary>
        /// Contrôleur qualité
        /// </summary>
        [MaxLength(100)]
        public string? QualityController { get; set; }

        /// <summary>
        /// Inspection requise
        /// </summary>
        public bool RequiresInspection { get; set; } = false;

        /// <summary>
        /// Inspection effectuée
        /// </summary>
        public bool InspectionDone { get; set; } = false;

        /// <summary>
        /// Résultat de l'inspection
        /// </summary>
        [MaxLength(100)]
        public string? InspectionResult { get; set; }

        // ================================================================
        // INFORMATIONS TECHNIQUES
        // ================================================================

        /// <summary>
        /// Méthode de mouvement
        /// Manual, Automatic, System_Generated, Import
        /// </summary>
        [MaxLength(50)]
        public string Method { get; set; } = "Manual";

        /// <summary>
        /// Source du mouvement
        /// WMS, ERP, Mobile_App, API, Manual_Entry
        /// </summary>
        [MaxLength(50)]
        public string Source { get; set; } = "ERP";

        /// <summary>
        /// Interface utilisée
        /// </summary>
        [MaxLength(100)]
        public string? Interface { get; set; }

        /// <summary>
        /// Adresse IP de l'utilisateur
        /// </summary>
        [MaxLength(50)]
        public string? UserIP { get; set; }

        /// <summary>
        /// Dispositif utilisé
        /// </summary>
        [MaxLength(100)]
        public string? Device { get; set; }

        /// <summary>
        /// Géolocalisation du mouvement
        /// </summary>
        [MaxLength(100)]
        public string? Geolocation { get; set; }

        // ================================================================
        // DOCUMENTS ET PIÈCES JOINTES
        // ================================================================

        /// <summary>
        /// URLs des documents associés (format JSON)
        /// Ex: ["receipt.pdf", "quality_report.pdf", "photos.zip"]
        /// </summary>
        [MaxLength(2000)]
        public string? DocumentUrls { get; set; }

        /// <summary>
        /// URLs des photos du mouvement
        /// </summary>
        [MaxLength(1000)]
        public string? PhotoUrls { get; set; }

        /// <summary>
        /// Signature électronique (base64)
        /// </summary>
        [MaxLength(5000)]
        public string? ElectronicSignature { get; set; }

        /// <summary>
        /// Nom du signataire
        /// </summary>
        [MaxLength(100)]
        public string? SignedBy { get; set; }

        // ================================================================
        // COMMENTAIRES ET NOTES
        // ================================================================

        /// <summary>
        /// Raison du mouvement
        /// </summary>
        [MaxLength(500)]
        public string? Reason { get; set; }

        /// <summary>
        /// Commentaires détaillés
        /// </summary>
        [MaxLength(1000)]
        public string? Comments { get; set; }

        /// <summary>
        /// Notes internes
        /// </summary>
        [MaxLength(1000)]
        public string? InternalNotes { get; set; }

        /// <summary>
        /// Instructions spéciales
        /// </summary>
        [MaxLength(500)]
        public string? SpecialInstructions { get; set; }

        /// <summary>
        /// Remarques sur la qualité
        /// </summary>
        [MaxLength(500)]
        public string? QualityRemarks { get; set; }

        // ================================================================
        // MÉTADONNÉES ET CONFIGURATION
        // ================================================================

        /// <summary>
        /// Priorité du mouvement (1=Très urgent, 5=Normal)
        /// </summary>
        [Range(1, 5)]
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Tags pour catégorisation (format JSON)
        /// </summary>
        [MaxLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// Métadonnées supplémentaires (format JSON)
        /// </summary>
        [MaxLength(2000)]
        public string? Metadata { get; set; }

        /// <summary>
        /// Configuration spécifique (format JSON)
        /// </summary>
        [MaxLength(1000)]
        public string? Configuration { get; set; }

        /// <summary>
        /// Données d'audit étendues (format JSON)
        /// </summary>
        [MaxLength(2000)]
        public string? AuditData { get; set; }

        // ================================================================
        // INTÉGRATIONS EXTERNES
        // ================================================================

        /// <summary>
        /// ID dans le système externe (WMS, ERP tiers, etc.)
        /// </summary>
        [MaxLength(100)]
        public string? ExternalSystemId { get; set; }

        /// <summary>
        /// Nom du système externe
        /// </summary>
        [MaxLength(100)]
        public string? ExternalSystemName { get; set; }

        /// <summary>
        /// Données de synchronisation (format JSON)
        /// </summary>
        [MaxLength(1000)]
        public string? SyncData { get; set; }

        /// <summary>
        /// Statut de synchronisation
        /// </summary>
        [MaxLength(50)]
        public string? SyncStatus { get; set; }

        /// <summary>
        /// Date de dernière synchronisation
        /// </summary>
        public DateTime? LastSyncDate { get; set; }

        // ================================================================
        // PERFORMANCES ET MÉTRIQUES
        // ================================================================

        /// <summary>
        /// Temps de traitement du mouvement (en minutes)
        /// </summary>
        public decimal? ProcessingTime { get; set; }

        /// <summary>
        /// Temps de déplacement physique (en minutes)
        /// </summary>
        public decimal? HandlingTime { get; set; }

        /// <summary>
        /// Distance parcourue pour le mouvement (en mètres)
        /// </summary>
        public decimal? DistanceTraveled { get; set; }

        /// <summary>
        /// Coût de la main-d'œuvre pour ce mouvement
        /// </summary>
        public decimal? LaborCost { get; set; }

        /// <summary>
        /// Coût de transport/manutention
        /// </summary>
        public decimal? HandlingCost { get; set; }

        /// <summary>
        /// Efficacité du mouvement (score 1-100)
        /// </summary>
        [Range(1, 100)]
        public int? EfficiencyScore { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES
        // ================================================================

        /// <summary>
        /// Entrepôt concerné par le mouvement
        /// </summary>
        public virtual Warehouse Warehouse { get; set; } = null!;

        /// <summary>
        /// Emplacement concerné par le mouvement
        /// </summary>
        public virtual Location Location { get; set; } = null!;

        /// <summary>
        /// Entrepôt source (pour les transferts)
        /// </summary>
        public virtual Warehouse? SourceWarehouse { get; set; }

        /// <summary>
        /// Emplacement source (pour les transferts)
        /// </summary>
        public virtual Location? SourceLocation { get; set; }

        /// <summary>
        /// Entrepôt destination (pour les transferts)
        /// </summary>
        public virtual Warehouse? DestinationWarehouse { get; set; }

        /// <summary>
        /// Emplacement destination (pour les transferts)
        /// </summary>
        public virtual Location? DestinationLocation { get; set; }

        /// <summary>
        /// Stock associé (référence faible)
        /// </summary>
        public virtual Stock? Stock { get; set; }

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Identifiant unique du mouvement pour affichage
        /// </summary>
        public string MovementId => $"{MovementNumber}-{ProductVariantSKU}";

        /// <summary>
        /// Description courte du mouvement
        /// </summary>
        public string ShortDescription => $"{MovementType} - {Direction} - {Quantity} units";

        /// <summary>
        /// Description complète du mouvement
        /// </summary>
        public string FullDescription => $"{MovementType} {Direction}: {Quantity} units of {ProductVariantSKU} @ {Location?.Code} on {EffectiveDate:yyyy-MM-dd}";

        /// <summary>
        /// Indique si c'est un mouvement d'entrée
        /// </summary>
        public bool IsInbound => Direction == "In";

        /// <summary>
        /// Indique si c'est un mouvement de sortie
        /// </summary>
        public bool IsOutbound => Direction == "Out";

        /// <summary>
        /// Indique si c'est un transfert
        /// </summary>
        public bool IsTransfer => Direction == "Transfer" || MovementType == "Transfer";

        /// <summary>
        /// Indique si le mouvement est en retard
        /// </summary>
        public bool IsLate => PlannedDate.HasValue && EffectiveDate > PlannedDate.Value;

        /// <summary>
        /// Nombre de jours de retard
        /// </summary>
        public int? DaysLate => IsLate && PlannedDate.HasValue
            ? (EffectiveDate - PlannedDate.Value).Days
            : null;

        /// <summary>
        /// Âge du mouvement en jours
        /// </summary>
        public int AgeInDays => (DateTime.UtcNow - EffectiveDate).Days;

        /// <summary>
        /// Indique si le mouvement est récent (moins de 24h)
        /// </summary>
        public bool IsRecent => AgeInDays == 0;

        /// <summary>
        /// Temps de traitement total (de la planification à la confirmation)
        /// </summary>
        public TimeSpan? TotalProcessingTime => ConfirmedDate.HasValue && PlannedDate.HasValue
            ? ConfirmedDate.Value - PlannedDate.Value
            : null;

        /// <summary>
        /// Marge réalisée (pour les ventes)
        /// </summary>
        public decimal? Margin => TotalPrice.HasValue && TotalCost > 0
            ? TotalPrice.Value - TotalCost
            : null;

        /// <summary>
        /// Pourcentage de marge (pour les ventes)
        /// </summary>
        public decimal? MarginPercentage => Margin.HasValue && TotalCost > 0
            ? Math.Round(Margin.Value / TotalCost * 100, 2)
            : null;

        /// <summary>
        /// Impact financier total du mouvement
        /// </summary>
        public decimal FinancialImpact
        {
            get
            {
                return MovementType switch
                {
                    "Purchase" => TotalCost, // Coût d'achat
                    "Sale" => TotalPrice ?? 0, // Chiffre d'affaires
                    "Damage" => -TotalCost, // Perte de valeur
                    "Loss" => -TotalCost, // Perte de valeur
                    "Found" => TotalCost, // Récupération de valeur
                    _ => 0
                };
            }
        }

        /// <summary>
        /// Statut de performance du mouvement
        /// </summary>
        public string PerformanceStatus
        {
            get
            {
                if (IsCancelled) return "Cancelled";
                if (IsLate) return "Late";
                if (!IsConfirmed) return "Pending";
                if (EfficiencyScore >= 90) return "Excellent";
                if (EfficiencyScore >= 75) return "Good";
                if (EfficiencyScore >= 60) return "Average";
                return "Poor";
            }
        }

        /// <summary>
        /// Indicateur de risque
        /// </summary>
        public string RiskLevel
        {
            get
            {
                if (MovementType == "Damage" || MovementType == "Loss") return "High";
                if (RequiresInspection && !InspectionDone) return "Medium";
                if (IsLate) return "Medium";
                if (QualityScore < 70) return "Medium";
                return "Low";
            }
        }

        /// <summary>
        /// Indicateur de conformité
        /// </summary>
        public bool IsCompliant => IsConfirmed &&
                                   (!RequiresInspection || InspectionDone) &&
                                   (!QualityControlDone || QualityControlResult != "Failed") &&
                                   !IsCancelled;

        /// <summary>
        /// Score global du mouvement (0-100)
        /// </summary>
        public int OverallScore
        {
            get
            {
                int score = 100;

                if (IsCancelled) return 0;
                if (!IsConfirmed) score -= 30;
                if (IsLate) score -= 20;
                if (RequiresInspection && !InspectionDone) score -= 15;
                if (QualityScore.HasValue && QualityScore < 80) score -= 10;
                if (EfficiencyScore.HasValue && EfficiencyScore < 70) score -= 10;

                return Math.Max(0, score);
            }
        }

        /// <summary>
        /// Représentation textuelle du mouvement
        /// </summary>
        public override string ToString() => $"{MovementNumber} - {ShortDescription}";
    }

    /// <summary>
    /// Énumération des types de mouvements de stock
    /// </summary>
    public static class StockMovementTypes
    {
        public const string Purchase = "Purchase";
        public const string Sale = "Sale";
        public const string Transfer = "Transfer";
        public const string Adjustment = "Adjustment";
        public const string Return = "Return";
        public const string Damage = "Damage";
        public const string Loss = "Loss";
        public const string Found = "Found";
        public const string Production = "Production";
        public const string Consumption = "Consumption";
        public const string Inventory = "Inventory";
        public const string Quarantine = "Quarantine";
        public const string Release = "Release";
        public const string Scrap = "Scrap";
        public const string Rework = "Rework";
    }

    /// <summary>
    /// Énumération des directions de mouvement
    /// </summary>
    public static class MovementDirections
    {
        public const string In = "In";
        public const string Out = "Out";
        public const string Transfer = "Transfer";
    }

    /// <summary>
    /// Énumération des statuts de mouvement
    /// </summary>
    public static class MovementStatuses
    {
        public const string Planned = "Planned";
        public const string InProgress = "In_Progress";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string Error = "Error";
        public const string OnHold = "On_Hold";
        public const string PartiallyCompleted = "Partially_Completed";
    }
}