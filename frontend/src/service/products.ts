// src/service/products.ts
import { api, apiPath } from "./api";
import type { PageResp } from "./api";
import type { Product, ProductDTO } from "../types";

type ListParams = {
  page: number;
  pageSize: number;
  name_like?: string;
  categoryId?: string;
};

export async function listProducts({
  page,
  pageSize,
  name_like: q,
  categoryId,
}: ListParams): Promise<PageResp<Product>> {
  const params: Record<string, string | number> = {
    _page: page,
    _limit: pageSize,
  };
  if (q) params["name_like"] = q;
  if (categoryId) params["categoryId"] = categoryId;

  const { data, headers } = await api.get<Product[]>(apiPath("products"), {
    params,
  });

  const total = Number(headers["x-total-count"] ?? data.length);
  const totalPages = Math.max(1, Math.ceil(total / pageSize));

  return { items: data, total, page, pageSize, totalPages };
}

export async function createProduct(payload: ProductDTO): Promise<Product> {
  const { data } = await api.post<Product>(apiPath("products"), payload);
  return data;
}

export async function deleteProduct(id: string): Promise<void> {
  await api.delete(apiPath(`products/${id}`));
}

