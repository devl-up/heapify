import apiClient from "../api-client";

import type { PaginatedResponse } from "../common/types";
import type { CreateProjectRequest, GetProjectsRequest, ProjectResponse } from "./types";

const url = "/api/v1/projects";

const getAll = async (params: GetProjectsRequest, signal?: AbortSignal) => {
  return await apiClient.get<PaginatedResponse<ProjectResponse>>(`${url}`, {
    params,
    signal,
  });
};

const create = async (request: CreateProjectRequest, signal?: AbortSignal) => {
  return await apiClient.post(`${url}`, request, { signal });
};

export const projectApi = {
  create,
  getAll,
};
