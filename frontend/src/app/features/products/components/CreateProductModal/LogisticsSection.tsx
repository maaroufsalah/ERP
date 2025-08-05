// src/app/features/products/components/CreateProductModal/LogisticsSection.tsx

'use client';

import React from 'react';
import { Truck } from 'lucide-react';
import { FormSectionProps, FormData } from './types';

interface LogisticsSectionProps extends Omit<FormSectionProps, 'onChange'> {
  onChange: (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void;
  onDateChange: (name: string, value: string) => void;
}

export default function LogisticsSection({
  formData,
  errors,
  loading,
  onChange,
  onDateChange
}: LogisticsSectionProps) {
  return (
    <div className="bg-orange-50 rounded-xl p-6">
      <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center gap-2">
        <Truck className="h-5 w-5 text-orange-600" />
        Fournisseur et logistique
      </h3>
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Nom du fournisseur *
          </label>
          <input
            type="text"
            name="supplierName"
            value={formData.supplierName}
            onChange={onChange}
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
              errors.supplierName ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="AppleStore Roma"
            disabled={loading}
          />
          {errors.supplierName && <p className="text-red-600 text-sm mt-1">{errors.supplierName}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Ville du fournisseur
          </label>
          <input
            type="text"
            name="supplierCity"
            value={formData.supplierCity || ''}
            onChange={onChange}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
            placeholder="Roma"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Date d'achat *
          </label>
          <input
            type="date"
            name="purchaseDate"
            value={formData.purchaseDate ? formData.purchaseDate.split('T')[0] : ''}
            onChange={(e) => onDateChange('purchaseDate', e.target.value)}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Date d'arrivée *
          </label>
          <input
            type="date"
            name="arrivalDate"
            value={formData.arrivalDate ? formData.arrivalDate.split('T')[0] : ''}
            onChange={(e) => onDateChange('arrivalDate', e.target.value)}
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
            disabled={loading}
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Lot d'import *
          </label>
          <input
            type="text"
            name="importBatch"
            value={formData.importBatch}
            onChange={onChange}
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
              errors.importBatch ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="IT2025006"
            disabled={loading}
          />
          {errors.importBatch && <p className="text-red-600 text-sm mt-1">{errors.importBatch}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            N° de facture *
          </label>
          <input
            type="text"
            name="invoiceNumber"
            value={formData.invoiceNumber}
            onChange={onChange}
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
              errors.invoiceNumber ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="INV-2025-009"
            disabled={loading}
          />
          {errors.invoiceNumber && <p className="text-red-600 text-sm mt-1">{errors.invoiceNumber}</p>}
        </div>
      </div>
    </div>
  );
}