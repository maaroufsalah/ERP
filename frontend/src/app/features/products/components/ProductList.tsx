'use client';

import React, { useState, useEffect } from 'react';
import { Product } from '../types';
import { useProducts } from '../hooks/useProducts';
import ProductCard from './ProductCard';
import { Button, Input, Modal } from '@/app/shared/components/ui';
import { formatCurrency } from '@/app/shared/utils';
import ProductDetailModal from './ProductDetailModal';

interface ProductListProps {
  initialProducts?: Product[];
  showFilters?: boolean;
  showCreateButton?: boolean;
}

export default function ProductList({ 
  initialProducts, 
  showFilters = true,
  showCreateButton = true 
}: ProductListProps) {
  
  // État local pour les filtres
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedProductType, setSelectedProductType] = useState('');
  const [selectedBrand, setSelectedBrand] = useState('');
  const [showLowStockOnly, setShowLowStockOnly] = useState(false);
  const [viewMode, setViewMode] = useState<'grid' | 'list'>('grid');

  // États pour les modals
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [showDetailModal, setShowDetailModal] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);

  // Hook personnalisé
  const {
    products,
    loading,
    error,
    loadProducts,
    searchProducts,
    filterByCategory,
    filterByBrand,
    loadLowStockProducts,
    deleteProduct,
    markAsSold,
    clearError
  } = useProducts();

  // Chargement initial
  useEffect(() => {
    if (initialProducts) {
      // Si des produits sont fournis en props, les utiliser
    } else {
      loadProducts();
    }
  }, [initialProducts, loadProducts]);

  // Filtrage local (côté client pour éviter trop d'appels API)
  const displayProducts = (initialProducts || products).filter(product => {
    if (searchTerm && !product.name.toLowerCase().includes(searchTerm.toLowerCase()) &&
        !product.brandName?.toLowerCase().includes(searchTerm.toLowerCase()) &&
        !product.modelName?.toLowerCase().includes(searchTerm.toLowerCase())) {
      return false;
    }
    
    if (selectedProductType && product.productTypeName !== selectedProductType) {
      return false;
    }
    
    if (selectedBrand && product.brandName !== selectedBrand) {
      return false;
    }
    
    if (showLowStockOnly && !product.isLowStock) {
      return false;
    }
    
    return true;
  });

  // Obtenir les options uniques pour les filtres
  const allProducts = initialProducts || products;
  const uniqueProductTypes = Array.from(new Set(allProducts.map(p => p.productTypeName).filter(Boolean))).sort();
  const uniqueBrands = Array.from(new Set(allProducts.map(p => p.brandName).filter(Boolean))).sort();

  // Gestionnaires d'événements
  const handleEdit = (product: Product) => {
    setSelectedProduct(product);
    setShowDetailModal(false);
    setShowEditModal(true);
  };

  const handleDelete = (product: Product) => {
    setSelectedProduct(product);
    setShowDetailModal(false);
    setShowDeleteModal(true);
  };

  const handleView = (product: Product) => {
    setSelectedProduct(product);
    setShowDetailModal(true);
  };

  const handleConfirmDelete = async () => {
    if (selectedProduct) {
      try {
        await deleteProduct(selectedProduct.id);
        setShowDeleteModal(false);
        setSelectedProduct(null);
      } catch (error) {
        // Erreur gérée par le hook
      }
    }
  };

  const handleMarkAsSold = async (product: Product) => {
    try {
      await markAsSold(product.id);
      setShowDetailModal(false);
    } catch (error) {
      // Erreur gérée par le hook
    }
  };

  const resetFilters = () => {
    setSearchTerm('');
    setSelectedProductType('');
    setSelectedBrand('');
    setShowLowStockOnly(false);
  };

  // Icônes
  const SearchIcon = () => (
    <svg className="h-5 w-5" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
    </svg>
  );

  const PlusIcon = () => (
    <svg className="h-5 w-5" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
    </svg>
  );

  const GridIcon = () => (
    <svg className="h-5 w-5" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="M3.375 19.5h17.25m-17.25 0a1.125 1.125 0 0 1-1.125-1.125M3.375 19.5h7.5c.621 0 1.125-.504 1.125-1.125m-9.75 0V5.625m0 12.75A1.125 1.125 0 0 1 2.25 18.375m0 0V5.625m0 12.75c0 .621.504 1.125 1.125 1.125M3.375 5.625a1.125 1.125 0 0 1 1.125-1.125h17.25c.621 0 1.125.504 1.125 1.125v12.75c0 .621-.504 1.125-1.125 1.125M3.375 5.625v7.5c0 .621.504 1.125 1.125 1.125h7.5m-7.5-7.5v-1.5m0 7.5h7.5" />
    </svg>
  );

  const ListIcon = () => (
    <svg className="h-5 w-5" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="M8.25 6.75h12M8.25 12h12m-12 5.25h12M3.75 6.75h.007v.008H3.75V6.75Zm.375 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0ZM3.75 12h.007v.008H3.75V12Zm.375 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Zm-.375 5.25h.007v.008H3.75v-.008Zm.375 0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Z" />
    </svg>
  );

  return (
    <div className="space-y-6">
      {/* Header avec filtres */}
      {showFilters && (
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between space-y-4 lg:space-y-0">
            
            {/* Filtres de recherche */}
            <div className="flex flex-1 space-x-4">
              <div className="flex-1 max-w-xs">
                <div className="relative">
                  <input
                    type="text"
                    placeholder="Rechercher un produit..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                  />
                  <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                    <SearchIcon />
                  </div>
                </div>
              </div>
              
              <select
                value={selectedProductType}
                onChange={(e) => setSelectedProductType(e.target.value)}
                className="px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent cursor-pointer"
              >
                <option value="">Tous les types</option>
                {uniqueProductTypes.map(type => (
                  <option key={type} value={type}>{type}</option>
                ))}
              </select>

              <select
                value={selectedBrand}
                onChange={(e) => setSelectedBrand(e.target.value)}
                className="px-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent cursor-pointer"
              >
                <option value="">Toutes les marques</option>
                {uniqueBrands.map(brand => (
                  <option key={brand} value={brand}>{brand}</option>
                ))}
              </select>

              <label className="flex items-center space-x-2 cursor-pointer">
                <input
                  type="checkbox"
                  checked={showLowStockOnly}
                  onChange={(e) => setShowLowStockOnly(e.target.checked)}
                  className="rounded border-gray-300 text-blue-600 focus:ring-blue-500 cursor-pointer"
                />
                <span className="text-sm text-gray-700">Stock faible</span>
              </label>

              <Button
                variant="secondary"
                size="sm"
                onClick={resetFilters}
                className="cursor-pointer"
              >
                Réinitialiser
              </Button>
            </div>

            {/* Actions et vues */}
            <div className="flex items-center space-x-4">
              {/* Toggle vue */}
              <div className="flex border border-gray-300 rounded-md">
                <button
                  onClick={() => setViewMode('grid')}
                  className={`p-2 cursor-pointer transition-colors ${
                    viewMode === 'grid' ? 'bg-blue-50 text-blue-600' : 'text-gray-400 hover:text-gray-600'
                  }`}
                >
                  <GridIcon />
                </button>
                <button
                  onClick={() => setViewMode('list')}
                  className={`p-2 cursor-pointer transition-colors ${
                    viewMode === 'list' ? 'bg-blue-50 text-blue-600' : 'text-gray-400 hover:text-gray-600'
                  }`}
                >
                  <ListIcon />
                </button>
              </div>

              {/* Bouton créer */}
              {showCreateButton && (
                <Button
                  onClick={() => setShowCreateModal(true)}
                  icon={<PlusIcon />}
                  className="cursor-pointer"
                >
                  Ajouter un produit
                </Button>
              )}
            </div>
          </div>
        </div>
      )}

      {/* Message d'erreur */}
      {error && (
        <div className="bg-red-50 border border-red-200 rounded-md p-4">
          <div className="flex items-center justify-between">
            <p className="text-sm text-red-800">{error}</p>
            <Button 
              variant="secondary" 
              size="sm" 
              onClick={clearError}
              className="cursor-pointer"
            >
              Fermer
            </Button>
          </div>
        </div>
      )}

      {/* État de chargement */}
      {loading && (
        <div className="flex items-center justify-center py-12">
          <div className="flex items-center space-x-2">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
            <span className="text-gray-600">Chargement des produits...</span>
          </div>
        </div>
      )}

      {/* Liste des produits */}
      {!loading && displayProducts.length > 0 && (
        <>
          {/* Statistiques */}
          <div className="flex items-center justify-between">
            <div className="text-sm text-gray-600">
              {displayProducts.length} produit(s) trouvé(s)
              {displayProducts.length !== allProducts.length && (
                <span className="text-gray-400"> sur {allProducts.length}</span>
              )}
            </div>
            <div className="text-sm text-gray-600">
              Valeur totale: {formatCurrency(displayProducts.reduce((sum, p) => sum + p.totalValue, 0))}
            </div>
          </div>

          {/* Grille de produits */}
          <div className={`
            ${viewMode === 'grid' 
              ? 'grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6' 
              : 'space-y-4'
            }
          `}>
            {displayProducts.map((product) => (
              <ProductCard
                key={product.id}
                product={product}
                onEdit={handleEdit}
                onDelete={handleDelete}
                onView={handleView}
                onMarkAsSold={handleMarkAsSold}
              />
            ))}
          </div>
        </>
      )}

      {/* État vide */}
      {!loading && displayProducts.length === 0 && (
        <div className="text-center py-12">
          <div className="mx-auto h-12 w-12 text-gray-400">
            <svg fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2M4 13h2m8-8v4m-4 0h8" />
            </svg>
          </div>
          <h3 className="mt-2 text-sm font-medium text-gray-900">Aucun produit trouvé</h3>
          <p className="mt-1 text-sm text-gray-500">
            {searchTerm || selectedProductType || selectedBrand || showLowStockOnly
              ? 'Aucun produit ne correspond à vos critères de recherche.'
              : 'Commencez par ajouter votre premier produit.'
            }
          </p>
          {(searchTerm || selectedProductType || selectedBrand || showLowStockOnly) ? (
            <div className="mt-6">
              <Button
                onClick={resetFilters}
                className="cursor-pointer"
              >
                Réinitialiser les filtres
              </Button>
            </div>
          ) : showCreateButton && (
            <div className="mt-6">
              <Button
                onClick={() => setShowCreateModal(true)}
                icon={<PlusIcon />}
                className="cursor-pointer"
              >
                Ajouter un produit
              </Button>
            </div>
          )}
        </div>
      )}

      {/* Modal de détail du produit */}
      <ProductDetailModal
        product={selectedProduct}
        isOpen={showDetailModal}
        onClose={() => {
          setShowDetailModal(false);
          setSelectedProduct(null);
        }}
        onEdit={handleEdit}
        onDelete={handleDelete}
        onMarkAsSold={handleMarkAsSold}
      />

      {/* Modal de confirmation suppression */}
      <Modal
        isOpen={showDeleteModal}
        onClose={() => setShowDeleteModal(false)}
        title="Confirmer la suppression"
        size="sm"
      >
        <div className="space-y-4">
          <p className="text-sm text-gray-600">
            Êtes-vous sûr de vouloir supprimer le produit "{selectedProduct?.name}" ?
            Cette action est irréversible.
          </p>
          
          <div className="flex justify-end space-x-3">
            <Button
              variant="secondary"
              onClick={() => setShowDeleteModal(false)}
              className="cursor-pointer"
            >
              Annuler
            </Button>
            <Button
              variant="danger"
              onClick={handleConfirmDelete}
              loading={loading}
              className="cursor-pointer"
            >
              Supprimer
            </Button>
          </div>
        </div>
      </Modal>

      {/* Modal de création - à implémenter */}
      {showCreateModal && (
        <Modal
          isOpen={showCreateModal}
          onClose={() => setShowCreateModal(false)}
          title="Créer un nouveau produit"
          size="lg"
        >
          <div className="p-4 text-center text-gray-500">
            Modal de création à implémenter
          </div>
        </Modal>
      )}

      {/* Modal d'édition - à implémenter */}
      {showEditModal && (
        <Modal
          isOpen={showEditModal}
          onClose={() => setShowEditModal(false)}
          title="Modifier le produit"
          size="lg"
        >
          <div className="p-4 text-center text-gray-500">
            Modal d'édition à implémenter
          </div>
        </Modal>
      )}
    </div>
  );
}