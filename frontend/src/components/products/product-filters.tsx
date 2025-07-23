import { useState } from "react";
import { Search, Filter, X } from "lucide-react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Label } from "@/components/ui/label";
import { ProductFilters as FilterType } from "@/types/api/product.types";
import { useCategories, useBrands, useSuppliers } from "@/hooks/use-products";

interface ProductFiltersProps {
  filters: FilterType;
  onFiltersChange: (filters: FilterType) => void;
  onSearch: (query: string) => void;
}

export function ProductFilters({ filters, onFiltersChange, onSearch }: ProductFiltersProps) {
  const [searchQuery, setSearchQuery] = useState("");
  const [isFilterDialogOpen, setIsFilterDialogOpen] = useState(false);

  const { data: categories = [] } = useCategories();
  const { data: brands = [] } = useBrands();
  const { data: suppliers = [] } = useSuppliers();

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch(searchQuery);
  };

  const handleFilterChange = (key: keyof FilterType, value: string | number | boolean | undefined) => {
    onFiltersChange({
      ...filters,
      [key]: value,
    });
  };

  const clearFilter = (key: keyof FilterType) => {
    const newFilters = { ...filters };
    delete newFilters[key];
    onFiltersChange(newFilters);
  };

  const clearAllFilters = () => {
    onFiltersChange({});
  };

  const activeFiltersCount = Object.keys(filters).length;

  return (
    <div className="space-y-4">
      {/* Barre de recherche */}
      <form onSubmit={handleSearchSubmit} className="flex space-x-2">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
          <Input
            type="text"
            placeholder="Rechercher des produits..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="pl-10"
          />
        </div>
        <Button type="submit">
          Rechercher
        </Button>
        <Dialog open={isFilterDialogOpen} onOpenChange={setIsFilterDialogOpen}>
          <DialogTrigger asChild>
            <Button variant="outline">
              <Filter className="mr-2 h-4 w-4" />
              Filtres
              {activeFiltersCount > 0 && (
                <Badge variant="secondary" className="ml-2">
                  {activeFiltersCount}
                </Badge>
              )}
            </Button>
          </DialogTrigger>
          <DialogContent className="max-w-md">
            <DialogHeader>
              <DialogTitle>Filtres de recherche</DialogTitle>
            </DialogHeader>
            <div className="space-y-4">
              {/* Catégorie */}
              <div className="space-y-2">
                <Label>Catégorie</Label>
                <select
                  value={filters.category || ""}
                  onChange={(e) => handleFilterChange("category", e.target.value || undefined)}
                  className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                >
                  <option value="">Toutes les catégories</option>
                  {categories.map((category) => (
                    <option key={category} value={category}>
                      {category}
                    </option>
                  ))}
                </select>
              </div>

              {/* Marque */}
              <div className="space-y-2">
                <Label>Marque</Label>
                <select
                  value={filters.brand || ""}
                  onChange={(e) => handleFilterChange("brand", e.target.value || undefined)}
                  className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                >
                  <option value="">Toutes les marques</option>
                  {brands.map((brand) => (
                    <option key={brand} value={brand}>
                      {brand}
                    </option>
                  ))}
                </select>
              </div>

              {/* Fournisseur */}
              <div className="space-y-2">
                <Label>Fournisseur</Label>
                <select
                  value={filters.supplier || ""}
                  onChange={(e) => handleFilterChange("supplier", e.target.value || undefined)}
                  className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                >
                  <option value="">Tous les fournisseurs</option>
                  {suppliers.map((supplier) => (
                    <option key={supplier} value={supplier}>
                      {supplier}
                    </option>
                  ))}
                </select>
              </div>

              {/* Statut */}
              <div className="space-y-2">
                <Label>Statut</Label>
                <select
                  value={filters.status || ""}
                  onChange={(e) => handleFilterChange("status", e.target.value || undefined)}
                  className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                >
                  <option value="">Tous les statuts</option>
                  <option value="Available">Disponible</option>
                  <option value="Unavailable">Indisponible</option>
                  <option value="Out_of_Stock">Rupture de stock</option>
                  <option value="Pending">En attente</option>
                </select>
              </div>

              {/* Prix */}
              <div className="grid grid-cols-2 gap-2">
                <div className="space-y-2">
                  <Label>Prix min (€)</Label>
                  <Input
                    type="number"
                    step="0.01"
                    value={filters.minPrice || ""}
                    onChange={(e) => handleFilterChange("minPrice", e.target.value ? Number(e.target.value) : undefined)}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Prix max (€)</Label>
                  <Input
                    type="number"
                    step="0.01"
                    value={filters.maxPrice || ""}
                    onChange={(e) => handleFilterChange("maxPrice", e.target.value ? Number(e.target.value) : undefined)}
                  />
                </div>
              </div>

              {/* Stock faible */}
              <div className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  id="lowStock"
                  checked={filters.lowStock || false}
                  onChange={(e) => handleFilterChange("lowStock", e.target.checked)}
                  className="rounded border-gray-300"
                />
                <Label htmlFor="lowStock">Afficher seulement les stocks faibles</Label>
              </div>

              <div className="flex justify-between pt-4">
                <Button variant="outline" onClick={clearAllFilters}>
                  Effacer tout
                </Button>
                <Button onClick={() => setIsFilterDialogOpen(false)}>
                  Appliquer
                </Button>
              </div>
            </div>
          </DialogContent>
        </Dialog>
      </form>

      {/* Filtres actifs */}
      {activeFiltersCount > 0 && (
        <div className="flex flex-wrap gap-2">
          <span className="text-sm text-muted-foreground">Filtres actifs:</span>
          {filters.category && (
            <Badge variant="secondary" className="flex items-center gap-1">
              Catégorie: {filters.category}
              <X
                className="h-3 w-3 cursor-pointer hover:text-red-500"
                onClick={() => clearFilter("category")}
              />
            </Badge>
          )}
          {filters.brand && (
            <Badge variant="secondary" className="flex items-center gap-1">
              Marque: {filters.brand}
              <X
                className="h-3 w-3 cursor-pointer hover:text-red-500"
                onClick={() => clearFilter("brand")}
              />
            </Badge>
          )}
          {filters.status && (
            <Badge variant="secondary" className="flex items-center gap-1">
              Statut: {filters.status}
              <X
                className="h-3 w-3 cursor-pointer hover:text-red-500"
                onClick={() => clearFilter("status")}
              />
            </Badge>
          )}
          {filters.lowStock && (
            <Badge variant="secondary" className="flex items-center gap-1">
              Stock faible
              <X
                className="h-3 w-3 cursor-pointer hover:text-red-500"
                onClick={() => clearFilter("lowStock")}
              />
            </Badge>
          )}
          <Button variant="ghost" size="sm" onClick={clearAllFilters}>
            Effacer tout
          </Button>
        </div>
      )}
    </div>
  );
}