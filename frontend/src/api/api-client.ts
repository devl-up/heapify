import axios from "axios";

const apiClient = axios.create();

apiClient.interceptors.request.use((config) => {
  const antiforgeryToken = document.cookie
    .split("; ")
    .find((x) => x.startsWith("XSRF-TOKEN="))
    ?.split("=")[1];

  if (antiforgeryToken) {
    config.headers["X-XSRF-TOKEN"] = antiforgeryToken;
  }
  return config;
});

let onUnauthenticated: (() => Promise<void>) | null = null;

export const setOnUnauthenticated = (callback: (() => Promise<void>) | null) => {
  onUnauthenticated = callback;
};

apiClient.interceptors.response.use(
  (response) => response,
  async (error: Error) => {
    if (!axios.isAxiosError(error)) {
      return Promise.reject(error);
    }

    const url = error.config?.url;

    // A failed /me attempt should clear the user's authentication state
    const meFailed = url?.endsWith("/api/v1/auth/me");

    // We exclude the login request when a 401 occurs, because invalid credentials should not trigger the unauthenticated handler
    const unauthorizedRequest =
      error.response?.status === 401 && !url?.endsWith("/api/v1/auth/login");

    if (meFailed || unauthorizedRequest) {
      await onUnauthenticated?.();
    }
    return Promise.reject(error);
  },
);

export default apiClient;
