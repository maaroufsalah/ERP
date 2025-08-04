// src/app/shared/services/apiConfig.ts

export function getApiUrl(): string {
  // En mode développement, utiliser l'URL de votre API backend (HTTPS)
  const developmentApiUrl = 'https://localhost:7003/api/';
  
  // En production, vous pouvez utiliser une variable d'environnement
  const productionApiUrl = process.env.NEXT_PUBLIC_API_URL || developmentApiUrl;
  
  // Déterminer l'environnement
  const isDevelopment = process.env.NODE_ENV === 'development';
  
  const baseUrl = isDevelopment ? developmentApiUrl : productionApiUrl;
  
  console.log('🔗 API Base URL:', baseUrl);
  
  return baseUrl;
}

export const API_ENDPOINTS = {
  // Produits
  PRODUCTS: 'products',
  PRODUCTS_LIST: 'products/list',
  PRODUCTS_SEARCH: 'products/search',
  PRODUCTS_BY_CATEGORY: 'products/category',
  PRODUCTS_BY_BRAND: 'products/brand',
  PRODUCTS_BY_SUPPLIER: 'products/supplier',
  PRODUCTS_LOW_STOCK: 'products/low-stock',
  
  // Autres endpoints futurs
  // ORDERS: 'orders',
  // CUSTOMERS: 'customers',
} as const;