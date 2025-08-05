// app/features/products/services/referenceService.ts

import { fetchWrapper } from '@/app/shared/services/fetchWrapper';
import { showToast } from '@/app/shared/services/toastService';
import { ProductType, Brand, Model, Color, Condition, DropdownOption } from '../types';

// ✅ INTERFACES POUR LES RÉPONSES API
interface ProductTypeDropdownDto {
  id: number;
  name: string;
  description?: string;
}

interface BrandDropdownDto {
  id: number;
  name: string;
  description?: string;
}

interface ModelDropdownDto {
  id: number;
  name: string;
  brandId: number;
  brandName: string;
  description?: string;
}

interface ColorDropdownDto {
  id: number;
  name: string;
  hexCode?: string;
}

interface ConditionDropdownDto {
  id: number;
  name: string;
  description?: string;
  grade?: string;
}

class ReferenceService {
  private readonly baseUrl = 'products/dropdowns';

  // ✅ PRODUCT TYPES

  async getProductTypes(): Promise<ProductTypeDropdownDto[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/product-types`);
      console.log('🔗 Product Types:', response);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des types de produits', 'error');
      throw error;
    }
  }

  async getProductTypesForDropdown(): Promise<DropdownOption[]> {
    const productTypes = await this.getProductTypes();
    return productTypes.map(pt => ({
      value: pt.id,
      label: pt.name,
      description: pt.description
    }));
  }

  // ✅ BRANDS

  async getBrands(productTypeId?: number): Promise<BrandDropdownDto[]> {
    try {
      const url = productTypeId 
        ? `${this.baseUrl}/brands?productTypeId=${productTypeId}`
        : `${this.baseUrl}/brands`;
      
      const response = await fetchWrapper.get(url);
      console.log('🔗 Brands:', response);
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

  async getBrandsForDropdown(productTypeId?: number): Promise<DropdownOption[]> {
    const brands = await this.getBrands(productTypeId);
    return brands.map(b => ({
      value: b.id,
      label: b.name,
      description: b.description
    }));
  }

  // ✅ MODELS

  async getModels(productTypeId?: number, brandId?: number): Promise<ModelDropdownDto[]> {
    try {
      let url = `${this.baseUrl}/models`;
      const params: string[] = [];
      
      if (productTypeId) {
        params.push(`productTypeId=${productTypeId}`);
      }
      if (brandId) {
        params.push(`brandId=${brandId}`);
      }
      
      if (params.length > 0) {
        url += `?${params.join('&')}`;
      }
      
      const response = await fetchWrapper.get(url);
      console.log('🔗 Models:', response);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des modèles', 'error');
      throw error;
    }
  }

  async getModelsByBrand(brandId: number, productTypeId?: number): Promise<ModelDropdownDto[]> {
    return this.getModels(productTypeId, brandId);
  }

  async getModelsForDropdown(brandId?: number, productTypeId?: number): Promise<DropdownOption[]> {
    const models = await this.getModels(productTypeId, brandId);
    return models.map(m => ({
      value: m.id,
      label: m.name,
      description: `${m.brandName} - ${m.description || ''}`
    }));
  }

  // ✅ COLORS

  async getColors(): Promise<ColorDropdownDto[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/colors`);
      console.log('🔗 Colors:', response);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des couleurs', 'error');
      throw error;
    }
  }

  async getColorsForDropdown(): Promise<DropdownOption[]> {
    const colors = await this.getColors();
    return colors.map(c => ({
      value: c.id,
      label: c.name,
      description: c.hexCode
    }));
  }

  // ✅ CONDITIONS

  async getConditions(): Promise<ConditionDropdownDto[]> {
    try {
      const response = await fetchWrapper.get(`${this.baseUrl}/conditions`);
      console.log('🔗 Conditions:', response);
      if (response.error) {
        showToast(response.error.message, 'error');
        throw new Error(response.error.message);
      }
      return response;
    } catch (error: any) {
      showToast('Erreur lors du chargement des conditions', 'error');
      throw error;
    }
  }

  async getConditionsForDropdown(): Promise<DropdownOption[]> {
    const conditions = await this.getConditions();
    return conditions.map(c => ({
      value: c.id,
      label: c.name,
      description: c.description || c.grade
    }));
  }

  // ✅ MÉTHODES UTILITAIRES

  async getAllReferencesForForm(): Promise<{
    productTypes: DropdownOption[];
    brands: DropdownOption[];
    models: DropdownOption[];
    colors: DropdownOption[];
    conditions: DropdownOption[];
  }> {
    try {
      console.log('🔄 Chargement de toutes les références...');
      
      const [productTypes, brands, models, colors, conditions] = await Promise.all([
        this.getProductTypesForDropdown(),
        this.getBrandsForDropdown(),
        this.getModelsForDropdown(),
        this.getColorsForDropdown(),
        this.getConditionsForDropdown()
      ]);

      console.log('✅ Références chargées:', {
        productTypes: productTypes.length,
        brands: brands.length,
        models: models.length,
        colors: colors.length,
        conditions: conditions.length
      });

      return {
        productTypes,
        brands,
        models,
        colors,
        conditions
      };
    } catch (error: any) {
      console.error('❌ Erreur lors du chargement des références:', error);
      showToast('Erreur lors du chargement des références', 'error');
      throw error;
    }
  }
}

export const referenceService = new ReferenceService();