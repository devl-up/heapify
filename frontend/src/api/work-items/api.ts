import apiClient from "../api-client";

import type { CreateWorkItemRequest } from "./types";

const url = "/api/v1/work-items";

const create = async (request: CreateWorkItemRequest, signal?: AbortSignal) => {
  return await apiClient.post(`${url}`, request, { signal });
};

const workItemsApi = {
  create,
};

export { workItemsApi };
