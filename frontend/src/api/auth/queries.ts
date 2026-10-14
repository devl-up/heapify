import { queryOptions } from "@tanstack/react-query";
import { authApi } from "./api";

import type { UserResponse } from "./types";

export const meOptions = () => {
  return queryOptions<UserResponse | null>({
    queryKey: ["auth", "me"],
    queryFn: async () => {
      const response = await authApi.me();
      return response.data;
    },
    staleTime: 1000 * 60 * 5,
  });
};
