using ERP.Application.DTOs;

namespace ERP.Application.Interfaces.IServices
{
    public interface IProductService
    {
        // ================================================================
        // MÉTHODES CRUD DE BASE
        // ================================================================
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);

        // ================================================================
        // MÉTHODES DE RECHERCHE ET FILTRAGE AVEC NOUVELLES RELATIONS
        // ================================================================
        Task<PagedResultDto<ProductForListDto>> GetProductsPagedAsync(ProductFilterDto filter);
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string query);

        // Recherche par nouvelles relations (IDs)
        Task<IEnumerable<ProductDto>> GetProductsByProductTypeAsync(int productTypeId);
        Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId);
        Task<IEnumerable<ProductDto>> GetProductsByModelAsync(int modelId);
        Task<IEnumerable<ProductDto>> GetProductsByColorAsync(int colorId);
        Task<IEnumerable<ProductDto>> GetProductsByConditionAsync(int conditionId);

        // Recherche par autres critères
        Task<IEnumerable<ProductDto>> GetProductsBySupplierAsync(string supplierName);
        Task<IEnumerable<ProductDto>> GetProductsByBatchAsync(string importBatch);
        Task<IEnumerable<ProductDto>> GetProductsByStatusAsync(string status);

        // ================================================================
        // MÉTHODES DE GESTION DU STOCK
        // ================================================================
        Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int? threshold = null);
        Task<bool> UpdateStockAsync(int productId, int newStock);
        Task<bool> AdjustStockAsync(int productId, int adjustment);
        Task<BulkOperationResultDto> BulkUpdateStockAsync(List<int> productIds, int stockAdjustment);

        // ================================================================
        // MÉTHODES POUR LES DROPDOWNS EN CASCADE
        // ================================================================
        Task<IEnumerable<DropdownOptionDto>> GetProductTypesForDropdownAsync();
        Task<IEnumerable<BrandDropdownDto>> GetBrandsForDropdownAsync(int? productTypeId = null);
        Task<IEnumerable<ModelDropdownDto>> GetModelsForDropdownAsync(int? productTypeId = null, int? brandId = null);
        Task<IEnumerable<ColorDropdownDto>> GetColorsForDropdownAsync();
        Task<IEnumerable<ConditionDropdownDto>> GetConditionsForDropdownAsync();

        // ================================================================
        // MÉTHODES POUR LES LISTES OPTIMISÉES
        // ================================================================
        Task<IEnumerable<ProductForListDto>> GetProductsForListAsync();
        Task<IEnumerable<ProductForListDto>> GetProductsForListByTypeAsync(int productTypeId);
        Task<IEnumerable<ProductForListDto>> GetProductsForListByBrandAsync(int brandId);

        // ================================================================
        // MÉTHODES DE STATISTIQUES ET RAPPORTS
        // ================================================================
        Task<ProductStatsDto> GetProductStatsAsync();
        Task<IEnumerable<ProductTypeStatsDto>> GetProductTypeStatsAsync();
        Task<IEnumerable<BrandStatsDto>> GetBrandStatsAsync();
        Task<decimal> GetTotalStockValueAsync();
        Task<decimal> GetTotalMarginAsync();
        Task<decimal> GetAverageMarginPercentageAsync();

        // ================================================================
        // MÉTHODES UTILITAIRES
        // ================================================================
        Task<bool> ProductExistsAsync(int id);
        Task<int> GetProductCountAsync();
        Task<int> GetProductCountByTypeAsync(int productTypeId);
        Task<int> GetProductCountByBrandAsync(int brandId);

        // ================================================================
        // MÉTHODES DE VALIDATION DES RELATIONS
        // ================================================================
        Task<bool> ValidateProductRelationsAsync(int productTypeId, int brandId, int modelId, int colorId, int conditionId);
        Task<bool> IsValidBrandForProductTypeAsync(int brandId, int productTypeId);
        Task<bool> IsValidModelForBrandAsync(int modelId, int brandId);

        // ================================================================
        // MÉTHODES D'OPÉRATIONS EN LOT
        // ================================================================
        Task<BulkOperationResultDto> BulkUpdatePricesAsync(List<int> productIds, decimal priceAdjustmentPercentage);
        Task<BulkOperationResultDto> BulkUpdateStatusAsync(List<int> productIds, string newStatus);
        Task<BulkOperationResultDto> BulkDeleteAsync(List<int> productIds);
    }

    // ================================================================
    // INTERFACES POUR LES SERVICES DES TABLES DE RÉFÉRENCE
    // ================================================================

    public interface IProductTypeService
    {
        Task<IEnumerable<ProductTypeDto>> GetAllProductTypesAsync();
        Task<ProductTypeDto?> GetProductTypeByIdAsync(int id);
        Task<ProductTypeDto> CreateProductTypeAsync(ProductTypeDto productTypeDto);
        Task<ProductTypeDto?> UpdateProductTypeAsync(int id, ProductTypeDto productTypeDto);
        Task<bool> DeleteProductTypeAsync(int id);
        Task<IEnumerable<DropdownOptionDto>> GetProductTypesForDropdownAsync();
    }

    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        Task<IEnumerable<BrandDto>> GetBrandsByProductTypeAsync(int productTypeId);
        Task<BrandDto?> GetBrandByIdAsync(int id);
        Task<BrandDto> CreateBrandAsync(BrandDto brandDto);
        Task<BrandDto?> UpdateBrandAsync(int id, BrandDto brandDto);
        Task<bool> DeleteBrandAsync(int id);
        Task<IEnumerable<BrandDropdownDto>> GetBrandsForDropdownAsync(int? productTypeId = null);
    }

    public interface IModelService
    {
        Task<IEnumerable<ModelDto>> GetAllModelsAsync();
        Task<IEnumerable<ModelDto>> GetModelsByBrandAsync(int brandId);
        Task<IEnumerable<ModelDto>> GetModelsByProductTypeAsync(int productTypeId);
        Task<ModelDto?> GetModelByIdAsync(int id);
        Task<ModelDto> CreateModelAsync(ModelDto modelDto);
        Task<ModelDto?> UpdateModelAsync(int id, ModelDto modelDto);
        Task<bool> DeleteModelAsync(int id);
        Task<IEnumerable<ModelDropdownDto>> GetModelsForDropdownAsync(int? productTypeId = null, int? brandId = null);
    }

    public interface IColorService
    {
        Task<IEnumerable<ColorDto>> GetAllColorsAsync();
        Task<ColorDto?> GetColorByIdAsync(int id);
        Task<ColorDto> CreateColorAsync(ColorDto colorDto);
        Task<ColorDto?> UpdateColorAsync(int id, ColorDto colorDto);
        Task<bool> DeleteColorAsync(int id);
        Task<IEnumerable<ColorDropdownDto>> GetColorsForDropdownAsync();
    }

    public interface IConditionService
    {
        Task<IEnumerable<ConditionDto>> GetAllConditionsAsync();
        Task<ConditionDto?> GetConditionByIdAsync(int id);
        Task<ConditionDto> CreateConditionAsync(ConditionDto conditionDto);
        Task<ConditionDto?> UpdateConditionAsync(int id, ConditionDto conditionDto);
        Task<bool> DeleteConditionAsync(int id);
        Task<IEnumerable<ConditionDropdownDto>> GetConditionsForDropdownAsync();
    }
}