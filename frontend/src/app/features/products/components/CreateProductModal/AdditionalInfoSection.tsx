// src/app/features/products/components/CreateProductModal/AdditionalInfoSection.tsx

'use client';

import React from 'react';
import { FormSectionProps } from './types';

export default function AdditionalInfoSection({
  formData,
  errors,
  loading,
  onChange
}: FormSectionProps) {
  return (
    <div className="bg-gray-50 rounded-xl p-6">
      <h3 className="text-lg font-medium text-gray-900 mb-4">
        Informations complémentaires
      </h3>
      
      <div className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Informations de garantie
          </label>
          <input
            type="text"
            name="warrantyInfo"
            value={formData.warrantyInfo || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Garantie Apple 1 an"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            URL de l'image
          </label>
          <input
            type="url"
            name="imageUrl"
            value={formData.imageUrl || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="https://example.com/iphone-15-pro-max.jpg"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Notes
          </label>
          <textarea
            name="notes"
            value={formData.notes || ''}
            onChange={onChange}
            rows={3}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Notes supplémentaires..."
            disabled={loading}
          />
        </div>
      </div>
    </div>
  );
}