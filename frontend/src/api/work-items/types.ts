export interface CreateWorkItemRequest {
  readonly projectId: string;
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
