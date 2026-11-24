import apiClient from "../api-client";

import type { PaginatedResponse } from "../common/types";
import type {
  CreateProjectRequest,
  GetProjectResponse,
  GetProjectsRequest,
  GetProjectsResponse,
} from "./types";

const url = "/api/v1/projects";

const getAll = async (params: GetProjectsRequest, signal?: AbortSignal) => {
  return await apiClient.get<PaginatedResponse<GetProjectsResponse>>(`${url}`, {
    params,
    signal,
  });
};

const get = async (projectId: string, signal?: AbortSignal) => {
  return await apiClient.get<GetProjectResponse>(`${url}/${projectId}`, { signal });
};

const create = async (request: CreateProjectRequest, signal?: AbortSignal) => {
  return await apiClient.post<void>(`${url}`, request, { signal });
};

export const projectApi = {
  create,
  getAll,
  get,
};
