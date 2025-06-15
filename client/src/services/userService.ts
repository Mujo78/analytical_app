import type { UserProfileType } from "../types/user.type";
import { apiClient } from "../utils/api";

export async function GetTopUsersByReputation(useDapper: boolean) {
  const res = await apiClient.get("users", { params: { useDapper } });
  return res.data;
}

export async function GetUsersAnalytics(userId: string, useDapper: boolean) {
  const res = await apiClient.get(`users/analytics/${userId}`, {
    params: { useDapper },
  });
  return res.data;
}

export async function GetUserProfileInfo(userId: string, useDapper: boolean) {
  const res = await apiClient.get(`users/${userId}`, {
    params: { useDapper },
  });
  return res.data;
}

export async function UpdateUserProfile(
  userId: string,
  data: UserProfileType,
  useDapper: boolean
) {
  const res = await apiClient.put(`users/${userId}`, data, {
    params: { useDapper },
  });

  return res.data;
}

export async function CreateUserProfile(
  data: UserProfileType,
  useDapper: boolean
) {
  const res = await apiClient.post(`users`, data, {
    params: { useDapper },
  });

  return res.data;
}

export async function DistributeReptationBouns(useDapper: boolean) {
  const data = await apiClient.put(
    "users/distribute-bonus",
    {},
    {
      params: {
        useDapper,
      },
    }
  );

  return data.status;
}
