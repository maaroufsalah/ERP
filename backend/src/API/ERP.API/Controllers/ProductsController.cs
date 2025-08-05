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
        /// Récupère un produit par son ID
        /// </summary>
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
        /// Récupère les produits par type de produit
        /// </summary>
        [HttpGet("type/{productTypeId}")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByProductType(int productTypeId)
        {
            try
            {
                _logger.LogInformation("Récupération des produits par type: {ProductTypeId}", productTypeId);
                var products = await _productService.GetProductsByProductTypeAsync(productTypeId);

                _logger.LogInformation("{ProductCount} produits trouvés pour le type {ProductTypeId}", products.Count(), productTypeId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits par type: {ProductTypeId}", productTypeId);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les produits par marque
        /// </summary>
        [HttpGet("brand/{brandId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(int brandId)
        {
            try
            {
                var products = await _productService.GetProductsByBrandAsync(brandId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des produits de la marque: {BrandId}", brandId);
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les produits par fournisseur
        /// </summary>
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

        // ✅ ================= ENDPOINTS POUR DROPDOWNS =================

        /// <summary>
        /// Récupère les types de produits pour dropdown
        /// </summary>
        [HttpGet("dropdowns/product-types")]
        public async Task<ActionResult<IEnumerable<DropdownOptionDto>>> GetProductTypesForDropdown()
        {
            try
            {
                var productTypes = await _productService.GetProductTypesForDropdownAsync();
                return Ok(productTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des types de produits");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les marques pour dropdown
        /// </summary>
        [HttpGet("dropdowns/brands")]
        public async Task<ActionResult<IEnumerable<BrandDropdownDto>>> GetBrandsForDropdown([FromQuery] int? productTypeId = null)
        {
            try
            {
                var brands = await _productService.GetBrandsForDropdownAsync(productTypeId);
                return Ok(brands);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des marques");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les modèles pour dropdown
        /// </summary>
        [HttpGet("dropdowns/models")]
        public async Task<ActionResult<IEnumerable<ModelDropdownDto>>> GetModelsForDropdown([FromQuery] int? productTypeId = null, [FromQuery] int? brandId = null)
        {
            try
            {
                var models = await _productService.GetModelsForDropdownAsync(productTypeId, brandId);
                return Ok(models);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des modèles");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les couleurs pour dropdown
        /// </summary>
        [HttpGet("dropdowns/colors")]
        public async Task<ActionResult<IEnumerable<ColorDropdownDto>>> GetColorsForDropdown()
        {
            try
            {
                var colors = await _productService.GetColorsForDropdownAsync();
                return Ok(colors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des couleurs");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        /// <summary>
        /// Récupère les conditions pour dropdown
        /// </summary>
        [HttpGet("dropdowns/conditions")]
        public async Task<ActionResult<IEnumerable<ConditionDropdownDto>>> GetConditionsForDropdown()
        {
            try
            {
                var conditions = await _productService.GetConditionsForDropdownAsync();
                return Ok(conditions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des conditions");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }
    }
}