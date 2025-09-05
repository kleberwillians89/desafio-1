// src/types.ts
export type Category = {
  id: string;
  name: string;
  description?: string;
};

export type Product = {
  id: string;
  name: string;
  price: number;
  stock: number;
  categoryId: string;
  description?: string;
};

export type PageResp<T> = {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
};

export type ProductDTO = Omit<Product, "id">;
export type CategoryDTO = Omit<Category, "id">;
