// ================================================================
// PROFIL DE MAPPING POUR LA NOUVELLE ARCHITECTURE
// ================================================================
// Mappings séparés par bounded context avec calculs automatiques
// ================================================================

using AutoMapper;
using ERP.Application.DTOs;
using ERP.Domain.Entities;
using ERP.Domain.Entities.Product;
using ERP.Domain.Entities.Catalog;
using ERP.Domain.Entities.Inventory;

namespace ERP.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // ================================================================
            // MAPPINGS CATALOG CONTEXT - PRODUCT MASTER/VARIANT
            // ================================================================
            ConfigureProductMasterMappings();
            ConfigureProductVariantMappings();

            // ================================================================
            // MAPPINGS INVENTORY CONTEXT - WAREHOUSE/LOCATION/STOCK
            // ================================================================
            ConfigureWarehouseMappings();
            ConfigureLocationMappings();
            ConfigureStockMappings();
            ConfigureStockMovementMappings();

            // ================================================================
            // MAPPINGS LEGACY - PRODUCT (à maintenir pour migration)
            // ================================================================
            ConfigureLegacyProductMappings();

            // ================================================================
            // MAPPINGS REFERENCE TABLES - SHARED KERNEL
            // ================================================================
            ConfigureReferenceMappings();
        }

        // ================================================================
        // PRODUCT MASTER MAPPINGS
        // ================================================================
        private void ConfigureProductMasterMappings()
        {
            // ProductMaster -> ProductMasterDto
            CreateMap<ProductMaster, ProductMasterDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ActiveVariantsCount, opt => opt.MapFrom(src => src.ActiveVariantsCount))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.MinPrice))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice))
                .ForMember(dest => dest.HasStockVariants, opt => opt.MapFrom(src => src.HasStockVariants));

            // CreateProductMasterDto -> ProductMaster
            CreateMap<CreateProductMasterDto, ProductMaster>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Draft"))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Sera défini par le service
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                // Navigation properties
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore());
        }

        // ================================================================
        // PRODUCT VARIANT MAPPINGS
        // ================================================================
        private void ConfigureProductVariantMappings()
        {
            // ProductVariant -> ProductVariantDto
            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(dest => dest.ProductMasterName, opt => opt.MapFrom(src => src.ProductMaster.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color != null ? src.Color.Name : null))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color != null ? src.Color.HexCode : null))
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition.Name))
                .ForMember(dest => dest.ConditionQualityPercentage, opt => opt.MapFrom(src => src.Condition.QualityPercentage))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.IsWarrantyValid, opt => opt.MapFrom(src => src.IsWarrantyValid))
                .ForMember(dest => dest.WarrantyDaysRemaining, opt => opt.MapFrom(src => src.WarrantyDaysRemaining))
                .ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => src.IsNew))
                .ForMember(dest => dest.AgeInDays, opt => opt.MapFrom(src => src.AgeInDays))
                // Stock info - sera rempli par le service depuis Inventory Context
                .ForMember(dest => dest.TotalStock, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableStock, opt => opt.Ignore())
                .ForMember(dest => dest.IsInStock, opt => opt.Ignore())
                .ForMember(dest => dest.IsLowStock, opt => opt.Ignore());

            // CreateProductVariantDto -> ProductVariant
            CreateMap<CreateProductVariantDto, ProductVariant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCostPrice, opt => opt.MapFrom(src => CalculateTotalCostPrice(src)))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => CalculateMargin(src)))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => CalculateMarginPercentage(src)))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => GenerateFullName(src)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Available"))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                // Navigation properties
                .ForMember(dest => dest.ProductMaster, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                .ForMember(dest => dest.Condition, opt => opt.Ignore());
        }

        // ================================================================
        // WAREHOUSE MAPPINGS
        // ================================================================
        private void ConfigureWarehouseMappings()
        {
            // Warehouse -> WarehouseDto
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.FullAddress))
                .ForMember(dest => dest.OccupancyRate, opt => opt.MapFrom(src => src.OccupancyRate))
                .ForMember(dest => dest.TotalStockItems, opt => opt.MapFrom(src => src.TotalStockItems))
                .ForMember(dest => dest.TotalStockValue, opt => opt.MapFrom(src => src.TotalStockValue))
                .ForMember(dest => dest.OccupiedLocations, opt => opt.MapFrom(src => src.OccupiedLocations))
                .ForMember(dest => dest.AvailableLocations, opt => opt.MapFrom(src => src.AvailableLocations));

            // CreateWarehouseDto -> Warehouse (si nécessaire)
            CreateMap<WarehouseDto, Warehouse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Locations, opt => opt.Ignore())
                .ForMember(dest => dest.Stocks, opt => opt.Ignore())
                .ForMember(dest => dest.StockMovements, opt => opt.Ignore());
        }

        // ================================================================
        // LOCATION MAPPINGS
        // ================================================================
        private void ConfigureLocationMappings()
        {
            // Location -> LocationDto
            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
                .ForMember(dest => dest.WarehouseCode, opt => opt.MapFrom(src => src.Warehouse.Code))
                .ForMember(dest => dest.FullCode, opt => opt.MapFrom(src => src.FullCode))
                .ForMember(dest => dest.CurrentVolume, opt => opt.MapFrom(src => src.CurrentVolume))
                .ForMember(dest => dest.CurrentWeight, opt => opt.MapFrom(src => src.CurrentWeight))
                .ForMember(dest => dest.CurrentItemCount, opt => opt.MapFrom(src => src.CurrentItemCount))
                .ForMember(dest => dest.VolumeOccupancyPercentage, opt => opt.MapFrom(src => src.VolumeOccupancyPercentage))
                .ForMember(dest => dest.WeightOccupancyPercentage, opt => opt.MapFrom(src => src.WeightOccupancyPercentage))
                .ForMember(dest => dest.ItemOccupancyPercentage, opt => opt.MapFrom(src => src.ItemOccupancyPercentage));
        }

        // ================================================================
        // STOCK MAPPINGS
        // ================================================================
        private void ConfigureStockMappings()
        {
            // Stock -> StockDto
            CreateMap<Stock, StockDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
                .ForMember(dest => dest.WarehouseCode, opt => opt.MapFrom(src => src.Warehouse.Code))
                .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.Location.Code))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.QuantityAvailable, opt => opt.MapFrom(src => src.QuantityAvailable))
                .ForMember(dest => dest.QuantityTotal, opt => opt.MapFrom(src => src.QuantityTotal))
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
                .ForMember(dest => dest.ReplacementValue, opt => opt.MapFrom(src => src.ReplacementValue))
                .ForMember(dest => dest.WeightOccupied, opt => opt.MapFrom(src => src.WeightOccupied))
                .ForMember(dest => dest.VolumeOccupied, opt => opt.MapFrom(src => src.VolumeOccupied))
                .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                .ForMember(dest => dest.IsOutOfStock, opt => opt.MapFrom(src => src.IsOutOfStock))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
                .ForMember(dest => dest.IsCriticalStock, opt => opt.MapFrom(src => src.IsCriticalStock))
                .ForMember(dest => dest.IsOverStock, opt => opt.MapFrom(src => src.IsOverStock))
                .ForMember(dest => dest.IsNearExpiry, opt => opt.MapFrom(src => src.IsNearExpiry))
                .ForMember(dest => dest.IsExpired, opt => opt.MapFrom(src => src.IsExpired))
                .ForMember(dest => dest.DaysToExpiry, opt => opt.MapFrom(src => src.DaysToExpiry))
                .ForMember(dest => dest.AgeInDays, opt => opt.MapFrom(src => src.AgeInDays))
                .ForMember(dest => dest.DaysSinceLastMovement, opt => opt.MapFrom(src => src.DaysSinceLastMovement))
                .ForMember(dest => dest.PerformanceScore, opt => opt.MapFrom(src => src.PerformanceScore))
                // ProductVariantName sera résolu par le service
                .ForMember(dest => dest.ProductVariantName, opt => opt.Ignore());

            // CreateStockDto -> Stock
            CreateMap<CreateStockDto, Stock>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FirstStockDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastStockInDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Available"))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.RotationMethod, opt => opt.MapFrom(src => "FIFO"))
                .ForMember(dest => dest.AllocationPriority, opt => opt.MapFrom(src => 100))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.StockMovements, opt => opt.Ignore());
        }

        // ================================================================
        // STOCK MOVEMENT MAPPINGS
        // ================================================================
        private void ConfigureStockMovementMappings()
        {
            // StockMovement -> StockMovementDto
            CreateMap<StockMovement, StockMovementDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
                .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.Location.Code))
                .ForMember(dest => dest.SourceWarehouseName, opt => opt.MapFrom(src => src.SourceWarehouse != null ? src.SourceWarehouse.Name : null))
                .ForMember(dest => dest.SourceLocationCode, opt => opt.MapFrom(src => src.SourceLocation != null ? src.SourceLocation.Code : null))
                .ForMember(dest => dest.DestinationWarehouseName, opt => opt.MapFrom(src => src.DestinationWarehouse != null ? src.DestinationWarehouse.Name : null))
                .ForMember(dest => dest.DestinationLocationCode, opt => opt.MapFrom(src => src.DestinationLocation != null ? src.DestinationLocation.Code : null))
                .ForMember(dest => dest.SignedQuantity, opt => opt.MapFrom(src => src.SignedQuantity))
                .ForMember(dest => dest.MovementId, opt => opt.MapFrom(src => src.MovementId))
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.ShortDescription))
                .ForMember(dest => dest.FullDescription, opt => opt.MapFrom(src => src.FullDescription))
                .ForMember(dest => dest.IsInbound, opt => opt.MapFrom(src => src.IsInbound))
                .ForMember(dest => dest.IsOutbound, opt => opt.MapFrom(src => src.IsOutbound))
                .ForMember(dest => dest.IsTransfer, opt => opt.MapFrom(src => src.IsTransfer))
                .ForMember(dest => dest.IsLate, opt => opt.MapFrom(src => src.IsLate))
                .ForMember(dest => dest.DaysLate, opt => opt.MapFrom(src => src.DaysLate))
                .ForMember(dest => dest.IsRecent, opt => opt.MapFrom(src => src.IsRecent))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => src.Margin))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => src.MarginPercentage))
                .ForMember(dest => dest.FinancialImpact, opt => opt.MapFrom(src => src.FinancialImpact))
                .ForMember(dest => dest.PerformanceStatus, opt => opt.MapFrom(src => src.PerformanceStatus))
                .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => src.RiskLevel))
                .ForMember(dest => dest.IsCompliant, opt => opt.MapFrom(src => src.IsCompliant))
                .ForMember(dest => dest.OverallScore, opt => opt.MapFrom(src => src.OverallScore))
                // ProductVariantName sera résolu par le service
                .ForMember(dest => dest.ProductVariantName, opt => opt.Ignore());

            // CreateStockMovementDto -> StockMovement
            CreateMap<CreateStockMovementDto, StockMovement>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MovementNumber, opt => opt.Ignore()) // Généré par le service
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.Quantity * src.UnitCost))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.UnitPrice.HasValue ? src.Quantity * src.UnitPrice.Value : (decimal?)null))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Completed"))
                .ForMember(dest => dest.IsConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.ConfirmedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => "Manual"))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => "ERP"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForMember(dest => dest.SourceWarehouse, opt => opt.Ignore())
                .ForMember(dest => dest.SourceLocation, opt => opt.Ignore())
                .ForMember(dest => dest.DestinationWarehouse, opt => opt.Ignore())
                .ForMember(dest => dest.DestinationLocation, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.Ignore());
        }

        // ================================================================
        // LEGACY PRODUCT MAPPINGS (pour migration)
        // ================================================================
        private void ConfigureLegacyProductMappings()
        {
            // Product -> ProductDto (mapping existant maintenu)
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.HexCode))
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition.Name))
                .ForMember(dest => dest.ConditionQualityPercentage, opt => opt.MapFrom(src => src.Condition.QualityPercentage))
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalValue))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock))
                .ForMember(dest => dest.DaysInStock, opt => opt.MapFrom(src => src.DaysInStock));

            // Product -> ProductForListDto (mapping existant maintenu)
            CreateMap<Product, ProductForListDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.HexCode))
                .ForMember(dest => dest.ConditionName, opt => opt.MapFrom(src => src.Condition.Name))
                .ForMember(dest => dest.IsLowStock, opt => opt.MapFrom(src => src.IsLowStock));

            // Mappings de création/modification (existants maintenus)
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductType, opt => opt.Ignore())
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Model, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore())
                .ForMember(dest => dest.Condition, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCostPrice, opt => opt.MapFrom(src => CalculateTotalCostPriceLegacy(src)))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => CalculateMarginLegacy(src)))
                .ForMember(dest => dest.MarginPercentage, opt => opt.MapFrom(src => CalculateMarginPercentageLegacy(src)))
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.ArrivalDate, opt => opt.MapFrom(src => src.ArrivalDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Available"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
        }

        // ================================================================
        // REFERENCE MAPPINGS (tables de référence)
        // ================================================================
        private void ConfigureReferenceMappings()
        {
            // ProductType mappings
            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<ProductType, DropdownOptionDto>();

            // Brand mappings
            CreateMap<Brand, BrandDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name));
            CreateMap<Brand, BrandDropdownDto>();

            // Model mappings
            CreateMap<Model, ModelDto>()
                .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));
            CreateMap<Model, ModelDropdownDto>();

            // Color mappings
            CreateMap<Color, ColorDto>();
            CreateMap<Color, ColorDropdownDto>();

            // Condition mappings
            CreateMap<Condition, ConditionDto>();
            CreateMap<Condition, ConditionDropdownDto>();
        }

        // ================================================================
        // MÉTHODES UTILITAIRES POUR LES CALCULS
        // ================================================================

        /// <summary>
        /// Calcule le coût total pour ProductVariant
        /// </summary>
        private static decimal CalculateTotalCostPrice(CreateProductVariantDto dto)
        {
            return dto.PurchasePrice + dto.AdditionalCosts;
        }

        /// <summary>
        /// Calcule la marge pour ProductVariant
        /// </summary>
        private static decimal CalculateMargin(CreateProductVariantDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.AdditionalCosts;
            return dto.SellingPrice - totalCost;
        }

        /// <summary>
        /// Calcule le pourcentage de marge pour ProductVariant
        /// </summary>
        private static decimal CalculateMarginPercentage(CreateProductVariantDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.AdditionalCosts;
            if (totalCost == 0) return 0;

            var margin = dto.SellingPrice - totalCost;
            return Math.Round((margin / totalCost) * 100, 2);
        }

        /// <summary>
        /// Génère le nom complet de la variante
        /// </summary>
        private static string GenerateFullName(CreateProductVariantDto dto)
        {
            // Le nom complet sera généré par le service qui a accès au ProductMaster
            return dto.VariantName;
        }

        /// <summary>
        /// Calcule le coût total pour Product legacy
        /// </summary>
        private static decimal CalculateTotalCostPriceLegacy(CreateProductDto dto)
        {
            return dto.PurchasePrice + dto.TransportCost;
        }

        /// <summary>
        /// Calcule la marge pour Product legacy
        /// </summary>
        private static decimal CalculateMarginLegacy(CreateProductDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.TransportCost;
            return dto.SellingPrice - totalCost;
        }

        /// <summary>
        /// Calcule le pourcentage de marge pour Product legacy
        /// </summary>
        private static decimal CalculateMarginPercentageLegacy(CreateProductDto dto)
        {
            var totalCost = dto.PurchasePrice + dto.TransportCost;
            if (totalCost == 0) return 0;

            var margin = dto.SellingPrice - totalCost;
            return Math.Round((margin / totalCost) * 100, 2);
        }
    }
}