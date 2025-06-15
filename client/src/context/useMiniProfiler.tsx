import { useContext } from "react";
import { MiniProfilerContext } from "./ProfilerContext";

function useMiniProfiler() {
  const context = useContext(MiniProfilerContext);
  if (!context) {
    throw new Error("useMiniProfiler must be used within MiniProfilerProvider");
  }
  return context;
}

export { useMiniProfiler };
