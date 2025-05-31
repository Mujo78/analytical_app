import { Menubar } from "primereact/menubar";
import { Menu, MenuItem, Sidebar } from "react-pro-sidebar";
import { Link, Outlet } from "react-router";

const AppLayout = () => {
  const items = [
    {
      label: "Company",
      root: true,

      items: [
        [
          {
            items: [
              {
                label: "Features",
                icon: "pi pi-list",
                subtext: "Subtext of item",
              },
              {
                label: "Customers",
                icon: "pi pi-users",
                subtext: "Subtext of item",
              },
              {
                label: "Case Studies",
                icon: "pi pi-file",
                subtext: "Subtext of item",
              },
            ],
          },
        ],
        [
          {
            items: [
              {
                label: "Solutions",
                icon: "pi pi-shield",
                subtext: "Subtext of item",
              },
              {
                label: "Faq",
                icon: "pi pi-question",
                subtext: "Subtext of item",
              },
              {
                label: "Library",
                icon: "pi pi-search",
                subtext: "Subtext of item",
              },
            ],
          },
        ],
        [
          {
            items: [
              {
                label: "Community",
                icon: "pi pi-comments",
                subtext: "Subtext of item",
              },
              {
                label: "Rewards",
                icon: "pi pi-star",
                subtext: "Subtext of item",
              },
              {
                label: "Investors",
                icon: "pi pi-globe",
                subtext: "Subtext of item",
              },
            ],
          },
        ],
        [
          {
            items: [
              {
                image:
                  "https://primefaces.org/cdn/primereact/images/uikit/uikit-system.png",
                label: "GET STARTED",
                subtext: "Build spectacular apps in no time.",
              },
            ],
          },
        ],
      ],
    },
    {
      label: "Resources",
      root: true,
    },
    {
      label: "Contact",
      root: true,
    },
  ];

  return (
    <div className="min-h-screen w-full flex bg-gray-100">
      <Sidebar className="min-h-full">
        <Menu
          menuItemStyles={{
            button: {
              [`&.active`]: {
                backgroundColor: "#13395e",
                color: "#b6c8d9",
              },
            },
          }}
        >
          <MenuItem component={<Link to="/" />}> Dashboard</MenuItem>
          <MenuItem component={<Link to="/entity-core" />}>
            {" "}
            Entity Core
          </MenuItem>
          <MenuItem component={<Link to="/dapper" />}> Dapper</MenuItem>
        </Menu>
      </Sidebar>
      <div className="flex flex-col gap-3 p-4">
        <Menubar
          model={items}
          className="p-3 surface-0 shadow-2"
          style={{ borderRadius: "3rem" }}
        />
        <main className="flex-1">
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default AppLayout;
