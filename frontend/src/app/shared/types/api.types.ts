// app/shared/types/api.types.ts

export interface ApiResponse<T = any> {
  data?: T;
  error?: {
    status: number;
    message: string;
  };
}

export interface PaginationParams {
  page: number;
  pageSize: number;
}

export interface PaginatedResponse<T> {
  data: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
