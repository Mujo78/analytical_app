import { useEffect, useState } from "react";
import useRequestState from "./useRequestState";
import type { TopUserReputation } from "../types/user.type";
import { GetTopUsersByReputation } from "../services/userService";
import { toast } from "react-toastify";

const useTopUsers = (orm: string) => {
  const { loading, setLoading } = useRequestState();
  const [data, setData] = useState<TopUserReputation[]>();

  useEffect(() => {
    async function getTopUserReputation() {
      setLoading(true);
      try {
        if (orm) {
          const useDapper = orm === "dapper";
          const data = await GetTopUsersByReputation(useDapper);
          setData(data);
        }
      } catch (error: unknown) {
        const errMsg =
          error instanceof Error ? error?.message : "Unexpected error";
        toast.error(errMsg);
      } finally {
        setLoading(false);
      }
    }

    getTopUserReputation();
  }, [setLoading, orm]);

  return {
    data,
    loading,
  };
};

export default useTopUsers;
