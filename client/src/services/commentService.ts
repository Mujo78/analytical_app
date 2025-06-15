import { apiClient } from "../utils/api";

export async function GetCommentsForPostByPostId(
  postId: number,
  useDapper: boolean
) {
  const res = await apiClient.get(`comments/post/${postId}`, {
    params: { useDapper },
  });
  return res.data;
}

export async function DeleteCommentFromPost(
  postId: string,
  commentId: number,
  useDapper: boolean
) {
  const res = await apiClient.delete(`comments/${postId}/delete/${commentId}`, {
    params: { useDapper },
  });
  return res.data;
}
