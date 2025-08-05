// src/app/shared/services/toastService.ts

type ToastType = 'success' | 'error' | 'info' | 'warning';

export function showToast(message: string, type: ToastType = 'info') {
  // Pour l'instant, utilisons console.log avec des emojis pour le debug
  // Plus tard, vous pourrez intégrer une vraie librairie de toast comme react-hot-toast
  
  const emoji = {
    success: '✅',
    error: '❌',
    warning: '⚠️',
    info: 'ℹ️'
  };

  console.log(`${emoji[type]} Toast ${type.toUpperCase()}: ${message}`);

  // Version simple avec alert pour le debug immédiat
  if (type === 'error') {
    console.error('🔥 ERREUR:', message);
    // Optionnel: décommenter pour avoir une popup
    // alert(`Erreur: ${message}`);
  } else if (type === 'success') {
    console.log('🎉 SUCCÈS:', message);
  }
}

// Export pour compatibilité avec les imports existants
export const toastService = {
  success: (message: string) => showToast(message, 'success'),
  error: (message: string) => showToast(message, 'error'),
  warning: (message: string) => showToast(message, 'warning'),
  info: (message: string) => showToast(message, 'info'),
};