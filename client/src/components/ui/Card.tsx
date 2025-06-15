import {
  Button,
  CardActions,
  CardContent,
  Typography,
  Card as MUICard,
} from "@mui/material";
import React from "react";

interface CardProps {
  title: string;
  content: React.ReactNode | string;
  onClick: () => void;
}

const Card: React.FC<CardProps> = ({ title, content, onClick }) => {
  return (
    <MUICard sx={{ padding: 4, boxShadow: "5px 5px 20px gray" }}>
      <CardContent sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
        <Typography gutterBottom sx={{ color: "text.secondary" }} variant="h5">
          {title}
        </Typography>

        <Typography variant="body2" textAlign="justify">
          {content}
        </Typography>
      </CardContent>
      <CardActions>
        <Button onClick={onClick} variant="contained" size="medium">
          Getting Started
        </Button>
      </CardActions>
    </MUICard>
  );
};

export default Card;
