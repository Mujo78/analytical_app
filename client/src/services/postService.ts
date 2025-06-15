import type { CreatePostType } from "../types/post.type";
import { apiClient } from "../utils/api";

export async function GetLastPostDetails(postId: string, useDapper: boolean) {
  const res = await apiClient.get(`posts/${postId}`, {
    params: {
      useDapper,
    },
  });

  return res.data;
}

export async function CreatePost(
  userId: string,
  data: CreatePostType,
  useDapper: boolean
) {
  const res = await apiClient.post(`posts/${userId}`, data, {
    params: {
      useDapper,
    },
  });

  return res.data;
}

export async function DeletePost(
  postId: string,
  userId: number,
  useDapper: boolean
) {
  const res = await apiClient.delete(`posts/${postId}`, {
    params: {
      userId,
      useDapper,
    },
  });

  return res.data;
}
