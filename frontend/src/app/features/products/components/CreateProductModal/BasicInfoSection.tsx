// src/app/features/products/components/CreateProductModal/BasicInfoSection.tsx

'use client';

import React from 'react';
import { Package } from 'lucide-react';
import { FormSectionProps } from './types';
import { useReferences } from '../../hooks/useReferences';
import Dropdown from '@/app/shared/components/form/Dropdown';

interface BasicInfoSectionProps extends FormSectionProps {
  onDropdownChange: (name: string, value: number) => void;
}

export default function BasicInfoSection({
  formData,
  errors,
  loading,
  onChange,
  onDropdownChange
}: BasicInfoSectionProps) {
  const {
    productTypes,
    brands,
    models,
    colors,
    conditions,
    loading: referencesLoading
  } = useReferences();

  return (
    <div className="bg-gray-50 rounded-xl p-6">
      <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center gap-2">
        <Package className="h-5 w-5 text-blue-600" />
        Informations du produit
      </h3>
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {/* Nom du produit */}
        <div className="md:col-span-2">
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Nom du produit *
          </label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={onChange}
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
              errors.name ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="Ex: iPhone 15 Pro Max 256GB"
            disabled={loading}
          />
          {errors.name && <p className="text-red-600 text-sm mt-1">{errors.name}</p>}
        </div>
        
        {/* Description */}
        <div className="md:col-span-2">
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Description *
          </label>
          <textarea
            name="description"
            value={formData.description}
            onChange={onChange}
            rows={3}
            className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
              errors.description ? 'border-red-300' : 'border-gray-300'
            }`}
            placeholder="Description détaillée du produit..."
            disabled={loading}
          />
          {errors.description && <p className="text-red-600 text-sm mt-1">{errors.description}</p>}
        </div>

        {/* Type de produit */}
        <Dropdown
          name="productTypeId"
          value={formData.productTypeId}
          options={productTypes}
          onChange={(value) => onDropdownChange('productTypeId', value)}
          label="Type de produit"
          placeholder="Sélectionner un type..."
          required
          disabled={loading}
          loading={referencesLoading}
          error={errors.productTypeId}
        />

        {/* Marque */}
        <Dropdown
          name="brandId"
          value={formData.brandId}
          options={brands}
          onChange={(value) => onDropdownChange('brandId', value)}
          label="Marque"
          placeholder="Sélectionner une marque..."
          required
          disabled={loading}
          loading={referencesLoading}
          error={errors.brandId}
        />

        {/* Modèle */}
        <Dropdown
          name="modelId"
          value={formData.modelId}
          options={models}
          onChange={(value) => onDropdownChange('modelId', value)}
          label="Modèle"
          placeholder="Sélectionner un modèle..."
          required
          disabled={loading || !formData.brandId}
          loading={referencesLoading}
          error={errors.modelId}
        />

        {/* Couleur */}
        <Dropdown
          name="colorId"
          value={formData.colorId || ''}
          options={colors}
          onChange={(value) => onDropdownChange('colorId', value)}
          label="Couleur"
          placeholder="Sélectionner une couleur..."
          disabled={loading}
          loading={referencesLoading}
          error={errors.colorId}
        />

        {/* Condition */}
        <div className="md:col-span-2">
          <Dropdown
            name="conditionId"
            value={formData.conditionId}
            options={conditions}
            onChange={(value) => onDropdownChange('conditionId', value)}
            label="Condition"
            placeholder="Sélectionner une condition..."
            required
            disabled={loading}
            loading={referencesLoading}
            error={errors.conditionId}
            showDescription={true}
          />
        </div>
      </div>
    </div>
  );
}