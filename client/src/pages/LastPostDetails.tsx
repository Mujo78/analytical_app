import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import type { Post } from "../types/post.type";
import { DeletePost, GetLastPostDetails } from "../services/postService";
import {
  IconButton,
  List,
  ListItem,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import {
  DeleteCommentFromPost,
  GetCommentsForPostByPostId,
} from "../services/commentService";
import type { Comment } from "../types/comment.type";
import DeleteIcon from "@mui/icons-material/Delete";
import DeleteForeverIcon from "@mui/icons-material/DeleteForever";

const LastPostDetails = () => {
  const navigate = useNavigate();
  const { orm, postId } = useParams();
  const [post, setPost] = useState<Post>();
  const [comments, setComments] = useState<Comment[]>();

  useEffect(() => {
    async function getPostDetails() {
      if (postId && orm) {
        const useDapper = orm === "dapper";
        const data = await GetLastPostDetails(postId, useDapper);

        setPost(data);
      }
    }

    getPostDetails();
  }, [orm, postId]);

  useEffect(() => {
    async function GetCommentsForPost() {
      if (post?.id) {
        const useDapper = orm === "dapper";
        const data = await GetCommentsForPostByPostId(post.id, useDapper);
        setComments(data);
      }
    }

    GetCommentsForPost();
  }, [orm, post?.id]);

  const handleDeleteComment = async (commentId: number) => {
    if (orm && postId) {
      const useDapper = orm === "dapper";
      const commentIdToDelete = await DeleteCommentFromPost(
        postId,
        commentId,
        useDapper
      );
      if (commentIdToDelete) {
        setComments((prev) =>
          prev?.filter((comment) => comment.id !== commentIdToDelete)
        );
      }
    }
  };

  const handleDeletePost = async () => {
    if (orm && postId && post?.ownerUserId) {
      const useDapper = orm === "dapper";
      const postIdToDelete = await DeletePost(
        postId,
        post.ownerUserId,
        useDapper
      );
      if (postIdToDelete) {
        navigate(`/${orm}/user-analytics/${post.ownerUserId}`);
      }
    }
  };

  return (
    <Stack flexDirection="column" gap={3}>
      <Stack
        justifyContent="space-between"
        borderBottom="1px solid lightgray"
        flexDirection="row"
      >
        <Typography variant="h6">
          {post?.title === null ? "Title not available" : post?.title}
        </Typography>
        <Stack width="auto" flexDirection="row">
          <Tooltip title="Delete Post">
            <IconButton color="error" onClick={handleDeletePost}>
              <DeleteForeverIcon />
            </IconButton>
          </Tooltip>
        </Stack>
      </Stack>
      {post !== undefined && (
        <Stack padding={2} bgcolor="darkgrey" borderRadius={2}>
          <div dangerouslySetInnerHTML={{ __html: post.body }}></div>
        </Stack>
      )}
      <Stack flexDirection="column">
        <Typography variant="subtitle2">Comments</Typography>
        <List>
          {comments?.map((comment) => (
            <ListItem
              key={comment.id}
              sx={{
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
              }}
            >
              <Stack flexDirection="column" gap={1}>
                <Typography
                  variant="subtitle1"
                  fontWeight="bold"
                  color="primary"
                >
                  {comment.userDisplayName}
                </Typography>

                {comment.text}
              </Stack>
              <IconButton
                color="error"
                onClick={() => handleDeleteComment(comment.id)}
              >
                <DeleteIcon />
              </IconButton>
            </ListItem>
          ))}
        </List>
      </Stack>
    </Stack>
  );
};

export default LastPostDetails;
