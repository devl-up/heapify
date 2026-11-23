import apiClient from "../api-client";

import type { PaginatedResponse } from "../common/types";
import type {
  CreateProjectRequest,
  CreateWorkItemRequest,
  GetProjectsRequest,
  ProjectResponse,
  GetWorkItemsRequest,
} from "./types";

const url = "/api/v1/projects";

const getAll = async (params: GetProjectsRequest, signal?: AbortSignal) => {
  return await apiClient.get<PaginatedResponse<ProjectResponse>>(`${url}`, {
    params,
    signal,
  });
};

const get = async (projectId: string, signal?: AbortSignal) => {
  return await apiClient.get<ProjectResponse>(`${url}/${projectId}`, { signal });
};

const getAllWorkItems = async (
  projectId: string,
  request: GetWorkItemsRequest,
  signal?: AbortSignal,
) => {
  return await apiClient.get(
    `${url}/${projectId}/work-items?index=${request.index}&size=${request.size}`,
    { signal },
  );
};

const getWorkItem = async (projectId: string, workItemNumber: number, signal?: AbortSignal) => {
  return await apiClient.get(`${url}/${projectId}/work-items/${workItemNumber}`, { signal });
};

const create = async (request: CreateProjectRequest, signal?: AbortSignal) => {
  return await apiClient.post(`${url}`, request, { signal });
};

const createWorkItem = async (
  projectId: string,
  request: CreateWorkItemRequest,
  signal?: AbortSignal,
) => {
  return await apiClient.post(`${url}/${projectId}/work-items`, request, { signal });
};

export const projectApi = {
  create,
  getAll,
  get,
  getAllWorkItems,
  getWorkItem,
  createWorkItem,
};
