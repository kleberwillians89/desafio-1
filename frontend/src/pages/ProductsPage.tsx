// src/pages/ProductsPage.tsx
import { useMemo, useState } from "react";
import {
  useMutation,
  useQuery,
  useQueryClient,
  keepPreviousData,
} from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import toast from "react-hot-toast";

import { listCategories } from "../service/categories";
import { listProducts, createProduct, deleteProduct } from "../service/products";
import type { Product, ProductDTO, Category } from "../types";
import type { PageResp } from "../service/api";

// --------- form + validação ----------
type FormValues = {
  name: string;
  price: number;
  stock: number;
  categoryId: string;
  description?: string;
};

const schema = z.object({
  name: z.string().min(1, "Informe o nome"),
  price: z.coerce.number().nonnegative(),
  stock: z.coerce.number().int().nonnegative(),
  categoryId: z.string().min(1, "Selecione uma categoria"),
  description: z.string().optional(),
});

// --------- página ----------
export default function ProductsPage() {
  const qc = useQueryClient();
  const [page, setPage] = useState(1);
  const pageSize = 5;

  // categorias
  const { data: categories = [] as Category[] } = useQuery({
    queryKey: ["categories"],
    queryFn: listCategories,
  });

  // produtos (com paginação e "previous data")
  const {
    data: productsResp = {
      items: [],
      total: 0,
      page: 1,
      pageSize,
      totalPages: 1,
    },
    isLoading,
  } = useQuery<PageResp<Product>>({
    queryKey: ["products", page, pageSize],
    queryFn: () => listProducts({ page, pageSize }),
    placeholderData: keepPreviousData, // v5
  });

  const products = productsResp.items;
  const totalPages = productsResp.totalPages;

  const catMap = useMemo(
    () => new Map(categories.map((c) => [String(c.id), c.name])),
    [categories]
  );

  // mutations
  const createMut = useMutation({
    mutationFn: (payload: ProductDTO) => createProduct(payload),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["products"] });
      qc.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Produto criado!");
    },
    onError: () => toast.error("Erro ao criar produto."),
  });

  const deleteMut = useMutation({
    mutationFn: (id: string) => deleteProduct(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["products"] });
      qc.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Produto removido!");
    },
    onError: () => toast.error("Erro ao remover produto."),
  });

  // form
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormValues>({ resolver: zodResolver(schema) });

  const onSubmit = (v: FormValues) =>
    createMut.mutate(v, {
      onSuccess: () => reset(),
    });

  return (
    <section className="space-y-8">
      <h1 className="text-4xl font-extrabold">Produtos</h1>
      <p className="text-white/70">Gerencie seu catálogo e estoque.</p>

      {/* form */}
      <form onSubmit={handleSubmit(onSubmit)} className="grid gap-3 md:grid-cols-3">
        <input
          {...register("name")}
          placeholder="Nome"
          className="h-10 w-full rounded-md bg-white/10 px-3"
        />
        <input
          {...register("price")}
          type="number"
          placeholder="Preço"
          className="h-10 w-full rounded-md bg-white/10 px-3"
        />
        <input
          {...register("stock")}
          type="number"
          placeholder="Estoque"
          className="h-10 w-full rounded-md bg-white/10 px-3"
        />
        <select
          {...register("categoryId")}
          className="h-10 w-full rounded-md bg-white/10 px-3"
        >
          <option value="">Selecione uma categoria</option>
          {categories.map((c) => (
            <option key={c.id} value={c.id}>
              {c.name}
            </option>
          ))}
        </select>
        <input
          {...register("description")}
          placeholder="Descrição (opcional)"
          className="h-10 w-full rounded-md bg-white/10 px-3 md:col-span-2"
        />
        <button
          type="submit"
          disabled={createMut.isPending}
          className="rounded-md bg-gradient-to-r from-indigo-500 to-purple-500 px-4 py-2 font-semibold"
        >
          {createMut.isPending ? "Salvando..." : "Adicionar"}
        </button>
      </form>

      {/* tabela */}
      {isLoading ? (
        <p>Carregando...</p>
      ) : (
        <table className="w-full border-collapse rounded-lg overflow-hidden">
          <thead className="bg-white/10 text-left">
            <tr>
              <th className="px-4 py-2">Nome</th>
              <th className="px-4 py-2">Categoria</th>
              <th className="px-4 py-2">Preço</th>
              <th className="px-4 py-2">Estoque</th>
              <th className="px-4 py-2">Ações</th>
            </tr>
          </thead>
          <tbody>
            {products.map((p) => (
              <tr key={p.id} className="border-b border-white/10">
                <td className="px-4 py-2">{p.name}</td>
                <td className="px-4 py-2">{catMap.get(p.categoryId)}</td>
                <td className="px-4 py-2">
                  {p.price.toLocaleString("pt-BR", {
                    style: "currency",
                    currency: "BRL",
                  })}
                </td>
                <td className="px-4 py-2">{p.stock}</td>
                <td className="px-4 py-2">
                  <button
                    onClick={() => deleteMut.mutate(p.id)}
                    disabled={deleteMut.isPending}
                    className="rounded bg-red-500 px-2 py-1 text-white hover:bg-red-600 disabled:opacity-60"
                  >
                    {deleteMut.isPending ? "Removendo..." : "Remover"}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {/* paginação */}
      <div className="flex justify-center gap-2">
        {Array.from({ length: totalPages }).map((_, i) => (
          <button
            key={i}
            onClick={() => setPage(i + 1)}
            className={`rounded px-3 py-1 ${
              page === i + 1
                ? "bg-indigo-500 text-white"
                : "bg-white/10 text-white/70"
            }`}
          >
            {i + 1}
          </button>
        ))}
      </div>
    </section>
  );
}
