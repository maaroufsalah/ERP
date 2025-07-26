// app/shared/services/apiConfig.ts

// Determine if we're in production by checking the URL
const isProduction = typeof window !== 'undefined' && 
  (window.location.hostname.includes('digitalocean.app') || 
   window.location.hostname.includes('erp-production'));

// For client-side requests (from the browser)
export const publicApiUrl = isProduction
  ? 'https://your-erp-api.com/api/'
  : (process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7003/api/');

// For server-side requests (from the Next.js server)
export const serverApiUrl = isProduction
  ? 'https://your-erp-api.com/api/'
  : (process.env.SERVER_API_URL || process.env.NEXT_PUBLIC_API_URL || 'https://localhost:7003/api/');

// Use this function to get the correct API URL based on context
export function getApiUrl() {
  return typeof window === 'undefined' ? serverApiUrl : publicApiUrl;
}
