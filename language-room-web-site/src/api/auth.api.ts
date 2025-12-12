import { api } from "./axios";
import type { RegisterRequest, AuthResponse, LoginRequest, LoginResponse } from "../types/auth.types";

export const register = async (data: RegisterRequest) => {
  const response = await api.post<AuthResponse>(
    "/Auth/register",
    data
  );
  return response.data;
};

export const login = async (data: LoginRequest) => {
  const response = await api.post<LoginResponse>(
    "/Auth/login",
    data
  );
  return response.data;
};