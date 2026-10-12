import apiClient from "../api-client";

import type { PaginatedResult } from "../common/types";
import type { CreateProjectDto, ProjectDto } from "./types";

const url = "/api/v1/projects";

const getAll = async (index: number, size: number) => {
  return await apiClient.get<PaginatedResult<ProjectDto>>(`${url}`, { params: { index, size } });
};

const create = async (dto: CreateProjectDto) => {
  return await apiClient.post(`${url}`, dto);
};

export const projectApi = {
  create,
  getAll,
};
