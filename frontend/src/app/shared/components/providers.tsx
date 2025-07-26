// src/shared/components/providers.tsx

"use client";

import React from 'react';

export function Providers({ children }: { children: React.ReactNode }) {
  return (
    <>
      {/* Ici vous pouvez ajouter des providers futurs comme : */}
      {/* - Theme Provider */}
      {/* - Redux Provider */}
      {/* - React Query Provider */}
      {/* - Context Providers */}
      
      {children}
    </>
  );
}