// ================================================================
// ENTITÉS POUR ATTRIBUTS DYNAMIQUES - ERP.Domain.Entities.Catalog
// ================================================================
// Permet de gérer tout type d'attribut (Taille, Couleur, Poids, etc.)
// ================================================================

using ERP.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Domain.Entities.Catalog
{
    /// <summary>
    /// Définition d'un type d'attribut (ex: Couleur, Taille, Poids, Famille)
    /// </summary>
    public class AttributeDefinition
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty; // COLOR, SIZE, WEIGHT, FAMILY

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // Couleur, Taille, Poids, Famille

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string DataType { get; set; } = "String"; // String, Number, Decimal, Date, Boolean, List

        [StringLength(50)]
        public string? UnitOfMeasure { get; set; } // KG, L, M, CM, etc.

        public bool IsRequired { get; set; } = false;
        public bool IsVariant { get; set; } = true; // Définit si cet attribut crée des variantes
        public bool AllowMultipleValues { get; set; } = false;

        [StringLength(50)]
        public string? ValidationRule { get; set; } // Regex ou règle de validation

        public decimal? MinValue { get; set; } // Pour les types numériques
        public decimal? MaxValue { get; set; }

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Relations
        public virtual ICollection<AttributeValue> PossibleValues { get; set; } = new List<AttributeValue>();
        public virtual ICollection<ProductTypeAttribute> ProductTypeAttributes { get; set; } = new List<ProductTypeAttribute>();

        // Multi-tenant
        public int TenantId { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }

    /// <summary>
    /// Valeurs possibles pour un attribut de type Liste
    /// </summary>
    public class AttributeValue
    {
        public int Id { get; set; }

        public int AttributeDefinitionId { get; set; }
        public virtual AttributeDefinition AttributeDefinition { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Value { get; set; } = string.Empty; // Rouge, XL, 1.5KG

        [StringLength(100)]
        public string? DisplayValue { get; set; } // Affichage personnalisé

        [StringLength(50)]
        public string? Code { get; set; } // RED, XL, etc.

        [StringLength(7)]
        public string? ColorHex { get; set; } // Pour les couleurs

        [StringLength(500)]
        public string? Description { get; set; }

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;

        // Pour hiérarchie (Famille/Sous-famille)
        public int? ParentValueId { get; set; }
        public virtual AttributeValue? ParentValue { get; set; }
        public virtual ICollection<AttributeValue> ChildValues { get; set; } = new List<AttributeValue>();

        // Multi-tenant
        public int TenantId { get; set; }
    }

    /// <summary>
    /// Association entre ProductType et AttributeDefinition
    /// Définit quels attributs sont disponibles pour quel type de produit
    /// </summary>
    public class ProductTypeAttribute
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; } = null!;

        public int AttributeDefinitionId { get; set; }
        public virtual AttributeDefinition AttributeDefinition { get; set; } = null!;

        public bool IsRequired { get; set; } = false;
        public bool IsVariant { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;

        [StringLength(100)]
        public string? DefaultValue { get; set; }

        // Configuration spécifique au type de produit
        [StringLength(2000)]
        public string? Configuration { get; set; } // JSON pour config avancée

        public bool IsActive { get; set; } = true;

        // Multi-tenant
        public int TenantId { get; set; }
    }

    /// <summary>
    /// Valeur d'attribut pour un ProductMaster
    /// </summary>
    public class ProductMasterAttribute
    {
        public int Id { get; set; }

        public int ProductMasterId { get; set; }
        public virtual ProductMaster ProductMaster { get; set; } = null!;

        public int AttributeDefinitionId { get; set; }
        public virtual AttributeDefinition AttributeDefinition { get; set; } = null!;

        // Valeur stockée selon le type
        [StringLength(500)]
        public string? StringValue { get; set; }

        public decimal? NumericValue { get; set; }

        public DateTime? DateValue { get; set; }

        public bool? BooleanValue { get; set; }

        // Pour les attributs de type Liste
        public int? AttributeValueId { get; set; }
        public virtual AttributeValue? AttributeValue { get; set; }

        // Pour stocker plusieurs valeurs (si AllowMultipleValues = true)
        [StringLength(2000)]
        public string? MultipleValuesJson { get; set; }

        public bool IsInherited { get; set; } = true; // Les variantes héritent par défaut

        // Multi-tenant
        public int TenantId { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// Valeur d'attribut pour un ProductVariant (surcharge l'attribut du master)
    /// </summary>
    public class ProductVariantAttribute
    {
        public int Id { get; set; }

        public int ProductVariantId { get; set; }
        public virtual ProductVariant ProductVariant { get; set; } = null!;

        public int AttributeDefinitionId { get; set; }
        public virtual AttributeDefinition AttributeDefinition { get; set; } = null!;

        // Valeur stockée selon le type
        [StringLength(500)]
        public string? StringValue { get; set; }

        public decimal? NumericValue { get; set; }

        public DateTime? DateValue { get; set; }

        public bool? BooleanValue { get; set; }

        // Pour les attributs de type Liste
        public int? AttributeValueId { get; set; }
        public virtual AttributeValue? AttributeValue { get; set; }

        // Configuration de prix spécifique à cette combinaison d'attributs
        public decimal? PriceAdjustment { get; set; } // +/- sur le prix de base
        public decimal? PriceMultiplier { get; set; } // Multiplicateur de prix

        // Multi-tenant
        public int TenantId { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// Template de variantes pour génération automatique
    /// </summary>
    public class VariantTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // "Vêtements Taille/Couleur"

        [StringLength(500)]
        public string? Description { get; set; }

        public int ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; } = null!;

        // Attributs utilisés pour générer les variantes
        public virtual ICollection<VariantTemplateAttribute> TemplateAttributes { get; set; } = new List<VariantTemplateAttribute>();

        // Configuration de génération
        public bool AutoGenerateSKU { get; set; } = true;
        public string? SKUPattern { get; set; } // {MASTER}-{COLOR}-{SIZE}

        public bool AutoGenerateName { get; set; } = true;
        public string? NamePattern { get; set; } // {MASTER} {COLOR} Taille {SIZE}

        public bool IsActive { get; set; } = true;

        // Multi-tenant
        public int TenantId { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// Attributs d'un template de variante
    /// </summary>
    public class VariantTemplateAttribute
    {
        public int Id { get; set; }

        public int VariantTemplateId { get; set; }
        public virtual VariantTemplate VariantTemplate { get; set; } = null!;

        public int AttributeDefinitionId { get; set; }
        public virtual AttributeDefinition AttributeDefinition { get; set; } = null!;

        public int DisplayOrder { get; set; } = 0;
        public bool IsRequired { get; set; } = true;

        // Valeurs à utiliser pour la génération
        public virtual ICollection<AttributeValue> SelectedValues { get; set; } = new List<AttributeValue>();

        // Multi-tenant
        public int TenantId { get; set; }
    }
}