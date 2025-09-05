// src/service/categories.ts
import { api, apiPath } from "./api";
import type { Category } from "../types";

export async function listCategories(): Promise<Category[]> {
  const { data } = await api.get<Category[]>(apiPath("categories"));
  return data ?? [];
}

type CreateCategoryDTO = { name: string; description?: string };

export async function createCategory(payload: CreateCategoryDTO): Promise<Category> {
  // json-server aceita id numérico automático, mas para ficar consistente com os seus dados (c1, c2…)
  const newCat: Category = {
    id: "c" + Math.random().toString(36).slice(2, 8),
    name: payload.name.trim(),
    description: payload.description?.trim() || "",
  };
  const { data } = await api.post<Category>(apiPath("categories"), newCat);
  return data;
}

export async function deleteCategory(id: string | number): Promise<void> {
  await api.delete(apiPath(`categories/${id}`));
}
