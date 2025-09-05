// src/pages/CategoriesPage.tsx
import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { listCategories, createCategory, deleteCategory } from "../service/categories";
import type { Category } from "../types";
import toast from "react-hot-toast";

export default function CategoriesPage() {
  const qc = useQueryClient();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

  const { data: categories = [] as Category[] } = useQuery({
    queryKey: ["categories"],
    queryFn: listCategories,
  });

  const createMut = useMutation({
    mutationFn: () => createCategory({ name, description }),
    onSuccess: () => {
      setName("");
      setDescription("");
      qc.invalidateQueries({ queryKey: ["categories"] });
      qc.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Categoria criada!");
    },
    onError: () => toast.error("Não foi possível criar a categoria."),
  });

  const removeMut = useMutation({
    mutationFn: (id: string | number) => deleteCategory(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["categories"] });
      qc.invalidateQueries({ queryKey: ["dashboard"] });
      toast.success("Categoria removida!");
    },
    onError: () => toast.error("Erro ao remover categoria."),
  });

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!name.trim()) return toast.error("Informe o nome da categoria");
    createMut.mutate();
  }

  return (
    <section className="space-y-8">
      <h1 className="text-4xl font-extrabold">Categorias</h1>

      <form onSubmit={handleSubmit} className="grid gap-3 md:grid-cols-[1fr_1fr_auto]">
        <input
          className="h-10 w-full rounded-md bg-white/10 px-3 outline-none ring-1 ring-white/10"
          placeholder="Nome da categoria"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <input
          className="h-10 w-full rounded-md bg-white/10 px-3 outline-none ring-1 ring-white/10"
          placeholder="Descrição (opcional)"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
        <button
          type="submit"
          disabled={createMut.isPending}
          className="h-10 rounded-md px-5 font-medium text-white
                     bg-gradient-to-r from-indigo-500 to-fuchsia-500
                     disabled:opacity-60"
        >
          {createMut.isPending ? "Salvando..." : "Adicionar"}
        </button>
      </form>

      <div className="rounded-2xl bg-white/5 ring-1 ring-white/10 overflow-hidden">
        <table className="w-full">
          <thead>
            <tr className="bg-white/10 text-left">
              <th className="px-4 py-2">Nome</th>
              <th className="px-4 py-2">Descrição</th>
              <th className="px-4 py-2 w-40">Ações</th>
            </tr>
          </thead>
          <tbody>
            {categories.map((c) => (
              <tr key={c.id} className="border-t border-white/10">
                <td className="px-4 py-2">{c.name}</td>
                <td className="px-4 py-2">{c.description}</td>
                <td className="px-4 py-2">
                  <button
                    onClick={() => removeMut.mutate(c.id)}
                    disabled={removeMut.isPending}
                    className="rounded-md bg-white/10 px-3 py-1 hover:bg-white/20 disabled:opacity-60"
                  >
                    {removeMut.isPending ? "Removendo..." : "Remover"}
                  </button>
                </td>
              </tr>
            ))}
            {categories.length === 0 && (
              <tr>
                <td className="px-4 py-6 text-white/60" colSpan={3}>
                  Sem categorias cadastradas.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </section>
  );
}
