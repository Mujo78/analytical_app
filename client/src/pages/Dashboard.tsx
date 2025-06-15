import { Stack } from "@mui/material";
import { useNavigate } from "react-router";
import Card from "../components/ui/Card";

const items = [
  {
    id: 1,
    title: "Entity Framework",
    content:
      "EF Core is an object-relational mapper (ORM) that enables .NET developers to work with databases using .NET objects. It supports LINQ queries, change tracking, and migrations — making database interactions intuitive and strongly typed.",
    url: "entity-core",
  },
  {
    id: 2,
    title: "Dapper",
    content:
      "Dapper is a lightweight and high-performance micro ORM for .NET. It maps query results to objects with minimal overhead and gives you full control over SQL, ideal for speed-critical scenarios.",
    url: "dapper",
  },
];

const Dashboard = () => {
  const navigate = useNavigate();

  const handleNavigate = (url: string) => {
    navigate(url);
  };

  return (
    <Stack spacing={2} height="100%" justifyContent="center">
      <Stack
        gap={5}
        flexDirection="row"
        justifyContent="center"
        alignItems="center"
      >
        {items.map((item) => (
          <Card
            key={item.id}
            title={item.title}
            content={item.content}
            onClick={() => handleNavigate(item.url)}
          ></Card>
        ))}
      </Stack>
    </Stack>
  );
};

export default Dashboard;
