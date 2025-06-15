import { useNavigate } from "react-router";
import {
  Box,
  Card,
  Skeleton,
  SpeedDial,
  SpeedDialAction,
  SpeedDialIcon,
  Stack,
  Typography,
} from "@mui/material";
import PersonAddAltIcon from "@mui/icons-material/PersonAddAlt";
import useTopUsers from "../hooks/useTopUsers";
import { toast } from "react-toastify";
import { DistributeReptationBouns } from "../services/userService";
import PlusOneIcon from "@mui/icons-material/PlusOne";

const Dapper = () => {
  const navigate = useNavigate();
  const { data, loading } = useTopUsers("dapper");

  const handleNavigate = (url: string, userId?: number) => {
    const key = userId ? `/${userId}` : "";
    navigate(`/dapper/${url}${key}`);
  };

  const handleDistributeBonus = async () => {
    try {
      const status = await DistributeReptationBouns(true);
      if (status === 200) {
        toast.success("Bonus reputation points distributed successfully");
      }
    } catch (error: unknown) {
      toast.error(
        error instanceof Error
          ? error.message
          : "Something went wrong, please try again later."
      );
    }
  };

  return (
    <Stack flexDirection="column" gap={2}>
      <Typography variant="h6">Top Users By Reputation</Typography>
      {loading || !data
        ? Array.from(new Array(10)).map((_item, index) => (
            <Skeleton variant="rounded" key={index} height={55} />
          ))
        : data?.map((user) => (
            <Card
              sx={{
                padding: 2,
                display: "flex",
                justifyContent: "space-between",
                cursor: "pointer",
                ":hover": {
                  bgcolor: "lightgray",
                  animationDuration: "300ms",
                  transition: "all",
                },
              }}
              key={user.id}
              onClick={() => handleNavigate("user-analytics", user.id)}
            >
              <Stack>
                <Typography variant="h6">{user.displayName}</Typography>
                <Typography mt="auto" variant="body2">
                  Creation Date: {new Date(user.creationDate).toDateString()}
                </Typography>
              </Stack>
              <Stack>
                <Typography variant="body2">
                  Reputation: <strong>{user.reputation}</strong>
                </Typography>
                <Typography variant="body2">
                  Views: <strong>{user.views}</strong>
                </Typography>
                <Typography variant="body2">
                  Up Votes: <strong>{user.upVotes}</strong>
                </Typography>
                <Typography variant="body2">
                  Down Votes: <strong>{user.downVotes}</strong>
                </Typography>
              </Stack>
            </Card>
          ))}

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
            icon={<PersonAddAltIcon />}
            tooltipTitle="Add User"
            onClick={() => handleNavigate("add-user")}
          />
          <SpeedDialAction
            icon={<PlusOneIcon />}
            tooltipTitle="Distribute Bonus"
            onClick={handleDistributeBonus}
          />
        </SpeedDial>
      </Box>
    </Stack>
  );
};

export default Dapper;
