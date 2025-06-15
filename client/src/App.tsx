import {
  createBrowserRouter,
  RouterProvider,
  type RouteObject,
} from "react-router";
import "./App.css";
import AppLayout from "./layouts/AppLayout";
import Dashboard from "./pages/Dashboard";
import EntityCore from "./pages/EntityCore";
import Dapper from "./pages/Dapper";
import Error404 from "./pages/Error404";
import UserAnalytics from "./pages/UserAnalytics";
import LastPostDetails from "./pages/LastPostDetails";
import UserProfile from "./pages/UserProfile";
import AddUser from "./pages/AddUser";
import AddPost from "./pages/AddPost";
import { MiniProfilerProvider } from "./context/ProfilerContext";

const routes: RouteObject = {
  path: "/",
  element: <AppLayout />,
  children: [
    {
      path: "",
      element: <Dashboard />,
    },
    {
      path: "/entity-core",
      element: <EntityCore />,
    },
    {
      path: "/dapper",
      element: <Dapper />,
    },
    {
      path: "/:orm/user-analytics/:userId",
      element: <UserAnalytics />,
    },
    {
      path: "/:orm/last-post/:postId",
      element: <LastPostDetails />,
    },
    {
      path: "/:orm/user-profile/:userId",
      element: <UserProfile />,
    },
    {
      path: "/:orm/add-user/",
      element: <AddUser />,
    },
    {
      path: "/:orm/add-post/:userId",
      element: <AddPost />,
    },
    {
      path: "*",
      element: <Error404 />,
    },
  ],
};

const router = createBrowserRouter([routes]);

function App() {
  return (
    <MiniProfilerProvider>
      <RouterProvider router={router} />{" "}
    </MiniProfilerProvider>
  );
}

export default App;
