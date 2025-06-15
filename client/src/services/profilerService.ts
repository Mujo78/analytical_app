import { apiClient, apiClientProfiler } from "../utils/api";

export async function GetMetrics(id: string) {
  const res = await apiClientProfiler.get(`/profiler/results`, {
    params: {
      id,
    },
    headers: {
      "Content-Type": "application/json",
    },
  });

  return res.data;
}

export async function CreateIndexes() {
  const res = await apiClient.post("db/create", {});
  return res.status;
}

export async function DropIndexes() {
  const res = await apiClient.post("db/drop", {});
  return res.status;
}
