// src/app/features/products/components/CreateProductModal/PricingSection.tsx

'use client';

import React from 'react';
import { Euro, Calculator } from 'lucide-react';
import { FormSectionProps } from './types';

export default function PricingSection({
  formData,
  errors,
  loading,
  onChange
}: FormSectionProps) {
  
  // Calculer automatiquement la marge
  const calculateMargin = () => {
    const purchase = formData.purchasePrice || 0;
    const transport = formData.transportCost || 0;
    const selling = formData.sellingPrice || 0;
    const totalCost = purchase + transport;
    const margin = selling - totalCost;
    const marginPercent = totalCost > 0 ? ((margin / totalCost) * 100) : 0;
    
    return { margin, marginPercent, totalCost };
  };

  const { margin, marginPercent, totalCost } = calculateMargin();

  return (
    <div className="bg-gradient-to-br from-green-50 to-emerald-50 rounded-xl p-6 border border-green-100">
      <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center gap-2">
        <Euro className="h-5 w-5 text-green-600" />
        Prix et rentabilité
      </h3>
      
      <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Prix d'achat * <span className="text-xs text-gray-500">(€)</span>
          </label>
          <input
            type="number"
            name="purchasePrice"
            value={formData.purchasePrice}
            onChange={onChange}
            min="0"
            step="0.01"
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 ${
              errors.purchasePrice ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="950.00"
            disabled={loading}
          />
          {errors.purchasePrice && <p className="text-red-600 text-sm mt-1">{errors.purchasePrice}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Frais de transport <span className="text-xs text-gray-500">(€)</span>
          </label>
          <input
            type="number"
            name="transportCost"
            value={formData.transportCost}
            onChange={onChange}
            min="0"
            step="0.01"
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500"
            placeholder="30.00"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Prix de vente * <span className="text-xs text-gray-500">(€)</span>
          </label>
          <input
            type="number"
            name="sellingPrice"
            value={formData.sellingPrice}
            onChange={onChange}
            min="0"
            step="0.01"
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 ${
              errors.sellingPrice ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="1299.00"
            disabled={loading}
          />
          {errors.sellingPrice && <p className="text-red-600 text-sm mt-1">{errors.sellingPrice}</p>}
        </div>
      </div>

      {/* Calcul automatique de la marge */}
      {(formData.purchasePrice > 0 || formData.transportCost > 0 || formData.sellingPrice > 0) && (
        <div className="bg-white rounded-lg p-4 border border-green-200">
          <div className="flex items-center gap-2 mb-3">
            <Calculator className="h-4 w-4 text-green-600" />
            <span className="text-sm font-medium text-gray-700">Calcul automatique</span>
          </div>
          <div className="grid grid-cols-3 gap-4 text-sm">
            <div>
              <span className="text-gray-500">Coût total:</span>
              <p className="font-semibold text-gray-900">{totalCost.toFixed(2)} €</p>
            </div>
            <div>
              <span className="text-gray-500">Marge:</span>
              <p className={`font-semibold ${margin >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {margin.toFixed(2)} €
              </p>
            </div>
            <div>
              <span className="text-gray-500">Marge %:</span>
              <p className={`font-semibold ${marginPercent >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {marginPercent.toFixed(1)}%
              </p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}