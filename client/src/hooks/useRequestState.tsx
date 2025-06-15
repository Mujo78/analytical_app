import { useState } from "react";

const useRequestState = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  return {
    loading,
    error,
    setLoading,
    setError,
  };
};

export default useRequestState;
