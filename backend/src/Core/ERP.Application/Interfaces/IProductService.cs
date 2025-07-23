using ERP.Application.DTOs;

namespace ERP.Application.Interfaces.IServices
{
    public interface IProductService
    {
        // ✅ Méthodes CRUD de base
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);

        // ✅ Méthodes de recherche et filtrage
        Task<IEnumerable<ProductDto>> SearchProductsAsync(string query);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<ProductDto>> GetProductsBySupplierAsync(string supplierName);
        Task<IEnumerable<ProductDto>> GetProductsByBatchAsync(string importBatch);
        Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(string brand);
        Task<IEnumerable<ProductDto>> GetProductsByConditionAsync(string condition);
        Task<IEnumerable<ProductDto>> GetProductsByStatusAsync(string status);

        // ✅ Méthodes de gestion du stock
        Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold = 10);
        Task<bool> UpdateStockAsync(int productId, int newStock);
        Task<bool> AdjustStockAsync(int productId, int adjustment); // +5 ou -3 par exemple

        // ✅ Méthodes utilitaires
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<IEnumerable<string>> GetBrandsAsync();
        Task<IEnumerable<string>> GetSuppliersAsync();
        Task<IEnumerable<string>> GetImportBatchesAsync();
        Task<bool> ProductExistsAsync(int id);
        Task<int> GetProductCountAsync();

        // ✅ Méthodes pour les listes (version allégée)
        Task<IEnumerable<ProductForListDto>> GetProductsForListAsync();
        Task<IEnumerable<ProductForListDto>> GetProductsForListByCategoryAsync(string category);

        // ✅ Méthodes de statistiques et rapports
        Task<ProductStatsDto> GetProductStatsAsync();
        Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync();
        Task<IEnumerable<SupplierStatsDto>> GetSupplierStatsAsync();
        Task<decimal> GetTotalStockValueAsync();
        Task<decimal> GetTotalMarginAsync();
        Task<decimal> GetAverageMarginPercentageAsync();

        // ✅ Méthodes de filtrage avancé
        Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<ProductDto>> GetProductsByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ProductDto>> GetRecentArrivalsAsync(int days = 30);
        Task<IEnumerable<ProductDto>> GetProductsNeedingAttentionAsync(); // Stock faible + anciens

        // ✅ Méthodes de gestion des prix et marges
        Task<bool> UpdateSellingPriceAsync(int productId, decimal newPrice);
        Task<bool> UpdateMarginPercentageAsync(int productId, decimal targetMarginPercentage);
        Task<IEnumerable<ProductDto>> GetProductsByMarginRangeAsync(decimal minMargin, decimal maxMargin);

        // ✅ Méthodes spécifiques à l'import d'Italie
        Task<IEnumerable<ProductDto>> GetProductsBySupplierCityAsync(string city);
        Task<IEnumerable<ProductDto>> GetProductsByArrivalDateAsync(DateTime date);
        Task<IEnumerable<ProductDto>> GetProductsByInvoiceNumberAsync(string invoiceNumber);
        Task<bool> MarkProductAsSoldAsync(int productId);
        Task<bool> MarkProductAsReservedAsync(int productId);
        Task<bool> MarkProductAsAvailableAsync(int productId);
    }
}