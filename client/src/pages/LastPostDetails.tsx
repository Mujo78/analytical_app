import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import type { LastPostDetailsType } from "../types/post.type";
import { DeletePost, GetLastPostDetails } from "../services/postService";
import {
  Button,
  IconButton,
  List,
  ListItem,
  Skeleton,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import { DeleteCommentFromPost } from "../services/commentService";
import DeleteIcon from "@mui/icons-material/Delete";
import DeleteForeverIcon from "@mui/icons-material/DeleteForever";
import useRequestState from "../hooks/useRequestState";
import { toast } from "react-toastify";

const LastPostDetails = () => {
  const navigate = useNavigate();
  const { orm, postId } = useParams();
  const [post, setPost] = useState<LastPostDetailsType>();
  const { loading, setLoading } = useRequestState();
  const [deleted, setDeleted] = useState<{
    success: boolean;
    userId: number | null;
  }>({ success: false, userId: null });

  useEffect(() => {
    async function getPostDetails() {
      try {
        setLoading(true);
        if (postId && orm) {
          const useDapper = orm === "dapper";
          const data = await GetLastPostDetails(postId, useDapper);

          setPost(data);
        }
      } catch (error: unknown) {
        toast.error(
          error instanceof Error
            ? error?.message
            : "Something went wrong, please try again later!"
        );
        setLoading(true);
      } finally {
        setLoading(false);
      }
    }

    getPostDetails();
  }, [orm, postId, setLoading]);

  const handleDeleteComment = async (commentId: number) => {
    if (orm && postId) {
      const useDapper = orm === "dapper";
      const commentIdToDelete = await DeleteCommentFromPost(
        postId,
        commentId,
        useDapper
      );
      if (commentIdToDelete) {
        setPost((prev) => {
          if (!prev) return prev;
          return {
            ...prev,
            comments: prev.comments.filter(
              (comment) => comment.commentId !== commentIdToDelete
            ),
          };
        });
      }
    }
  };

  const handleDeletePost = async () => {
    setLoading(true);
    if (orm && postId && post?.ownerUserId) {
      const useDapper = orm === "dapper";
      const postIdToDelete = await DeletePost(
        postId,
        post.ownerUserId,
        useDapper
      );
      if (postIdToDelete) {
        setDeleted({ success: true, userId: post.ownerUserId });
        setLoading(false);
      }
    }
  };

  const handleNavigateBack = () => {
    navigate(`/${orm}/user-analytics/${deleted.userId}`);
    setDeleted({ success: false, userId: null });
  };

  return (
    <Stack flexDirection="column" gap={3}>
      {deleted.success ? (
        <Stack
          justifyContent="center"
          gap={2}
          flexDirection="column"
          alignItems="center"
        >
          Post Successfully Deleted
          <Button onClick={handleNavigateBack}>Go Back</Button>
        </Stack>
      ) : (
        <>
          {loading ? (
            <Skeleton variant="rectangular" height={50} />
          ) : (
            <Stack
              justifyContent="space-between"
              borderBottom="1px solid lightgray"
              flexDirection="row"
            >
              <Button onClick={() => navigate(-1)}>Go Back</Button>
              <Typography variant="h6">
                {post?.title === null ? "Title not available" : post?.title}
              </Typography>
              <Stack width="auto" flexDirection="row">
                {post !== undefined && post?.ownerUserId !== null && (
                  <Tooltip title="Delete Post">
                    <IconButton color="error" onClick={handleDeletePost}>
                      <DeleteForeverIcon />
                    </IconButton>
                  </Tooltip>
                )}
              </Stack>
            </Stack>
          )}
          {post !== undefined && !loading ? (
            <Stack padding={2} bgcolor="darkgrey" borderRadius={2}>
              <div dangerouslySetInnerHTML={{ __html: post.body }}></div>
            </Stack>
          ) : (
            <Skeleton variant="rounded" height={210} />
          )}
          <Stack flexDirection="column">
            {loading ? (
              <Skeleton variant="text" />
            ) : (
              post?.comments && (
                <Typography variant="subtitle2">Comments</Typography>
              )
            )}
            <List>
              {loading ? (
                <>
                  <Skeleton />
                  <Skeleton />
                  <Skeleton />
                </>
              ) : (
                post?.comments?.map((comment) => (
                  <ListItem
                    key={comment.commentId}
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
                      onClick={() => handleDeleteComment(comment.commentId)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </ListItem>
                ))
              )}
            </List>
          </Stack>
        </>
      )}
    </Stack>
  );
};

export default LastPostDetails;
