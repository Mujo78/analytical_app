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
      path: "*",
      element: <Error404 />,
    },
  ],
};

const router = createBrowserRouter([routes]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
