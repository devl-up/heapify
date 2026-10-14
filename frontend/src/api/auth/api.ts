import apiClient from "@/api/api-client";

import type { UserResponse, RegisterRequest, LoginRequest } from "./types";

const url = "/api/v1/auth";

const me = async () => {
  return await apiClient.get<UserResponse>(`${url}/me`);
};

const register = async (request: RegisterRequest) => {
  return await apiClient.post(`${url}/register`, request);
};

const login = async (request: LoginRequest) => {
  await apiClient.post(`${url}/login`, request);
  await antiforgery();
};

const logout = async () => {
  await apiClient.post(`${url}/logout`);
  await antiforgery();
};

const antiforgery = async () => {
  await apiClient.get(`${url}/antiforgery`);
};

export const authApi = {
  me,
  register,
  login,
  logout,
  antiforgery,
};
