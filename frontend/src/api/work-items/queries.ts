import { queryOptions } from "@tanstack/react-query";
import { workItemApi } from "./api";

import type { PaginatedResponse } from "../common/types";
import type { GetWorkItemsRequest, WorkItemResponse, WorkItemsResponse } from "./types";

export const workItemsKey = (projectId: string) => ["project", projectId, "work-items"] as const;

export const getWorkItemsOptions = (projectId: string, request: GetWorkItemsRequest) => {
  return queryOptions<PaginatedResponse<WorkItemsResponse>>({
    queryKey: [...workItemsKey(projectId), request],
    queryFn: async ({ signal }) => {
      const response = await workItemApi.getAll(projectId, request, signal);
      return response.data;
    },
    refetchOnMount: false,
  });
};

export const workItemKey = (projectId: string, workItemNumber: number) =>
  ["project", projectId, "work-items", workItemNumber] as const;

export const getWorkItemOptions = (projectId: string, workItemNumber: number) => {
  return queryOptions<WorkItemResponse>({
    queryKey: workItemKey(projectId, workItemNumber),
    queryFn: async ({ signal }) => {
      const response = await workItemApi.get(projectId, workItemNumber, signal);
      return response.data;
    },
    refetchOnMount: false,
  });
};
