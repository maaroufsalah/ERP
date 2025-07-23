// src/app/products/page.tsx

"use client";

import { useState, useMemo } from "react";
import { Plus, Package, TrendingUp, AlertCircle, DollarSign } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { ProductTable } from "@/components/products/product-table";
import { ProductFilters } from "@/components/products/product-filters";
import { ProductForm } from "@/components/products/product-form";
import { ProductDetails } from "@/components/products/product-details";
import { 
  useProductsList, 
  useCreateProduct, 
  useUpdateProduct,
  useSearchProducts 
} from "@/hooks/use-products";
import { 
  ProductForListDto, 
  CreateProductDto, 
  ProductFilters as FilterType 
} from "@/types/api/product.types";
import { formatCurrency } from "@/lib/utils";

export default function ProductsPage() {
  // États pour les modals
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [detailsDialogOpen, setDetailsDialogOpen] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState<ProductForListDto | null>(null);

  // États pour la recherche et les filtres
  const [searchQuery, setSearchQuery] = useState("");
  const [filters, setFilters] = useState<FilterType>({});

  // Hooks pour les données
  const { data: products = [], isLoading, error } = useProductsList();
  const { data: searchResults = [], isLoading: isSearching } = useSearchProducts(searchQuery);
  const createProductMutation = useCreateProduct();
  const updateProductMutation = useUpdateProduct();

  // Filtrage des produits
  const filteredProducts = useMemo(() => {
    let result = searchQuery ? searchResults : products;

    // Appliquer les filtres
    if (filters.category) {
      result = result.filter(p => p.category === filters.category);
    }
    if (filters.brand) {
      result = result.filter(p => p.brand === filters.brand);
    }
    if (filters.status) {
      result = result.filter(p => p.status === filters.status);
    }
    if (filters.minPrice) {
      result = result.filter(p => p.sellingPrice >= filters.minPrice!);
    }
    if (filters.maxPrice) {
      result = result.filter(p => p.sellingPrice <= filters.maxPrice!);
    }
    if (filters.lowStock) {
      result = result.filter(p => p.isLowStock);
    }

    return result;
  }, [products, searchResults, searchQuery, filters]);

  // Calcul des statistiques
  const stats = useMemo(() => {
    const totalProducts = filteredProducts.length;
    const totalValue = filteredProducts.reduce((sum, p) => sum + p.totalValue, 0);
    const lowStockCount = filteredProducts.filter(p => p.isLowStock).length;
    const availableCount = filteredProducts.filter(p => p.status === "Available").length;

    return {
      totalProducts,
      totalValue,
      lowStockCount,
      availableCount,
    };
  }, [filteredProducts]);

  // Handlers pour les actions CRUD
  const handleCreateProduct = async (data: CreateProductDto) => {
    try {
      await createProductMutation.mutateAsync(data);
      setCreateDialogOpen(false);
    } catch (error) {
      console.error('Erreur lors de la création:', error);
    }
  };

  const handleEditProduct = async (data: CreateProductDto) => {
    if (!selectedProduct) return;
    
    try {
      await updateProductMutation.mutateAsync({
        id: selectedProduct.id,
        data,
      });
      setEditDialogOpen(false);
      setSelectedProduct(null);
    } catch (error) {
      console.error('Erreur lors de la modification:', error);
    }
  };

  const handleViewProduct = (product: ProductForListDto) => {
    setSelectedProduct(product);
    setDetailsDialogOpen(true);
  };

  const handleEditClick = (product: ProductForListDto) => {
    setSelectedProduct(product);
    setEditDialogOpen(true);
  };

  const handleDeleteProduct = (product: ProductForListDto) => {
    // La suppression est gérée dans ProductTable
    console.log('Produit supprimé:', product.name);
  };

  if (error) {
    return (
      <div className="container mx-auto py-8">
        <Card className="border-red-200 bg-red-50">
          <CardContent className="flex items-center space-x-2 py-6">
            <AlertCircle className="h-5 w-5 text-red-600" />
            <p className="text-red-600">
              Erreur lors du chargement des produits. Veuillez réessayer.
            </p>
          </CardContent>
        </Card>
      </div>
    );
  }

  return (
    <div className="container mx-auto py-8 space-y-8">
      {/* En-tête */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">Gestion des Produits</h1>
          <p className="text-muted-foreground">
            Gérez votre inventaire et vos produits en toute simplicité
          </p>
        </div>
        <Button onClick={() => setCreateDialogOpen(true)} size="lg">
          <Plus className="mr-2 h-4 w-4" />
          Nouveau Produit
        </Button>
      </div>

      {/* Statistiques */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Total Produits</CardTitle>
            <Package className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{stats.totalProducts}</div>
            <p className="text-xs text-muted-foreground">
              {stats.availableCount} disponibles
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Valeur Totale</CardTitle>
            <DollarSign className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{formatCurrency(stats.totalValue)}</div>
            <p className="text-xs text-muted-foreground">
              Valeur de l'inventaire
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Stock Faible</CardTitle>
            <AlertCircle className="h-4 w-4 text-orange-500" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-orange-600">{stats.lowStockCount}</div>
            <p className="text-xs text-muted-foreground">
              Produits à réapprovisionner
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium">Taux Disponibilité</CardTitle>
            <TrendingUp className="h-4 w-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold text-green-600">
              {stats.totalProducts > 0 
                ? Math.round((stats.availableCount / stats.totalProducts) * 100)
                : 0}%
            </div>
            <p className="text-xs text-muted-foreground">
              Produits disponibles
            </p>
          </CardContent>
        </Card>
      </div>

      {/* Filtres et recherche */}
      <Card>
        <CardHeader>
          <CardTitle>Recherche et Filtres</CardTitle>
        </CardHeader>
        <CardContent>
          <ProductFilters
            filters={filters}
            onFiltersChange={setFilters}
            onSearch={setSearchQuery}
          />
        </CardContent>
      </Card>

      {/* Tableau des produits */}
      <Card>
        <CardHeader>
          <CardTitle>
            Liste des Produits ({filteredProducts.length})
          </CardTitle>
        </CardHeader>
        <CardContent>
          <ProductTable
            products={filteredProducts}
            isLoading={isLoading || isSearching}
            onView={handleViewProduct}
            onEdit={handleEditClick}
            onDelete={handleDeleteProduct}
          />
        </CardContent>
      </Card>

      {/* Dialog Création */}
      <Dialog open={createDialogOpen} onOpenChange={setCreateDialogOpen}>
        <DialogContent className="max-w-4xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>Créer un nouveau produit</DialogTitle>
          </DialogHeader>
          <ProductForm
            onSubmit={handleCreateProduct}
            isLoading={createProductMutation.isPending}
            mode="create"
          />
        </DialogContent>
      </Dialog>

      {/* Dialog Modification */}
      <Dialog open={editDialogOpen} onOpenChange={setEditDialogOpen}>
        <DialogContent className="max-w-4xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>Modifier le produit</DialogTitle>
          </DialogHeader>
          {selectedProduct && (
            <ProductForm
              initialData={{
                // Convertir ProductForListDto en données pour le formulaire
                name: selectedProduct.name,
                category: selectedProduct.category,
                brand: selectedProduct.brand || "",
                sellingPrice: selectedProduct.sellingPrice,
                stockQuantity: selectedProduct.stockQuantity,
                // Ajouter d'autres champs selon les besoins
              }}
              onSubmit={handleEditProduct}
              isLoading={updateProductMutation.isPending}
              mode="edit"
            />
          )}
        </DialogContent>
      </Dialog>

      {/* Dialog Détails */}
      <Dialog open={detailsDialogOpen} onOpenChange={setDetailsDialogOpen}>
        <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>Détails du produit</DialogTitle>
          </DialogHeader>
          {selectedProduct && (
            <ProductDetails productId={selectedProduct.id} />
          )}
        </DialogContent>
      </Dialog>
    </div>
  );
}

// src/components/products/product-details.tsx

import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { useProduct } from "@/hooks/use-products";
import { formatCurrency, formatDate, formatPercentage } from "@/lib/utils";
import { ProductStatusBadge } from "./product-status-badge";
import { Loader2 } from "lucide-react";

interface ProductDetailsProps {
  productId: number;
}

export function ProductDetails({ productId }: ProductDetailsProps) {
  const { data: product, isLoading, error } = useProduct(productId);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center py-8">
        <Loader2 className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  if (error || !product) {
    return (
      <div className="py-8 text-center">
        <p className="text-red-600">Erreur lors du chargement du produit</p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Informations générales */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center justify-between">
            {product.name}
            <ProductStatusBadge status={product.status} isLowStock={product.isLowStock} />
          </CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-sm text-muted-foreground">Catégorie</p>
              <Badge variant="outline">{product.category}</Badge>
            </div>
            {product.brand && (
              <div>
                <p className="text-sm text-muted-foreground">Marque</p>
                <p className="font-medium">{product.brand}</p>
              </div>
            )}
            {product.model && (
              <div>
                <p className="text-sm text-muted-foreground">Modèle</p>
                <p className="font-medium">{product.model}</p>
              </div>
            )}
            <div>
              <p className="text-sm text-muted-foreground">ID Produit</p>
              <p className="font-medium">#{product.id}</p>
            </div>
          </div>
          {product.description && (
            <div>
              <p className="text-sm text-muted-foreground">Description</p>
              <p className="mt-1">{product.description}</p>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Prix et marges */}
      <Card>
        <CardHeader>
          <CardTitle>Prix et Marges</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Prix d'achat</p>
              <p className="text-lg font-semibold">{formatCurrency(product.purchasePrice)}</p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Coût transport</p>
              <p className="text-lg font-semibold">{formatCurrency(product.transportCost)}</p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Coût total</p>
              <p className="text-lg font-semibold">{formatCurrency(product.totalCostPrice)}</p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Prix de vente</p>
              <p className="text-lg font-semibold text-green-600">{formatCurrency(product.sellingPrice)}</p>
            </div>
          </div>
          <Separator className="my-4" />
          <div className="grid grid-cols-2 gap-6">
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Marge</p>
              <p className={`text-xl font-bold ${product.margin >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {formatCurrency(product.margin)}
              </p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Marge %</p>
              <p className={`text-xl font-bold ${product.marginPercentage >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {formatPercentage(product.marginPercentage)}
              </p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Stock et inventaire */}
      <Card>
        <CardHeader>
          <CardTitle>Stock et Inventaire</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Quantité en stock</p>
              <p className={`text-lg font-semibold ${product.isLowStock ? 'text-orange-600' : 'text-gray-900'}`}>
                {product.stockQuantity}
              </p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Stock minimum</p>
              <p className="text-lg font-semibold">{product.minStockLevel}</p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Valeur totale</p>
              <p className="text-lg font-semibold">{formatCurrency(product.totalValue)}</p>
            </div>
            <div className="text-center">
              <p className="text-sm text-muted-foreground">Jours en stock</p>
              <p className="text-lg font-semibold">{product.daysInStock}</p>
            </div>
          </div>
          {product.location && (
            <div className="mt-4">
              <p className="text-sm text-muted-foreground">Emplacement</p>
              <Badge variant="outline">{product.location}</Badge>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Spécifications techniques */}
      {(product.storage || product.memory || product.color || product.processor || product.screenSize) && (
        <Card>
          <CardHeader>
            <CardTitle>Spécifications Techniques</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4">
              {product.storage && (
                <div>
                  <p className="text-sm text-muted-foreground">Stockage</p>
                  <p className="font-medium">{product.storage}</p>
                </div>
              )}
              {product.memory && (
                <div>
                  <p className="text-sm text-muted-foreground">Mémoire</p>
                  <p className="font-medium">{product.memory}</p>
                </div>
              )}
              {product.color && (
                <div>
                  <p className="text-sm text-muted-foreground">Couleur</p>
                  <p className="font-medium">{product.color}</p>
                </div>
              )}
              {product.screenSize && (
                <div>
                  <p className="text-sm text-muted-foreground">Taille écran</p>
                  <p className="font-medium">{product.screenSize}</p>
                </div>
              )}
              {product.processor && (
                <div className="col-span-2">
                  <p className="text-sm text-muted-foreground">Processeur</p>
                  <p className="font-medium">{product.processor}</p>
                </div>
              )}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Condition et état */}
      {(product.condition || product.conditionGrade) && (
        <Card>
          <CardHeader>
            <CardTitle>Condition et État</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4">
              {product.condition && (
                <div>
                  <p className="text-sm text-muted-foreground">Condition</p>
                  <Badge variant="outline">{product.condition}</Badge>
                </div>
              )}
              {product.conditionGrade && (
                <div>
                  <p className="text-sm text-muted-foreground">Note</p>
                  <Badge variant="secondary">{product.conditionGrade}</Badge>
                </div>
              )}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Informations fournisseur */}
      {(product.supplierName || product.supplierCity || product.importBatch || product.invoiceNumber) && (
        <Card>
          <CardHeader>
            <CardTitle>Informations Fournisseur</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4">
              {product.supplierName && (
                <div>
                  <p className="text-sm text-muted-foreground">Fournisseur</p>
                  <p className="font-medium">{product.supplierName}</p>
                </div>
              )}
              {product.supplierCity && (
                <div>
                  <p className="text-sm text-muted-foreground">Ville</p>
                  <p className="font-medium">{product.supplierCity}</p>
                </div>
              )}
              {product.importBatch && (
                <div>
                  <p className="text-sm text-muted-foreground">Lot d'import</p>
                  <Badge variant="outline">{product.importBatch}</Badge>
                </div>
              )}
              {product.invoiceNumber && (
                <div>
                  <p className="text-sm text-muted-foreground">N° Facture</p>
                  <p className="font-medium">{product.invoiceNumber}</p>
                </div>
              )}
            </div>
          </CardContent>
        </Card>
      )}

      {/* Notes et garantie */}
      {(product.notes || product.warrantyInfo) && (
        <Card>
          <CardHeader>
            <CardTitle>Informations Supplémentaires</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            {product.notes && (
              <div>
                <p className="text-sm text-muted-foreground">Notes</p>
                <p className="mt-1">{product.notes}</p>
              </div>
            )}
            {product.warrantyInfo && (
              <div>
                <p className="text-sm text-muted-foreground">Garantie</p>
                <p className="mt-1">{product.warrantyInfo}</p>
              </div>
            )}
          </CardContent>
        </Card>
      )}

      {/* Audit et traçabilité */}
      <Card>
        <CardHeader>
          <CardTitle>Audit et Traçabilité</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <p className="text-sm text-muted-foreground">Créé le</p>
              <p className="font-medium">{formatDate(product.createdAt)}</p>
              <p className="text-sm text-muted-foreground">par {product.createdBy}</p>
            </div>
            {product.updatedAt && (
              <div>
                <p className="text-sm text-muted-foreground">Modifié le</p>
                <p className="font-medium">{formatDate(product.updatedAt)}</p>
                {product.updatedBy && (
                  <p className="text-sm text-muted-foreground">par {product.updatedBy}</p>
                )}
              </div>
            )}
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

// src/components/ui/separator.tsx

import * as React from "react"
import { cn } from "@/lib/utils"

const Separator = React.forwardRef<
  HTMLDivElement,
  React.HTMLAttributes<HTMLDivElement> & {
    orientation?: "horizontal" | "vertical"
  }
>(({ className, orientation = "horizontal", ...props }, ref) => (
  <div
    ref={ref}
    className={cn(
      "shrink-0 bg-border",
      orientation === "horizontal" ? "h-[1px] w-full" : "h-full w-[1px]",
      className
    )}
    {...props}
  />
))
Separator.displayName = "Separator"

export { Separator }