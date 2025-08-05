using AutoMapper;
using ERP.Application.DTOs;
using ERP.Application.Interfaces;
using ERP.Application.Interfaces.IServices;
using ERP.Domain.Entities;
using ERP.Domain.Entities.Product;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ERP.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductType> _productTypeRepository;
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Model> _modelRepository;
        private readonly IRepository<Color> _colorRepository;
        private readonly IRepository<Condition> _conditionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IRepository<Product> productRepository,
            IRepository<ProductType> productTypeRepository,
            IRepository<Brand> brandRepository,
            IRepository<Model> modelRepository,
            IRepository<Color> colorRepository,
            IRepository<Condition> conditionRepository,
            IMapper mapper,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _brandRepository = brandRepository;
            _modelRepository = modelRepository;
            _colorRepository = colorRepository;
            _conditionRepository = conditionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // ================================================================
        // MÉTHODES CRUD DE BASE AVEC RELATIONS
        // ================================================================

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Récupération de tous les produits avec relations");

                // Récupérer les produits avec toutes leurs relations
                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
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

                var product = await _productRepository.GetByIdWithIncludesAsync(id,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                if (product == null || product.IsDeleted)
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

                // Valider les relations avant création
                var isValidRelations = await ValidateProductRelationsAsync(
                    createProductDto.ProductTypeId,
                    createProductDto.BrandId,
                    createProductDto.ModelId,
                    createProductDto.ColorId,
                    createProductDto.ConditionId
                );

                if (!isValidRelations)
                {
                    throw new ArgumentException("Les relations spécifiées ne sont pas valides");
                }

                var product = _mapper.Map<Product>(createProductDto);
                product.CreatedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                var createdProduct = await _productRepository.CreateAsync(product);

                // Récupérer le produit créé avec ses relations pour le retour
                var productWithRelations = await _productRepository.GetByIdWithIncludesAsync(createdProduct.Id,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                _logger.LogInformation("Produit créé avec succès. ID: {ProductId}", createdProduct.Id);
                return _mapper.Map<ProductDto>(productWithRelations);
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
                if (existingProduct == null || existingProduct.IsDeleted)
                {
                    _logger.LogWarning("Tentative de mise à jour d'un produit inexistant ou supprimé: {ProductId}", id);
                    return null;
                }

                // Valider les nouvelles relations si elles changent
                var isValidRelations = await ValidateProductRelationsAsync(
                    updateProductDto.ProductTypeId,
                    updateProductDto.BrandId,
                    updateProductDto.ModelId,
                    updateProductDto.ColorId,
                    updateProductDto.ConditionId
                );

                if (!isValidRelations)
                {
                    throw new ArgumentException("Les nouvelles relations spécifiées ne sont pas valides");
                }

                _mapper.Map(updateProductDto, existingProduct);
                existingProduct.UpdatedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                var updatedProduct = await _productRepository.UpdateAsync(existingProduct);

                // Récupérer avec relations pour le retour
                var productWithRelations = await _productRepository.GetByIdWithIncludesAsync(id,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                _logger.LogInformation("Produit {ProductId} mis à jour avec succès", id);
                return _mapper.Map<ProductDto>(productWithRelations);
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
                if (product == null || product.IsDeleted)
                {
                    _logger.LogWarning("Tentative de suppression d'un produit inexistant: {ProductId}", id);
                    return false;
                }

                // Suppression logique
                product.IsDeleted = true;
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

        // ================================================================
        // MÉTHODES DE RECHERCHE PAR NOUVELLES RELATIONS
        // ================================================================

        public async Task<IEnumerable<ProductDto>> GetProductsByProductTypeAsync(int productTypeId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par type: {ProductTypeId}", productTypeId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.ProductTypeId == productTypeId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par type {ProductTypeId}", productTypeId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBrandAsync(int brandId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par marque: {BrandId}", brandId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.BrandId == brandId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par marque {BrandId}", brandId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByModelAsync(int modelId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par modèle: {ModelId}", modelId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.ModelId == modelId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par modèle {ModelId}", modelId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByColorAsync(int colorId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par couleur: {ColorId}", colorId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.ColorId == colorId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par couleur {ColorId}", colorId);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByConditionAsync(int conditionId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par condition: {ConditionId}", conditionId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.ConditionId == conditionId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par condition {ConditionId}", conditionId);
                throw;
            }
        }

        // ================================================================
        // MÉTHODES POUR LES DROPDOWNS EN CASCADE
        // ================================================================

        public async Task<IEnumerable<DropdownOptionDto>> GetProductTypesForDropdownAsync()
        {
            try
            {
                var productTypes = await _productTypeRepository.GetWithFilterAsync(pt => pt.IsActive);
                var orderedTypes = productTypes.OrderBy(pt => pt.SortOrder).ThenBy(pt => pt.Name);
                return _mapper.Map<IEnumerable<DropdownOptionDto>>(orderedTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des types de produits pour dropdown");
                throw;
            }
        }

        public async Task<IEnumerable<BrandDropdownDto>> GetBrandsForDropdownAsync(int? productTypeId = null)
        {
            try
            {
                var brands = productTypeId.HasValue
                    ? await _brandRepository.GetWithFilterAsync(b => b.ProductTypeId == productTypeId.Value && b.IsActive)
                    : await _brandRepository.GetWithFilterAsync(b => b.IsActive);

                var orderedBrands = brands.OrderBy(b => b.SortOrder).ThenBy(b => b.Name);
                return _mapper.Map<IEnumerable<BrandDropdownDto>>(orderedBrands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des marques pour dropdown");
                throw;
            }
        }

        public async Task<IEnumerable<ModelDropdownDto>> GetModelsForDropdownAsync(int? productTypeId = null, int? brandId = null)
        {
            try
            {
                var filter = PredicateBuilder.True<Model>();
                filter = filter.And(m => m.IsActive);

                if (productTypeId.HasValue)
                    filter = filter.And(m => m.ProductTypeId == productTypeId.Value);

                if (brandId.HasValue)
                    filter = filter.And(m => m.BrandId == brandId.Value);

                var models = await _modelRepository.GetWithFilterAsync(filter);
                var orderedModels = models.OrderBy(m => m.SortOrder).ThenBy(m => m.Name);
                return _mapper.Map<IEnumerable<ModelDropdownDto>>(orderedModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des modèles pour dropdown");
                throw;
            }
        }

        public async Task<IEnumerable<ColorDropdownDto>> GetColorsForDropdownAsync()
        {
            try
            {
                var colors = await _colorRepository.GetWithFilterAsync(c => c.IsActive);
                var orderedColors = colors.OrderBy(c => c.SortOrder).ThenBy(c => c.Name);
                return _mapper.Map<IEnumerable<ColorDropdownDto>>(orderedColors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des couleurs pour dropdown");
                throw;
            }
        }

        public async Task<IEnumerable<ConditionDropdownDto>> GetConditionsForDropdownAsync()
        {
            try
            {
                var conditions = await _conditionRepository.GetWithFilterAsync(c => c.IsActive);
                var orderedConditions = conditions.OrderBy(c => c.SortOrder).ThenBy(c => c.Name);
                return _mapper.Map<IEnumerable<ConditionDropdownDto>>(orderedConditions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des conditions pour dropdown");
                throw;
            }
        }

        // ================================================================
        // MÉTHODES DE VALIDATION DES RELATIONS
        // ================================================================

        public async Task<bool> ValidateProductRelationsAsync(int productTypeId, int brandId, int modelId, int colorId, int conditionId)
        {
            try
            {
                // Vérifier que tous les éléments existent et sont actifs
                var productType = await _productTypeRepository.GetByIdAsync(productTypeId);
                var brand = await _brandRepository.GetByIdAsync(brandId);
                var model = await _modelRepository.GetByIdAsync(modelId);
                var color = await _colorRepository.GetByIdAsync(colorId);
                var condition = await _conditionRepository.GetByIdAsync(conditionId);

                if (productType == null || !productType.IsActive ||
                    brand == null || !brand.IsActive ||
                    model == null || !model.IsActive ||
                    color == null || !color.IsActive ||
                    condition == null || !condition.IsActive)
                {
                    return false;
                }

                // Vérifier que la marque appartient au bon type de produit
                if (brand.ProductTypeId != productTypeId)
                {
                    _logger.LogWarning("La marque {BrandId} n'appartient pas au type de produit {ProductTypeId}", brandId, productTypeId);
                    return false;
                }

                // Vérifier que le modèle appartient à la bonne marque et au bon type
                if (model.BrandId != brandId || model.ProductTypeId != productTypeId)
                {
                    _logger.LogWarning("Le modèle {ModelId} n'appartient pas à la marque {BrandId} ou au type {ProductTypeId}", modelId, brandId, productTypeId);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation des relations");
                return false;
            }
        }

        public async Task<bool> IsValidBrandForProductTypeAsync(int brandId, int productTypeId)
        {
            try
            {
                var brand = await _brandRepository.GetByIdAsync(brandId);
                return brand != null && brand.IsActive && brand.ProductTypeId == productTypeId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation de la marque pour le type de produit");
                return false;
            }
        }

        public async Task<bool> IsValidModelForBrandAsync(int modelId, int brandId)
        {
            try
            {
                var model = await _modelRepository.GetByIdAsync(modelId);
                return model != null && model.IsActive && model.BrandId == brandId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation du modèle pour la marque");
                return false;
            }
        }

        // ================================================================
        // MÉTHODES UTILITAIRES HÉRITÉES
        // ================================================================

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                    return await GetAllProductsAsync();

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => (p.Name.Contains(query) ||
                                  p.Description.Contains(query) ||
                                  p.ProductType.Name.Contains(query) ||
                                  p.Brand.Name.Contains(query) ||
                                  p.Model.Name.Contains(query)) &&
                                 !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de produits avec le terme: {Query}", query);
                throw;
            }
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                return product != null && !product.IsDeleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'existence du produit {ProductId}", id);
                return false;
            }
        }

        public async Task<int> GetProductCountAsync()
        {
            try
            {
                return await _productRepository.CountWithFilterAsync(p => !p.IsDeleted && p.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des produits");
                throw;
            }
        }

        public async Task<int> GetProductCountByTypeAsync(int productTypeId)
        {
            try
            {
                return await _productRepository.CountWithFilterAsync(p => p.ProductTypeId == productTypeId && !p.IsDeleted && p.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des produits par type {ProductTypeId}", productTypeId);
                throw;
            }
        }

        public async Task<int> GetProductCountByBrandAsync(int brandId)
        {
            try
            {
                return await _productRepository.CountWithFilterAsync(p => p.BrandId == brandId && !p.IsDeleted && p.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des produits par marque {BrandId}", brandId);
                throw;
            }
        }

        // ================================================================
        // MÉTHODES IMPLÉMENTÉES (PRÉCÉDEMMENT STUB)
        // ================================================================

        public async Task<PagedResultDto<ProductForListDto>> GetProductsPagedAsync(ProductFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Récupération paginée des produits");

                // Construire le filtre
                Expression<Func<Product, bool>> predicate = p => !p.IsDeleted && p.IsActive;

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    predicate = predicate.And(p => p.Name.Contains(filter.SearchTerm) ||
                                                   p.Description.Contains(filter.SearchTerm));
                }

                if (filter.ProductTypeId.HasValue)
                {
                    predicate = predicate.And(p => p.ProductTypeId == filter.ProductTypeId.Value);
                }

                if (filter.BrandId.HasValue)
                {
                    predicate = predicate.And(p => p.BrandId == filter.BrandId.Value);
                }

                if (filter.LowStockOnly == true)
                {
                    predicate = predicate.And(p => p.Stock <= p.MinStockLevel);
                }

                // Compter le total
                var totalCount = await _productRepository.CountWithFilterAsync(predicate);

                // Récupérer les données paginées avec relations
                var products = await _productRepository.GetPagedWithFilterAsync(
                    predicate,
                    filter.Page,
                    filter.PageSize,
                    p => p.CreatedAt,
                    filter.SortDescending
                );

                // Charger les relations manuellement (limitation du repository actuel)
                var productsWithRelations = new List<Product>();
                foreach (var product in products)
                {
                    var productWithRelations = await _productRepository.GetByIdWithIncludesAsync(product.Id,
                        p => p.ProductType, p => p.Brand, p => p.Model, p => p.Color, p => p.Condition);

                    if (productWithRelations != null)
                        productsWithRelations.Add(productWithRelations);
                }

                var productDtos = _mapper.Map<List<ProductForListDto>>(productsWithRelations);

                return new PagedResultDto<ProductForListDto>
                {
                    Items = productDtos,
                    TotalCount = totalCount,
                    Page = filter.Page,
                    PageSize = filter.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize),
                    HasNextPage = filter.Page * filter.PageSize < totalCount,
                    HasPreviousPage = filter.Page > 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération paginée des produits");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsBySupplierAsync(string supplierName)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par fournisseur: {SupplierName}", supplierName);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.SupplierName.Contains(supplierName) && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par fournisseur");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByBatchAsync(string importBatch)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par lot: {ImportBatch}", importBatch);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.ImportBatch == importBatch && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par lot");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByStatusAsync(string status)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par statut: {Status}", status);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.Status == status && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par statut");
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync(int? threshold = null)
        {
            try
            {
                var thresholdValue = threshold ?? 5;
                _logger.LogInformation("Récupération des produits en stock faible (seuil: {Threshold})", thresholdValue);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.Stock <= thresholdValue && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductDto>>(products);
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
                if (product == null || product.IsDeleted)
                {
                    return false;
                }

                product.Stock = newStock;
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                await _productRepository.UpdateAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du stock pour le produit {ProductId}", productId);
                return false;
            }
        }

        public async Task<bool> AdjustStockAsync(int productId, int adjustment)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null || product.IsDeleted)
                {
                    return false;
                }

                product.Stock += adjustment;
                if (product.Stock < 0)
                {
                    product.Stock = 0;
                }

                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "System"; // TODO: Récupérer l'utilisateur connecté

                await _productRepository.UpdateAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajustement du stock pour le produit {ProductId}", productId);
                return false;
            }
        }

        public async Task<BulkOperationResultDto> BulkUpdateStockAsync(List<int> productIds, int stockAdjustment)
        {
            var result = new BulkOperationResultDto();

            try
            {
                foreach (var productId in productIds)
                {
                    try
                    {
                        var success = await AdjustStockAsync(productId, stockAdjustment);
                        if (success)
                        {
                            result.SuccessCount++;
                            result.ProcessedIds.Add(productId);
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Échec de la mise à jour du stock pour le produit {productId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Erreur lors de la mise à jour du produit {productId}: {ex.Message}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour en lot du stock");
                throw;
            }
        }

        public async Task<IEnumerable<ProductForListDto>> GetProductsForListAsync()
        {
            try
            {
                _logger.LogInformation("Récupération des produits pour liste");

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductForListDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits pour liste");
                throw;
            }
        }

        public async Task<IEnumerable<ProductForListDto>> GetProductsForListByTypeAsync(int productTypeId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits pour liste par type: {ProductTypeId}", productTypeId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.ProductTypeId == productTypeId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductForListDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits pour liste par type");
                throw;
            }
        }

        public async Task<IEnumerable<ProductForListDto>> GetProductsForListByBrandAsync(int brandId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits pour liste par marque: {BrandId}", brandId);

                var products = await _productRepository.GetWithFilterAndIncludesAsync(
                    filter: p => p.BrandId == brandId && !p.IsDeleted && p.IsActive,
                    p => p.ProductType,
                    p => p.Brand,
                    p => p.Model,
                    p => p.Color,
                    p => p.Condition
                );

                return _mapper.Map<IEnumerable<ProductForListDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits pour liste par marque");
                throw;
            }
        }

        public async Task<ProductStatsDto> GetProductStatsAsync()
        {
            try
            {
                _logger.LogInformation("Calcul des statistiques produits");

                var allProducts = await _productRepository.GetWithFilterAsync(p => !p.IsDeleted);
                var activeProducts = allProducts.Where(p => p.IsActive).ToList();
                var lowStockProducts = activeProducts.Where(p => p.Stock <= p.MinStockLevel);

                return new ProductStatsDto
                {
                    TotalProducts = allProducts.Count(),
                    ActiveProducts = activeProducts.Count,
                    LowStockProducts = lowStockProducts.Count(),
                    TotalStockValue = activeProducts.Sum(p => p.Stock * p.SellingPrice),
                    AverageMarginPercentage = activeProducts.Any() ? activeProducts.Average(p => p.MarginPercentage) : 0,
                    TotalProductTypes = await _productTypeRepository.CountWithFilterAsync(pt => pt.IsActive),
                    TotalBrands = await _brandRepository.CountWithFilterAsync(b => b.IsActive),
                    TotalModels = await _modelRepository.CountWithFilterAsync(m => m.IsActive)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques produits");
                throw;
            }
        }

        public async Task<IEnumerable<ProductTypeStatsDto>> GetProductTypeStatsAsync()
        {
            try
            {
                _logger.LogInformation("Calcul des statistiques par type de produit");

                var productTypes = await _productTypeRepository.GetWithFilterAsync(pt => pt.IsActive);
                var stats = new List<ProductTypeStatsDto>();

                foreach (var productType in productTypes)
                {
                    var products = await _productRepository.GetWithFilterAsync(p =>
                        p.ProductTypeId == productType.Id && !p.IsDeleted && p.IsActive);

                    var productsList = products.ToList();
                    var lowStockCount = productsList.Count(p => p.Stock <= p.MinStockLevel);

                    stats.Add(new ProductTypeStatsDto
                    {
                        ProductTypeId = productType.Id,
                        ProductTypeName = productType.Name,
                        ProductCount = productsList.Count,
                        TotalValue = productsList.Sum(p => p.Stock * p.SellingPrice),
                        AveragePrice = productsList.Any() ? productsList.Average(p => p.SellingPrice) : 0,
                        LowStockCount = lowStockCount
                    });
                }

                return stats.OrderByDescending(s => s.ProductCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques par type de produit");
                throw;
            }
        }

        public async Task<IEnumerable<BrandStatsDto>> GetBrandStatsAsync()
        {
            try
            {
                _logger.LogInformation("Calcul des statistiques par marque");

                var brands = await _brandRepository.GetAllWithIncludesAsync(b => b.ProductType);
                var stats = new List<BrandStatsDto>();

                foreach (var brand in brands.Where(b => b.IsActive))
                {
                    var products = await _productRepository.GetWithFilterAsync(p =>
                        p.BrandId == brand.Id && !p.IsDeleted && p.IsActive);

                    var productsList = products.ToList();

                    stats.Add(new BrandStatsDto
                    {
                        BrandId = brand.Id,
                        BrandName = brand.Name,
                        ProductTypeName = brand.ProductType?.Name ?? "N/A",
                        ProductCount = productsList.Count,
                        TotalValue = productsList.Sum(p => p.Stock * p.SellingPrice),
                        AverageMarginPercentage = productsList.Any() ? productsList.Average(p => p.MarginPercentage) : 0
                    });
                }

                return stats.OrderByDescending(s => s.TotalValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques par marque");
                throw;
            }
        }

        public async Task<decimal> GetTotalStockValueAsync()
        {
            try
            {
                return await _productRepository.SumWithFilterAsync(
                    filter: p => !p.IsDeleted && p.IsActive,
                    selector: p => p.Stock * p.SellingPrice
                );
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
                return await _productRepository.SumWithFilterAsync(
                    filter: p => !p.IsDeleted && p.IsActive,
                    selector: p => p.Stock * p.Margin
                );
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
                return await _productRepository.AverageDecimalWithFilterAsync(
                    filter: p => !p.IsDeleted && p.IsActive,
                    selector: p => p.MarginPercentage
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul du pourcentage de marge moyen");
                throw;
            }
        }

        public async Task<BulkOperationResultDto> BulkUpdatePricesAsync(List<int> productIds, decimal priceAdjustmentPercentage)
        {
            var result = new BulkOperationResultDto();

            try
            {
                foreach (var productId in productIds)
                {
                    try
                    {
                        var product = await _productRepository.GetByIdAsync(productId);
                        if (product != null && !product.IsDeleted)
                        {
                            var adjustmentFactor = 1 + (priceAdjustmentPercentage / 100);
                            product.SellingPrice = Math.Round(product.SellingPrice * adjustmentFactor, 2);

                            // Recalculer la marge
                            product.Margin = product.SellingPrice - product.TotalCostPrice;
                            product.MarginPercentage = product.TotalCostPrice > 0
                                ? Math.Round((product.Margin / product.TotalCostPrice) * 100, 2)
                                : 0;

                            product.UpdatedAt = DateTime.UtcNow;
                            product.UpdatedBy = "System";

                            await _productRepository.UpdateAsync(product);

                            result.SuccessCount++;
                            result.ProcessedIds.Add(productId);
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Produit {productId} non trouvé ou supprimé");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Erreur lors de la mise à jour du prix pour le produit {productId}: {ex.Message}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour en lot des prix");
                throw;
            }
        }

        public async Task<BulkOperationResultDto> BulkUpdateStatusAsync(List<int> productIds, string newStatus)
        {
            var result = new BulkOperationResultDto();

            try
            {
                foreach (var productId in productIds)
                {
                    try
                    {
                        var product = await _productRepository.GetByIdAsync(productId);
                        if (product != null && !product.IsDeleted)
                        {
                            product.Status = newStatus;
                            product.UpdatedAt = DateTime.UtcNow;
                            product.UpdatedBy = "System";

                            await _productRepository.UpdateAsync(product);

                            result.SuccessCount++;
                            result.ProcessedIds.Add(productId);
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Produit {productId} non trouvé ou supprimé");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Erreur lors de la mise à jour du statut pour le produit {productId}: {ex.Message}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour en lot du statut");
                throw;
            }
        }

        public async Task<BulkOperationResultDto> BulkDeleteAsync(List<int> productIds)
        {
            var result = new BulkOperationResultDto();

            try
            {
                foreach (var productId in productIds)
                {
                    try
                    {
                        var success = await DeleteProductAsync(productId);
                        if (success)
                        {
                            result.SuccessCount++;
                            result.ProcessedIds.Add(productId);
                        }
                        else
                        {
                            result.ErrorCount++;
                            result.Errors.Add($"Échec de la suppression du produit {productId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorCount++;
                        result.Errors.Add($"Erreur lors de la suppression du produit {productId}: {ex.Message}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression en lot");
                throw;
            }
        }
    }

    // ================================================================
    // HELPER CLASS POUR CONSTRUIRE DES PRÉDICATS DYNAMIQUES
    // ================================================================
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() => f => true;
        public static Expression<Func<T, bool>> False<T>() => f => false;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));
            var combined = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter).Visit(expr1.Body);
            var secondBody = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter).Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(combined, secondBody), parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));
            var combined = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter).Visit(expr1.Body);
            var secondBody = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter).Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(combined, secondBody), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }
}