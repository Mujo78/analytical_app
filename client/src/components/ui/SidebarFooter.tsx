import { Button, Stack } from "@mui/material";
import { useLocation } from "react-router";
import { CreateIndexes, DropIndexes } from "../../services/profilerService";
import { toast } from "react-toastify";

type SidebarFooterProps = {
  mini?: boolean;
  setShowModal: React.Dispatch<React.SetStateAction<boolean>>;
  show: boolean;
  setLoading: React.Dispatch<React.SetStateAction<boolean>>;
};

const SidebarFooter = ({
  mini,
  setShowModal,
  show,
  setLoading,
}: SidebarFooterProps) => {
  const location = useLocation();
  console.log(mini);
  const currentState = localStorage.getItem("currentState");

  const handleChangeDatabase = async () => {
    try {
      setLoading(true);
      let dropStatus, createStatus;
      if (currentState === "optimised") {
        dropStatus = await DropIndexes();
      } else {
        createStatus = await CreateIndexes();
      }

      if (createStatus === 200) {
        setLoading(false);
        localStorage.setItem("currentState", "optimised");
        toast.success("Database changed successfully.");
      }

      if (dropStatus === 200) {
        setLoading(false);
        localStorage.setItem("currentState", "unoptimized");
        toast.success("Database changed successfully.");
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
    <Stack
      p={2}
      flexGrow={1}
      flexDirection="column"
      justifyContent="space-between"
    >
      {location.pathname !== "/" && show && (
        <Button
          sx={{ marginBottom: "auto" }}
          color="info"
          onClick={() => setShowModal(true)}
        >
          Show Metrics
        </Button>
      )}
      <Button
        onClick={handleChangeDatabase}
        variant="contained"
        sx={{ marginBottom: "10%", marginTop: "auto" }}
      >
        Use {currentState === "optimised" ? "unoptimized" : "optimised"} DB
      </Button>
    </Stack>
  );
};

export default SidebarFooter;
