// src/app/features/products/components/CreateProductModal.tsx

'use client';

import React, { useState } from 'react';
import { X, Plus, Save, Calculator, Package, Truck, Euro, AlertCircle } from 'lucide-react';
import Dropdown from '@/app/shared/components/form/Dropdown';
import { CreateProductDto } from '../../types';
import { useReferences } from '../../hooks/useReferences';

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
  const {
    productTypes,
    brands,
    models,
    colors,
    conditions,
    loading: referencesLoading,
    loadModelsByBrand,
    error: referencesError
  } = useReferences();

  // 🚀 DONNÉES DE TEST PRÉ-REMPLIES
  const [formData, setFormData] = useState<CreateProductDto>({
    // Informations de base - DONNÉES DE TEST
    name: 'iPhone 15 Pro Max 256GB Titanium Blue',
    description: 'Apple iPhone 15 Pro Max avec 256GB de stockage, couleur Titanium Blue, état neuf avec garantie Apple.',
    productTypeId: 1, // Supposons que 1 = Smartphone
    brandId: 2, // Supposons que 2 = Apple
    modelId: 5, // Supposons que 5 = iPhone 15 Pro Max
    colorId: 7, // Supposons que 7 = Titanium Blue
    conditionId: 1, // Supposons que 1 = Neuf
    
    // Prix et coûts - DONNÉES DE TEST
    purchasePrice: 950.00,
    transportCost: 30.00,
    sellingPrice: 1299.00,
    
    // Stock - DONNÉES DE TEST
    stock: 2,
    minStockLevel: 1,
    
    // Caractéristiques - DONNÉES DE TEST
    storage: '256GB',
    memory: '8GB',
    processor: 'A17 Pro',
    screenSize: '6.7"',
    
    // Fournisseur et logistique - DONNÉES DE TEST
    supplierName: 'AppleStore Roma',
    supplierCity: 'Roma',
    purchaseDate: '2025-01-18T00:00:00Z',
    arrivalDate: '2025-01-21T00:00:00Z',
    importBatch: 'IT2025006',
    invoiceNumber: 'INV-2025-009',
    status: 'Available',
    
    // Optionnel - DONNÉES DE TEST
    warrantyInfo: 'Garantie Apple 1 an',
    imageUrl: 'https://example.com/iphone-15-pro-max.jpg',
    notes: 'Produit de test - État parfait, emballage d\'origine'
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

  // Debug: Log des références chargées
  React.useEffect(() => {
    console.log('🔍 État des références:', {
      productTypes: productTypes.length,
      brands: brands.length,
      models: models.length,
      colors: colors.length,
      conditions: conditions.length,
      loading: referencesLoading,
      error: referencesError
    });
  }, [productTypes, brands, models, colors, conditions, referencesLoading, referencesError]);

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
    
    setFormData((prev: CreateProductDto) => ({ ...prev, [name]: processedValue }));
    
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors((prev: Record<string, string>) => ({ ...prev, [name]: '' }));
    }
  };

  // Fonction pour les dropdowns - TYPAGE CORRIGÉ
  const handleDropdownChange = (name: string, value: number) => {
    console.log(`🔄 Dropdown changé: ${name} = ${value}`);
    setFormData((prev: CreateProductDto) => ({ ...prev, [name]: value }));
    
    // Clear error
    if (errors[name]) {
      setErrors((prev: Record<string, string>) => ({ ...prev, [name]: '' }));
    }

    // Charger les modèles quand la marque change
    if (name === 'brandId' && value > 0) {
      console.log('🔄 Chargement des modèles pour la marque:', value);
      loadModelsByBrand(value);
      setFormData((prev: CreateProductDto) => ({ ...prev, modelId: 0 }));
    }
  };

  // Fonction pour les dates - TYPAGE CORRIGÉ
  const handleDateChange = (name: string, value: string) => {
    const dateValue = value ? value + 'T00:00:00Z' : '';
    console.log(`🔄 Date changée: ${name} = ${dateValue}`);
    setFormData((prev: CreateProductDto) => ({ ...prev, [name]: dateValue }));
  };

  // 🎯 FONCTION POUR REMPLIR AVEC DES DONNÉES ALÉATOIRES
  const fillWithRandomTestData = () => {
    const testProducts = [
      {
        name: 'Samsung Galaxy S24 Ultra 512GB',
        description: 'Samsung Galaxy S24 Ultra avec 512GB, S Pen inclus, état neuf',
        supplierName: 'Samsung Milano',
        importBatch: 'IT2025007',
        invoiceNumber: 'INV-2025-010',
        purchasePrice: 850,
        sellingPrice: 1199,
        storage: '512GB',
        memory: '12GB',
        processor: 'Snapdragon 8 Gen 3',
        screenSize: '6.8"'
      },
      {
        name: 'MacBook Air M2 13" 256GB',
        description: 'MacBook Air M2 13 pouces, 256GB SSD, 8GB RAM, état neuf',
        supplierName: 'AppleStore Milano',
        importBatch: 'IT2025008',
        invoiceNumber: 'INV-2025-011',
        purchasePrice: 950,
        sellingPrice: 1299,
        storage: '256GB SSD',
        memory: '8GB',
        processor: 'Apple M2',
        screenSize: '13.6"'
      },
      {
        name: 'iPad Pro 12.9" M2 128GB',
        description: 'iPad Pro 12.9 pouces avec puce M2, 128GB, Wi-Fi, état neuf',
        supplierName: 'TechItalia Roma',
        importBatch: 'IT2025009',
        invoiceNumber: 'INV-2025-012',
        purchasePrice: 750,
        sellingPrice: 1049,
        storage: '128GB',
        memory: '8GB',
        processor: 'Apple M2',
        screenSize: '12.9"'
      }
    ];

    const randomProduct = testProducts[Math.floor(Math.random() * testProducts.length)];
    const today = new Date().toISOString().split('T')[0];
    
    setFormData((prev: CreateProductDto) => ({
      ...prev,
      ...randomProduct,
      purchaseDate: today + 'T00:00:00Z',
      arrivalDate: today + 'T00:00:00Z',
      transportCost: Math.floor(Math.random() * 50) + 20, // 20-70€
      stock: Math.floor(Math.random() * 5) + 1, // 1-5 unités
    }));
  };

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {};
    
    // Validations obligatoires
    if (!formData.name.trim()) newErrors.name = 'Le nom est requis';
    if (!formData.description.trim()) newErrors.description = 'La description est requise';
    if (!formData.productTypeId || formData.productTypeId === 0) newErrors.productTypeId = 'Le type de produit est requis';
    if (!formData.brandId || formData.brandId === 0) newErrors.brandId = 'La marque est requise';
    if (!formData.modelId || formData.modelId === 0) newErrors.modelId = 'Le modèle est requis';
    if (!formData.conditionId || formData.conditionId === 0) newErrors.conditionId = 'La condition est requise';
    if (formData.purchasePrice <= 0) newErrors.purchasePrice = 'Le prix d\'achat doit être supérieur à 0';
    if (formData.sellingPrice <= 0) newErrors.sellingPrice = 'Le prix de vente doit être supérieur à 0';
    if (formData.stock < 0) newErrors.stock = 'Le stock doit être positif ou nul';
    if (!formData.supplierName.trim()) newErrors.supplierName = 'Le fournisseur est requis';
    if (!formData.importBatch.trim()) newErrors.importBatch = 'Le lot d\'import est requis';
    if (!formData.invoiceNumber.trim()) newErrors.invoiceNumber = 'Le numéro de facture est requis';
    
    setErrors(newErrors);
    console.log('🔍 Erreurs de validation:', newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    console.log('🚀 Tentative de soumission du formulaire...');
    console.log('📋 Données du formulaire avant validation:', formData);
    
    if (!validateForm()) {
      console.log('❌ Validation échouée');
      return;
    }
    
    try {
      // Ne pas envoyer colorId si c'est 0
      const submitData = { ...formData };
      if (submitData.colorId === 0) {
        delete submitData.colorId;
      }
      
      console.log('📤 Données à envoyer à l\'API:', JSON.stringify(submitData, null, 2));
      
      await onSubmit(submitData);
      
      console.log('✅ Produit créé avec succès');
      
      // Reset form après succès - AVEC NOUVELLES DONNÉES DE TEST
      fillWithRandomTestData();
      setErrors({});
    } catch (error) {
      console.error('❌ Erreur lors de la création du produit:', error);
    }
  };

  if (!isOpen) return null;

  // Affichage de l'erreur des références
  if (referencesError) {
    return (
      <div className="fixed inset-0 z-50 overflow-y-auto">
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" onClick={onClose} />
        <div className="flex min-h-full items-center justify-center p-4">
          <div className="relative w-full max-w-md bg-white rounded-2xl shadow-2xl p-6">
            <div className="text-center">
              <AlertCircle className="mx-auto h-12 w-12 text-red-500 mb-4" />
              <h3 className="text-lg font-medium text-gray-900 mb-2">Erreur de chargement</h3>
              <p className="text-sm text-gray-500 mb-4">{referencesError}</p>
              <button
                onClick={onClose}
                className="px-4 py-2 bg-gray-600 text-white rounded-lg hover:bg-gray-700"
              >
                Fermer
              </button>
            </div>
          </div>
        </div>
      </div>
    );
  }

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
                <p className="text-sm text-gray-500">Données de test pré-remplies pour faciliter les tests</p>
              </div>
            </div>
            <div className="flex items-center gap-2">
              {/* Bouton pour générer de nouvelles données de test */}
              <button
                type="button"
                onClick={fillWithRandomTestData}
                className="px-3 py-1 text-xs bg-yellow-100 text-yellow-800 rounded-md hover:bg-yellow-200 transition-colors"
                disabled={loading}
              >
                🎲 Nouvelles données
              </button>
              <button
                onClick={onClose}
                className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
                disabled={loading}
              >
                <X className="h-5 w-5 text-gray-500" />
              </button>
            </div>
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
                      placeholder="Ex: iPhone 15 Pro Max 256GB"
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

                  {/* Dropdowns */}
                  <Dropdown
                    name="productTypeId"
                    value={formData.productTypeId}
                    options={productTypes}
                    onChange={(value) => handleDropdownChange('productTypeId', value)}
                    label="Type de produit"
                    placeholder="Sélectionner un type..."
                    required
                    disabled={loading}
                    loading={referencesLoading}
                    error={errors.productTypeId}
                  />

                  <Dropdown
                    name="brandId"
                    value={formData.brandId}
                    options={brands}
                    onChange={(value) => handleDropdownChange('brandId', value)}
                    label="Marque"
                    placeholder="Sélectionner une marque..."
                    required
                    disabled={loading}
                    loading={referencesLoading}
                    error={errors.brandId}
                  />

                  <Dropdown
                    name="modelId"
                    value={formData.modelId}
                    options={models}
                    onChange={(value) => handleDropdownChange('modelId', value)}
                    label="Modèle"
                    placeholder="Sélectionner un modèle..."
                    required
                    disabled={loading || !formData.brandId}
                    loading={referencesLoading}
                    error={errors.modelId}
                  />

                  <Dropdown
                    name="colorId"
                    value={formData.colorId || ''}
                    options={colors}
                    onChange={(value) => handleDropdownChange('colorId', value)}
                    label="Couleur"
                    placeholder="Sélectionner une couleur..."
                    disabled={loading}
                    loading={referencesLoading}
                    error={errors.colorId}
                  />

                  <div className="md:col-span-2">
                    <Dropdown
                      name="conditionId"
                      value={formData.conditionId}
                      options={conditions}
                      onChange={(value) => handleDropdownChange('conditionId', value)}
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

              {/* Caractéristiques techniques */}
              <div className="bg-gray-50 rounded-xl p-6">
                <h3 className="text-lg font-medium text-gray-900 mb-4">Caractéristiques techniques</h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">Stockage</label>
                    <input
                      type="text"
                      name="storage"
                      value={formData.storage || ''}
                      onChange={handleInputChange}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Ex: 256GB"
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
                      placeholder="Ex: 8GB"
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
                      placeholder="Ex: A17 Pro"
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
                      placeholder="Ex: 6.7\"
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
                      onChange={handleInputChange}
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
                      onChange={handleInputChange}
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
                      placeholder="2"
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

              {/* Fournisseur et logistique */}
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
                      onChange={handleInputChange}
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
                      onChange={(e) => handleDateChange('purchaseDate', e.target.value)}
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
                      onChange={(e) => handleDateChange('arrivalDate', e.target.value)}
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
                      onChange={handleInputChange}
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
                      onChange={handleInputChange}
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
                      onChange={handleInputChange}
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
                      onChange={handleInputChange}
                      rows={3}
                      className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                      placeholder="Notes supplémentaires..."
                      disabled={loading}
                    />
                  </div>
                </div>
              </div>

              {/* Debug Info */}
              {process.env.NODE_ENV === 'development' && (
                <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
                  <h4 className="text-sm font-medium text-yellow-800 mb-2">🔍 Debug Info</h4>
                  <div className="text-xs text-yellow-700 space-y-1">
                    <p><strong>Références chargées:</strong> Types({productTypes.length}), Marques({brands.length}), Modèles({models.length}), Couleurs({colors.length}), Conditions({conditions.length})</p>
                    <p><strong>IDs sélectionnés:</strong> ProductType={formData.productTypeId}, Brand={formData.brandId}, Model={formData.modelId}, Color={formData.colorId}, Condition={formData.conditionId}</p>
                    <p><strong>Prix:</strong> Achat={formData.purchasePrice}€, Transport={formData.transportCost}€, Vente={formData.sellingPrice}€</p>
                    <p><strong>Marge calculée:</strong> {margin.toFixed(2)}€ ({marginPercent.toFixed(1)}%)</p>
                  </div>
                </div>
              )}
            </div>

            {/* Footer */}
            <div className="flex items-center justify-between p-6 border-t border-gray-200 bg-gray-50">
              <div className="flex items-center gap-2 text-sm text-gray-500">
                <AlertCircle className="h-4 w-4" />
                <span>🚀 Version de test avec données pré-remplies</span>
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
                  disabled={loading || referencesLoading}
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
                      🚀 Créer le produit de test
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