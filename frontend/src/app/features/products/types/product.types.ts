// app/features/products/types/product.types.ts

export interface Product {
  id: number;
  name: string;
  description: string;
  category: string;
  brand: string;
  model: string;
  purchasePrice: number;
  transportCost: number;
  totalCostPrice: number;
  sellingPrice: number;
  margin: number;
  marginPercentage: number;
  stock: number;
  minStockLevel: number;
  condition: string;
  conditionGrade: string;
  storage?: string;
  color?: string;
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
}

export interface CreateProductDto {
  name: string;
  description: string;
  category: string;
  brand: string;
  model: string;
  purchasePrice: number;
  transportCost: number;
  sellingPrice: number;
  stock: number;
  minStockLevel: number;
  condition: string;
  conditionGrade: string;
  storage?: string;
  color?: string;
  memory?: string;
  processor?: string;
  screenSize?: string;
  supplierName: string;
  supplierCity: string;
  importBatch: string;
  invoiceNumber: string;
  notes?: string;
  warrantyInfo?: string;
  imageUrl?: string;
}

export interface UpdateProductDto extends Partial<CreateProductDto> {
  id: number;
}

export interface ProductFilters {
  search?: string;
  category?: string;
  brand?: string;
  lowStock?: boolean;
}

// Constantes pour les options
export const PRODUCT_CATEGORIES = [
  'Smartphones',
  'Tablets', 
  'Laptops',
  'Accessories'
] as const;

export const PRODUCT_CONDITIONS = [
  'Neuf',
  'Reconditionné', 
  'Occasion'
] as const;

export const CONDITION_GRADES = [
  'A+',
  'A',
  'B+', 
  'B',
  'C'
] as const;

export type ProductCategory = typeof PRODUCT_CATEGORIES[number];
export type ProductCondition = typeof PRODUCT_CONDITIONS[number];
export type ConditionGrade = typeof CONDITION_GRADES[number];