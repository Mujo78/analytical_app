import { Button, Stack } from "@mui/material";
import { useNavigate } from "react-router";

const Dashboard = () => {
  const navigate = useNavigate();

  const handleNavigate = (url: string) => {
    navigate(url);
  };

  return (
    <Stack spacing={2} height="100%" justifyContent="center">
      <Stack
        gap={3}
        flexDirection="row"
        justifyContent="center"
        alignItems="center"
      >
        <Button
          color="primary"
          variant="contained"
          onClick={() => handleNavigate("entity-core")}
        >
          Entity Framework
        </Button>
        <Button
          color="secondary"
          variant="contained"
          onClick={() => handleNavigate("dapper")}
        >
          Dapper
        </Button>
      </Stack>
    </Stack>
  );
};

export default Dashboard;
