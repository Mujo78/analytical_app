import { useNavigate, useParams } from "react-router";
import type { UsersAnalyticsKey } from "../types/user.type";
import {
  Box,
  Button,
  Card,
  IconButton,
  Paper,
  Skeleton,
  SpeedDial,
  SpeedDialAction,
  SpeedDialIcon,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import UpdateIcon from "@mui/icons-material/Update";
import PostAddIcon from "@mui/icons-material/PostAdd";
import useAnalytics from "../hooks/useAnalytics";

const UserAnalytics = () => {
  const { orm, userId } = useParams();
  const navigate = useNavigate();
  const { data, loading } = useAnalytics();

  const handleNavigate = (url: string, key?: UsersAnalyticsKey) => {
    if (orm && (userId || data)) {
      let keyToUse;
      if (key === "userId") {
        keyToUse = `/${userId}`;
      }

      if (key !== "userId" && data) {
        keyToUse = key ? `/${data[key]}` : "";
      }

      navigate(`/${orm}/${url}${keyToUse}`);
    }
  };

  return (
    <Stack flexDirection="column" gap={3}>
      <Card
        sx={{
          padding: 3,
          display: "flex",
          flexDirection: "column",
          gap: 5,
          boxShadow: "2px 2px 15px 5px lightgray",
        }}
      >
        {loading || !data ? (
          <Skeleton variant="text" sx={{ fontSize: "1rem" }} />
        ) : (
          <Stack
            flexDirection="row"
            alignItems="center"
            justifyContent="space-between"
          >
            <Typography fontWeight="bold">{data?.displayName}</Typography>
            <Tooltip title="Update User Profile">
              <IconButton
                color="info"
                onClick={() => handleNavigate("user-profile", "userId")}
              >
                <UpdateIcon />
              </IconButton>
            </Tooltip>
          </Stack>
        )}

        <Stack
          flexWrap="wrap"
          flexDirection="row"
          justifyContent="space-around"
          gap={1}
        >
          {loading || !data ? (
            <>
              <Skeleton variant="rectangular" height={100} width={125} />
              <Skeleton variant="rectangular" height={100} width={125} />
              <Skeleton variant="rectangular" height={100} width={125} />
              <Skeleton variant="rectangular" height={100} width={125} />
            </>
          ) : (
            <>
              <Tooltip title="Average Post Score">
                <Paper
                  sx={{
                    padding: 4,
                    width: "20%",
                    cursor: "pointer",
                    textAlign: "center",
                  }}
                >
                  {data?.averagePostScore.toFixed(2)}
                </Paper>
              </Tooltip>
              <Tooltip title="Comment Count">
                <Paper
                  sx={{
                    padding: 4,
                    width: "20%",
                    cursor: "pointer",
                    textAlign: "center",
                  }}
                >
                  {data?.commentsCount}
                </Paper>
              </Tooltip>
              <Tooltip title="Posts Count">
                <Paper
                  sx={{
                    padding: 4,
                    width: "20%",
                    cursor: "pointer",
                    textAlign: "center",
                  }}
                >
                  {data?.postsCount}
                </Paper>
              </Tooltip>
              <Tooltip title="Total Views on Posts">
                <Paper
                  sx={{
                    padding: 4,
                    width: "20%",
                    cursor: "pointer",
                    textAlign: "center",
                  }}
                >
                  {data?.totalViewsOnPosts}
                </Paper>
              </Tooltip>
            </>
          )}
        </Stack>

        {loading || !data ? (
          <Skeleton variant="rectangular" height={115} />
        ) : (
          <Stack
            flexDirection="row"
            justifyContent="space-between"
            flexWrap="wrap"
          >
            <Stack gap={2}>
              <Typography>
                <strong>Latest Post Date: </strong>
                {new Date(data?.latestPostDate).toDateString()}
              </Typography>
              <Typography>
                <strong> Earliest Post Date: </strong>
                {new Date(data?.earliestPostDate).toDateString()}
              </Typography>
            </Stack>
            <Button
              sx={{ width: "auto" }}
              onClick={() => handleNavigate("last-post", "latestPostCreatedId")}
              variant="contained"
            >
              Check Last Created Post
            </Button>
          </Stack>
        )}
        <Button onClick={() => navigate(-1)} sx={{ marginRight: "auto" }}>
          Back
        </Button>
      </Card>
      <Box
        sx={{
          position: "absolute",
          bottom: 20,
          right: 20,
        }}
      >
        <SpeedDial
          ariaLabel="SpeedDial basic example"
          sx={{ position: "absolute", bottom: 16, right: 16 }}
          icon={<SpeedDialIcon />}
        >
          <SpeedDialAction
            icon={<PostAddIcon />}
            tooltipTitle="Add Post"
            onClick={() => handleNavigate("add-post", "userId")}
          />
        </SpeedDial>
      </Box>
    </Stack>
  );
};

export default UserAnalytics;
