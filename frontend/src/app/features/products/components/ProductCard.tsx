'use client';

import React from 'react';
import { Product } from '../types';
import { Button } from '@/app/shared/components/ui';
import { formatCurrency } from '@/app/shared/utils';

interface ProductCardProps {
  product: Product;
  onEdit?: (product: Product) => void;
  onDelete?: (product: Product) => void;
  onView?: (product: Product) => void;
  onUpdateStock?: (product: Product) => void;
  onMarkAsSold?: (product: Product) => void;
}

export default function ProductCard({
  product,
  onEdit,
  onDelete,
  onView,
  onUpdateStock,
  onMarkAsSold
}: ProductCardProps) {

  // Icônes SVG
  const EditIcon = () => (
    <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
    </svg>
  );

  const DeleteIcon = () => (
    <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
    </svg>
  );

  const EyeIcon = () => (
    <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="M2.036 12.322a1.012 1.012 0 0 1 0-.639C3.423 7.51 7.36 4.5 12 4.5c4.638 0 8.573 3.007 9.963 7.178.07.207.07.431 0 .639C20.577 16.49 16.64 19.5 12 19.5c-4.638 0-8.573-3.007-9.963-7.178Z" />
      <path strokeLinecap="round" strokeLinejoin="round" d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z" />
    </svg>
  );

  const WarningIcon = () => (
    <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z" />
    </svg>
  );

  // Badge de statut
  const getStatusBadge = () => {
    const statusClasses = {
      Available: 'bg-green-100 text-green-800',
      Sold: 'bg-gray-100 text-gray-800',
      Reserved: 'bg-yellow-100 text-yellow-800',
      OutOfStock: 'bg-red-100 text-red-800'
    };

    return (
      <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${statusClasses[product.status as keyof typeof statusClasses] || 'bg-gray-100 text-gray-800'}`}>
        {product.status}
      </span>
    );
  };

  return (
    <div className="bg-white rounded-lg shadow-sm border border-gray-200 hover:shadow-md transition-shadow duration-200">
      {/* Header avec image et badge */}
      <div className="relative">
        {/* Image ou placeholder */}
        <div className="h-48 bg-gradient-to-br from-blue-50 to-purple-50 rounded-t-lg flex items-center justify-center">
          {product.imageUrl ? (
            <img 
              src={product.imageUrl} 
              alt={product.name}
              className="h-full w-full object-cover rounded-t-lg"
            />
          ) : (
            <div className="text-center">
              <div className="h-16 w-16 mx-auto bg-gradient-to-br from-blue-500 to-purple-600 rounded-lg flex items-center justify-center mb-2">
                <span className="text-white font-bold text-xl">
                  {product.brand.charAt(0)}
                </span>
              </div>
              <span className="text-sm text-gray-500">{product.brand}</span>
            </div>
          )}
        </div>

        {/* Badges en overlay */}
        <div className="absolute top-3 left-3">
          {getStatusBadge()}
        </div>

        <div className="absolute top-3 right-3">
          {product.isLowStock && (
            <div className="bg-red-100 text-red-800 p-1 rounded-full" title="Stock faible">
              <WarningIcon />
            </div>
          )}
        </div>
      </div>

      {/* Contenu */}
      <div className="p-4">
        {/* Titre et marque */}
        <div className="mb-3">
          <h3 className="text-lg font-semibold text-gray-900 line-clamp-2">
            {product.name}
          </h3>
          <p className="text-sm text-gray-600">
            {product.brand} - {product.model}
          </p>
        </div>

        {/* Spécifications clés */}
        <div className="mb-4 space-y-1">
          {product.storage && (
            <div className="flex justify-between text-sm">
              <span className="text-gray-500">Stockage:</span>
              <span className="text-gray-900">{product.storage}</span>
            </div>
          )}
          {product.color && (
            <div className="flex justify-between text-sm">
              <span className="text-gray-500">Couleur:</span>
              <span className="text-gray-900">{product.color}</span>
            </div>
          )}
          <div className="flex justify-between text-sm">
            <span className="text-gray-500">État:</span>
            <span className="text-gray-900">{product.condition} ({product.conditionGrade})</span>
          </div>
        </div>

        {/* Prix et marge */}
        <div className="mb-4 p-3 bg-gray-50 rounded-lg">
          <div className="flex justify-between items-center mb-2">
            <span className="text-lg font-bold text-gray-900">
              {formatCurrency(product.sellingPrice)}
            </span>
            <span className="text-sm font-medium text-green-600">
              Marge: {product.marginPercentage.toFixed(1)}%
            </span>
          </div>
          <div className="text-sm text-gray-600">
            Coût: {formatCurrency(product.totalCostPrice)} | 
            Bénéfice: {formatCurrency(product.margin)}
          </div>
        </div>

        {/* Stock */}
        <div className="mb-4 flex items-center justify-between">
          <div>
            <span className="text-sm text-gray-500">Stock:</span>
            <span className={`ml-2 font-medium ${product.isLowStock ? 'text-red-600' : 'text-gray-900'}`}>
              {product.stock}
            </span>
            {product.isLowStock && (
              <span className="ml-1 text-xs text-red-500">(Min: {product.minStockLevel})</span>
            )}
          </div>
          <div className="text-sm text-gray-500">
            Valeur: {formatCurrency(product.totalValue)}
          </div>
        </div>

        {/* Fournisseur */}
        <div className="mb-4 text-sm">
          <div className="text-gray-500">Fournisseur:</div>
          <div className="text-gray-900">{product.supplierName}, {product.supplierCity}</div>
          {product.importBatch && (
            <div className="text-gray-500">Lot: {product.importBatch}</div>
          )}
        </div>
      </div>

      {/* Actions */}
      <div className="px-4 pb-4 border-t border-gray-100 pt-4">
        <div className="flex flex-wrap gap-2">
          {/* Actions principales */}
          <Button
            size="sm"
            variant="secondary"
            onClick={() => onView?.(product)}
            icon={<EyeIcon />}
          >
            Voir
          </Button>

          <Button
            size="sm"
            variant="primary"
            onClick={() => onEdit?.(product)}
            icon={<EditIcon />}
          >
            Modifier
          </Button>

          <Button
            size="sm"
            variant="danger"
            onClick={() => onDelete?.(product)}
            icon={<DeleteIcon />}
          >
            Supprimer
          </Button>

          {/* Actions conditionnelles */}
          {product.status === 'Available' && (
            <Button
              size="sm"
              variant="success"
              onClick={() => onMarkAsSold?.(product)}
            >
              Marquer vendu
            </Button>
          )}

          {product.isLowStock && (
            <Button
              size="sm"
              variant="warning"
              onClick={() => onUpdateStock?.(product)}
            >
              Réapprovisionner
            </Button>
          )}
        </div>

        {/* Informations supplémentaires */}
        <div className="mt-3 pt-3 border-t border-gray-100 text-xs text-gray-500">
          <div className="flex justify-between">
            <span>Créé: {new Date(product.createdAt).toLocaleDateString('fr-FR')}</span>
            <span>En stock depuis: {product.daysInStock} jours</span>
          </div>
        </div>
      </div>
    </div>
  );
}