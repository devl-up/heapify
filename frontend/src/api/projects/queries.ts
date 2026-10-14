import { queryOptions } from "@tanstack/react-query";
import { projectApi } from "./api";

import type { PaginatedResponse } from "../common/types";
import type { GetProjectsRequest, ProjectResponse } from "./types";

export const getProjectsOptions = (request: GetProjectsRequest) => {
  return queryOptions<PaginatedResponse<ProjectResponse>>({
    queryKey: ["projects", request],
    queryFn: async ({ signal }) => {
      const response = await projectApi.getAll(request, signal);
      return response.data;
    },
  });
};
