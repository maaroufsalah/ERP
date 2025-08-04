using AutoMapper;
using ERP.Application.DTOs;
using ERP.Domain.Entities;
using ERP.Domain.Entities.Product;

namespace ERP.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // ================================================================
            // MAPPINGS PRINCIPAUX - PRODUCT ENTITY VERS DTOs
            // ================================================================

            // ✅ Product -> ProductDto (Mapping complet avec relations)
            CreateMap<Product, ProductDto>()
                // Navigation Properties - Noms des entités liées
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.HexCode))
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition.Name))
                .ForMember(dest => dest.ConditionQualityPercentage, opt => opt.MapFrom(src => src.Condition.QualityPercentage))
                // Propriétés calculées
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
                .ForMember(dest => dest.DaysInStock, opt => opt.MapFrom(src => src.DaysInStock));

            // ✅ Product -> ProductForListDto (Version allégée pour les listes)
            CreateMap<Product, ProductForListDto>()
                // Relations essentielles pour les listes
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.HexCode))
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition.Name))
                // Propriétés calculées
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock));

            // ================================================================
            // MAPPINGS DTOs VERS ENTITY - CRÉATION ET MODIFICATION
            // ================================================================

            // ✅ CreateProductDto -> Product
            CreateMap<CreateProductDto, Product>()
                // Ignorer les propriétés auto-générées
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Model, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                .ForMember(dest => dest.Condition, opt => opt.Ignore())
                // Calculs automatiques
                .ForMember(dest => dest.TotalCostPrice, opt => opt.MapFrom(src => CalculateTotalCostPrice(src)))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => CalculateMargin(src)))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => CalculateMarginPercentage(src)))
                // Dates - utiliser les valeurs fournies ou valeurs par défaut
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.ArrivalDate, opt => opt.MapFrom(src => src.ArrivalDate))
                // Status par défaut
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Available"))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                // Audit - propriétés gérées par BaseAuditableEntity
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Sera défini par le service
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.FlagDelete, opt => opt.MapFrom(src => false));

            // ✅ UpdateProductDto -> Product
            CreateMap<UpdateProductDto, Product>()
                // Ne pas modifier l'ID et les relations navigation
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Model, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                .ForMember(dest => dest.Condition, opt => opt.Ignore())
                // Recalculer les valeurs financières
                .ForMember(dest => dest.TotalCostPrice, opt => opt.MapFrom(src => CalculateTotalCostPrice(src)))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => CalculateMargin(src)))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => CalculateMarginPercentage(src)))
                // Audit pour modification
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Sera défini par le service
                                                                        // Ne pas modifier les propriétés de création
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.FlagDelete, opt => opt.Ignore())
                // Ignorer les valeurs nulles lors de la mise à jour
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ================================================================
            // MAPPINGS POUR LES TABLES DE RÉFÉRENCE
            // ================================================================

            // ✅ ProductType -> ProductTypeDto
            CreateMap<ProductType, ProductTypeDto>();

            // ✅ Brand -> BrandDto
            CreateMap<Brand, BrandDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name));

            // ✅ Model -> ModelDto
            CreateMap<Model, ModelDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));

            // ✅ Color -> ColorDto
            CreateMap<Color, ColorDto>();

            // ✅ Condition -> ConditionDto
            CreateMap<Condition, ConditionDto>();

            // ================================================================
            // MAPPINGS POUR LES DROPDOWNS
            // ================================================================

            // ✅ ProductType -> DropdownOptionDto
            CreateMap<ProductType, DropdownOptionDto>();

            // ✅ Brand -> BrandDropdownDto
            CreateMap<Brand, BrandDropdownDto>();

            // ✅ Model -> ModelDropdownDto
            CreateMap<Model, ModelDropdownDto>();

            // ✅ Color -> ColorDropdownDto
            CreateMap<Color, ColorDropdownDto>();

            // ✅ Condition -> ConditionDropdownDto
            CreateMap<Condition, ConditionDropdownDto>();
        }

        // ================================================================
        // MÉTHODES UTILITAIRES POUR LES CALCULS AUTOMATIQUES
        // ================================================================

        /// <summary>
        /// Calcule le coût total (Prix d'achat + Transport)
        /// </summary>
        private static decimal CalculateTotalCostPrice(CreateProductDto dto)
        {
            return dto.PurchasePrice + dto.TransportCost;
        }

        /// <summary>
        /// Calcule le coût total pour UpdateProductDto
        /// </summary>
        private static decimal CalculateTotalCostPrice(UpdateProductDto dto)
        {
            return dto.PurchasePrice + dto.TransportCost;
        }

        /// <summary>
        /// Calcule la marge bénéficiaire (Prix de vente - Coût total)
        /// </summary>
        private static decimal CalculateMargin(CreateProductDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.TransportCost;
            return dto.SellingPrice - totalCost;
        }

        /// <summary>
        /// Calcule la marge pour UpdateProductDto
        /// </summary>
        private static decimal CalculateMargin(UpdateProductDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.TransportCost;
            return dto.SellingPrice - totalCost;
        }

        /// <summary>
        /// Calcule le pourcentage de marge ((Marge / Coût total) * 100)
        /// Corrigé pour utiliser le coût total comme base de calcul
        /// </summary>
        private static decimal CalculateMarginPercentage(CreateProductDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.TransportCost;
            if (totalCost == 0) return 0;

            var margin = dto.SellingPrice - totalCost;
            return Math.Round((margin / totalCost) * 100, 2);
        }

        /// <summary>
        /// Calcule le pourcentage de marge pour UpdateProductDto
        /// </summary>
        private static decimal CalculateMarginPercentage(UpdateProductDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.TransportCost;
            if (totalCost == 0) return 0;

            var margin = dto.SellingPrice - totalCost;
            return Math.Round((margin / totalCost) * 100, 2);
        }
    }
}