import React from 'react';

export default function Footer() {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="bg-white border-t border-gray-200 mt-auto">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="py-8">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            
            {/* Section ERP */}
            <div className="col-span-1 md:col-span-2">
              <div className="flex items-center space-x-3 mb-4">
                <div className="h-8 w-8 bg-gradient-to-br from-blue-600 to-purple-600 rounded-lg flex items-center justify-center">
                  <span className="text-white font-bold text-sm">ERP</span>
                </div>
                <span className="text-lg font-semibold text-gray-900">
                  ERP System
                </span>
              </div>
              <p className="text-gray-600 text-sm max-w-md">
                Système de gestion d'entreprise moderne avec interface intuitive. 
                Gérez vos produits, stocks et statistiques en toute simplicité.
              </p>
              <div className="mt-4 flex space-x-4">
                <span className="text-sm text-gray-500">
                  Frontend: Next.js 14 + TypeScript
                </span>
                <span className="text-sm text-gray-500">
                  Backend: C# .NET Core
                </span>
              </div>
            </div>

            {/* Section Fonctionnalités */}
            <div>
              <h3 className="text-sm font-semibold text-gray-900 uppercase tracking-wider mb-4">
                Fonctionnalités
              </h3>
              <ul className="space-y-2">
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Gestion des produits
                  </span>
                </li>
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Contrôle des stocks
                  </span>
                </li>
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Statistiques avancées
                  </span>
                </li>
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Import depuis l'Italie
                  </span>
                </li>
              </ul>
            </div>

            {/* Section Support */}
            <div>
              <h3 className="text-sm font-semibold text-gray-900 uppercase tracking-wider mb-4">
                Support
              </h3>
              <ul className="space-y-2">
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Documentation API
                  </span>
                </li>
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Guide utilisateur
                  </span>
                </li>
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Swagger Documentation
                  </span>
                </li>
                <li>
                  <span className="text-sm text-gray-600 hover:text-gray-900 cursor-pointer">
                    Contact technique
                  </span>
                </li>
              </ul>
            </div>
          </div>

          {/* Ligne de séparation */}
          <div className="mt-8 pt-8 border-t border-gray-200">
            <div className="flex flex-col md:flex-row justify-between items-center">
              <div className="text-sm text-gray-500">
                © {currentYear} ERP System. Architecture modulaire avec Next.js et C# .NET Core.
              </div>
              
              <div className="mt-4 md:mt-0 flex items-center space-x-6">
                <div className="flex items-center space-x-2">
                  <div className="h-2 w-2 bg-green-400 rounded-full"></div>
                  <span className="text-sm text-gray-500">
                    API opérationnelle
                  </span>
                </div>
                
                <div className="text-sm text-gray-500">
                  Version 1.0.0
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
}