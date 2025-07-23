using AutoMapper;
using ERP.Application.DTOs;
using ERP.Domain.Entities;

namespace ERP.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // ✅ Product -> ProductDto (Mapping complet)
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
                .ForMember(dest => dest.DaysInStock, opt => opt.MapFrom(src => src.DaysInStock));

            // ✅ Product -> ProductForListDto (Version allégée pour les listes)
            CreateMap<Product, ProductForListDto>()
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
                .ForMember(dest => dest.DaysInStock, opt => opt.MapFrom(src => src.DaysInStock));

            // ✅ CreateProductDto -> Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCostPrice, opt => opt.MapFrom(src => CalculateTotalCostPrice(src)))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => CalculateMargin(src)))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => CalculateMarginPercentage(src)))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Sera défini par le service
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.FlagDelete, opt => opt.MapFrom(src => false));

            // ✅ UpdateProductDto -> Product (avec conditions pour éviter l'écrasement des valeurs nulles)
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCostPrice, opt => opt.MapFrom(src => CalculateTotalCostPrice(src)))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => CalculateMargin(src)))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => CalculateMarginPercentage(src)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Sera défini par le service
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.FlagDelete, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

        // ✅ Méthodes utilitaires pour les calculs automatiques

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
        /// Calcule le pourcentage de marge ((Marge / Prix de vente) * 100)
        /// </summary>
        private static decimal CalculateMarginPercentage(CreateProductDto dto)
        {
            if (dto.SellingPrice == 0) return 0;

            var totalCost = dto.PurchasePrice + dto.TransportCost;
            var margin = dto.SellingPrice - totalCost;
            return Math.Round((margin / dto.SellingPrice) * 100, 2);
        }

        /// <summary>
        /// Calcule le pourcentage de marge pour UpdateProductDto
        /// </summary>
        private static decimal CalculateMarginPercentage(UpdateProductDto dto)
        {
            if (dto.SellingPrice == 0) return 0;

            var totalCost = dto.PurchasePrice + dto.TransportCost;
            var margin = dto.SellingPrice - totalCost;
            return Math.Round((margin / dto.SellingPrice) * 100, 2);
        }
    }
}