// src/app/features/products/components/ProductTable.tsx

'use client';

import React, { useState, useEffect } from 'react';
import { Product } from '../types';
import { useProducts } from '../hooks/useProducts';
import { 
  Search, 
  Plus, 
  Edit, 
  Trash2, 
  Eye, 
  AlertTriangle,
  Package,
  TrendingUp,
  DollarSign,
  MoreHorizontal,
  Filter
} from 'lucide-react';

export default function ProductTable() {
  const {
    products,
    loading,
    error,
    loadProducts,
    deleteProduct,
    searchProducts,
    clearError
  } = useProducts();

  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [showFilters, setShowFilters] = useState(false);

  // ✅ AJOUT : Chargement automatique des produits au montage du composant
  useEffect(() => {
    console.log('ProductTable mounted, loading products...');
    loadProducts();
  }, [loadProducts]);

  // Statistiques rapides
  const stats = {
    total: products.length,
    lowStock: products.filter(p => p.stock && p.stock < 10).length, // Ajustement pour éviter les erreurs
    totalValue: products.reduce((sum, p) => sum + (p.sellingPrice * (p.stock || 0)), 0),
    averageMargin: products.length > 0 
      ? products.reduce((sum, p) => sum + (p.marginPercentage || 0), 0) / products.length 
      : 0
  };

  const categories = [...new Set(products.map(p => p.category).filter(Boolean))];

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (searchTerm.trim()) {
      await searchProducts(searchTerm);
    } else {
      await loadProducts(); // Recharger tous les produits si recherche vide
    }
  };

  const handleDelete = async (id: number, name: string) => {
    if (confirm(`Êtes-vous sûr de vouloir supprimer "${name}" ?`)) {
      try {
        await deleteProduct(id);
      } catch (error) {
        console.error('Erreur lors de la suppression:', error);
      }
    }
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('fr-FR', {
      style: 'currency',
      currency: 'EUR'
    }).format(amount);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('fr-FR');
  };

  const getStatusColor = (status: string) => {
    switch (status?.toLowerCase()) {
      case 'available': return 'bg-green-100 text-green-800';
      case 'sold': return 'bg-gray-100 text-gray-800';
      case 'reserved': return 'bg-yellow-100 text-yellow-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status?.toLowerCase()) {
      case 'available': return 'Disponible';
      case 'sold': return 'Vendu';
      case 'reserved': return 'Réservé';
      default: return status || 'Inconnu';
    }
  };

  // Affichage du loading initial
  if (loading && products.length === 0) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="flex items-center space-x-2">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
          <span className="text-gray-600">Chargement des produits...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        
        {/* Header */}
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-gray-900">Gestion des Produits</h1>
          <p className="mt-2 text-gray-600">
            CRUD complet pour vos produits avec gestion des stocks et prix
          </p>
        </div>

        {/* Message d'erreur */}
        {error && (
          <div className="mb-6 bg-red-50 border border-red-200 rounded-md p-4">
            <div className="flex items-center justify-between">
              <div className="flex items-center">
                <AlertTriangle className="h-5 w-5 text-red-400 mr-2" />
                <p className="text-sm text-red-800">{error}</p>
              </div>
              <button
                onClick={clearError}
                className="text-red-400 hover:text-red-600"
              >
                ×
              </button>
            </div>
          </div>
        )}

        {/* Statistiques */}
        <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
          <div className="bg-white overflow-hidden shadow-sm rounded-lg border border-gray-200">
            <div className="p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <Package className="h-8 w-8 text-blue-600" />
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Total Produits</dt>
                    <dd className="text-2xl font-bold text-gray-900">{stats.total}</dd>
                  </dl>
                </div>
              </div>
            </div>
          </div>

          <div className="bg-white overflow-hidden shadow-sm rounded-lg border border-gray-200">
            <div className="p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <DollarSign className="h-8 w-8 text-green-600" />
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Valeur Totale</dt>
                    <dd className="text-2xl font-bold text-green-600">{formatCurrency(stats.totalValue)}</dd>
                  </dl>
                </div>
              </div>
            </div>
          </div>

          <div className="bg-white overflow-hidden shadow-sm rounded-lg border border-gray-200">
            <div className="p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <TrendingUp className="h-8 w-8 text-purple-600" />
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Marge Moyenne</dt>
                    <dd className="text-2xl font-bold text-purple-600">{stats.averageMargin.toFixed(1)}%</dd>
                  </dl>
                </div>
              </div>
            </div>
          </div>

          <div className="bg-white overflow-hidden shadow-sm rounded-lg border border-gray-200">
            <div className="p-6">
              <div className="flex items-center">
                <div className="flex-shrink-0">
                  <AlertTriangle className="h-8 w-8 text-red-600" />
                </div>
                <div className="ml-5 w-0 flex-1">
                  <dl>
                    <dt className="text-sm font-medium text-gray-500 truncate">Stock Faible</dt>
                    <dd className="text-2xl font-bold text-red-600">{stats.lowStock}</dd>
                  </dl>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Barre d'actions */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 mb-6">
          <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between space-y-4 lg:space-y-0">
            
            {/* Recherche et filtres */}
            <div className="flex flex-1 space-x-4">
              <form onSubmit={handleSearch} className="flex space-x-2">
                <div className="relative">
                  <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <input
                    type="text"
                    placeholder="Rechercher un produit..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                  />
                </div>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 transition-colors"
                >
                  Rechercher
                </button>
              </form>

              <button
                onClick={() => setShowFilters(!showFilters)}
                className="px-4 py-2 border border-gray-300 rounded-md hover:bg-gray-50 transition-colors flex items-center"
              >
                <Filter className="h-4 w-4 mr-2" />
                Filtres
              </button>
            </div>

            {/* Bouton d'ajout */}
            <button className="inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500">
              <Plus className="h-4 w-4 mr-2" />
              Ajouter un produit
            </button>
          </div>

          {/* Filtres avancés */}
          {showFilters && (
            <div className="mt-4 pt-4 border-t border-gray-200">
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <select
                  value={selectedCategory}
                  onChange={(e) => setSelectedCategory(e.target.value)}
                  className="px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                >
                  <option value="">Toutes les catégories</option>
                  {categories.map(category => (
                    <option key={category} value={category}>{category}</option>
                  ))}
                </select>
                
                <select className="px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent">
                  <option value="">Tous les statuts</option>
                  <option value="Available">Disponible</option>
                  <option value="Sold">Vendu</option>
                  <option value="Reserved">Réservé</option>
                </select>

                <select className="px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent">
                  <option value="">Tous les stocks</option>
                  <option value="low">Stock faible</option>
                  <option value="normal">Stock normal</option>
                </select>
              </div>
            </div>
          )}
        </div>

        {/* Tableau des produits */}
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Produit
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Prix & Marge
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Stock
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Fournisseur
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Statut
                  </th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {products.map((product) => (
                  <tr key={product.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <div className="flex-shrink-0 h-10 w-10">
                          <div className="h-10 w-10 rounded-lg bg-gradient-to-br from-blue-500 to-purple-600 flex items-center justify-center">
                            <Package className="h-5 w-5 text-white" />
                          </div>
                        </div>
                        <div className="ml-4">
                          <div className="text-sm font-medium text-gray-900">
                            {product.name}
                          </div>
                          <div className="text-sm text-gray-500">
                            {product.brand} - {product.model}
                          </div>
                          <div className="text-xs text-gray-400">
                            {product.category}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900">
                        <div className="font-medium">{formatCurrency(product.sellingPrice)}</div>
                        <div className="text-gray-500">
                          Achat: {formatCurrency(product.purchasePrice)}
                        </div>
                        <div className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                          (product.marginPercentage || 0) > 20 ? 'bg-green-100 text-green-800' : 'bg-orange-100 text-orange-800'
                        }`}>
                          {(product.marginPercentage || 0).toFixed(1)}% marge
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="text-sm text-gray-900">
                        <div className="font-medium">{product.stock || 0} unités</div>
                        {(product.stock || 0) < 10 && (
                          <div className="text-red-600 text-xs font-medium">
                            ⚠️ Stock faible
                          </div>
                        )}
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      <div className="font-medium">{product.supplier || 'Non défini'}</div>
                      {product.supplierCity && (
                        <div className="text-gray-500 text-xs">{product.supplierCity}</div>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${getStatusColor(product.status || 'available')}`}>
                        {getStatusText(product.status || 'available')}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <div className="flex items-center justify-end space-x-2">
                        <button
                          className="text-blue-600 hover:text-blue-900 p-1 rounded hover:bg-blue-50 transition-colors"
                          title="Voir"
                        >
                          <Eye className="h-4 w-4" />
                        </button>
                        <button
                          className="text-green-600 hover:text-green-900 p-1 rounded hover:bg-green-50 transition-colors"
                          title="Modifier"
                        >
                          <Edit className="h-4 w-4" />
                        </button>
                        <button
                          onClick={() => handleDelete(product.id, product.name)}
                          className="text-red-600 hover:text-red-900 p-1 rounded hover:bg-red-50 transition-colors"
                          title="Supprimer"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                        <button
                          className="text-gray-400 hover:text-gray-600 p-1 rounded hover:bg-gray-50 transition-colors"
                          title="Plus d'options"
                        >
                          <MoreHorizontal className="h-4 w-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* État vide */}
          {products.length === 0 && !loading && (
            <div className="text-center py-12">
              <Package className="mx-auto h-12 w-12 text-gray-400" />
              <h3 className="mt-2 text-sm font-medium text-gray-900">Aucun produit</h3>
              <p className="mt-1 text-sm text-gray-500">
                Commencez par ajouter votre premier produit.
              </p>
              <div className="mt-6">
                <button className="inline-flex items-center px-4 py-2 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700">
                  <Plus className="h-4 w-4 mr-2" />
                  Ajouter un produit
                </button>
              </div>
            </div>
          )}
        </div>

        {/* Footer avec pagination */}
        {products.length > 0 && (
          <div className="mt-6 bg-white px-6 py-3 border border-gray-200 rounded-lg">
            <div className="flex items-center justify-between">
              <div className="text-sm text-gray-700">
                Affichage de <span className="font-medium">1</span> à <span className="font-medium">{products.length}</span> sur{' '}
                <span className="font-medium">{products.length}</span> résultats
              </div>
              <div className="text-sm text-gray-500">
                Dernière mise à jour: {formatDate(new Date().toISOString())}
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}