import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Typography,
} from "@mui/material";
import type {
  ChildChild,
  MiniProfiler,
  Resource,
  RootChild,
  SQL,
} from "../../types/miniprofiler.type";

function renderSqlTimings(sqls: SQL[], level: number, hideDetails: boolean) {
  return sqls.map((sql) => (
    <TableRow key={sql.Id}>
      <TableCell sx={{ pl: `${level * 3}px` }}>
        <Typography
          variant="body2"
          color={sql.Errored ? "error" : "text.secondary"}
        >
          SQL: {sql.ExecuteType}
        </Typography>
      </TableCell>
      <TableCell>{sql.DurationMilliseconds.toFixed(2)}</TableCell>
      <TableCell>SQL Timing</TableCell>
      <TableCell>
        {!hideDetails && (
          <Typography
            variant="caption"
            component="pre"
            sx={{ whiteSpace: "pre-wrap", m: 0 }}
          >
            {sql.CommandString}
          </Typography>
        )}
      </TableCell>
    </TableRow>
  ));
}

function renderChildChildren(
  children: ChildChild[],
  level: number,
  hideDetails: boolean
) {
  return children.flatMap((child) => {
    const rows = [
      <TableRow key={child.Id}>
        <TableCell sx={{ pl: `${level * 3}px` }}>{child.Name}</TableCell>
        <TableCell>{child.DurationMilliseconds.toFixed(2)}</TableCell>
        <TableCell>ChildChild</TableCell>
        <TableCell></TableCell>
      </TableRow>,
    ];

    if (child.CustomTimings?.sql) {
      rows.push(
        ...renderSqlTimings(child.CustomTimings.sql, level + 1, hideDetails)
      );
    }

    return rows;
  });
}

function renderRootChildren(
  children: RootChild[],
  level: number,
  hideDetails: boolean
) {
  return children.flatMap((child) => {
    const rows = [
      <TableRow key={child.Id}>
        <TableCell sx={{ pl: `${level * 3}px` }}>{child.Name}</TableCell>
        <TableCell>{child.DurationMilliseconds.toFixed(2)}</TableCell>
        <TableCell>RootChild</TableCell>
        <TableCell></TableCell>
      </TableRow>,
    ];

    if (child.Children.length > 0) {
      rows.push(...renderChildChildren(child.Children, level + 1, hideDetails));
    }

    return rows;
  });
}

function renderResourceTimings(resources: Resource[], level: number) {
  return resources.map((res) => (
    <TableRow key={res.Id}>
      <TableCell sx={{ pl: `${level * 3}px` }}>
        <Typography variant="body2" color="text.secondary">
          Resource
        </Typography>
      </TableCell>
      <TableCell>-</TableCell>
      <TableCell>Resource Timing</TableCell>
      <TableCell>
        <Typography
          variant="caption"
          component="pre"
          sx={{ whiteSpace: "pre-wrap", m: 0 }}
        >
          {res.CommandString}
        </Typography>
      </TableCell>
    </TableRow>
  ));
}

export function MiniProfilerTable({ profiler }: { profiler: MiniProfiler }) {
  const hideDetails = profiler.Name === "User/DistributeReputationBonus";
  const root = profiler.Root;

  return (
    <TableContainer component={Paper} sx={{ maxHeight: 600 }}>
      <Table stickyHeader size="small" aria-label="Mini Profiler metrics">
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>Duration (ms)</TableCell>
            <TableCell>Type</TableCell>
            <TableCell>Details</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {/* Root Request */}
          <TableRow>
            <TableCell sx={{ fontWeight: "bold" }}>{root.Name}</TableCell>
            <TableCell sx={{ fontWeight: "bold" }}>
              {root.DurationMilliseconds.toFixed(2)}
            </TableCell>
            <TableCell sx={{ fontWeight: "bold" }}>Root</TableCell>
            <TableCell></TableCell>
          </TableRow>

          {/* Root children */}
          {renderRootChildren(root.Children, 1, hideDetails)}

          {/* Root custom timings (resource) */}
          {root.HasCustomTimings &&
            root.CustomTimings.resource &&
            renderResourceTimings(root.CustomTimings.resource, 1)}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
