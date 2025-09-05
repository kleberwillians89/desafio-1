import { Link, Outlet, useLocation } from "react-router-dom";

function NavLink({ to, children }: { to: string; children: React.ReactNode }) {
  const { pathname } = useLocation();
  const active = pathname === to;
  return (
    <Link
      to={to}
      className={`rounded-md px-3 py-1.5 text-sm font-medium ${
        active ? "bg-indigo-600 text-white" : "bg-white/5 hover:bg-white/10 text-white"
      }`}
    >
      {children}
    </Link>
  );
}

export default function App() {
  return (
    <div className="min-h-screen bg-slate-950 text-white">
      <header className="sticky top-0 z-10 backdrop-blur bg-slate-950/60">
        <div className="mx-auto max-w-6xl px-4 py-4 flex items-center justify-between">
          <Link to="/dashboard" className="text-2xl font-extrabold">Hypesoft Inventory</Link>
          <nav className="flex gap-2">
            <NavLink to="/dashboard">Dashboard</NavLink>
            <NavLink to="/products">Produtos</NavLink>
            <NavLink to="/categories">Categorias</NavLink>
          </nav>
        </div>
      </header>

      <main className="mx-auto max-w-6xl px-4 py-8">
        <Outlet />
      </main>
    </div>
  );
}
