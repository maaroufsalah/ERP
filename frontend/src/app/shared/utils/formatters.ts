// app/shared/utils/formatters.ts

export const formatCurrency = (amount: number, currency = 'EUR'): string => {
  return new Intl.NumberFormat('fr-FR', {
    style: 'currency',
    currency
  }).format(amount);
};

export const formatDate = (dateString: string): string => {
  return new Date(dateString).toLocaleDateString('fr-FR');
};

export const formatPercentage = (value: number, decimals = 1): string => {
  return value.toFixed(decimals) + '%';
};
