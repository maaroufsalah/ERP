// app/features/products/hooks/useReferences.ts

import { useState, useEffect, useCallback } from 'react';
import { DropdownOption } from '../types';
import { referenceService } from '../services/referenceService';

interface UseReferencesState {
  productTypes: DropdownOption[];
  brands: DropdownOption[];
  models: DropdownOption[];
  colors: DropdownOption[];
  conditions: DropdownOption[];
  loading: boolean;
  error: string | null;
}

interface UseReferencesActions {
  loadAllReferences: () => Promise<void>;
  loadModelsByBrand: (brandId: number) => Promise<void>;
  clearError: () => void;
  refreshReferences: () => Promise<void>;
}

export function useReferences(): UseReferencesState & UseReferencesActions {
  const [state, setState] = useState<UseReferencesState>({
    productTypes: [],
    brands: [],
    models: [],
    colors: [],
    conditions: [],
    loading: false,
    error: null
  });

  const updateState = useCallback((updates: Partial<UseReferencesState>) => {
    setState(prev => ({ ...prev, ...updates }));
  }, []);

  // ✅ CHARGER TOUTES LES RÉFÉRENCES
  const loadAllReferences = useCallback(async () => {
    updateState({ loading: true, error: null });
    try {
      console.log('🔄 Début du chargement des références...');
      const references = await referenceService.getAllReferencesForForm();
      
      console.log('✅ Références chargées avec succès:', references);
      updateState({ 
        ...references,
        loading: false 
      });
    } catch (error: any) {
      console.error('❌ Erreur lors du chargement des références:', error);
      updateState({ 
        error: error.message || 'Erreur lors du chargement des références',
        loading: false 
      });
    }
  }, [updateState]);

  // ✅ CHARGER LES MODÈLES PAR MARQUE
  const loadModelsByBrand = useCallback(async (brandId: number) => {
    try {
      console.log('🔄 Chargement des modèles pour la marque:', brandId);
      const models = await referenceService.getModelsForDropdown(brandId);
      console.log('✅ Modèles chargés:', models);
      updateState({ models });
    } catch (error: any) {
      console.error('❌ Erreur lors du chargement des modèles:', error);
      updateState({ 
        error: error.message || 'Erreur lors du chargement des modèles'
      });
    }
  }, [updateState]);

  // ✅ ACTIONS UTILITAIRES
  const clearError = useCallback(() => {
    updateState({ error: null });
  }, [updateState]);

  const refreshReferences = useCallback(async () => {
    await loadAllReferences();
  }, [loadAllReferences]);

  // ✅ CHARGEMENT INITIAL
  useEffect(() => {
    console.log('🚀 Hook useReferences initialisé, chargement des références...');
    loadAllReferences();
  }, [loadAllReferences]);

  return {
    // État
    ...state,
    
    // Actions
    loadAllReferences,
    loadModelsByBrand,
    clearError,
    refreshReferences
  };
}