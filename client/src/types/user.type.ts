import type { InferType } from "yup";
import type { profileDataValidationSchema } from "../validations/userProfile";

export interface TopUserReputation {
  id: number;
  displayName: string;
  downVotes: number;
  reputation: number;
  upVotes: number;
  views: number;
  creationDate: Date;
}

export interface UserType {
  id: number;
  age: number;
  aboutMe: string;
  displayName: string;
  downVotes: number;
  emailHash: string;
  location: string;
  reputation: number;
  upVotes: number;
  views: number;
  creationDate: Date;
  lastAccessDate: Date;
}

export interface UsersAnalytics {
  userId: number;
  displayName: string;
  postsCount: number;
  commentsCount: number;
  averagePostScore: number;
  totalViewsOnPosts: number;
  earliestPostDate: Date;
  latestPostDate: Date;
  latestPostCreatedId: number;
}

export type UsersAnalyticsKey = keyof UsersAnalytics;
export type UserProfileType = InferType<typeof profileDataValidationSchema>;
