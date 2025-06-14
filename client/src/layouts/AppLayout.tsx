import { Stack } from "@mui/material";
import { Outlet } from "react-router";
import { ReactRouterAppProvider } from "@toolpad/core/react-router";
import { type Branding, type Navigation } from "@toolpad/core/AppProvider";
import DashboardIcon from "@mui/icons-material/Dashboard";
import Timeline from "@mui/icons-material/Timeline";
import QueryStatsIcon from "@mui/icons-material/QueryStats";
import { DashboardLayout, PageContainer } from "@toolpad/core";

const AppLayout = () => {
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

  const branding: Branding = {
    logo: <QueryStatsIcon sx={{ height: 60, width: 35 }} />,
    homeUrl: "/",
    title: "Query App",
  };

  return (
    <Stack sx={{ maxWidth: "100%", minHeight: "100vh" }}>
      <ReactRouterAppProvider navigation={navigation}>
        <DashboardLayout disableCollapsibleSidebar branding={branding}>
          <PageContainer>
            <Outlet />
          </PageContainer>
        </DashboardLayout>
      </ReactRouterAppProvider>
    </Stack>
  );
};

export default AppLayout;
