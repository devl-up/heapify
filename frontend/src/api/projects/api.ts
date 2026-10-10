import apiClient from "../api-client";

import type { CreateProjectDto } from "./types";

const url = "/api/v1/projects";

const create = async (dto: CreateProjectDto) => {
  return await apiClient.post(`${url}`, dto);
};

export const projectApi = {
  create,
};
