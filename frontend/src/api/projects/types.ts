export interface CreateProjectRequest {
  readonly name: string;
}

export interface GetProjectsRequest {
  readonly index: number;
  readonly size: number;
}

export interface ProjectResponse {
  readonly id: string;
  readonly name: string;
}
