import { Box, Modal, Skeleton, Stack } from "@mui/material";
import { Outlet } from "react-router";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { type Branding, type Navigation } from "@toolpad/core/AppProvider";
import DashboardIcon from "@mui/icons-material/Dashboard";
import Timeline from "@mui/icons-material/Timeline";
import QueryStatsIcon from "@mui/icons-material/QueryStats";
import { DashboardLayout, PageContainer } from "@toolpad/core";
import { ToastContainer } from "react-toastify";
import useProfilerResult from "../hooks/useProfilerResult";
import { useEffect, useRef, useState } from "react";
import { apiClient } from "../utils/api";
import { MiniProfilerTable } from "../components/ui/MiniProfilerTable";
import SidebarFooter from "../components/ui/SidebarFooter";
import useRequestState from "../hooks/useRequestState";

const navigation: Navigation = [
  {
    kind: "header",
    title: "Main items",
  },
  {
    segment: "entity-core",
    title: "Entity Framework",
    icon: <DashboardIcon />,
  },
  {
    segment: "dapper",
    title: "Dapper",
    icon: <Timeline />,
  },
];

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 1000,
  bgcolor: "background.paper",
  borderRadius: "12px",
  boxShadow: 24,
  p: 3,
};

const AppLayout = () => {
  const [show, setShow] = useState<boolean>(false);
  const { loading, setLoading } = useRequestState();
  const [showModal, setShowModal] = useState<boolean>(false);
  const branding: Branding = {
    logo: <QueryStatsIcon sx={{ height: 60, width: 35 }} />,
    homeUrl: "/",
    title: "Query App",
  };

  useEffect(() => {
    localStorage.setItem("currentState", "unoptimised");
  }, []);

  const { data } = useProfilerResult();

  const [loadingCount, setLoadingCount] = useState(0);
  const timerRef = useRef<number | null>(null);

  useEffect(() => {
    const requestInterceptor = apiClient.interceptors.request.use((config) => {
      setLoadingCount((prev) => prev + 1);
      return config;
    });

    const responseInterceptor = apiClient.interceptors.response.use(
      (response) => {
        setLoadingCount((prev) => Math.max(prev - 1, 0));
        return response;
      },
      (error) => {
        setLoadingCount((prev) => Math.max(prev - 1, 0));
        return Promise.reject(error);
      }
    );

    return () => {
      apiClient.interceptors.request.eject(requestInterceptor);
      apiClient.interceptors.response.eject(responseInterceptor);
    };
  }, []);

  useEffect(() => {
    if (loadingCount === 0) {
      timerRef.current = setTimeout(() => {
        setShow(true);
      }, 5000);
    } else {
      setShow(false);
      if (timerRef.current) {
        clearTimeout(timerRef.current);
        timerRef.current = null;
      }
    }
  }, [loadingCount]);

  return (
    <Stack sx={{ maxWidth: "100%", minHeight: "100vh" }}>
      <ReactRouterAppProvider navigation={navigation}>
        {loading ? (
          <Stack flexDirection="column" gap={2}>
            <Skeleton width="100%" />
            <Stack gap={2}>
              <Skeleton height="100%" width={120} />
              <Skeleton height="100%" width="100%" />
            </Stack>
          </Stack>
        ) : (
          <DashboardLayout
            slots={{
              sidebarFooter: () => (
                <SidebarFooter
                  mini
                  setShowModal={setShowModal}
                  show={show}
                  setLoading={setLoading}
                />
              ),
            }}
            disableCollapsibleSidebar
            branding={branding}
          >
            <PageContainer>
              <Outlet />
              <ToastContainer />
              <Modal open={showModal} onClose={() => setShowModal(false)}>
                <Box sx={style}>
                  {data && <MiniProfilerTable profiler={data} />}
                </Box>
              </Modal>
            </PageContainer>
          </DashboardLayout>
        )}
      </ReactRouterAppProvider>
    </Stack>
  );
};

export default AppLayout;
