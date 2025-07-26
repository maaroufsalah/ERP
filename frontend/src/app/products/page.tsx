// src/app/products/page.tsx

import Link from 'next/link';

export default function ProductsPage() {
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="container mx-auto px-4 py-16">
        {/* Header */}
        <div className="mb-8">
          <Link 
            href="/" 
            className="text-blue-600 hover:text-blue-800 mb-4 inline-block"
          >
            ← Retour à l'accueil
          </Link>
          <h1 className="text-4xl font-bold text-gray-900 mb-4">
            Gestion des Produits
          </h1>
          <p className="text-xl text-gray-600">
            CRUD complet pour vos produits avec gestion des stocks et prix
          </p>
        </div>

        {/* Test Tailwind */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <h3 className="text-lg font-semibold text-blue-600 mb-2">
              Test Tailwind
            </h3>
            <p className="text-gray-600">
              Si vous voyez cette carte correctement stylée, Tailwind fonctionne !
            </p>
          </div>

          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <h3 className="text-lg font-semibold text-green-600 mb-2">
              Architecture
            </h3>
            <p className="text-gray-600">
              Structure modulaire avec features séparées
            </p>
          </div>

          <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-200">
            <h3 className="text-lg font-semibold text-purple-600 mb-2">
              Backend
            </h3>
            <p className="text-gray-600">
              API C# .NET Core prête à être connectée
            </p>
          </div>
        </div>

        {/* Status */}
        <div className="bg-blue-50 border border-blue-200 rounded-lg p-6">
          <h2 className="text-xl font-semibold text-blue-900 mb-4">
            🚀 Statut de développement
          </h2>
          <div className="space-y-2">
            <div className="flex items-center">
              <span className="text-green-600 mr-2">✅</span>
              <span>Page d'accueil fonctionnelle</span>
            </div>
            <div className="flex items-center">
              <span className="text-green-600 mr-2">✅</span>
              <span>Tailwind CSS configuré</span>
            </div>
            <div className="flex items-center">
              <span className="text-green-600 mr-2">✅</span>
              <span>Routing Next.js opérationnel</span>
            </div>
            <div className="flex items-center">
              <span className="text-yellow-600 mr-2">🔄</span>
              <span>Composants de feature en cours de développement</span>
            </div>
            <div className="flex items-center">
              <span className="text-yellow-600 mr-2">🔄</span>
              <span>Connexion API backend à implémenter</span>
            </div>
          </div>
        </div>

        {/* Prochaines étapes */}
        <div className="mt-8 bg-white p-6 rounded-lg shadow-sm border border-gray-200">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">
            📋 Prochaines étapes
          </h2>
          <ol className="list-decimal list-inside space-y-2 text-gray-600">
            <li>Créer les composants UI de base (Button, Card, Modal)</li>
            <li>Implémenter les services API pour la connexion backend</li>
            <li>Développer les composants ProductCard et ProductList</li>
            <li>Ajouter le hook useProducts pour la gestion d'état</li>
            <li>Intégrer le CRUD complet avec votre API C#</li>
          </ol>
        </div>

        {/* Test de navigation */}
        <div className="mt-8 text-center">
          <Link 
            href="/"
            className="bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors inline-block"
          >
            Retourner à l'accueil
          </Link>
        </div>
      </div>
    </div>
  );
}