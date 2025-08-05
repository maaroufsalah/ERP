// app/features/products/types/product.types.ts

export interface Product {
  id: number;
  name: string;
  description: string;
  productTypeId: number;
  brandId: number;
  modelId: number;
  colorId?: number;
  conditionId: number;
  
  // ✅ AJOUTER LES LIBELLÉS DE L'API
  productTypeName: string;
  brandName: string;
  modelName: string;
  colorName: string;
  colorHexCode?: string;
  conditionName: string;
  conditionQualityPercentage: number;
  
  purchasePrice: number;
  transportCost: number;
  totalCostPrice: number;
  sellingPrice: number;
  margin: number;
  marginPercentage: number;
  stock: number;
  minStockLevel: number;
  storage?: string;
  memory?: string;
  processor?: string;
  screenSize?: string;
  supplierName: string;
  supplierCity: string;
  purchaseDate: string;
  arrivalDate: string;
  importBatch: string;
  invoiceNumber: string;
  status: string;
  isActive: boolean;
  notes?: string;
  warrantyInfo?: string;
  imageUrl?: string;
  imagesUrls?: string;
  documentsUrls?: string;
  createdAt: string;
  createdBy: string;
  updatedAt?: string;
  updatedBy?: string;
  totalValue: number;
  isLowStock: boolean;
  daysInStock: number;
  
  // ✅ GARDER AUSSI LES RELATIONS POUR COMPATIBILITÉ
  productType?: ProductType;
  brand?: Brand;
  model?: Model;
  color?: Color;
  condition?: Condition;
}

export interface CreateProductDto {
  name: string;
  description: string;
  productTypeId: number;
  brandId: number;
  modelId: number;
  colorId?: number;
  conditionId: number;
  purchasePrice: number;
  transportCost: number;
  sellingPrice: number;
  stock: number;
  minStockLevel: number;
  storage?: string;
  memory?: string;
  processor?: string;
  screenSize?: string;
  supplierName: string;
  supplierCity: string;
  purchaseDate: string;
  arrivalDate: string;
  importBatch: string;
  invoiceNumber: string;
  status: string;
  notes?: string;
  warrantyInfo?: string;
  imageUrl?: string;
}

export interface UpdateProductDto extends Partial<CreateProductDto> {
  id: number;
}

// ✅ NOUVELLES INTERFACES POUR LES RÉFÉRENCES

export interface ProductType {
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface Brand {
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
}

export interface Model {
  id: number;
  name: string;
  brandId: number;
  description?: string;
  isActive: boolean;
  brand?: Brand;
}

export interface Color {
  id: number;
  name: string;
  hexCode?: string;
  isActive: boolean;
}

export interface Condition {
  id: number;
  name: string;
  description?: string;
  grade?: string;
  isActive: boolean;
}

// ✅ INTERFACES POUR LES DROPDOWNS
export interface DropdownOption {
  value: number;
  label: string;
  description?: string;
  disabled?: boolean;
}

export interface ProductFilters {
  search?: string;
  productTypeId?: number;
  brandId?: number;
  modelId?: number;
  colorId?: number;
  conditionId?: number;
  lowStock?: boolean;
}

// ✅ STATUS CONSTANTS (conservés)
export const PRODUCT_STATUS = [
  'Available',
  'Reserved',
  'Sold',
  'Inactive'
] as const;

export type ProductStatus = typeof PRODUCT_STATUS[number];