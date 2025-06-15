import type { InferType } from "yup";
import type { createPostValidationSchema } from "../validations/createPost";
import type { Comment } from "./comment.type";

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

export interface LastPostDetailsType {
  id: number;
  body: string;
  closedDate: Date;
  creationDate: Date;
  lastEditDate: Date;
  postTypeId: number;
  score: number;
  tags: string;
  title: string;
  ownerUserId: number;
  viewCount: number;
  comments: Comment[];
}

export type CreatePostType = InferType<typeof createPostValidationSchema>;
