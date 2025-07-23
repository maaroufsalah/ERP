// src/lib/api-client.ts

import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';

class ApiClient {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:5000/api',
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Request interceptor
    this.client.interceptors.request.use(
      (config) => {
        // Add auth token if available
        const token = localStorage.getItem('auth_token');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    // Response interceptor
    this.client.interceptors.response.use(
      (response: AxiosResponse) => {
        return response;
      },
      (error) => {
        if (error.response?.status === 401) {
          // Handle unauthorized access
          localStorage.removeItem('auth_token');
          // Redirect to login if needed
        }
        return Promise.reject(error);
      }
    );
  }

  async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.get<T>(url, config);
    return response.data;
  }

  async post<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.post<T>(url, data, config);
    return response.data;
  }

  async put<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.put<T>(url, data, config);
    return response.data;
  }

  async patch<T>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.patch<T>(url, data, config);
    return response.data;
  }

  async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
    const response = await this.client.delete<T>(url, config);
    return response.data;
  }
}

export const apiClient = new ApiClient();

// src/services/product.service.ts

import { 
  ProductDto, 
  ProductForListDto, 
  CreateProductDto, 
  UpdateProductDto,
  ProductFilters 
} from '@/types/api/product.types';
import { apiClient } from '@/lib/api-client';

export class ProductService {
  private readonly baseUrl = '/products';

  // ✅ CRUD Operations
  async getAllProducts(): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(this.baseUrl);
  }

  async getProductsList(): Promise<ProductForListDto[]> {
    return apiClient.get<ProductForListDto[]>(`${this.baseUrl}/list`);
  }

  async getProductById(id: number): Promise<ProductDto> {
    return apiClient.get<ProductDto>(`${this.baseUrl}/${id}`);
  }

  async createProduct(data: CreateProductDto): Promise<ProductDto> {
    return apiClient.post<ProductDto>(this.baseUrl, data);
  }

  async updateProduct(id: number, data: UpdateProductDto): Promise<ProductDto> {
    return apiClient.put<ProductDto>(`${this.baseUrl}/${id}`, data);
  }

  async deleteProduct(id: number): Promise<void> {
    return apiClient.delete<void>(`${this.baseUrl}/${id}`);
  }

  // ✅ Search and Filter Operations
  async searchProducts(query: string): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(`${this.baseUrl}/search?query=${encodeURIComponent(query)}`);
  }

  async getProductsByCategory(category: string): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(`${this.baseUrl}/category/${encodeURIComponent(category)}`);
  }

  async getProductsByBrand(brand: string): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(`${this.baseUrl}/brand/${encodeURIComponent(brand)}`);
  }

  async getProductsBySupplier(supplier: string): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(`${this.baseUrl}/supplier/${encodeURIComponent(supplier)}`);
  }

  async getProductsByBatch(batch: string): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(`${this.baseUrl}/batch/${encodeURIComponent(batch)}`);
  }

  // ✅ Stock Management
  async getLowStockProducts(threshold: number = 10): Promise<ProductDto[]> {
    return apiClient.get<ProductDto[]>(`${this.baseUrl}/low-stock?threshold=${threshold}`);
  }

  async updateStock(productId: number, newStock: number): Promise<void> {
    return apiClient.patch<void>(`${this.baseUrl}/${productId}/stock?newStock=${newStock}`);
  }

  async adjustStock(productId: number, adjustment: number): Promise<void> {
    return apiClient.patch<void>(`${this.baseUrl}/${productId}/adjust-stock?adjustment=${adjustment}`);
  }

  // ✅ Utility Methods
  async getCategories(): Promise<string[]> {
    return apiClient.get<string[]>(`${this.baseUrl}/categories`);
  }

  async getBrands(): Promise<string[]> {
    return apiClient.get<string[]>(`${this.baseUrl}/brands`);
  }

  async getSuppliers(): Promise<string[]> {
    return apiClient.get<string[]>(`${this.baseUrl}/suppliers`);
  }

  async getImportBatches(): Promise<string[]> {
    return apiClient.get<string[]>(`${this.baseUrl}/import-batches`);
  }

  async getProductCount(): Promise<number> {
    return apiClient.get<number>(`${this.baseUrl}/count`);
  }

  // ✅ Status Management
  async markAsUnavailable(productId: number): Promise<void> {
    return apiClient.patch<void>(`${this.baseUrl}/${productId}/mark-unavailable`);
  }

  async markAsAvailable(productId: number): Promise<void> {
    return apiClient.patch<void>(`${this.baseUrl}/${productId}/mark-available`);
  }

  async updateSellingPrice(productId: number, newPrice: number): Promise<void> {
    return apiClient.patch<void>(`${this.baseUrl}/${productId}/price?newPrice=${newPrice}`);
  }
}

