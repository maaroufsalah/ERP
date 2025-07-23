import { useState } from "react";
import { Eye, Edit, Trash2, Package, AlertTriangle, MoreHorizontal } from "lucide-react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { ProductForListDto } from "@/types/api/product.types";
import { ProductStatusBadge } from "./product-status-badge";
import { formatCurrency, formatDate } from "@/lib/utils";
import { useDeleteProduct } from "@/hooks/use-products";

interface ProductTableProps {
  products: ProductForListDto[];
  isLoading?: boolean;
  onView?: (product: ProductForListDto) => void;
  onEdit?: (product: ProductForListDto) => void;
  onDelete?: (product: ProductForListDto) => void;
}

export function ProductTable({ 
  products, 
  isLoading, 
  onView, 
  onEdit, 
  onDelete 
}: ProductTableProps) {
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [productToDelete, setProductToDelete] = useState<ProductForListDto | null>(null);
  
  const deleteProductMutation = useDeleteProduct();

  const handleDeleteClick = (product: ProductForListDto) => {
    setProductToDelete(product);
    setDeleteDialogOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (productToDelete) {
      try {
        await deleteProductMutation.mutateAsync(productToDelete.id);
        setDeleteDialogOpen(false);
        setProductToDelete(null);
        onDelete?.(productToDelete);
      } catch (error) {
        console.error('Erreur lors de la suppression:', error);
      }
    }
  };

  if (isLoading) {
    return (
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Nom</TableHead>
              <TableHead>Catégorie</TableHead>
              <TableHead>Marque</TableHead>
              <TableHead>Prix</TableHead>
              <TableHead>Stock</TableHead>
              <TableHead>Statut</TableHead>
              <TableHead>Valeur totale</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {[...Array(5)].map((_, i) => (
              <TableRow key={i}>
                {[...Array(8)].map((_, j) => (
                  <TableCell key={j}>
                    <div className="h-4 bg-gray-200 rounded animate-pulse" />
                  </TableCell>
                ))}
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    );
  }

  if (!products || products.length === 0) {
    return (
      <div className="rounded-md border p-8 text-center">
        <Package className="mx-auto h-12 w-12 text-gray-400" />
        <h3 className="mt-2 text-sm font-semibold text-gray-900">Aucun produit</h3>
        <p className="mt-1 text-sm text-gray-500">
          Commencez par ajouter votre premier produit.
        </p>
      </div>
    );
  }

  return (
    <>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Nom</TableHead>
              <TableHead>Catégorie</TableHead>
              <TableHead>Marque</TableHead>
              <TableHead>Prix</TableHead>
              <TableHead>Stock</TableHead>
              <TableHead>Statut</TableHead>
              <TableHead>Valeur totale</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {products.map((product) => (
              <TableRow key={product.id} className="hover:bg-muted/50">
                <TableCell className="font-medium">
                  <div className="flex items-center space-x-2">
                    <div>
                      <p className="font-semibold">{product.name}</p>
                      <p className="text-sm text-muted-foreground">
                        ID: {product.id}
                      </p>
                    </div>
                    {product.isLowStock && (
                      <AlertTriangle className="h-4 w-4 text-orange-500" />
                    )}
                  </div>
                </TableCell>
                
                <TableCell>
                  <Badge variant="outline">{product.category}</Badge>
                </TableCell>
                
                <TableCell>
                  {product.brand || (
                    <span className="text-muted-foreground">-</span>
                  )}
                </TableCell>
                
                <TableCell className="font-medium">
                  {formatCurrency(product.sellingPrice)}
                </TableCell>
                
                <TableCell>
                  <div className="flex items-center space-x-2">
                    <span className={`font-medium ${
                      product.isLowStock ? 'text-orange-600' : 'text-gray-900'
                    }`}>
                      {product.stockQuantity}
                    </span>
                    {product.isLowStock && (
                      <Badge variant="warning" className="text-xs">
                        Faible
                      </Badge>
                    )}
                  </div>
                </TableCell>
                
                <TableCell>
                  <ProductStatusBadge 
                    status={product.status} 
                    isLowStock={product.isLowStock}
                  />
                </TableCell>
                
                <TableCell className="font-medium">
                  {formatCurrency(product.totalValue)}
                </TableCell>
                
                <TableCell className="text-right">
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" className="h-8 w-8 p-0">
                        <span className="sr-only">Ouvrir le menu</span>
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <DropdownMenuItem onClick={() => onView?.(product)}>
                        <Eye className="mr-2 h-4 w-4" />
                        Voir les détails
                      </DropdownMenuItem>
                      <DropdownMenuItem onClick={() => onEdit?.(product)}>
                        <Edit className="mr-2 h-4 w-4" />
                        Modifier
                      </DropdownMenuItem>
                      <DropdownMenuItem 
                        onClick={() => handleDeleteClick(product)}
                        className="text-red-600"
                      >
                        <Trash2 className="mr-2 h-4 w-4" />
                        Supprimer
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      {/* Dialog de confirmation de suppression */}
      <Dialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Confirmer la suppression</DialogTitle>
            <DialogDescription>
              Êtes-vous sûr de vouloir supprimer le produit "{productToDelete?.name}" ?
              Cette action est irréversible.
            </DialogDescription>
          </DialogHeader>
          <DialogFooter>
            <Button
              variant="outline"
              onClick={() => setDeleteDialogOpen(false)}
              disabled={deleteProductMutation.isPending}
            >
              Annuler
            </Button>
            <Button
              variant="destructive"
              onClick={handleConfirmDelete}
              disabled={deleteProductMutation.isPending}
            >
              {deleteProductMutation.isPending ? "Suppression..." : "Supprimer"}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </>
  );
}