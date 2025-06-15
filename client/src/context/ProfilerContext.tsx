import { createContext, useEffect, useState, type ReactNode } from "react";
import { apiClient } from "../utils/api";

type MiniProfilerContextType = {
  id: string | null;
  setId: (id: string) => void;
  clearId: () => void;
};

const MiniProfilerContext = createContext<MiniProfilerContextType | undefined>(
  undefined
);

function MiniProfilerProvider({ children }: { children: ReactNode }) {
  const [id, setIdState] = useState<string | null>(null);

  const setId = (newId: string) => {
    setIdState(newId);
  };

  const clearId = () => {
    setIdState(null);
  };

  useEffect(() => {
    apiClient.interceptors.response.use((response) => {
      const miniProfilerHeader = response.headers["x-miniprofiler-ids"];
      const arrayMiniProfiler = JSON.parse(miniProfilerHeader);
      const idToUse = arrayMiniProfiler[arrayMiniProfiler.length - 1];
      if (idToUse) setId(idToUse as string);
      return response;
    });
  }, []);

  return (
    <MiniProfilerContext.Provider value={{ id, setId, clearId }}>
      {children}
    </MiniProfilerContext.Provider>
  );
}

export { MiniProfilerProvider, MiniProfilerContext };
