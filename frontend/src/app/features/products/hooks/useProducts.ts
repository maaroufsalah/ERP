﻿// app/features/products/hooks/useProducts.ts

import { useState, useEffect, useCallback } from 'react';
import { Product, CreateProductDto, UpdateProductDto } from '../types';
import { productService } from '../services';

interface UseProductsState {
  products: Product[];
  loading: boolean;
  error: string | null;
  selectedProduct: Product | null;
}

interface UseProductsActions {
  // Actions de base
  loadProducts: () => Promise<void>;
  createProduct: (product: CreateProductDto) => Promise<void>;
  updateProduct: (id: number, product: UpdateProductDto) => Promise<void>;
  deleteProduct: (id: number) => Promise<void>;
  
  // Actions de recherche
  searchProducts: (query: string) => Promise<void>;
  filterByCategory: (category: string) => Promise<void>;
  filterByBrand: (brand: string) => Promise<void>;
  
  // Actions de stock
  loadLowStockProducts: () => Promise<void>;
  updateStock: (id: number, newStock: number) => Promise<void>;
  
  // Actions de statut
  markAsSold: (id: number) => Promise<void>;
  markAsReserved: (id: number) => Promise<void>;
  markAsAvailable: (id: number) => Promise<void>;
  
  // Sélection
  selectProduct: (product: Product | null) => void;
  
  // Reset
  resetProducts: () => void;
  clearError: () => void;
}

