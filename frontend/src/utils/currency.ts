export function formatBRL(n: number | undefined | null) {
  const v = Number(n ?? 0);
  return v.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
}
