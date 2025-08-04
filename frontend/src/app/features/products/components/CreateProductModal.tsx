// src/app/features/products/components/CreateProductModal.tsx

'use client';

import React, { useState } from 'react';
import { X, Plus, Save, Calculator, Package, Truck, Euro, AlertCircle } from 'lucide-react';
import { CreateProductDto } from '../types';

interface CreateProductModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (productData: CreateProductDto) => Promise<void>;
  loading?: boolean;
}

export default function CreateProductModal({ 
  isOpen, 
  onClose, 
  onSubmit, 
  loading = false 
}: CreateProductModalProps) {
  const [formData, setFormData] = useState<CreateProductDto>({
    // Informations de base
    name: '',
    description: '',
    category: 'Smartphones',
    brand: '',
    model: '',
    
    // Prix et coûts
    purchasePrice: 0,
    transportCost: 0,
    sellingPrice: 0,
    
    // Stock
    stock: 0,
    minStockLevel: 1,
    
    // Caractéristiques
    condition: 'Neuf',
    conditionGrade: 'A+',
    storage: '',
    color: '',
    memory: '',
    processor: '',
    screenSize: '',
    
    // Fournisseur et logistique
    supplierName: '',
    supplierCity: '',
    importBatch: '',
    invoiceNumber: '',
    
    // Optionnel
    warrantyInfo: '',
    imageUrl: '',
    notes: ''
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

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

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    
    let processedValue: any = value;
    
    // Convertir les nombres
    if (type === 'number') {
      processedValue = value === '' ? 0 : parseFloat(value);
    }
    
    setFormData(prev => ({ ...prev, [name]: processedValue }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};
    
    // Validations obligatoires
    if (!formData.name.trim()) newErrors.name = 'Le nom est requis';
    if (!formData.description.trim()) newErrors.description = 'La description est requise';
    if (!formData.brand.trim()) newErrors.brand = 'La marque est requise';
    if (!formData.model.trim()) newErrors.model = 'Le modèle est requis';
    if (formData.purchasePrice <= 0) newErrors.purchasePrice = 'Le prix d\'achat doit être supérieur à 0';
    if (formData.sellingPrice <= 0) newErrors.sellingPrice = 'Le prix de vente doit être supérieur à 0';
    if (formData.stock < 0) newErrors.stock = 'Le stock doit être positif ou nul';
    if (!formData.supplierName.trim()) newErrors.supplierName = 'Le fournisseur est requis';
    if (!formData.importBatch.trim()) newErrors.importBatch = 'Le lot d\'import est requis';
    if (!formData.invoiceNumber.trim()) newErrors.invoiceNumber = 'Le numéro de facture est requis';
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) return;
    
    try {
      await onSubmit(formData);
      
      // Reset form après succès
      setFormData({
        name: '', description: '', category: 'Smartphones', brand: '', model: '',
        purchasePrice: 0, transportCost: 0, sellingPrice: 0, stock: 0, minStockLevel: 1,
        condition: 'Neuf', conditionGrade: 'A+', storage: '', color: '', memory: '',
        processor: '', screenSize: '', supplierName: '', supplierCity: '', importBatch: '',
        invoiceNumber: '', warrantyInfo: '', imageUrl: '', notes: ''
      });
      setErrors({});
    } catch (error) {
      console.error('Error creating product:', error);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto">
      {/* Backdrop */}
      <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" onClick={onClose} />
      
      {/* Modal */}
      <div className="flex min-h-full items-center justify-center p-4">
        <div className="relative w-full max-w-4xl bg-white rounded-2xl shadow-2xl max-h-[90vh] overflow-hidden">
          
          {/* Header */}
          <div className="flex items-center justify-between p-6 border-b border-gray-200 bg-gradient-to-r from-blue-50 to-indigo-50">
            <div className="flex items-center gap-3">
              <div className="p-2 bg-blue-100 rounded-lg">
                <Plus className="h-5 w-5 text-blue-600" />
              </div>
              <div>
                <h2 className="text-xl font-semibold text-gray-900">Ajouter un nouveau produit</h2>
                <p className="text-sm text-gray-500">Créez un nouveau produit dans votre inventaire</p>
              </div>
            </div>
            <button
              onClick={onClose}
              className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
              disabled={loading}
            >
              <X className="h-5 w-5 text-gray-500" />
            </button>
          </div>

          {/* Form */}
          <form onSubmit={handleSubmit} className="overflow-y-auto max-h-[calc(90vh-140px)]">
            <div className="p-6 space-y-8">
              
              {/* Informations de base */}
              <div className="bg-gray-50 rounded-xl p-6">
                <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center gap-2">
                  <Package className="h-5 w-5 text-blue-600" />
                  Informations du produit
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Nom du produit *
                    </label>
                    <input
                      type="text"
                      name="name"
                      value={formData.name}
                      onChange={handleInputChange}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                        errors.name ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="Ex: Samsung Galaxy S24 Ultra"
                      disabled={loading}
                    />
                    {errors.name && <p className="text-red-600 text-sm mt-1">{errors.name}</p>}
                  </div>
                  
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Description *
                    </label>
                    <textarea
                      name="description"
                      value={formData.description}
                      onChange={handleInputChange}
                      rows={3}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                        errors.description ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="Description détaillée du produit..."
                      disabled={loading}
                    />
                    {errors.description && <p className="text-red-600 text-sm mt-1">{errors.description}</p>}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Catégorie</label>
                    <select
                      name="category"
                      value={formData.category}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      disabled={loading}
                    >
                      <option value="Smartphones">Smartphones</option>
                      <option value="Tablets">Tablettes</option>
                      <option value="Laptops">Ordinateurs portables</option>
                      <option value="Accessories">Accessoires</option>
                    </select>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Marque *</label>
                    <input
                      type="text"
                      name="brand"
                      value={formData.brand}
                      onChange={handleInputChange}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                        errors.brand ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="Ex: Samsung, Apple..."
                      disabled={loading}
                    />
                    {errors.brand && <p className="text-red-600 text-sm mt-1">{errors.brand}</p>}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Modèle *</label>
                    <input
                      type="text"
                      name="model"
                      value={formData.model}
                      onChange={handleInputChange}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                        errors.model ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="Ex: Galaxy S24 Ultra"
                      disabled={loading}
                    />
                    {errors.model && <p className="text-red-600 text-sm mt-1">{errors.model}</p>}
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Condition</label>
                    <select
                      name="condition"
                      value={formData.condition}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      disabled={loading}
                    >
                      <option value="Neuf">Neuf</option>
                      <option value="Reconditionné">Reconditionné</option>
                      <option value="Occasion">Occasion</option>
                    </select>
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Grade</label>
                    <select
                      name="conditionGrade"
                      value={formData.conditionGrade}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      disabled={loading}
                    >
                      <option value="A+">A+</option>
                      <option value="A">A</option>
                      <option value="B+">B+</option>
                      <option value="B">B</option>
                      <option value="C">C</option>
                    </select>
                  </div>
                </div>
              </div>

              {/* Caractéristiques techniques */}
              <div className="bg-gray-50 rounded-xl p-6">
                <h3 className="text-lg font-medium text-gray-900 mb-4">Caractéristiques techniques</h3>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Stockage</label>
                    <input
                      type="text"
                      name="storage"
                      value={formData.storage || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Ex: 512GB"
                      disabled={loading}
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Couleur</label>
                    <input
                      type="text"
                      name="color"
                      value={formData.color || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Ex: Titanium Black"
                      disabled={loading}
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Mémoire</label>
                    <input
                      type="text"
                      name="memory"
                      value={formData.memory || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Ex: 12GB"
                      disabled={loading}
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Processeur</label>
                    <input
                      type="text"
                      name="processor"
                      value={formData.processor || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Ex: Snapdragon 8 Gen 3"
                      disabled={loading}
                    />
                  </div>

                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Taille écran</label>
                    <input
                      type="text"
                      name="screenSize"
                      value={formData.screenSize || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Ex: 6.8\"
                      disabled={loading}
                    />
                  </div>
                </div>
              </div>

              {/* Prix et coûts */}
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
                      onChange={handleInputChange}
                      min="0"
                      step="0.01"
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 ${
                        errors.purchasePrice ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="850.00"
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
                      onChange={handleInputChange}
                      min="0"
                      step="0.01"
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500"
                      placeholder="25.00"
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
                      onChange={handleInputChange}
                      min="0"
                      step="0.01"
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-green-500 focus:border-green-500 ${
                        errors.sellingPrice ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="1199.00"
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

              {/* Stock */}
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
                      onChange={handleInputChange}
                      min="0"
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500 ${
                        errors.stock ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="3"
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
                      onChange={handleInputChange}
                      min="0"
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="1"
                      disabled={loading}
                    />
                  </div>
                </div>
              </div>

              {/* Fournisseur */}
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
                      onChange={handleInputChange}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
                        errors.supplierName ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="TechItalia Milano"
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
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500"
                      placeholder="Milano"
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
                      onChange={handleInputChange}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
                        errors.importBatch ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="IT2025005"
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
                      onChange={handleInputChange}
                      className={`w-full px-4 py-3 border rounded-lg focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
                        errors.invoiceNumber ? 'border-red-300' : 'border-gray-300'
                      }`}
                      placeholder="INV-2025-008"
                      disabled={loading}
                    />
                    {errors.invoiceNumber && <p className="text-red-600 text-sm mt-1">{errors.invoiceNumber}</p>}
                  </div>
                </div>
              </div>

              {/* Informations complémentaires */}
              <div className="bg-gray-50 rounded-xl p-6">
                <h3 className="text-lg font-medium text-gray-900 mb-4">Informations complémentaires</h3>
                <div className="space-y-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      Informations de garantie
                    </label>
                    <input
                      type="text"
                      name="warrantyInfo"
                      value={formData.warrantyInfo || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Garantie constructeur 2 ans"
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
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="https://example.com/galaxy-s24-ultra.jpg"
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
                      onChange={handleInputChange}
                      rows={3}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Notes supplémentaires..."
                      disabled={loading}
                    />
                  </div>
                </div>
              </div>
            </div>

            {/* Footer */}
            <div className="flex items-center justify-between p-6 border-t border-gray-200 bg-gray-50">
              <div className="flex items-center gap-2 text-sm text-gray-500">
                <AlertCircle className="h-4 w-4" />
                <span>Les champs marqués d'un * sont obligatoires</span>
              </div>
              
              <div className="flex items-center gap-3">
                <button
                  type="button"
                  onClick={onClose}
                  className="px-6 py-2.5 text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
                  disabled={loading}
                >
                  Annuler
                </button>
                <button
                  type="submit"
                  disabled={loading}
                  className="px-6 py-2.5 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
                >
                  {loading ? (
                    <>
                      <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin" />
                      Création...
                    </>
                  ) : (
                    <>
                      <Save className="h-4 w-4" />
                      Créer le produit
                    </>
                  )}
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}