export function useProducts(): UseProductsState & UseProductsActions {
  // État principal
  const [state, setState] = useState<UseProductsState>({
    products: [],
    loading: false,
    error: null,
    selectedProduct: null
  });

  // Helper pour mettre à jour l'état
  const updateState = useCallback((updates: Partial<UseProductsState>) => {
    setState(prev => ({ ...prev, ...updates }));
  }, []);

  // ✅ ACTIONS DE BASE

  const loadProducts = useCallback(async () => {
    updateState({ loading: true, error: null });
    try {
      const products = await productService.getAllProducts();
      updateState({ products, loading: false });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du chargement des produits',
        loading: false 
      });
    }
  }, [updateState]);

  const createProduct = useCallback(async (product: CreateProductDto) => {
    updateState({ loading: true, error: null });
    try {
      const newProduct = await productService.createProduct(product);
      updateState({ 
        products: [...state.products, newProduct],
        loading: false 
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors de la création du produit',
        loading: false 
      });
      throw error; // Re-throw pour que le composant puisse gérer
    }
  }, [state.products, updateState]);

  const updateProduct = useCallback(async (id: number, product: UpdateProductDto) => {
    updateState({ loading: true, error: null });
    try {
      const updatedProduct = await productService.updateProduct(id, product);
      updateState({
        products: state.products.map(p => p.id === id ? updatedProduct : p),
        selectedProduct: state.selectedProduct?.id === id ? updatedProduct : state.selectedProduct,
        loading: false
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors de la mise à jour du produit',
        loading: false 
      });
      throw error;
    }
  }, [state.products, state.selectedProduct, updateState]);

  const deleteProduct = useCallback(async (id: number) => {
    updateState({ loading: true, error: null });
    try {
      await productService.deleteProduct(id);
      updateState({
        products: state.products.filter(p => p.id !== id),
        selectedProduct: state.selectedProduct?.id === id ? null : state.selectedProduct,
        loading: false
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors de la suppression du produit',
        loading: false 
      });
      throw error;
    }
  }, [state.products, state.selectedProduct, updateState]);

  // ✅ ACTIONS DE RECHERCHE

  const searchProducts = useCallback(async (query: string) => {
    if (!query.trim()) {
      loadProducts();
      return;
    }
    
    updateState({ loading: true, error: null });
    try {
      const products = await productService.searchProducts(query);
      updateState({ products, loading: false });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors de la recherche',
        loading: false 
      });
    }
  }, [loadProducts, updateState]);

  const filterByCategory = useCallback(async (category: string) => {
    updateState({ loading: true, error: null });
    try {
      const products = await productService.getProductsByCategory(category);
      updateState({ products, loading: false });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du filtrage par catégorie',
        loading: false 
      });
    }
  }, [updateState]);

  const filterByBrand = useCallback(async (brand: string) => {
    updateState({ loading: true, error: null });
    try {
      const products = await productService.getProductsByBrand(brand);
      updateState({ products, loading: false });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du filtrage par marque',
        loading: false 
      });
    }
  }, [updateState]);

  // ✅ ACTIONS DE STOCK

  const loadLowStockProducts = useCallback(async () => {
    updateState({ loading: true, error: null });
    try {
      const products = await productService.getLowStockProducts();
      updateState({ products, loading: false });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du chargement des produits en stock faible',
        loading: false 
      });
    }
  }, [updateState]);

  const updateStock = useCallback(async (id: number, newStock: number) => {
    try {
      await productService.updateStock(id, newStock);
      // Recharger le produit spécifique pour avoir les données à jour
      const updatedProduct = await productService.getProductById(id);
      updateState({
        products: state.products.map(p => p.id === id ? updatedProduct : p),
        selectedProduct: state.selectedProduct?.id === id ? updatedProduct : state.selectedProduct
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors de la mise à jour du stock'
      });
      throw error;
    }
  }, [state.products, state.selectedProduct, updateState]);

  // ✅ ACTIONS DE STATUT

  const markAsSold = useCallback(async (id: number) => {
    try {
      await productService.markAsSold(id);
      const updatedProduct = await productService.getProductById(id);
      updateState({
        products: state.products.map(p => p.id === id ? updatedProduct : p),
        selectedProduct: state.selectedProduct?.id === id ? updatedProduct : state.selectedProduct
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du marquage comme vendu'
      });
      throw error;
    }
  }, [state.products, state.selectedProduct, updateState]);

  const markAsReserved = useCallback(async (id: number) => {
    try {
      await productService.markAsReserved(id);
      const updatedProduct = await productService.getProductById(id);
      updateState({
        products: state.products.map(p => p.id === id ? updatedProduct : p),
        selectedProduct: state.selectedProduct?.id === id ? updatedProduct : state.selectedProduct
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du marquage comme réservé'
      });
      throw error;
    }
  }, [state.products, state.selectedProduct, updateState]);

  const markAsAvailable = useCallback(async (id: number) => {
    try {
      await productService.markAsAvailable(id);
      const updatedProduct = await productService.getProductById(id);
      updateState({
        products: state.products.map(p => p.id === id ? updatedProduct : p),
        selectedProduct: state.selectedProduct?.id === id ? updatedProduct : state.selectedProduct
      });
    } catch (error: any) {
      updateState({ 
        error: error.message || 'Erreur lors du marquage comme disponible'
      });
      throw error;
    }
  }, [state.products, state.selectedProduct, updateState]);

  // ✅ ACTIONS UTILITAIRES

  const selectProduct = useCallback((product: Product | null) => {
    updateState({ selectedProduct: product });
  }, [updateState]);

  const resetProducts = useCallback(() => {
    updateState({ 
      products: [], 
      selectedProduct: null, 
      error: null,
      loading: false 
    });
  }, [updateState]);

  const clearError = useCallback(() => {
    updateState({ error: null });
  }, [updateState]);

  // ✅ CHARGEMENT INITIAL (optionnel)
  useEffect(() => {
    // Décommentez si vous voulez charger automatiquement les produits
    // loadProducts();
  }, []);

  return {
    // État
    ...state,
    
    // Actions
    loadProducts,
    createProduct,
    updateProduct,
    deleteProduct,
    searchProducts,
    filterByCategory,
    filterByBrand,
    loadLowStockProducts,
    updateStock,
    markAsSold,
    markAsReserved,
    markAsAvailable,
    selectProduct,
    resetProducts,
    clearError
  };
}