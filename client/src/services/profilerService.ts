import { apiClientProfiler } from "../utils/api";

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
