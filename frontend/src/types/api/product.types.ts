// src/types/api/product.types.ts

export interface ProductDto {
  id: number;
  name: string;
  description?: string;
  category: string;
  brand?: string;
  model?: string;
  
  // Pricing & Costs
  purchasePrice: number;
  transportCost: number;
  totalCostPrice: number;
  sellingPrice: number;
  margin: number;
  marginPercentage: number;
  
  // Technical Specifications
  storage?: string;
  color?: string;
  memory?: string;
  processor?: string;
  screenSize?: string;
  
  // Condition & Status
  condition?: string;
  conditionGrade?: string;
  status: string;
  
  // Import Information
  supplierName?: string;
  supplierCity?: string;
  importBatch?: string;
  invoiceNumber?: string;
  
  // Stock & Inventory
  stockQuantity: number;
  minStockLevel: number;
  location?: string;
  
  // Additional Fields
  notes?: string;
  warrantyInfo?: string;
  
  // Computed Properties
  totalValue: number;
  isLowStock: boolean;
  daysInStock: number;
  
  // Audit Fields
  isActive: boolean;
  createdAt: string;
  createdBy: string;
  updatedAt?: string;
  updatedBy?: string;
}

export interface ProductForListDto {
  id: number;
  name: string;
  category: string;
  brand?: string;
  sellingPrice: number;
  stockQuantity: number;
  status: string;
  totalValue: number;
  isLowStock: boolean;
  daysInStock: number;
}

export interface CreateProductDto {
  name: string;
  description?: string;
  category: string;
  brand?: string;
  model?: string;
  
  // Pricing & Costs (required for calculations)
  purchasePrice: number;
  transportCost: number;
  sellingPrice: number;
  
  // Technical Specifications
  storage?: string;
  color?: string;
  memory?: string;
  processor?: string;
  screenSize?: string;
  
  // Condition & Status
  condition?: string;
  conditionGrade?: string;
  status?: string;
  
  // Import Information
  supplierName?: string;
  supplierCity?: string;
  importBatch?: string;
  invoiceNumber?: string;
  
  // Stock & Inventory
  stockQuantity: number;
  minStockLevel: number;
  location?: string;
  
  // Additional Fields
  notes?: string;
  warrantyInfo?: string;
}

export interface UpdateProductDto {
  name?: string;
  description?: string;
  category?: string;
  brand?: string;
  model?: string;
  
  // Pricing & Costs
  purchasePrice?: number;
  transportCost?: number;
  sellingPrice?: number;
  
  // Technical Specifications
  storage?: string;
  color?: string;
  memory?: string;
  processor?: string;
  screenSize?: string;
  
  // Condition & Status
  condition?: string;
  conditionGrade?: string;
  status?: string;
  
  // Import Information
  supplierName?: string;
  supplierCity?: string;
  importBatch?: string;
  invoiceNumber?: string;
  
  // Stock & Inventory
  stockQuantity?: number;
  minStockLevel?: number;
  location?: string;
  
  // Additional Fields
  notes?: string;
  warrantyInfo?: string;
}

// API Response Types
export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

// Filter and Search Types
export interface ProductFilters {
  category?: string;
  brand?: string;
  supplier?: string;
  status?: string;
  condition?: string;
  minPrice?: number;
  maxPrice?: number;
  lowStock?: boolean;
  search?: string;
}

export interface ProductStats {
  totalProducts: number;
  totalValue: number;
  lowStockCount: number;
  averageMargin: number;
}

// src/types/forms/product.forms.ts

import { z } from 'zod';

export const createProductSchema = z.object({
  name: z.string().min(1, 'Le nom est requis').max(200),
  description: z.string().max(1000).optional(),
  category: z.string().min(1, 'La catégorie est requise').max(100),
  brand: z.string().max(100).optional(),
  model: z.string().max(100).optional(),
  
  // Pricing & Costs
  purchasePrice: z.number().min(0, 'Le prix d\'achat doit être positif'),
  transportCost: z.number().min(0, 'Le coût de transport doit être positif'),
  sellingPrice: z.number().min(0, 'Le prix de vente doit être positif'),
  
  // Technical Specifications
  storage: z.string().max(50).optional(),
  color: z.string().max(50).optional(),
  memory: z.string().max(50).optional(),
  processor: z.string().max(150).optional(),
  screenSize: z.string().max(20).optional(),
  
  // Condition & Status
  condition: z.string().max(50).optional(),
  conditionGrade: z.string().max(10).optional(),
  status: z.string().max(50).optional(),
  
  // Import Information
  supplierName: z.string().max(200).optional(),
  supplierCity: z.string().max(100).optional(),
  importBatch: z.string().max(50).optional(),
  invoiceNumber: z.string().max(100).optional(),
  
  // Stock & Inventory
  stockQuantity: z.number().int().min(0, 'La quantité en stock doit être positive'),
  minStockLevel: z.number().int().min(0, 'Le niveau de stock minimum doit être positif'),
  location: z.string().max(100).optional(),
  
  // Additional Fields
  notes: z.string().max(500).optional(),
  warrantyInfo: z.string().max(300).optional(),
});

export const updateProductSchema = createProductSchema.partial();

export type CreateProductForm = z.infer<typeof createProductSchema>;
export type UpdateProductForm = z.infer<typeof updateProductSchema>;