// app/features/products/services/productService.ts

import { fetchWrapper } from '@/app/shared/services/fetchWrapper';
import { showToast } from '@/app/shared/services/toastService';
import { Product, CreateProductDto, UpdateProductDto } from '../types';

class ProductService {
  private readonly baseUrl = 'products';

  // ✅ CRUD DE BASE - Aligné avec votre contrôleur C#

  /**
   * Récupère tous les produits
   * Endpoint: GET /api/products
   */
  async getAllProducts(): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(this.baseUrl);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des produits', 'error');
      throw error;
    }
  }

  /**
   * Récupère la liste allégée des produits
   * Endpoint: GET /api/products/list
   */
  async getProductsList(): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/list`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement de la liste des produits', 'error');
      throw error;
    }
  }

  /**
   * Récupère un produit par ID
   * Endpoint: GET /api/products/{id}
   */
  async getProductById(id: number): Promise<Product> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/${id}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement du produit', 'error');
      throw error;
    }
  }

  /**
   * Crée un nouveau produit (générique)
   * Endpoint: POST /api/products
   */
  async createProduct(product: CreateProductDto): Promise<Product> {
    try {
      const response = await fetchWrapper.post(this.baseUrl, product);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast('Produit créé avec succès !', 'success');
      return response;
    } catch (error: any) {
      showToast('Erreur lors de la création du produit', 'error');
      throw error;
    }
  }

  /**
   * Met à jour un produit
   * Endpoint: PUT /api/products/{id}
   */
  async updateProduct(id: number, product: UpdateProductDto): Promise<Product> {
    try {
      const response = await fetchWrapper.put(`${this.baseUrl}/${id}`, product);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast('Produit mis à jour avec succès !', 'success');
      return response;
    } catch (error: any) {
      showToast('Erreur lors de la mise à jour du produit', 'error');
      throw error;
    }
  }

  /**
   * Supprime un produit
   * Endpoint: DELETE /api/products/{id}
   */
  async deleteProduct(id: number): Promise<void> {
    try {
      const response = await fetchWrapper.del(`${this.baseUrl}/${id}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast('Produit supprimé avec succès !', 'success');
    } catch (error: any) {
      showToast('Erreur lors de la suppression du produit', 'error');
      throw error;
    }
  }

  // ✅ RECHERCHE ET FILTRAGE

  /**
   * Recherche des produits par terme
   * Endpoint: GET /api/products/search?query=...
   */
  async searchProducts(query: string): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/search?query=${encodeURIComponent(query)}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors de la recherche', 'error');
      throw error;
    }
  }

  /**
   * Récupère les produits par catégorie
   * Endpoint: GET /api/products/category/{category}
   */
  async getProductsByCategory(category: string): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/category/${encodeURIComponent(category)}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du filtrage par catégorie', 'error');
      throw error;
    }
  }

  /**
   * Récupère les produits par marque
   * Endpoint: GET /api/products/brand/{brand}
   */
  async getProductsByBrand(brand: string): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/brand/${encodeURIComponent(brand)}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du filtrage par marque', 'error');
      throw error;
    }
  }

  /**
   * Récupère les produits par fournisseur
   * Endpoint: GET /api/products/supplier/{supplier}
   */
  async getProductsBySupplier(supplier: string): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/supplier/${encodeURIComponent(supplier)}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du filtrage par fournisseur', 'error');
      throw error;
    }
  }

  // ✅ GESTION DU STOCK

  /**
   * Récupère les produits en stock faible
   * Endpoint: GET /api/products/low-stock?threshold=...
   */
  async getLowStockProducts(threshold: number = 10): Promise<Product[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/low-stock?threshold=${threshold}`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des produits en stock faible', 'error');
      throw error;
    }
  }

  /**
   * Met à jour le stock d'un produit
   * Endpoint: PATCH /api/products/{id}/stock?newStock=...
   */
  async updateStock(id: number, newStock: number): Promise<void> {
    try {
      const response = await fetchWrapper.patch(`${this.baseUrl}/${id}/stock?newStock=${newStock}`, {});
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast(`Stock mis à jour: ${newStock}`, 'success');
    } catch (error: any) {
      showToast('Erreur lors de la mise à jour du stock', 'error');
      throw error;
    }
  }

  /**
   * Ajuste le stock d'un produit (+/-)
   * Endpoint: PATCH /api/products/{id}/adjust-stock?adjustment=...
   */
  async adjustStock(id: number, adjustment: number): Promise<void> {
    try {
      const response = await fetchWrapper.patch(`${this.baseUrl}/${id}/adjust-stock?adjustment=${adjustment}`, {});
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast(`Stock ajusté de ${adjustment}`, 'success');
    } catch (error: any) {
      showToast('Erreur lors de l\'ajustement du stock', 'error');
      throw error;
    }
  }

  // ✅ STATISTIQUES

  /**
   * Récupère les statistiques générales
   * Endpoint: GET /api/products/stats
   */
  async getProductStats(): Promise<any> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/stats`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des statistiques', 'error');
      throw error;
    }
  }

  /**
   * Récupère les statistiques par catégorie
   * Endpoint: GET /api/products/stats/categories
   */
  async getCategoryStats(): Promise<any[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/stats/categories`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des statistiques par catégorie', 'error');
      throw error;
    }
  }

  // ✅ UTILITAIRES

  /**
   * Récupère toutes les catégories
   * Endpoint: GET /api/products/categories
   */
  async getCategories(): Promise<string[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/categories`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des catégories', 'error');
      throw error;
    }
  }

  /**
   * Récupère toutes les marques
   * Endpoint: GET /api/products/brands
   */
  async getBrands(): Promise<string[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/brands`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des marques', 'error');
      throw error;
    }
  }

  /**
   * Récupère tous les fournisseurs
   * Endpoint: GET /api/products/suppliers
   */
  async getSuppliers(): Promise<string[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/suppliers`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des fournisseurs', 'error');
      throw error;
    }
  }

  // ✅ GESTION DES STATUTS

  /**
   * Marque un produit comme vendu
   * Endpoint: PATCH /api/products/{id}/mark-sold
   */
  async markAsSold(id: number): Promise<void> {
    try {
      const response = await fetchWrapper.patch(`${this.baseUrl}/${id}/mark-sold`, {});
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast('Produit marqué comme vendu', 'success');
    } catch (error: any) {
      showToast('Erreur lors du marquage comme vendu', 'error');
      throw error;
    }
  }

  /**
   * Marque un produit comme réservé
   * Endpoint: PATCH /api/products/{id}/mark-reserved
   */
  async markAsReserved(id: number): Promise<void> {
    try {
      const response = await fetchWrapper.patch(`${this.baseUrl}/${id}/mark-reserved`, {});
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast('Produit marqué comme réservé', 'success');
    } catch (error: any) {
      showToast('Erreur lors du marquage comme réservé', 'error');
      throw error;
    }
  }

  /**
   * Marque un produit comme disponible
   * Endpoint: PATCH /api/products/{id}/mark-available
   */
  async markAsAvailable(id: number): Promise<void> {
    try {
      const response = await fetchWrapper.patch(`${this.baseUrl}/${id}/mark-available`, {});
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast('Produit marqué comme disponible', 'success');
    } catch (error: any) {
      showToast('Erreur lors du marquage comme disponible', 'error');
      throw error;
    }
  }

  // ✅ GESTION DES PRIX

  /**
   * Met à jour le prix de vente
   * Endpoint: PATCH /api/products/{id}/price?newPrice=...
   */
  async updatePrice(id: number, newPrice: number): Promise<void> {
    try {
      const response = await fetchWrapper.patch(`${this.baseUrl}/${id}/price?newPrice=${newPrice}`, {});
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      showToast(`Prix mis à jour: ${newPrice}€`, 'success');
    } catch (error: any) {
      showToast('Erreur lors de la mise à jour du prix', 'error');
      throw error;
    }
  }

  /**
   * Récupère le nombre total de produits
   * Endpoint: GET /api/products/count
   */
  async getProductCount(): Promise<number> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/count`);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du comptage des produits', 'error');
      throw error;
    }
  }
}

export const productService = new ProductService();