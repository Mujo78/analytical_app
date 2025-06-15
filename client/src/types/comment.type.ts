export interface Comment {
  commentId: number;
  postId: number;
  userId: number;
  text: string;
  creationDate: Date;
  score: number;
  userDisplayName: string;
}
