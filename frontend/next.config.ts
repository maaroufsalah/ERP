/** @type {import('next').NextConfig} */
const nextConfig = {
  // ✅ appDir supprimé - par défaut dans Next.js 15
  
  // ✅ Configuration des images
  images: {
    remotePatterns: [
      {
        protocol: 'http',
        hostname: 'localhost',
        port: '3000',
        pathname: '/**',
      },
      {
        protocol: 'http', 
        hostname: 'localhost',
        port: '5000',
        pathname: '/**',
      },
      // Ajoutez d'autres domaines si nécessaire
      {
        protocol: 'https',
        hostname: 'example.com',
        pathname: '/**',
      }
    ],
  },
  
  // ✅ Variables d'environnement personnalisées
  env: {
    CUSTOM_KEY: 'my-value',
    API_BASE_URL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000/api/',
  },

  // ✅ Configuration TypeScript stricte
  typescript: {
    // Empêche le build si il y a des erreurs TypeScript
    ignoreBuildErrors: false,
  },

  // ✅ Configuration ESLint
  eslint: {
    // Empêche le build si il y a des erreurs ESLint
    ignoreDuringBuilds: false,
  },

  // ✅ Optimisations pour production
  compiler: {
    // Supprime les console.log en production
    removeConsole: process.env.NODE_ENV === 'production',
  },

  // ✅ Configuration expérimentale (si nécessaire)
  experimental: {
    // Nouvelles features Next.js 15 si nécessaire
    // turbo: {}, // Turbopack (optionnel)
  },
}

module.exports = nextConfig