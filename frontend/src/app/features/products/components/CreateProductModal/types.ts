// src/app/features/products/components/CreateProductModal/types.ts

import { CreateProductDto } from '../../types';

export interface FormData extends CreateProductDto {}

export interface FormErrors {
  [key: string]: string;
}

export interface CreateProductModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (productData: CreateProductDto) => Promise<void>;
  loading?: boolean;
}

export interface FormSectionProps {
  formData: FormData;
  errors: FormErrors;
  loading: boolean;
  onChange: (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => void;
  onDropdownChange?: (name: string, value: number) => void;
}