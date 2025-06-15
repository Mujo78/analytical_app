export interface MiniProfiler {
  Id: string;
  Name: string;
  Started: Date;
  DurationMilliseconds: number;
  MachineName: string;
  Root: Root;
  RootTimingId: string;
  User: string;
  HasUserViewed: boolean;
}

export interface Root {
  Id: string;
  Name: string;
  DurationMilliseconds: number;
  StartMilliseconds: number;
  Children: RootChild[];
  CustomTimings: RootCustomTimings;
  CustomTimingsJson: string;
  HasCustomTimings: boolean;
}

export interface RootChild {
  Id: string;
  Name: string;
  DurationMilliseconds: number;
  StartMilliseconds: number;
  Children: ChildChild[];
  HasCustomTimings: boolean;
}

export interface ChildChild {
  Id: string;
  Name: string;
  DurationMilliseconds: number;
  StartMilliseconds: number;
  HasCustomTimings: boolean;
  CustomTimings?: ChildCustomTimings;
  CustomTimingsJson?: string;
}

export interface ChildCustomTimings {
  sql: SQL[];
}

export interface SQL {
  Id: string;
  CommandString: string;
  ExecuteType: string;
  StackTraceSnippet: string;
  StartMilliseconds: number;
  DurationMilliseconds: number;
  Errored: boolean;
}

export interface RootCustomTimings {
  resource: Resource[];
}

export interface Resource {
  Id: string;
  CommandString: string;
  StackTraceSnippet: string;
  StartMilliseconds: number;
  Errored: boolean;
}
