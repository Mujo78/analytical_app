import { useEffect, useState } from "react";
import useRequestState from "./useRequestState";
import { useParams } from "react-router";
import type { UsersAnalytics } from "../types/user.type";
import { GetUsersAnalytics } from "../services/userService";
import { toast } from "react-toastify";

const useAnalytics = () => {
  const { orm, userId } = useParams();
  const { loading, setLoading } = useRequestState();
  const [data, setData] = useState<UsersAnalytics>();

  useEffect(() => {
    async function getUserAnalytics() {
      setLoading(true);
      try {
        if (userId) {
          const useDapper = orm === "dapper";
          const data = await GetUsersAnalytics(userId, useDapper);

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

    getUserAnalytics();
  }, [orm, userId, setLoading]);

  return {
    data,
    loading,
  };
};

export default useAnalytics;
