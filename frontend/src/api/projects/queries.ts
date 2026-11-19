import { queryOptions } from "@tanstack/react-query";
import { projectApi } from "./api";

import type { PaginatedResponse } from "../common/types";
import type { GetWorkItemsRequest } from "../work-items/types";
import type { WorkItemResponse } from "../work-items/types";
import type { GetProjectsRequest, ProjectResponse } from "./types";

export const getProjectsKey = ["projects"] as const;

export const getProjectsOptions = (request: GetProjectsRequest) => {
  return queryOptions<PaginatedResponse<ProjectResponse>>({
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
  return queryOptions<ProjectResponse>({
    queryKey: getProjectKey(projectId),
    queryFn: async ({ signal }) => {
      const response = await projectApi.get(projectId, signal);
      return response.data;
    },
    refetchOnMount: false,
  });
};

export const projectWorkItemsKey = (projectId: string) =>
  ["project", projectId, "work-items"] as const;

export const getProjectWorkItemsOptions = (projectId: string, request: GetWorkItemsRequest) => {
  return queryOptions<PaginatedResponse<WorkItemResponse>>({
    queryKey: [...projectWorkItemsKey(projectId), request],
    queryFn: async ({ signal }) => {
      const response = await projectApi.getAllWorkItems(projectId, request, signal);
      return response.data;
    },
    refetchOnMount: false,
  });
};
