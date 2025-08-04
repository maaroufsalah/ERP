using AutoMapper;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Interfaces.IServices;
using ERP.Domain.Entities.Product;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ERP.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IRepository<Product> productRepository,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // ✅ ================= MÉTHODES CRUD DE BASE =================

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Récupération de tous les produits");
                var products = await _productRepository.GetAllAsync();
                var activeProducts = products.Where(p => !p.FlagDelete);
                return _mapper.Map<IEnumerable<ProductDto>>(activeProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits");
                throw;
            }
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Récupération du produit avec l'ID: {ProductId}", id);
                var product = await _productRepository.GetByIdAsync(id);

                if (product == null || product.FlagDelete)
                {
                    _logger.LogWarning("Produit non trouvé ou supprimé avec l'ID: {ProductId}", id);
                    return null;
                }

                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du produit {ProductId}", id);
                throw;
            }
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            try
            {
                _logger.LogInformation("Création d'un nouveau produit: {ProductName}", createProductDto.Name);

                var product = _mapper.Map<Product>(createProductDto);
                product.CreatedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                var createdProduct = await _productRepository.CreateAsync(product);

                _logger.LogInformation("Produit créé avec succès. ID: {ProductId}", createdProduct.Id);
                return _mapper.Map<ProductDto>(createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du produit: {@CreateProductDto}", createProductDto);
                throw;
            }
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            try
            {
                _logger.LogInformation("Mise à jour du produit {ProductId}", id);

                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null || existingProduct.FlagDelete)
                {
                    _logger.LogWarning("Tentative de mise à jour d'un produit inexistant ou supprimé: {ProductId}", id);
                    return null;
                }

                _mapper.Map(updateProductDto, existingProduct);
                existingProduct.UpdatedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                var updatedProduct = await _productRepository.UpdateAsync(existingProduct);

                _logger.LogInformation("Produit {ProductId} mis à jour avec succès", id);
                return _mapper.Map<ProductDto>(updatedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du produit {ProductId}: {@UpdateProductDto}", id, updateProductDto);
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                _logger.LogInformation("Suppression du produit {ProductId}", id);

                var product = await _productRepository.GetByIdAsync(id);
                if (product == null || product.FlagDelete)
                {
                    _logger.LogWarning("Tentative de suppression d'un produit inexistant: {ProductId}", id);
                    return false;
                }

                // Suppression logique
                product.FlagDelete = true;
                product.DeletedAt = DateTime.UtcNow;
                product.DeletedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                await _productRepository.UpdateAsync(product);

                _logger.LogInformation("Produit {ProductId} supprimé avec succès", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du produit {ProductId}", id);
                throw;
            }
        }

        // ✅ ================= MÉTHODES DE RECHERCHE ET FILTRAGE =================

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string query)
        {
            try
            {
                _logger.LogInformation("Recherche de produits avec le terme: {SearchQuery}", query);

                var products = await _productRepository.GetAllAsync();
                var filteredProducts = products.Where(p => !p.FlagDelete &&
                    (p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     p.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     p.Brand.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     p.Model.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                     p.Category.Contains(query, StringComparison.OrdinalIgnoreCase)));

                _logger.LogInformation("Recherche terminée. {ProductCount} produits trouvés", filteredProducts.Count());
                return _mapper.Map<IEnumerable<ProductDto>>(filteredProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de produits avec le terme: {SearchQuery}", query);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var categoryProducts = products.Where(p => !p.FlagDelete &&
                    p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(categoryProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la catégorie: {Category}", category);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierAsync(string supplierName)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var supplierProducts = products.Where(p => !p.FlagDelete &&
                    p.SupplierName.Equals(supplierName, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(supplierProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits du fournisseur: {SupplierName}", supplierName);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBatchAsync(string importBatch)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var batchProducts = products.Where(p => !p.FlagDelete &&
                    p.ImportBatch.Equals(importBatch, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(batchProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits du lot: {ImportBatch}", importBatch);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(string brand)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var brandProducts = products.Where(p => !p.FlagDelete &&
                    p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(brandProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la marque: {Brand}", brand);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByConditionAsync(string condition)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var conditionProducts = products.Where(p => !p.FlagDelete &&
                    p.Condition.Equals(condition, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(conditionProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits en condition: {Condition}", condition);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByStatusAsync(string status)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var statusProducts = products.Where(p => !p.FlagDelete &&
                    p.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(statusProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits avec le statut: {Status}", status);
                throw;
            }
        }

        // ✅ ================= MÉTHODES DE GESTION DU STOCK =================

        public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int threshold = 10)
        {
            try
            {
                _logger.LogInformation("Récupération des produits en stock faible (seuil: {Threshold})", threshold);

                var products = await _productRepository.GetAllAsync();
                var lowStockProducts = products.Where(p => !p.FlagDelete && p.Stock <= threshold);

                _logger.LogInformation("{ProductCount} produits en stock faible trouvés", lowStockProducts.Count());
                return _mapper.Map<IEnumerable<ProductDto>>(lowStockProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits en stock faible");
                throw;
            }
        }

        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.FlagDelete) return false;

                product.Stock = newStock;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System";

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Stock du produit {ProductId} mis à jour: {NewStock}", productId, newStock);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du stock du produit {ProductId}", productId);
                throw;
            }
        }

        public async Task<bool> AdjustStockAsync(int productId, int adjustment)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.FlagDelete) return false;

                product.Stock += adjustment;
                if (product.Stock < 0) product.Stock = 0;

                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System";

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Stock du produit {ProductId} ajusté de {Adjustment}: nouveau stock {NewStock}",
                    productId, adjustment, product.Stock);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajustement du stock du produit {ProductId}", productId);
                throw;
            }
        }

        // ✅ ================= MÉTHODES UTILITAIRES =================

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Where(p => !p.FlagDelete)
                             .Select(p => p.Category)
                             .Distinct()
                             .OrderBy(c => c)
                             .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des catégories");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetBrandsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Where(p => !p.FlagDelete)
                             .Select(p => p.Brand)
                             .Distinct()
                             .OrderBy(b => b)
                             .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des marques");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetSuppliersAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Where(p => !p.FlagDelete)
                             .Select(p => p.SupplierName)
                             .Distinct()
                             .OrderBy(s => s)
                             .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des fournisseurs");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetImportBatchesAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Where(p => !p.FlagDelete)
                             .Select(p => p.ImportBatch)
                             .Distinct()
                             .OrderBy(b => b)
                             .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des lots d'import");
                throw;
            }
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                return product != null && !product.FlagDelete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'existence du produit {ProductId}", id);
                throw;
            }
        }

        public async Task<int> GetProductCountAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Count(p => !p.FlagDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des produits");
                throw;
            }
        }

        // ✅ ================= MÉTHODES POUR LES LISTES =================

        public async Task<IEnumerable<ProductForListDto>> GetProductsForListAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var activeProducts = products.Where(p => !p.FlagDelete);
                return _mapper.Map<IEnumerable<ProductForListDto>>(activeProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la liste des produits");
                throw;
            }
        }

        public async Task<IEnumerable<ProductForListDto>> GetProductsForListByCategoryAsync(string category)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var categoryProducts = products.Where(p => !p.FlagDelete &&
                    p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductForListDto>>(categoryProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la liste des produits par catégorie: {Category}", category);
                throw;
            }
        }

        // ✅ ================= MÉTHODES DE STATISTIQUES =================

        public async Task<ProductStatsDto> GetProductStatsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var activeProducts = products.Where(p => !p.FlagDelete).ToList();

                return new ProductStatsDto
                {
                    TotalProducts = activeProducts.Count,
                    ActiveProducts = activeProducts.Count(p => p.IsActive),
                    LowStockProducts = activeProducts.Count(p => p.Stock <= p.MinStockLevel),
                    TotalStockValue = activeProducts.Sum(p => p.Stock * p.SellingPrice),
                    TotalMargin = activeProducts.Sum(p => p.Margin * p.Stock),
                    AverageMarginPercentage = activeProducts.Average(p => p.MarginPercentage),
                    CategoryStats = (await GetCategoryStatsAsync()).ToList(),
                    SupplierStats = (await GetSupplierStatsAsync()).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques produits");
                throw;
            }
        }

        public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var activeProducts = products.Where(p => !p.FlagDelete);

                return activeProducts.GroupBy(p => p.Category)
                    .Select(g => new CategoryStatsDto
                    {
                        Category = g.Key,
                        ProductCount = g.Count(),
                        TotalValue = g.Sum(p => p.Stock * p.SellingPrice),
                        AveragePrice = g.Average(p => p.SellingPrice)
                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques par catégorie");
                throw;
            }
        }

        public async Task<IEnumerable<SupplierStatsDto>> GetSupplierStatsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var activeProducts = products.Where(p => !p.FlagDelete);

                return activeProducts.GroupBy(p => new { p.SupplierName, p.SupplierCity })
                    .Select(g => new SupplierStatsDto
                    {
                        SupplierName = g.Key.SupplierName,
                        SupplierCity = g.Key.SupplierCity,
                        ProductCount = g.Count(),
                        TotalPurchaseValue = g.Sum(p => p.PurchasePrice * p.Stock),
                        TotalSellingValue = g.Sum(p => p.SellingPrice * p.Stock),
                        TotalMargin = g.Sum(p => p.Margin * p.Stock)
                    }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques par fournisseur");
                throw;
            }
        }

        public async Task<decimal> GetTotalStockValueAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Where(p => !p.FlagDelete).Sum(p => p.Stock * p.SellingPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de la valeur totale du stock");
                throw;
            }
        }

        public async Task<decimal> GetTotalMarginAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return products.Where(p => !p.FlagDelete).Sum(p => p.Margin * p.Stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de la marge totale");
                throw;
            }
        }

        public async Task<decimal> GetAverageMarginPercentageAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var activeProducts = products.Where(p => !p.FlagDelete).ToList();
                return activeProducts.Any() ? activeProducts.Average(p => p.MarginPercentage) : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul du pourcentage de marge moyen");
                throw;
            }
        }

        // ✅ ================= MÉTHODES DE FILTRAGE AVANCÉ =================

        public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var rangeProducts = products.Where(p => !p.FlagDelete &&
                    p.SellingPrice >= minPrice && p.SellingPrice <= maxPrice);
                return _mapper.Map<IEnumerable<ProductDto>>(rangeProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par gamme de prix");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var dateRangeProducts = products.Where(p => !p.FlagDelete &&
                    p.PurchaseDate >= fromDate && p.PurchaseDate <= toDate);
                return _mapper.Map<IEnumerable<ProductDto>>(dateRangeProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par période");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetRecentArrivalsAsync(int days = 30)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-days);
                var products = await _productRepository.GetAllAsync();
                var recentProducts = products.Where(p => !p.FlagDelete &&
                    p.ArrivalDate.HasValue && p.ArrivalDate >= cutoffDate);
                return _mapper.Map<IEnumerable<ProductDto>>(recentProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des arrivées récentes");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsNeedingAttentionAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var attentionProducts = products.Where(p => !p.FlagDelete &&
                    (p.Stock <= p.MinStockLevel || p.DaysInStock > 90));
                return _mapper.Map<IEnumerable<ProductDto>>(attentionProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits nécessitant attention");
                throw;
            }
        }

        // ✅ ================= MÉTHODES DE GESTION DES PRIX =================

        public async Task<bool> UpdateSellingPriceAsync(int productId, decimal newPrice)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.FlagDelete) return false;

                product.SellingPrice = newPrice;
                product.Margin = newPrice - product.TotalCostPrice;
                product.MarginPercentage = newPrice > 0 ? Math.Round((product.Margin / newPrice) * 100, 2) : 0;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System";

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Prix de vente du produit {ProductId} mis à jour: {NewPrice}€", productId, newPrice);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du prix du produit {ProductId}", productId);
                throw;
            }
        }

        public async Task<bool> UpdateMarginPercentageAsync(int productId, decimal targetMarginPercentage)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.FlagDelete) return false;

                var targetSellingPrice = product.TotalCostPrice / (1 - (targetMarginPercentage / 100));
                product.SellingPrice = Math.Round(targetSellingPrice, 2);
                product.Margin = product.SellingPrice - product.TotalCostPrice;
                product.MarginPercentage = targetMarginPercentage;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System";

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Marge du produit {ProductId} ajustée à {MarginPercentage}%", productId, targetMarginPercentage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajustement de la marge du produit {ProductId}", productId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByMarginRangeAsync(decimal minMargin, decimal maxMargin)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var marginProducts = products.Where(p => !p.FlagDelete &&
                    p.MarginPercentage >= minMargin && p.MarginPercentage <= maxMargin);
                return _mapper.Map<IEnumerable<ProductDto>>(marginProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par marge");
                throw;
            }
        }

        // ✅ ================= MÉTHODES SPÉCIFIQUES IMPORT ITALIE =================

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierCityAsync(string city)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var cityProducts = products.Where(p => !p.FlagDelete &&
                    p.SupplierCity.Equals(city, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(cityProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la ville: {City}", city);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByArrivalDateAsync(DateTime date)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var arrivalProducts = products.Where(p => !p.FlagDelete &&
                    p.ArrivalDate.HasValue && p.ArrivalDate.Value.Date == date.Date);
                return _mapper.Map<IEnumerable<ProductDto>>(arrivalProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits arrivés le: {Date}", date);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByInvoiceNumberAsync(string invoiceNumber)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                var invoiceProducts = products.Where(p => !p.FlagDelete &&
                    p.InvoiceNumber.Equals(invoiceNumber, StringComparison.OrdinalIgnoreCase));
                return _mapper.Map<IEnumerable<ProductDto>>(invoiceProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la facture: {InvoiceNumber}", invoiceNumber);
                throw;
            }
        }

        public async Task<bool> MarkProductAsSoldAsync(int productId)
        {
            return await UpdateProductStatusAsync(productId, "Sold");
        }

        public async Task<bool> MarkProductAsReservedAsync(int productId)
        {
            return await UpdateProductStatusAsync(productId, "Reserved");
        }

        public async Task<bool> MarkProductAsAvailableAsync(int productId)
        {
            return await UpdateProductStatusAsync(productId, "Available");
        }

        // ✅ ================= MÉTHODE UTILITAIRE PRIVÉE =================

        private async Task<bool> UpdateProductStatusAsync(int productId, string status)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.FlagDelete) return false;

                product.Status = status;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System";

                await _productRepository.UpdateAsync(product);
                _logger.LogInformation("Statut du produit {ProductId} changé en: {Status}", productId, status);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut du produit {ProductId} en {Status}", productId, status);
                throw;
            }
        }
    }
}