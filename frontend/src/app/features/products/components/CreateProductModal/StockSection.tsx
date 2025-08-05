// src/app/features/products/components/CreateProductModal/StockSection.tsx

'use client';

import React from 'react';
import { Package } from 'lucide-react';
import { FormSectionProps } from './types';

export default function StockSection({
  formData,
  errors,
  loading,
  onChange
}: FormSectionProps) {
  return (
    <div className="bg-blue-50 rounded-xl p-6">
      <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center gap-2">
        <Package className="h-5 w-5 text-blue-600" />
        Gestion du stock
      </h3>
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Quantité en stock *
          </label>
          <input
            type="number"
            name="stock"
            value={formData.stock}
            onChange={onChange}
            min="0"
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
              errors.stock ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="2"
            disabled={loading}
          />
          {errors.stock && <p className="text-red-600 text-sm mt-1">{errors.stock}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Stock minimum
          </label>
          <input
            type="number"
            name="minStockLevel"
            value={formData.minStockLevel}
            onChange={onChange}
            min="0"
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="1"
            disabled={loading}
          />
        </div>
      </div>
    </div>
  );
}