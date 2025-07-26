// src/shared/components/ui/Card.tsx

import React from 'react';

interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
}

interface CardHeaderProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
}

interface CardTitleProps extends React.HTMLAttributes<HTMLHeadingElement> {
  children: React.ReactNode;
}

interface CardContentProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
}

export function Card({ children, className = '', ...props }: CardProps) {
  return (
    <div 
      className={`bg-white rounded-lg border border-gray-200 shadow-sm ${className}`}
      {...props}
    >
      {children}
    </div>
  );
}

export function CardHeader({ children, className = '', ...props }: CardHeaderProps) {
  return (
    <div 
      className={`p-6 ${className}`}
      {...props}
    >
      {children}
    </div>
  );
}

export function CardTitle({ children, className = '', ...props }: CardTitleProps) {
  return (
    <h3 
      className={`text-lg font-semibold text-gray-900 leading-none tracking-tight ${className}`}
      {...props}
    >
      {children}
    </h3>
  );
}

export function CardContent({ children, className = '', ...props }: CardContentProps) {
  return (
    <div 
      className={`p-6 pt-0 ${className}`}
      {...props}
    >
      {children}
    </div>
  );
}

// Export par défaut pour compatibilité
export default Card;