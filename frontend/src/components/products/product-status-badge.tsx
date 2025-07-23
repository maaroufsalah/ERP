// src/components/products/product-status-badge.tsx

import { Badge } from "@/components/ui/badge";

interface ProductStatusBadgeProps {
  status: string;
  isLowStock?: boolean;
}

export function ProductStatusBadge({ status, isLowStock }: ProductStatusBadgeProps) {
  const getVariant = () => {
    if (isLowStock) return "warning";
    
    switch (status.toLowerCase()) {
      case "available":
        return "success";
      case "unavailable":
      case "out_of_stock":
        return "destructive";
      case "pending":
        return "warning";
      default:
        return "secondary";
    }
  };

  const getDisplayText = () => {
    if (isLowStock) return "Stock faible";
    
    switch (status.toLowerCase()) {
      case "available":
        return "Disponible";
      case "unavailable":
        return "Indisponible";
      case "out_of_stock":
        return "Rupture";
      case "pending":
        return "En attente";
      default:
        return status;
    }
  };

  return (
    <Badge variant={getVariant()}>
      {getDisplayText()}
    </Badge>
  );
}

// src/components/products/product-form.tsx

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { createProductSchema, CreateProductForm } from "@/types/forms/product.forms";
import { CreateProductDto, UpdateProductDto } from "@/types/api/product.types";

interface ProductFormProps {
  initialData?: Partial<CreateProductDto>;
  onSubmit: (data: CreateProductDto) => void;
  isLoading?: boolean;
  mode?: "create" | "edit";
}

