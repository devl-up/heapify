import apiClient from "@/api/api-client";

import type { UserDto, RegisterDto, LoginDto } from "./types";

const url = "/api/v1/auth";

const me = async () => {
  return await apiClient.get<UserDto>(`${url}/me`);
};

const register = async (dto: RegisterDto) => {
  return await apiClient.post(`${url}/register`, dto);
};

const login = async (dto: LoginDto) => {
  await apiClient.post(`${url}/login`, dto);
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
