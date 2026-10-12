import { queryOptions } from "@tanstack/react-query";
import { projectApi } from "./api";

import type { PaginatedResult } from "../common/types";
import type { ProjectDto } from "./types";

export const getProjectsOptions = (index: number, size: number) => {
  return queryOptions<PaginatedResult<ProjectDto>>({
    queryKey: ["projects", index, size],
    queryFn: async () => {
      const response = await projectApi.getAll(index, size);
      return response.data;
    },
  });
};
