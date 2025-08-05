'use client';

import React from 'react';
import { Modal, Button } from '@/app/shared/components/ui';
import { formatCurrency, formatDate } from '@/app/shared/utils';
import { Product } from '../types';

interface ProductDetailModalProps {
  product: Product | null;
  isOpen: boolean;
  onClose: () => void;
  onEdit?: (product: Product) => void;
  onDelete?: (product: Product) => void;
  onMarkAsSold?: (product: Product) => void;
  onUpdateStock?: (product: Product) => void;
}

export default function ProductDetailModal({
  product,
  isOpen,
  onClose,
  onEdit,
  onDelete,
  onMarkAsSold,
  onUpdateStock
}: ProductDetailModalProps) {
  if (!product) return null;

  const getStatusColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case 'available': return 'bg-green-100 text-green-800 border-green-200';
      case 'sold': return 'bg-gray-100 text-gray-800 border-gray-200';
      case 'reserved': return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'inactive': return 'bg-red-100 text-red-800 border-red-200';
      default: return 'bg-blue-100 text-blue-800 border-blue-200';
    }
  };

  const getConditionColor = (condition?: string) => {
    if (!condition) return 'bg-gray-100 text-gray-800';
    const lowerCondition = condition.toLowerCase();
    if (lowerCondition.includes('neuf') || lowerCondition.includes('excellent')) {
      return 'bg-green-100 text-green-800';
    }
    if (lowerCondition.includes('bon') || lowerCondition.includes('reconditionné')) {
      return 'bg-yellow-100 text-yellow-800';
    }
    return 'bg-orange-100 text-orange-800';
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title="Détails du produit"
      size="xl"
    >
      <div className="space-y-6">
        {/* En-tête avec image et infos principales */}
        <div className="flex items-start space-x-6">
          {/* Image */}
          <div className="flex-shrink-0">
            <div className="w-32 h-32 bg-gray-100 rounded-lg overflow-hidden">
              {product.imageUrl ? (
                <img 
                  src={product.imageUrl} 
                  alt={product.name}
                  className="w-full h-full object-cover"
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center bg-gradient-to-br from-blue-500 to-purple-600">
                  <span className="text-white font-bold text-3xl">
                    {product.brandName?.charAt(0) || 'P'}
                  </span>
                </div>
              )}
            </div>
          </div>

          {/* Informations principales */}
          <div className="flex-1">
            <div className="flex items-start justify-between">
              <div>
                <h2 className="text-2xl font-bold text-gray-900 mb-2">{product.name}</h2>
                <div className="space-y-1 text-gray-600">
                  <p className="text-lg">
                    <span className="font-medium">{product.brandName || 'Sans marque'}</span>
                    {product.modelName && (
                      <span> - {product.modelName}</span>
                    )}
                  </p>
                  <p>{product.productTypeName || 'Type non défini'}</p>
                  {product.description && (
                    <p className="text-sm mt-2">{product.description}</p>
                  )}
                </div>
              </div>
              
              {/* Badges de statut */}
              <div className="flex flex-col space-y-2">
                <span className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium border ${getStatusColor(product.status)}`}>
                  {product.status}
                </span>
                {product.isLowStock && (
                  <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-red-100 text-red-800 border border-red-200">
                    ⚠️ Stock faible
                  </span>
                )}
              </div>
            </div>
          </div>
        </div>

        {/* Grille d'informations */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          
          {/* Informations générales */}
          <div className="bg-gray-50 rounded-lg p-4">
            <h3 className="font-semibold text-gray-900 mb-4 flex items-center">
              <svg className="w-5 h-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              Informations générales
            </h3>
            <div className="space-y-3">
              {product.color?.name && (
                <div className="flex justify-between items-center">
                  <span className="text-gray-600">Couleur:</span>
                  <div className="flex items-center space-x-2">
                    {product.color.hexCode && (
                      <div 
                        className="w-4 h-4 rounded-full border border-gray-300"
                        style={{ backgroundColor: product.color.hexCode }}
                      />
                    )}
                    <span className="font-medium">{product.color.name}</span>
                  </div>
                </div>
              )}
              
              {product.condition?.name && (
                <div className="flex justify-between items-center">
                  <span className="text-gray-600">Condition:</span>
                  <span className={`px-2 py-1 rounded-full text-xs font-medium ${getConditionColor(product.condition.name)}`}>
                    {product.condition.name}
                    {product.condition.description && ` - ${product.condition.description}`}
                  </span>
                </div>
              )}

              {product.storage && (
                <div className="flex justify-between">
                  <span className="text-gray-600">Stockage:</span>
                  <span className="font-medium">{product.storage}</span>
                </div>
              )}

              {product.memory && (
                <div className="flex justify-between">
                  <span className="text-gray-600">RAM:</span>
                  <span className="font-medium">{product.memory}</span>
                </div>
              )}

              {product.processor && (
                <div className="flex justify-between">
                  <span className="text-gray-600">Processeur:</span>
                  <span className="font-medium">{product.processor}</span>
                </div>
              )}

              {product.screenSize && (
                <div className="flex justify-between">
                  <span className="text-gray-600">Écran:</span>
                  <span className="font-medium">{product.screenSize}</span>
                </div>
              )}
            </div>
          </div>

          {/* Informations financières */}
          <div className="bg-green-50 rounded-lg p-4">
            <h3 className="font-semibold text-gray-900 mb-4 flex items-center">
              <svg className="w-5 h-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1" />
              </svg>
              Informations financières
            </h3>
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="text-gray-600">Prix d'achat:</span>
                <span className="font-medium">{formatCurrency(product.purchasePrice)}</span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Frais de transport:</span>
                <span className="font-medium">{formatCurrency(product.transportCost)}</span>
              </div>
              
              <div className="flex justify-between border-t pt-2">
                <span className="text-gray-600">Coût total:</span>
                <span className="font-bold">{formatCurrency(product.totalCostPrice)}</span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Prix de vente:</span>
                <span className="font-bold text-green-600 text-lg">{formatCurrency(product.sellingPrice)}</span>
              </div>
              
              <div className="bg-white rounded p-3 border">
                <div className="flex justify-between mb-1">
                  <span className="text-gray-600">Marge:</span>
                  <span className={`font-bold ${product.margin >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                    {formatCurrency(product.margin)}
                  </span>
                </div>
                <div className="flex justify-between">
                  <span className="text-gray-600">Pourcentage:</span>
                  <span className={`font-bold ${product.marginPercentage >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                    {product.marginPercentage.toFixed(1)}%
                  </span>
                </div>
              </div>
            </div>
          </div>

          {/* Stock et logistique */}
          <div className="bg-blue-50 rounded-lg p-4">
            <h3 className="font-semibold text-gray-900 mb-4 flex items-center">
              <svg className="w-5 h-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
              </svg>
              Stock & Logistique
            </h3>
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="text-gray-600">Stock actuel:</span>
                <span className={`font-bold ${product.isLowStock ? 'text-red-600' : 'text-gray-900'}`}>
                  {product.stock} unités
                </span>
              </div>
              
              <div className="flex justify-between">
                <span className="text-gray-600">Stock minimum:</span>
                <span className="font-medium">{product.minStockLevel}</span>
              </div>
              
              <div className="flex justify-between border-t pt-2">
                <span className="text-gray-600">Valeur totale:</span>
                <span className="font-bold">{formatCurrency(product.totalValue)}</span>
              </div>
              
              <div className="bg-white rounded p-3 border">
                <div className="text-sm space-y-2">
                  <div className="flex justify-between">
                    <span className="text-gray-600">Fournisseur:</span>
                    <span className="font-medium">{product.supplierName}</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-600">Ville:</span>
                    <span>{product.supplierCity}</span>
                  </div>
                  {product.importBatch && (
                    <div className="flex justify-between">
                      <span className="text-gray-600">Lot d'import:</span>
                      <span>{product.importBatch}</span>
                    </div>
                  )}
                  {product.invoiceNumber && (
                    <div className="flex justify-between">
                      <span className="text-gray-600">N° facture:</span>
                      <span>{product.invoiceNumber}</span>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Dates et informations additionnelles */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="bg-gray-50 rounded-lg p-4">
            <h3 className="font-semibold text-gray-900 mb-3">Dates importantes</h3>
            <div className="space-y-2 text-sm">
              <div className="flex justify-between">
                <span className="text-gray-600">Date d'achat:</span>
                <span>{formatDate(product.purchaseDate)}</span>
              </div>
              {product.arrivalDate && (
                <div className="flex justify-between">
                  <span className="text-gray-600">Date d'arrivée:</span>
                  <span>{formatDate(product.arrivalDate)}</span>
                </div>
              )}
              <div className="flex justify-between">
                <span className="text-gray-600">Créé le:</span>
                <span>{formatDate(product.createdAt)}</span>
              </div>
              {product.updatedAt && (
                <div className="flex justify-between">
                  <span className="text-gray-600">Modifié le:</span>
                  <span>{formatDate(product.updatedAt)}</span>
                </div>
              )}
              <div className="flex justify-between">
                <span className="text-gray-600">Jours en stock:</span>
                <span className="font-medium">{product.daysInStock} jours</span>
              </div>
            </div>
          </div>

          <div className="bg-gray-50 rounded-lg p-4">
            <h3 className="font-semibold text-gray-900 mb-3">Informations supplémentaires</h3>
            <div className="space-y-2 text-sm">
              {product.warrantyInfo && (
                <div>
                  <span className="text-gray-600">Garantie:</span>
                  <p className="mt-1">{product.warrantyInfo}</p>
                </div>
              )}
              {product.notes && (
                <div>
                  <span className="text-gray-600">Notes:</span>
                  <p className="mt-1">{product.notes}</p>
                </div>
              )}
              <div className="flex justify-between">
                <span className="text-gray-600">Créé par:</span>
                <span>{product.createdBy || 'Système'}</span>
              </div>
              {product.updatedBy && (
                <div className="flex justify-between">
                  <span className="text-gray-600">Modifié par:</span>
                  <span>{product.updatedBy}</span>
                </div>
              )}
              <div className="flex justify-between">
                <span className="text-gray-600">Statut:</span>
                <span className={product.isActive ? 'text-green-600' : 'text-red-600'}>
                  {product.isActive ? 'Actif' : 'Inactif'}
                </span>
              </div>
            </div>
          </div>
        </div>

        {/* Actions */}
        <div className="flex justify-between items-center pt-6 border-t border-gray-200">
          <div className="flex space-x-3">
            <Button 
              variant="secondary" 
              onClick={onClose}
              className="cursor-pointer"
            >
              Fermer
            </Button>
          </div>
          
          <div className="flex space-x-3">
            {product.isLowStock && onUpdateStock && (
              <Button 
                variant="warning" 
                onClick={() => onUpdateStock(product)}
                className="cursor-pointer"
              >
                Réapprovisionner
              </Button>
            )}
            
            {product.status === 'Available' && onMarkAsSold && (
              <Button 
                variant="success" 
                onClick={() => onMarkAsSold(product)}
                className="cursor-pointer"
              >
                Marquer vendu
              </Button>
            )}
            
            {onEdit && (
              <Button 
                variant="primary" 
                onClick={() => onEdit(product)}
                className="cursor-pointer"
              >
                Modifier
              </Button>
            )}
            
            {onDelete && (
              <Button 
                variant="danger" 
                onClick={() => onDelete(product)}
                className="cursor-pointer"
              >
                Supprimer
              </Button>
            )}
          </div>
        </div>
      </div>
    </Modal>
  );
}