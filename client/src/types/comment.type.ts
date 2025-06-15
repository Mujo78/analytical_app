export interface Comment {
  id: number;
  postId: number;
  userId: number;
  text: string;
  creationDate: Date;
  score: number;
  userDisplayName: string;
}