export const productService = new ProductService();

// src/hooks/use-products.ts

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { productService } from '@/services/product.service';
import { CreateProductDto, UpdateProductDto } from '@/types/api/product.types';
import { toast } from 'react-hot-toast';

// Query keys
export const productKeys = {
  all: ['products'] as const,
  lists: () => [...productKeys.all, 'list'] as const,
  list: (filters: string) => [...productKeys.lists(), filters] as const,
  details: () => [...productKeys.all, 'detail'] as const,
  detail: (id: number) => [...productKeys.details(), id] as const,
  search: (query: string) => [...productKeys.all, 'search', query] as const,
  categories: () => [...productKeys.all, 'categories'] as const,
  brands: () => [...productKeys.all, 'brands'] as const,
  suppliers: () => [...productKeys.all, 'suppliers'] as const,
  stats: () => [...productKeys.all, 'stats'] as const,
};

// ✅ Query Hooks
export function useProducts() {
  return useQuery({
    queryKey: productKeys.all,
    queryFn: () => productService.getAllProducts(),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

export function useProductsList() {
  return useQuery({
    queryKey: productKeys.lists(),
    queryFn: () => productService.getProductsList(),
    staleTime: 5 * 60 * 1000,
  });
}

export function useProduct(id: number, enabled: boolean = true) {
  return useQuery({
    queryKey: productKeys.detail(id),
    queryFn: () => productService.getProductById(id),
    enabled: enabled && !!id,
    staleTime: 5 * 60 * 1000,
  });
}

export function useSearchProducts(query: string) {
  return useQuery({
    queryKey: productKeys.search(query),
    queryFn: () => productService.searchProducts(query),
    enabled: !!query.trim(),
    staleTime: 2 * 60 * 1000, // 2 minutes for search results
  });
}

export function useCategories() {
  return useQuery({
    queryKey: productKeys.categories(),
    queryFn: () => productService.getCategories(),
    staleTime: 30 * 60 * 1000, // 30 minutes
  });
}

export function useBrands() {
  return useQuery({
    queryKey: productKeys.brands(),
    queryFn: () => productService.getBrands(),
    staleTime: 30 * 60 * 1000,
  });
}

export function useSuppliers() {
  return useQuery({
    queryKey: productKeys.suppliers(),
    queryFn: () => productService.getSuppliers(),
    staleTime: 30 * 60 * 1000,
  });
}

export function useLowStockProducts(threshold: number = 10) {
  return useQuery({
    queryKey: [...productKeys.all, 'low-stock', threshold],
    queryFn: () => productService.getLowStockProducts(threshold),
    staleTime: 5 * 60 * 1000,
  });
}

// ✅ Mutation Hooks
export function useCreateProduct() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: CreateProductDto) => productService.createProduct(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: productKeys.all });
      toast.success('Produit créé avec succès!');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Erreur lors de la création du produit');
    },
  });
}

export function useUpdateProduct() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateProductDto }) => 
      productService.updateProduct(id, data),
    onSuccess: (_, { id }) => {
      queryClient.invalidateQueries({ queryKey: productKeys.detail(id) });
      queryClient.invalidateQueries({ queryKey: productKeys.all });
      toast.success('Produit mis à jour avec succès!');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Erreur lors de la mise à jour du produit');
    },
  });
}

export function useDeleteProduct() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => productService.deleteProduct(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: productKeys.all });
      toast.success('Produit supprimé avec succès!');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Erreur lors de la suppression du produit');
    },
  });
}

export function useUpdateStock() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ productId, newStock }: { productId: number; newStock: number }) =>
      productService.updateStock(productId, newStock),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: productKeys.all });
      toast.success('Stock mis à jour avec succès!');
    },
    onError: (error: any) => {
      toast.error(error.response?.data?.message || 'Erreur lors de la mise à jour du stock');
    },
  });
}