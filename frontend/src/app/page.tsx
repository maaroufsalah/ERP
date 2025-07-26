// src/app/page.tsx - Version avec imports directs

import Link from 'next/link';
// ✅ Imports directs qui fonctionnent à coup sûr
import { Card, CardContent, CardHeader, CardTitle } from './shared/components/ui/Card';
import { Button } from './shared/components/ui';
import { Package, TrendingUp, Users, BarChart3 } from 'lucide-react';

export default function HomePage() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
      <div className="container mx-auto py-16 px-4">
        {/* Hero Section */}
        <div className="text-center mb-16">
          <h1 className="text-4xl md:text-6xl font-bold text-gray-900 mb-6">
            ERP Frontend
          </h1>
          <p className="text-xl text-gray-600 mb-8 max-w-2xl mx-auto">
            Gérez vos produits et votre inventaire avec une interface moderne et intuitive
          </p>
          <Link href="/products">
            <Button size="lg" className="text-lg px-8 py-3">
              <Package className="mr-2 h-5 w-5" />
              Accéder aux Produits
            </Button>
          </Link>
        </div>

        {/* Features Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-16">
          <Card className="hover:shadow-lg transition-shadow">
            <CardHeader className="text-center">
              <Package className="h-12 w-12 text-blue-600 mx-auto mb-4" />
              <CardTitle>Gestion Produits</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-gray-600 text-center">
                CRUD complet pour vos produits avec gestion des stocks et prix
              </p>
            </CardContent>
          </Card>

          <Card className="hover:shadow-lg transition-shadow">
            <CardHeader className="text-center">
              <TrendingUp className="h-12 w-12 text-green-600 mx-auto mb-4" />
              <CardTitle>Suivi des Marges</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-gray-600 text-center">
                Calculs automatiques des marges et rentabilité en temps réel
              </p>
            </CardContent>
          </Card>

          <Card className="hover:shadow-lg transition-shadow">
            <CardHeader className="text-center">
              <BarChart3 className="h-12 w-12 text-purple-600 mx-auto mb-4" />
              <CardTitle>Statistiques</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-gray-600 text-center">
                Tableaux de bord avec métriques et indicateurs clés
              </p>
            </CardContent>
          </Card>

          <Card className="hover:shadow-lg transition-shadow">
            <CardHeader className="text-center">
              <Users className="h-12 w-12 text-orange-600 mx-auto mb-4" />
              <CardTitle>Multi-utilisateurs</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-gray-600 text-center">
                Gestion des utilisateurs avec traçabilité des actions
              </p>
            </CardContent>
          </Card>
        </div>

        {/* Tech Stack */}
        <div className="bg-white rounded-lg shadow-lg p-8">
          <h2 className="text-2xl font-bold text-center mb-8">Technologies Utilisées</h2>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6 text-center">
            <div>
              <h3 className="font-semibold text-lg mb-2">Frontend</h3>
              <p className="text-gray-600">Next.js 15</p>
              <p className="text-gray-600">TypeScript</p>
              <p className="text-gray-600">Tailwind CSS</p>
            </div>
            <div>
              <h3 className="font-semibold text-lg mb-2">UI/UX</h3>
              <p className="text-gray-600">Components UI</p>
              <p className="text-gray-600">Lucide Icons</p>
              <p className="text-gray-600">Responsive Design</p>
            </div>
            <div>
              <h3 className="font-semibold text-lg mb-2">Architecture</h3>
              <p className="text-gray-600">Modulaire</p>
              <p className="text-gray-600">React Hooks</p>
              <p className="text-gray-600">TypeScript</p>
            </div>
            <div>
              <h3 className="font-semibold text-lg mb-2">Backend</h3>
              <p className="text-gray-600">.NET Core</p>
              <p className="text-gray-600">Entity Framework</p>
              <p className="text-gray-600">SQL Server</p>
            </div>
          </div>
        </div>

        {/* Footer */}
        <div className="text-center mt-16 text-gray-500">
          <p>&copy; 2024 ERP Frontend. Interface moderne pour la gestion d'entreprise.</p>
        </div>
      </div>
    </div>
  );
}