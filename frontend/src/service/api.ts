// src/service/api.ts
import axios from "axios";

export const api = axios.create({
  baseURL: "http://127.0.0.1:5088",
});

// no json-server não tem prefixo; deixo aqui por simetria
export const apiPath = (p: string) => `/${p}`;

export type PageResp<T> = {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
};
