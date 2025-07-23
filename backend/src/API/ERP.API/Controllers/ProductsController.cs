using ERP.Application.DTOs;
using ERP.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // ✅ ================= ENDPOINTS CRUD DE BASE =================

        /// <summary>
        /// Récupère tous les produits
        /// </summary>
        /// <returns>Liste complète des produits actifs</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                _logger.LogInformation("Récupération de tous les produits");
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère la liste allégée des produits pour l'affichage
        /// </summary>
        /// <returns>Liste allégée des produits</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(IEnumerable<ProductForListDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductForListDto>>> GetProductsList()
        {
            try
            {
                var products = await _productService.GetProductsForListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la liste des produits");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère un produit par son ID
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Le produit demandé</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("Récupération du produit avec l'ID: {ProductId}", id);
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Produit non trouvé avec l'ID: {ProductId}", id);
                    return NotFound($"Produit avec l'ID {id} non trouvé");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Crée un nouveau produit
        /// </summary>
        /// <param name="createProductDto">Données du produit à créer</param>
        /// <returns>Le produit créé</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Données invalides pour la création du produit: {@CreateProductDto}", createProductDto);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Création d'un nouveau produit: {ProductName}", createProductDto.Name);
                var product = await _productService.CreateProductAsync(createProductDto);

                _logger.LogInformation("Produit créé avec succès. ID: {ProductId}", product.Id);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du produit: {@CreateProductDto}", createProductDto);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Met à jour un produit existant
        /// </summary>
        /// <param name="id">ID du produit à mettre à jour</param>
        /// <param name="updateProductDto">Nouvelles données du produit</param>
        /// <returns>Le produit mis à jour</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Données invalides pour la mise à jour du produit {ProductId}: {@UpdateProductDto}", id, updateProductDto);
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Mise à jour du produit {ProductId}", id);
                var product = await _productService.UpdateProductAsync(id, updateProductDto);

                if (product == null)
                {
                    _logger.LogWarning("Tentative de mise à jour d'un produit inexistant: {ProductId}", id);
                    return NotFound($"Produit avec l'ID {id} non trouvé");
                }

                _logger.LogInformation("Produit {ProductId} mis à jour avec succès", id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du produit {ProductId}: {@UpdateProductDto}", id, updateProductDto);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Supprime un produit (suppression logique)
        /// </summary>
        /// <param name="id">ID du produit à supprimer</param>
        /// <returns>Confirmation de suppression</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Suppression du produit {ProductId}", id);
                var result = await _productService.DeleteProductAsync(id);

                if (!result)
                {
                    _logger.LogWarning("Tentative de suppression d'un produit inexistant: {ProductId}", id);
                    return NotFound($"Produit avec l'ID {id} non trouvé");
                }

                _logger.LogInformation("Produit {ProductId} supprimé avec succès", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // ✅ ================= ENDPOINTS DE RECHERCHE =================

        /// <summary>
        /// Recherche des produits par terme global
        /// </summary>
        /// <param name="query">Terme de recherche</param>
        /// <returns>Liste des produits correspondants</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    _logger.LogWarning("Recherche avec query vide");
                    return BadRequest("Le terme de recherche ne peut pas être vide");
                }

                _logger.LogInformation("Recherche de produits avec le terme: {SearchQuery}", query);
                var products = await _productService.SearchProductsAsync(query);

                _logger.LogInformation("Recherche terminée. {ProductCount} produits trouvés", products.Count());
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de produits avec le terme: {SearchQuery}", query);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les produits par catégorie
        /// </summary>
        /// <param name="category">Nom de la catégorie</param>
        /// <returns>Liste des produits de la catégorie</returns>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(string category)
        {
            try
            {
                _logger.LogInformation("Récupération des produits de la catégorie: {Category}", category);
                var products = await _productService.GetProductsByCategoryAsync(category);

                _logger.LogInformation("{ProductCount} produits trouvés dans la catégorie {Category}", products.Count(), category);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la catégorie: {Category}", category);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les produits par marque
        /// </summary>
        /// <param name="brand">Nom de la marque</param>
        /// <returns>Liste des produits de la marque</returns>
        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(string brand)
        {
            try
            {
                var products = await _productService.GetProductsByBrandAsync(brand);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la marque: {Brand}", brand);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les produits par fournisseur
        /// </summary>
        /// <param name="supplier">Nom du fournisseur</param>
        /// <returns>Liste des produits du fournisseur</returns>
        [HttpGet("supplier/{supplier}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySupplier(string supplier)
        {
            try
            {
                var products = await _productService.GetProductsBySupplierAsync(supplier);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits du fournisseur: {Supplier}", supplier);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les produits par lot d'importation
        /// </summary>
        /// <param name="batch">Numéro de lot</param>
        /// <returns>Liste des produits du lot</returns>
        [HttpGet("batch/{batch}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBatch(string batch)
        {
            try
            {
                var products = await _productService.GetProductsByBatchAsync(batch);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits du lot: {Batch}", batch);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // ✅ ================= ENDPOINTS DE GESTION DU STOCK =================

        /// <summary>
        /// Récupère les produits en stock faible
        /// </summary>
        /// <param name="threshold">Seuil de stock minimum (défaut: 10)</param>
        /// <returns>Liste des produits en stock faible</returns>
        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            try
            {
                var products = await _productService.GetLowStockProductsAsync(threshold);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits en stock faible");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Met à jour le stock d'un produit
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <param name="newStock">Nouveau niveau de stock</param>
        /// <returns>Confirmation de mise à jour</returns>
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int newStock)
        {
            try
            {
                var result = await _productService.UpdateStockAsync(id, newStock);
                if (!result) return NotFound($"Produit avec l'ID {id} non trouvé");

                return Ok($"Stock du produit {id} mis à jour à {newStock}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du stock du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Ajuste le stock d'un produit (+/-)
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <param name="adjustment">Ajustement (+5 ou -3)</param>
        /// <returns>Confirmation d'ajustement</returns>
        [HttpPatch("{id}/adjust-stock")]
        public async Task<IActionResult> AdjustStock(int id, [FromQuery] int adjustment)
        {
            try
            {
                var result = await _productService.AdjustStockAsync(id, adjustment);
                if (!result) return NotFound($"Produit avec l'ID {id} non trouvé");

                return Ok($"Stock du produit {id} ajusté de {adjustment}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajustement du stock du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // ✅ ================= ENDPOINTS DE STATISTIQUES =================

        /// <summary>
        /// Récupère les statistiques générales des produits
        /// </summary>
        /// <returns>Statistiques complètes</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<ProductStatsDto>> GetProductStats()
        {
            try
            {
                var stats = await _productService.GetProductStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des statistiques");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les statistiques par catégorie
        /// </summary>
        /// <returns>Statistiques par catégorie</returns>
        [HttpGet("stats/categories")]
        public async Task<ActionResult<IEnumerable<CategoryStatsDto>>> GetCategoryStats()
        {
            try
            {
                var stats = await _productService.GetCategoryStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des statistiques par catégorie");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les statistiques par fournisseur
        /// </summary>
        /// <returns>Statistiques par fournisseur</returns>
        [HttpGet("stats/suppliers")]
        public async Task<ActionResult<IEnumerable<SupplierStatsDto>>> GetSupplierStats()
        {
            try
            {
                var stats = await _productService.GetSupplierStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des statistiques par fournisseur");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // ✅ ================= ENDPOINTS UTILITAIRES =================

        /// <summary>
        /// Récupère toutes les catégories disponibles
        /// </summary>
        /// <returns>Liste des catégories</returns>
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            try
            {
                var categories = await _productService.GetCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des catégories");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère toutes les marques disponibles
        /// </summary>
        /// <returns>Liste des marques</returns>
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            try
            {
                var brands = await _productService.GetBrandsAsync();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des marques");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère tous les fournisseurs disponibles
        /// </summary>
        /// <returns>Liste des fournisseurs</returns>
        [HttpGet("suppliers")]
        public async Task<ActionResult<IEnumerable<string>>> GetSuppliers()
        {
            try
            {
                var suppliers = await _productService.GetSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des fournisseurs");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // ✅ ================= ENDPOINTS SPÉCIFIQUES IMPORT ITALIE =================

        /// <summary>
        /// Récupère les produits par ville de fournisseur
        /// </summary>
        /// <param name="city">Ville du fournisseur</param>
        /// <returns>Liste des produits de la ville</returns>
        [HttpGet("supplier-city/{city}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsBySupplierCity(string city)
        {
            try
            {
                var products = await _productService.GetProductsBySupplierCityAsync(city);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la ville: {City}", city);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les arrivées récentes
        /// </summary>
        /// <param name="days">Nombre de jours (défaut: 30)</param>
        /// <returns>Liste des produits arrivés récemment</returns>
        [HttpGet("recent-arrivals")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetRecentArrivals([FromQuery] int days = 30)
        {
            try
            {
                var products = await _productService.GetRecentArrivalsAsync(days);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des arrivées récentes");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Marque un produit comme vendu
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Confirmation</returns>
        [HttpPatch("{id}/mark-sold")]
        public async Task<IActionResult> MarkProductAsSold(int id)
        {
            try
            {
                var result = await _productService.MarkProductAsSoldAsync(id);
                if (!result) return NotFound($"Produit avec l'ID {id} non trouvé");

                return Ok($"Produit {id} marqué comme vendu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du marquage comme vendu du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Marque un produit comme réservé
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Confirmation</returns>
        [HttpPatch("{id}/mark-reserved")]
        public async Task<IActionResult> MarkProductAsReserved(int id)
        {
            try
            {
                var result = await _productService.MarkProductAsReservedAsync(id);
                if (!result) return NotFound($"Produit avec l'ID {id} non trouvé");

                return Ok($"Produit {id} marqué comme réservé");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du marquage comme réservé du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Marque un produit comme disponible
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <returns>Confirmation</returns>
        [HttpPatch("{id}/mark-available")]
        public async Task<IActionResult> MarkProductAsAvailable(int id)
        {
            try
            {
                var result = await _productService.MarkProductAsAvailableAsync(id);
                if (!result) return NotFound($"Produit avec l'ID {id} non trouvé");

                return Ok($"Produit {id} marqué comme disponible");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du marquage comme disponible du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        // ✅ ================= ENDPOINTS DE GESTION DES PRIX =================

        /// <summary>
        /// Met à jour le prix de vente d'un produit
        /// </summary>
        /// <param name="id">ID du produit</param>
        /// <param name="newPrice">Nouveau prix de vente</param>
        /// <returns>Confirmation</returns>
        [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdateSellingPrice(int id, [FromQuery] decimal newPrice)
        {
            try
            {
                var result = await _productService.UpdateSellingPriceAsync(id, newPrice);
                if (!result) return NotFound($"Produit avec l'ID {id} non trouvé");

                return Ok($"Prix du produit {id} mis à jour à {newPrice:C}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour du prix du produit {ProductId}", id);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère le nombre total de produits
        /// </summary>
        /// <returns>Nombre de produits</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetProductCount()
        {
            try
            {
                var count = await _productService.GetProductCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des produits");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }
    }
}