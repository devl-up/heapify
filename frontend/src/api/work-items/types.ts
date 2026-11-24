export interface CreateWorkItemRequest {
  readonly title: string;
}

export interface GetWorkItemsRequest {
  readonly index: number;
  readonly size: number;
}

export interface WorkItemsResponse {
  readonly number: number;
  readonly title: string;
}

export interface WorkItemResponse {
  readonly number: number;
  readonly title: string;
}
