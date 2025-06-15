import { useEffect, useState } from "react";
import { useMiniProfiler } from "../context/useMiniProfiler";
import useRequestState from "./useRequestState";
import { GetMetrics } from "../services/profilerService";
import { type MiniProfiler } from "../types/miniprofiler.type";

const useProfilerResult = () => {
  const { id } = useMiniProfiler();
  const { loading, setLoading } = useRequestState();
  const [data, setData] = useState<MiniProfiler>();

  console.log("objec heret");

  useEffect(() => {
    async function FetchMetricsForRequest() {
      setLoading(true);
      try {
        if (id) {
          const data = await GetMetrics(id);
          setData(data);
        }
      } finally {
        setLoading(false);
      }
    }

    FetchMetricsForRequest();
  }, [id, setLoading]);

  return { loading, data };
};

export default useProfilerResult;
