export interface CreateProjectRequest {
  readonly name: string;
}

export interface GetProjectsRequest {
  readonly index: number;
  readonly size: number;
}

export interface GetProjectsResponse {
  readonly id: string;
  readonly name: string;
}

export interface GetProjectResponse {
  readonly id: string;
  readonly name: string;
}
