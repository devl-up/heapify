import apiClient from "../api-client";

import type { PaginatedResponse } from "../common/types";
import type {
  CreateWorkItemRequest,
  GetWorkItemsRequest,
  WorkItemResponse,
  WorkItemsResponse,
} from "./types";

const url = (projectId: string) => `/api/v1/projects/${projectId}`;

const getAll = async (projectId: string, request: GetWorkItemsRequest, signal?: AbortSignal) => {
  return await apiClient.get<PaginatedResponse<WorkItemsResponse>>(
    `${url(projectId)}/work-items?index=${request.index}&size=${request.size}`,
    { signal },
  );
};

const get = async (projectId: string, workItemNumber: number, signal?: AbortSignal) => {
  return await apiClient.get<WorkItemResponse>(`${url(projectId)}/work-items/${workItemNumber}`, {
    signal,
  });
};

const create = async (projectId: string, request: CreateWorkItemRequest, signal?: AbortSignal) => {
  return await apiClient.post<void>(`${url(projectId)}/work-items`, request, { signal });
};

export const workItemApi = {
  getAll,
  get,
  create,
};
