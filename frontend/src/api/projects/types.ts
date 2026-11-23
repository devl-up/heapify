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

export interface CreateWorkItemRequest {
  readonly title: string;
}

export interface GetWorkItemsRequest {
  readonly index: number;
  readonly size: number;
}

export interface WorkItemResponse {
  readonly number: number;
  readonly title: string;
}