export function ProductForm({ 
  initialData, 
  onSubmit, 
  isLoading, 
  mode = "create" 
}: ProductFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
    watch,
  } = useForm<CreateProductForm>({
    resolver: zodResolver(createProductSchema),
    defaultValues: initialData,
  });

  const watchedPurchasePrice = watch("purchasePrice", 0);
  const watchedTransportCost = watch("transportCost", 0);
  const watchedSellingPrice = watch("sellingPrice", 0);

  const totalCost = (watchedPurchasePrice || 0) + (watchedTransportCost || 0);
  const margin = (watchedSellingPrice || 0) - totalCost;
  const marginPercentage = watchedSellingPrice > 0 ? (margin / watchedSellingPrice) * 100 : 0;

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      {/* Informations de base */}
      <Card>
        <CardHeader>
          <CardTitle>Informations générales</CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nom du produit *</Label>
            <Input
              id="name"
              {...register("name")}
              placeholder="ex: iPhone 14 Pro"
              className={errors.name ? "border-red-500" : ""}
            />
            {errors.name && (
              <p className="text-sm text-red-600">{errors.name.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="category">Catégorie *</Label>
            <Input
              id="category"
              {...register("category")}
              placeholder="ex: Smartphones"
              className={errors.category ? "border-red-500" : ""}
            />
            {errors.category && (
              <p className="text-sm text-red-600">{errors.category.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="brand">Marque</Label>
            <Input
              id="brand"
              {...register("brand")}
              placeholder="ex: Apple"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="model">Modèle</Label>
            <Input
              id="model"
              {...register("model")}
              placeholder="ex: A2894"
            />
          </div>

          <div className="space-y-2 md:col-span-2">
            <Label htmlFor="description">Description</Label>
            <textarea
              id="description"
              {...register("description")}
              className="flex min-h-[80px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              placeholder="Description détaillée du produit..."
            />
          </div>
        </CardContent>
      </Card>

      {/* Prix et coûts */}
      <Card>
        <CardHeader>
          <CardTitle>Prix et coûts</CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <div className="space-y-2">
            <Label htmlFor="purchasePrice">Prix d'achat (€) *</Label>
            <Input
              id="purchasePrice"
              type="number"
              step="0.01"
              {...register("purchasePrice", { valueAsNumber: true })}
              className={errors.purchasePrice ? "border-red-500" : ""}
            />
            {errors.purchasePrice && (
              <p className="text-sm text-red-600">{errors.purchasePrice.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="transportCost">Coût transport (€) *</Label>
            <Input
              id="transportCost"
              type="number"
              step="0.01"
              {...register("transportCost", { valueAsNumber: true })}
              className={errors.transportCost ? "border-red-500" : ""}
            />
            {errors.transportCost && (
              <p className="text-sm text-red-600">{errors.transportCost.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="sellingPrice">Prix de vente (€) *</Label>
            <Input
              id="sellingPrice"
              type="number"
              step="0.01"
              {...register("sellingPrice", { valueAsNumber: true })}
              className={errors.sellingPrice ? "border-red-500" : ""}
            />
            {errors.sellingPrice && (
              <p className="text-sm text-red-600">{errors.sellingPrice.message}</p>
            )}
          </div>

          {/* Calculs automatiques */}
          <div className="md:col-span-3 grid grid-cols-1 md:grid-cols-3 gap-4 p-4 bg-gray-50 rounded-lg">
            <div className="text-center">
              <p className="text-sm text-gray-600">Coût total</p>
              <p className="text-lg font-semibold">{totalCost.toFixed(2)} €</p>
            </div>
            <div className="text-center">
              <p className="text-sm text-gray-600">Marge</p>
              <p className={`text-lg font-semibold ${margin >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {margin.toFixed(2)} €
              </p>
            </div>
            <div className="text-center">
              <p className="text-sm text-gray-600">Marge %</p>
              <p className={`text-lg font-semibold ${marginPercentage >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {marginPercentage.toFixed(2)}%
              </p>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Spécifications techniques */}
      <Card>
        <CardHeader>
          <CardTitle>Spécifications techniques</CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="space-y-2">
            <Label htmlFor="storage">Stockage</Label>
            <Input
              id="storage"
              {...register("storage")}
              placeholder="ex: 256GB"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="memory">Mémoire</Label>
            <Input
              id="memory"
              {...register("memory")}
              placeholder="ex: 8GB"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="color">Couleur</Label>
            <Input
              id="color"
              {...register("color")}
              placeholder="ex: Noir Sidéral"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="screenSize">Taille écran</Label>
            <Input
              id="screenSize"
              {...register("screenSize")}
              placeholder="ex: 6.7 pouces"
            />
          </div>

          <div className="space-y-2 md:col-span-2">
            <Label htmlFor="processor">Processeur</Label>
            <Input
              id="processor"
              {...register("processor")}
              placeholder="ex: Apple A16 Bionic"
            />
          </div>
        </CardContent>
      </Card>

      {/* État et condition */}
      <Card>
        <CardHeader>
          <CardTitle>État et condition</CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="space-y-2">
            <Label htmlFor="condition">Condition</Label>
            <select
              id="condition"
              {...register("condition")}
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2"
            >
              <option value="">Sélectionner...</option>
              <option value="Neuf">Neuf</option>
              <option value="Comme neuf">Comme neuf</option>
              <option value="Très bon état">Très bon état</option>
              <option value="Bon état">Bon état</option>
              <option value="État correct">État correct</option>
              <option value="Pour pièces">Pour pièces</option>
            </select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="conditionGrade">Note condition</Label>
            <select
              id="conditionGrade"
              {...register("conditionGrade")}
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2"
            >
              <option value="">Sélectionner...</option>
              <option value="A+">A+ (Excellent)</option>
              <option value="A">A (Très bon)</option>
              <option value="B+">B+ (Bon)</option>
              <option value="B">B (Correct)</option>
              <option value="C">C (Médiocre)</option>
            </select>
          </div>
        </CardContent>
      </Card>

      {/* Stock et fournisseur */}
      <Card>
        <CardHeader>
          <CardTitle>Stock et fournisseur</CardTitle>
        </CardHeader>
        <CardContent className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div className="space-y-2">
            <Label htmlFor="stockQuantity">Quantité en stock *</Label>
            <Input
              id="stockQuantity"
              type="number"
              {...register("stockQuantity", { valueAsNumber: true })}
              className={errors.stockQuantity ? "border-red-500" : ""}
            />
            {errors.stockQuantity && (
              <p className="text-sm text-red-600">{errors.stockQuantity.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="minStockLevel">Niveau stock minimum *</Label>
            <Input
              id="minStockLevel"
              type="number"
              {...register("minStockLevel", { valueAsNumber: true })}
              className={errors.minStockLevel ? "border-red-500" : ""}
            />
            {errors.minStockLevel && (
              <p className="text-sm text-red-600">{errors.minStockLevel.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="supplierName">Nom du fournisseur</Label>
            <Input
              id="supplierName"
              {...register("supplierName")}
              placeholder="ex: TechDistribution"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="supplierCity">Ville fournisseur</Label>
            <Input
              id="supplierCity"
              {...register("supplierCity")}
              placeholder="ex: Paris"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="importBatch">Lot d'import</Label>
            <Input
              id="importBatch"
              {...register("importBatch")}
              placeholder="ex: BATCH-2024-001"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="invoiceNumber">Numéro facture</Label>
            <Input
              id="invoiceNumber"
              {...register("invoiceNumber")}
              placeholder="ex: INV-2024-123"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="location">Emplacement</Label>
            <Input
              id="location"
              {...register("location")}
              placeholder="ex: A1-B2-C3"
            />
          </div>
        </CardContent>
      </Card>

      {/* Informations supplémentaires */}
      <Card>
        <CardHeader>
          <CardTitle>Informations supplémentaires</CardTitle>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="notes">Notes</Label>
            <textarea
              id="notes"
              {...register("notes")}
              className="flex min-h-[80px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              placeholder="Notes supplémentaires..."
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="warrantyInfo">Informations garantie</Label>
            <Input
              id="warrantyInfo"
              {...register("warrantyInfo")}
              placeholder="ex: Garantie 2 ans Apple"
            />
          </div>
        </CardContent>
      </Card>

      {/* Boutons d'action */}
      <div className="flex justify-end space-x-4">
        <Button type="button" variant="outline">
          Annuler
        </Button>
        <Button type="submit" disabled={isLoading}>
          {isLoading ? "Enregistrement..." : mode === "create" ? "Créer le produit" : "Mettre à jour"}
        </Button>
      </div>
    </form>
  );
}