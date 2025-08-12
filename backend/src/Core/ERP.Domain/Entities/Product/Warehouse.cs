// ================================================================
// ENTITÉ WAREHOUSE - ENTREPÔTS/MAGASINS (INVENTORY CONTEXT)
// ================================================================
// Lieux de stockage physique des produits
// ================================================================

using System.ComponentModel.DataAnnotations;
using ERP.Domain.Entities.Shared;

namespace ERP.Domain.Entities.Inventory
{
    /// <summary>
    /// Entrepôt ou lieu de stockage physique
    /// Exemple: Entrepôt Principal Casablanca, Magasin Rabat, Dépôt Tanger
    /// </summary>
    public class Warehouse : BaseAuditableEntity
    {
        // ================================================================
        // INFORMATIONS GÉNÉRALES
        // ================================================================

        /// <summary>
        /// Nom de l'entrepôt
        /// Ex: "Entrepôt Principal Casablanca", "Magasin Rabat Centre"
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Code court de l'entrepôt
        /// Ex: "CASA-01", "RAB-CTR", "TANG-DEP"
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Description de l'entrepôt
        /// </summary>
        [MaxLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Type d'entrepôt
        /// Ex: "Warehouse", "Store", "Distribution_Center", "Supplier_Dropship"
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = "Warehouse";

        // ================================================================
        // LOCALISATION
        // ================================================================

        /// <summary>
        /// Adresse complète de l'entrepôt
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Ville
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Région/Province
        /// </summary>
        [MaxLength(100)]
        public string? Region { get; set; }

        /// <summary>
        /// Code postal
        /// </summary>
        [MaxLength(20)]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Pays
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = "Maroc";

        /// <summary>
        /// Coordonnées GPS - Latitude
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Coordonnées GPS - Longitude
        /// </summary>
        public decimal? Longitude { get; set; }

        // ================================================================
        // INFORMATIONS DE CONTACT
        // ================================================================

        /// <summary>
        /// Numéro de téléphone principal
        /// </summary>
        [MaxLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Adresse email de contact
        /// </summary>
        [MaxLength(200)]
        public string? Email { get; set; }

        /// <summary>
        /// Nom du responsable de l'entrepôt
        /// </summary>
        [MaxLength(200)]
        public string? ManagerName { get; set; }

        /// <summary>
        /// Téléphone du responsable
        /// </summary>
        [MaxLength(20)]
        public string? ManagerPhone { get; set; }

        // ================================================================
        // CARACTÉRISTIQUES PHYSIQUES
        // ================================================================

        /// <summary>
        /// Surface totale en mètres carrés
        /// </summary>
        public decimal? TotalArea { get; set; }

        /// <summary>
        /// Surface de stockage utilisable
        /// </summary>
        public decimal? StorageArea { get; set; }

        /// <summary>
        /// Capacité maximale (nombre d'articles)
        /// </summary>
        public int? MaxCapacity { get; set; }

        /// <summary>
        /// Nombre d'emplacements de stockage disponibles
        /// </summary>
        public int? TotalLocations { get; set; }

        /// <summary>
        /// Hauteur sous plafond
        /// </summary>
        public decimal? CeilingHeight { get; set; }

        /// <summary>
        /// Nombre de quais de chargement
        /// </summary>
        public int? LoadingDocks { get; set; }

        // ================================================================
        // ÉQUIPEMENTS ET SERVICES
        // ================================================================

        /// <summary>
        /// Équipements disponibles (format JSON)
        /// Ex: ["forklift", "conveyor", "packaging_station", "security_system"]
        /// </summary>
        [MaxLength(1000)]
        public string? Equipment { get; set; }

        /// <summary>
        /// Services disponibles (format JSON)
        /// Ex: ["receiving", "shipping", "quality_control", "packaging", "returns"]
        /// </summary>
        [MaxLength(1000)]
        public string? Services { get; set; }

        /// <summary>
        /// Systèmes de sécurité
        /// Ex: "Alarme, Vidéosurveillance, Gardiennage 24h/24"
        /// </summary>
        [MaxLength(500)]
        public string? SecuritySystems { get; set; }

        /// <summary>
        /// Contrôle climatique disponible
        /// </summary>
        public bool HasClimateControl { get; set; } = false;

        /// <summary>
        /// Plage de température contrôlée
        /// Ex: "18-25°C"
        /// </summary>
        [MaxLength(50)]
        public string? TemperatureRange { get; set; }

        /// <summary>
        /// Contrôle d'humidité disponible
        /// </summary>
        public bool HasHumidityControl { get; set; } = false;

        // ================================================================
        // HORAIRES ET DISPONIBILITÉ
        // ================================================================

        /// <summary>
        /// Horaires d'ouverture (format JSON)
        /// Ex: {"monday": "08:00-18:00", "tuesday": "08:00-18:00", ...}
        /// </summary>
        [MaxLength(1000)]
        public string? OperatingHours { get; set; }

        /// <summary>
        /// Fuseau horaire
        /// Ex: "Africa/Casablanca"
        /// </summary>
        [MaxLength(50)]
        public string? TimeZone { get; set; } = "Africa/Casablanca";

        /// <summary>
        /// Disponible pour réception
        /// </summary>
        public bool AllowsReceiving { get; set; } = true;

        /// <summary>
        /// Disponible pour expédition
        /// </summary>
        public bool AllowsShipping { get; set; } = true;

        /// <summary>
        /// Autorise les ventes directes (magasin)
        /// </summary>
        public bool AllowsDirectSales { get; set; } = false;

        /// <summary>
        /// Autorise les transferts inter-entrepôts
        /// </summary>
        public bool AllowsTransfers { get; set; } = true;

        // ================================================================
        // STATUT ET CONFIGURATION
        // ================================================================

        /// <summary>
        /// Statut de l'entrepôt
        /// Active, Inactive, Maintenance, Closed
        /// </summary>
        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// Indique si cet entrepôt est actif
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Entrepôt principal (par défaut)
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Priorité pour l'allocation automatique de stock
        /// Plus la valeur est élevée, plus l'entrepôt est prioritaire
        /// </summary>
        public int Priority { get; set; } = 100;

        /// <summary>
        /// Date d'ouverture de l'entrepôt
        /// </summary>
        public DateTime? OpeningDate { get; set; }

        /// <summary>
        /// Date de fermeture (si applicable)
        /// </summary>
        public DateTime? ClosingDate { get; set; }

        // ================================================================
        // COÛTS ET TARIFICATION
        // ================================================================

        /// <summary>
        /// Coût mensuel de fonctionnement
        /// </summary>
        public decimal? MonthlyCost { get; set; }

        /// <summary>
        /// Coût par mètre carré
        /// </summary>
        public decimal? CostPerSquareMeter { get; set; }

        /// <summary>
        /// Devise utilisée pour les coûts
        /// </summary>
        [MaxLength(10)]
        public string? Currency { get; set; } = "MAD";

        /// <summary>
        /// Tarifs de stockage (format JSON)
        /// Ex: {"standard": 5.0, "premium": 8.0, "bulk": 3.0}
        /// </summary>
        [MaxLength(1000)]
        public string? StorageRates { get; set; }

        // ================================================================
        // INTÉGRATIONS ET SYSTÈMES
        // ================================================================

        /// <summary>
        /// Système WMS (Warehouse Management System) utilisé
        /// </summary>
        [MaxLength(100)]
        public string? WMSSystem { get; set; }

        /// <summary>
        /// Système ERP intégré
        /// </summary>
        [MaxLength(100)]
        public string? ERPSystem { get; set; }

        /// <summary>
        /// APIs externes intégrées (format JSON)
        /// Ex: ["shipping_api", "inventory_api", "customs_api"]
        /// </summary>
        [MaxLength(1000)]
        public string? ExternalIntegrations { get; set; }

        /// <summary>
        /// Configuration spécifique (format JSON)
        /// Paramètres personnalisés pour cet entrepôt
        /// </summary>
        [MaxLength(2000)]
        public string? Configuration { get; set; }

        // ================================================================
        // RÈGLES ET RESTRICTIONS
        // ================================================================

        /// <summary>
        /// Types de produits autorisés (format JSON)
        /// Ex: ["electronics", "clothing", "books"] ou null = tous autorisés
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
        /// Poids maximum par article (en kg)
        /// </summary>
        public decimal? MaxItemWeight { get; set; }

        /// <summary>
        /// Dimensions maximales par article (LxlxH en cm)
        /// Ex: "100x100x200"
        /// </summary>
        [MaxLength(50)]
        public string? MaxItemDimensions { get; set; }

        /// <summary>
        /// Exigences de conformité (format JSON)
        /// Ex: ["ISO9001", "HACCP", "customs_bonded"]
        /// </summary>
        [MaxLength(500)]
        public string? ComplianceRequirements { get; set; }

        // ================================================================
        // INFORMATIONS COMPLÉMENTAIRES
        // ================================================================

        /// <summary>
        /// Notes et remarques diverses
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }

        /// <summary>
        /// Instructions spéciales
        /// </summary>
        [MaxLength(1000)]
        public string? SpecialInstructions { get; set; }

        /// <summary>
        /// Informations d'assurance
        /// </summary>
        [MaxLength(500)]
        public string? InsuranceInfo { get; set; }

        /// <summary>
        /// Certifications obtenues
        /// Ex: "ISO 9001:2015, OHSAS 18001"
        /// </summary>
        [MaxLength(500)]
        public string? Certifications { get; set; }

        // ================================================================
        // NAVIGATION PROPERTIES
        // ================================================================

        /// <summary>
        /// Emplacements de stockage dans cet entrepôt
        /// </summary>
        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();

        /// <summary>
        /// Stocks présents dans cet entrepôt
        /// </summary>
        public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

        /// <summary>
        /// Mouvements de stock pour cet entrepôt
        /// </summary>
        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

        // ================================================================
        // PROPRIÉTÉS CALCULÉES
        // ================================================================

        /// <summary>
        /// Adresse complète formatée
        /// </summary>
        public string FullAddress => $"{Address}, {City}, {Region}, {Country}";

        /// <summary>
        /// Taux d'occupation actuel (en pourcentage)
        /// </summary>
        public decimal? OccupancyRate => TotalLocations > 0
            ? Math.Round((decimal)Locations.Count(l => l.IsOccupied) / TotalLocations * 100, 2)
            : null;

        /// <summary>
        /// Nombre total d'articles en stock
        /// </summary>
        public int TotalStockItems => Stocks?.Sum(s => s.QuantityOnHand) ?? 0;

        /// <summary>
        /// Valeur totale du stock dans cet entrepôt
        /// </summary>
        public decimal TotalStockValue => Stocks?.Sum(s => s.TotalValue) ?? 0;

        /// <summary>
        /// Nombre d'emplacements occupés
        /// </summary>
        public int OccupiedLocations => Locations?.Count(l => l.IsOccupied) ?? 0;

        /// <summary>
        /// Nombre d'emplacements libres
        /// </summary>
        public int AvailableLocations => (TotalLocations ?? 0) - OccupiedLocations;

        /// <summary>
        /// Indique si l'entrepôt est ouvert actuellement
        /// </summary>
        public bool IsCurrentlyOpen
        {
            get
            {
                if (!IsActive || Status != "Active") return false;

                // Ici on pourrait implémenter la logique des horaires
                // Pour l'instant, on considère ouvert si actif
                return true;
            }
        }

        /// <summary>
        /// Âge de l'entrepôt en jours
        /// </summary>
        public int? AgeInDays => OpeningDate.HasValue
            ? (DateTime.UtcNow - OpeningDate.Value).Days
            : null;

        /// <summary>
        /// Représentation textuelle de l'entrepôt
        /// </summary>
        public override string ToString() => $"{Code} - {Name}";
    }
}