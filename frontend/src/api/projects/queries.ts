import { queryOptions } from "@tanstack/react-query";
import { projectApi } from "./api";

import type { PaginatedResponse } from "../common/types";
import type { GetProjectResponse, GetProjectsRequest, GetProjectsResponse } from "./types";

export const getProjectsKey = ["projects"] as const;

export const getProjectsOptions = (request: GetProjectsRequest) => {
  return queryOptions<PaginatedResponse<GetProjectsResponse>>({
    queryKey: [...getProjectsKey, request],
    queryFn: async ({ signal }) => {
      const response = await projectApi.getAll(request, signal);
      return response.data;
    },
    refetchOnMount: false,
  });
};

export const getProjectKey = (projectId: string) => ["project", projectId] as const;

export const getProjectOptions = (projectId: string) => {
  return queryOptions<GetProjectResponse>({
    queryKey: getProjectKey(projectId),
    queryFn: async ({ signal }) => {
      const response = await projectApi.get(projectId, signal);
      return response.data;
    },
    refetchOnMount: false,
  });
};
