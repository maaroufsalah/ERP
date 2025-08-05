// src/app/shared/components/form/Dropdown.tsx

'use client';

import React from 'react';
import { ChevronDown, AlertCircle, Loader2 } from 'lucide-react';

export interface DropdownOption {
  value: number;
  label: string;
  description?: string;
  disabled?: boolean;
}

interface DropdownProps {
  name: string;
  value: number | string;
  options: DropdownOption[];
  onChange: (value: number) => void;
  label?: string;
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
  loading?: boolean;
  error?: string;
  className?: string;
  showDescription?: boolean;
}

export default function Dropdown({
  name,
  value,
  options,
  onChange,
  label,
  placeholder = "Sélectionner...",
  required = false,
  disabled = false,
  loading = false,
  error,
  className = "",
  showDescription = false
}: DropdownProps) {
  
  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedValue = parseInt(e.target.value);
    if (!isNaN(selectedValue)) {
      onChange(selectedValue);
    }
  };

  return (
    <div className={`w-full ${className}`}>
      {label && (
        <label className="block text-sm font-medium text-gray-700 mb-2">
          {label}
          {required && <span className="text-red-500 ml-1">*</span>}
        </label>
      )}
      
      <div className="relative">
        {loading ? (
          <div className="w-full px-4 py-3 border border-gray-300 rounded-lg bg-gray-50 flex items-center justify-center">
            <Loader2 className="h-4 w-4 animate-spin text-gray-400 mr-2" />
            <span className="text-sm text-gray-500">Chargement...</span>
          </div>
        ) : (
          <>
            <select
              name={name}
              value={value || ''}
              onChange={handleChange}
              disabled={disabled}
              className={`
                w-full px-4 py-3 border rounded-lg appearance-none bg-white
                focus:ring-2 focus:ring-blue-500 focus:border-blue-500
                disabled:bg-gray-50 disabled:text-gray-500 disabled:cursor-not-allowed
                ${error ? 'border-red-300 focus:ring-red-500 focus:border-red-500' : 'border-gray-300'}
                ${disabled ? 'bg-gray-50' : 'bg-white'}
              `}
            >
              <option value="">{placeholder}</option>
              {options.map((option) => (
                <option 
                  key={option.value} 
                  value={option.value}
                  disabled={option.disabled}
                >
                  {option.label}
                  {showDescription && option.description && ` - ${option.description}`}
                </option>
              ))}
            </select>
            
            <div className="absolute inset-y-0 right-0 flex items-center pr-3 pointer-events-none">
              <ChevronDown className="h-4 w-4 text-gray-400" />
            </div>
          </>
        )}
      </div>

      {error && (
        <div className="flex items-center gap-1 mt-1">
          <AlertCircle className="h-4 w-4 text-red-500" />
          <p className="text-red-600 text-sm">{error}</p>
        </div>
      )}
    </div>
  );
}