import type { InferType } from "yup";
import type { createPostValidationSchema } from "../validations/createPost";

export interface Post {
  id: number;
  acceptedAnswerId: number;
  answerCount: number;
  body: string;
  closedDate: Date;
  commentCount: number;
  communityOwnedDate: Date;
  creationDate: Date;
  favoriteCount: number;
  lastActivityDate: Date;
  lastEditDate: Date;
  lastEditorDisplayName: null;
  lastEditorUserId: number;
  ownerUserId: number;
  parentId: number;
  postTypeId: number;
  score: number;
  tags: string;
  title: string;
  viewCount: number;
}

export type CreatePostType = InferType<typeof createPostValidationSchema>;
