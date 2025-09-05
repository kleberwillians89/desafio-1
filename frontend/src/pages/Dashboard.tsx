// src/pages/Dashboard.tsx
import { useQuery } from "@tanstack/react-query";
import { listProducts } from "../service/products";
import { listCategories } from "../service/categories";
import type { Product, Category } from "../types";

import {
  ResponsiveContainer,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Cell,
} from "recharts";

export default function Dashboard() {
  const { data: products = [] as Product[] } = useQuery({
    queryKey: ["products", "all"],
    queryFn: () => listProducts({ page: 1, pageSize: 1000 }).then(r => r.items),
  });

  const { data: categories = [] as Category[] } = useQuery({
    queryKey: ["categories"],
    queryFn: listCategories,
  });

  const totalProdutos = products.length;
  const valorTotal = products.reduce((acc, p) => acc + (p.price || 0) * (p.stock || 0), 0);
  const baixoEstoque = products.filter(p => (p.stock ?? 0) < 10).length;

  const dataChart = categories.map((c) => ({
    name: c.name,
    value: products.filter(p => p.categoryId === c.id).length,
  }));

  const COLORS = ["#6366f1", "#22d3ee", "#f472b6", "#f59e0b", "#10b981", "#ef4444"];

  return (
    <section className="space-y-8">
      <h1 className="text-4xl font-extrabold">Dashboard</h1>

      {/* cards */}
      <div className="grid gap-4 md:grid-cols-3">
        <div className="rounded-xl bg-white/5 p-6 ring-1 ring-white/10">
          <p className="text-sm text-white/70">Total de produtos</p>
          <p className="mt-2 text-3xl font-semibold">{totalProdutos}</p>
        </div>
        <div className="rounded-xl bg-white/5 p-6 ring-1 ring-white/10">
          <p className="text-sm text-white/70">Valor total do estoque</p>
          <p className="mt-2 text-3xl font-semibold">
            {valorTotal.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}
          </p>
        </div>
        <div className="rounded-xl bg-white/5 p-6 ring-1 ring-white/10">
          <p className="text-sm text-white/70">Baixo estoque (&lt; 10)</p>
          <p className="mt-2 text-3xl font-semibold">{baixoEstoque}</p>
        </div>
      </div>

      {/* gráfico */}
      <div className="rounded-2xl bg-white/5 p-4 ring-1 ring-white/10">
        <h2 className="mb-4 text-xl font-semibold">Produtos por categoria</h2>

        {dataChart.some(d => d.value > 0) ? (
          <div className="h-[340px]">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={dataChart} margin={{ top: 8, right: 16, left: 0, bottom: 8 }}>
                <CartesianGrid strokeDasharray="3 3" opacity={0.15} />
                <XAxis dataKey="name" />
                <YAxis allowDecimals={false} />
                <Tooltip />
                <Bar dataKey="value" radius={[6, 6, 0, 0]}>
                  {dataChart.map((_, i) => (
                    <Cell key={`cell-${i}`} fill={COLORS[i % COLORS.length]} />
                  ))}
                </Bar>
              </BarChart>
            </ResponsiveContainer>
          </div>
        ) : (
          <p className="text-white/60">Sem dados suficientes para exibir o gráfico.</p>
        )}
      </div>
    </section>
  );
}

