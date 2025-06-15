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

const Dapper = () => {
  const navigate = useNavigate();
  const { data, loading } = useTopUsers("dapper");

  const handleNavigate = (url: string, userId?: number) => {
    const key = userId ? `/${userId}` : "";
    navigate(`/dapper/${url}${key}`);
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
              <Typography variant="subtitle2">{user.displayName}</Typography>
              <Typography variant="body2">
                Reputation: <strong>{user.reputation}</strong>
              </Typography>
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
        </SpeedDial>
      </Box>
    </Stack>
  );
};

export default Dapper;
