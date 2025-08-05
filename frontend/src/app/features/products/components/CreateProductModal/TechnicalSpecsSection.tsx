// src/app/features/products/components/CreateProductModal/TechnicalSpecsSection.tsx

'use client';

import React from 'react';
import { FormSectionProps } from './types';

export default function TechnicalSpecsSection({
  formData,
  errors,
  loading,
  onChange
}: FormSectionProps) {
  return (
    <div className="bg-gray-50 rounded-xl p-6">
      <h3 className="text-lg font-medium text-gray-900 mb-4">
        Caractéristiques techniques
      </h3>
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Stockage
          </label>
          <input
            type="text"
            name="storage"
            value={formData.storage || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Ex: 256GB"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Mémoire
          </label>
          <input
            type="text"
            name="memory"
            value={formData.memory || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Ex: 8GB"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Processeur
          </label>
          <input
            type="text"
            name="processor"
            value={formData.processor || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Ex: A17 Pro"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Taille écran
          </label>
          <input
            type="text"
            name="screenSize"
            value={formData.screenSize || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Ex: 6.7\"
            disabled={loading}
          />
        </div>
      </div>
    </div>
  );
}